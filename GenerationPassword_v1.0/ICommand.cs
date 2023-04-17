using GenerationPassword_v1._0.Model;
using NTextCat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GenerationPassword_v1._0
{
    public interface ICommand
    {
        void Execute();
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public int Command { get; set; }

        public CommandAttribute(int command)
        {
            Command = command;
        }
    }
    [Command(1)]
    public class GenerateUserCommand : ICommand //a command that creates a user
    {
        private readonly List<string> _outputStrings;

        public GenerateUserCommand(List<string> outputStrings)
        {
            _outputStrings = outputStrings;
        }
        public void Execute()
        {
            var user = new User { Guid = Guid.NewGuid() };

            user.FirstName = GetUserFirstName();
            user.LastName = GetUserLastName();

            SetUserCredentials(user);

            _outputStrings.Add($"Guid:{user.Guid}\nLogin:{user.Login}\nFirstName:{user.FirstName}\nLastName:{user.LastName}\nPassword:{user.PasswordWithFormatting}");
            Console.WriteLine(_outputStrings.LastOrDefault());
            SaveResult.SaveResultChoice();
        }
        public static string GetUserFirstName()
        {
            Console.WriteLine("Введите Имя");
            return CheckInputLogin();
        }

        public static string GetUserLastName()
        {
            Console.WriteLine("Введите Фамилию");
            return CheckInputLogin();
        }

        private static void SetUserCredentials(User user)
        {
            user.Login = new DictionaryTranslite().BuildLogin(user.FirstName, user.LastName);
            user.PasswordWithoutTranslite = new WordsGenerator().GeneratePasswordWithoutTranslite();
            user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);
            user.PasswordWithFormatting = DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite);
        }
        public static void ValidateLanguageLogin(string userInfo)
        {
            var factory = new RankedLanguageIdentifierFactory();
            var identifier = factory.Load("Core14.profile.xml");
            var language = identifier.Identify(userInfo).FirstOrDefault()?.Item1.Iso639_2T.ToLower();

            if (language != "rus")
            {
                throw new Exception("Вводить необходимо на русском языке.");
            }
        }

        public static string CheckInputLogin()
        {
            while (true)
            {
                try
                {
                    string userInfo = Console.ReadLine();
                    ValidateLanguageLogin(userInfo);

                    if (!Regex.IsMatch(userInfo, @"^[а-яА-ЯёЁ]+$"))
                    {
                        Console.WriteLine("Введите на русском языке");
                        continue;
                    }

                    return userInfo;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static string GeneratePassword(User user)
        {
            var wordsGenerator = new WordsGenerator();

            user.PasswordWithoutTranslite = wordsGenerator.GeneratePasswordWithoutTranslite();
            user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);

            var result = DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite);
            Console.WriteLine(result);
            return result;
        }
    }
    [Command(2)]
    public class GeneratePasswordCommand : ICommand
    {
        private readonly List<string> _outputStrings;
        public GeneratePasswordCommand(List<string> outputStrings)
        {
            _outputStrings = outputStrings;
        }
        public void Execute()
        {
            Console.WriteLine("Введите сколько паролей необходимо сгенерировать");
            int countPassword = Convert.ToInt32(ValidateChoiceMethod<ChoiceMethod>(Console.ReadLine()));

            var generatedPasswords = new List<string>();
            for (int i = 0; i < countPassword; i++)
            {
                var user = new User();
                _outputStrings.Add(GenerateUserCommand.GeneratePassword(user));
            }
            SaveResult.SaveResultChoice();
        }

    }

    [Command(1)]
    public class SaveResult
    {
        public static IEnumerable<Type> GetTypesWithMyAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(CommandAttribute), true).Length > 0)
                {
                    yield return type;
                }
            }
        }
        public static List<int> GetAttributeValues<T>(string attributeName) where T : class
        {
            var attributeValues = new List<int>();
            var fields = typeof(T).GetFields();

            foreach (var field in fields)
            {
                var customAttribute = field.GetCustomAttributes(false).FirstOrDefault(a => a.GetType().Name == attributeName);
                if (customAttribute != null)
                {
                    var value = (int)customAttribute.GetType().GetProperty("Value").GetValue(customAttribute, null);
                    attributeValues.Add(value);
                }
            }

            return attributeValues;
        }

        public static readonly List<string> _outputStrings = new();
        public static void PrintSaveResultChoice()
        {
            Console.WriteLine("Сохранить информацию в блокнот?");
            GetAttributeValues<SaveChoiceMethod>("Command");
        }
        public static SaveChoiceMethod GetUserInput()
        {
            string userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                throw new ArgumentException("Выбран неверный вариант. Попробуйте еще раз.");
            }
            var saveChoice = ValidateChoiceMethod<SaveChoiceMethod>(userInput);
            return saveChoice;
        }
        public static void ProcessUserChoice(SaveChoiceMethod saveChoice)
        {
            if (saveChoice == SaveChoiceMethod.SaveResult)
            {
                SaveToFile(FileNaming());
                _outputStrings.Clear();
            }
            else if (saveChoice == SaveChoiceMethod.GoBack)
            {
                Program.Main();
                _outputStrings.Clear();
            }
            else
            {
                throw new ArgumentException("Выбран неверный вариант. Попробуйте еще раз.");
            }
        }
        public static void SaveResultChoice()
        {
            PrintSaveResultChoice();
            try
            {
                var saveChoice = GetUserInput();
                ProcessUserChoice(saveChoice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Program.Main();
            }
        }
        public static string FileNaming()
        {
            Console.WriteLine("Введите имя файла для сохранения:");
            string fileName = Console.ReadLine().Trim();
            return fileName;
        }


        public static void SaveToFile(string fileName)
        {
            Console.WriteLine("Выберите папку для сохранения файла:");

            var folderPath = Console.ReadLine().Trim();

            if (Directory.Exists(folderPath))
            {
                var savePath = Path.Combine(folderPath, fileName + ".txt");

                try
                {
                    using (var file = new StreamWriter(savePath, true))
                    {
                        foreach (var outputString in _outputStrings)
                        {
                            file.WriteLine(outputString);
                        }
                    }

                    Console.WriteLine($"Файл {fileName}.txt успешно сохранен в {savePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось сохранить файл {fileName}: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Папка {folderPath} не существует.");
            }
        }
    }

}