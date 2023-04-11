using GenerationPassword_v1._0;
using GenerationPassword_v1._0.Model;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using NTextCat;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
public class Program
{
    private static readonly List<string> _outputStrings = new();

    public enum ChoiceMethod
    {
        [Description("сгенерировать пользователя")]
        GenerateUser = 1,
        [Description("сгенерировать несколько паролей")]
        GeneratePassword = 2
    }
    public enum SaveChoiceMethod
    {
        [Description("Сохранить результат")]
        SaveResult = 1,
        [Description("Вернуться назад")]
        GoBack = 2
    }



    public static T ValidateChoiceMethod<T>([AllowNull] string choiceString) where T : Enum
    {
        if (!int.TryParse(choiceString, out int choiceInt))
        {
            throw new Exception("Разрешается вводить только числа");
        }

        try
        {
            return (T)Enum.Parse(typeof(T), choiceInt.ToString());
        }

        catch (Exception)
        {
            throw new Exception("Такого варианта еще нет в программе :(");
        }
    }

    public static void PrintEnumValue<T>() where T : Enum
    {
        foreach (var value in Enum.GetValues(typeof(T)))
        {
            string description = ((Enum)value).GetDescription();
            Console.WriteLine($"{(int)value} - {description}");
        }
    }
    private static ChoiceMethod CheckChoiceUsers()
    {
        while (true)
        {
            try
            {
                return ValidateChoiceMethod<ChoiceMethod>(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
        }
    }

    public static void Main()
    {
        while (true)
        {
            _outputStrings.Clear();

            Console.WriteLine($"Выберите функцию:");
            PrintEnumValue<ChoiceMethod>();

            ChoiceMethod choice = CheckChoiceUsers();

            // Это тоже можно зарефачить но в виде ООП. Представь, что каждое значение из енамки - команда. Это объект.
            // У объекта метод допустим Execute(), в котором и есть вся логика этой команду
            // И у генерации пароля и пользователя этот метод будет с одинаковым названиями, ведь они оба - команда. Это сообщает нам о родстве и необходимости использовать
            // Либо интерфейс либо наследование. Нужно разобраться, что и зачем использовать (философская тема больше, но понимание должно быть)
            // С.е. нужно перетащить все это в ООП, но только после того, как зарефачишь остальные места
            if (choice == ChoiceMethod.GenerateUser)
            {

                SetUserData.SetUser();
                SaveResult.SaveResultChoice();

            }
            else if (choice == ChoiceMethod.GeneratePassword)
            {

                Console.WriteLine("Введите сколько паролей необходимо сгенерировать");
                int countPassword = (Int32)ValidateChoiceMethod<ChoiceMethod>(Console.ReadLine());

                var generatedPasswords = new List<string>();
                for (int i = 0; i < countPassword; i++)
                {
                    var user = new User();
                    _outputStrings.Add(GeneratePassword(user));
                }
                SaveResult.SaveResultChoice();
            }
        }
    }
    public class SaveResult
    {
        public static void PrintSaveResultChoice()
        {
            Console.WriteLine("Сохранить информацию в блокнот?");
            PrintEnumValue<SaveChoiceMethod>();
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
                Main();
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
                Main();
            }
        }
        public static string FileNaming()
        {
            Console.WriteLine("Введите имя файла для сохранения:");
            string fileName = Console.ReadLine().Trim();
            return fileName;
        }
        public static void SaveToFile(string fileName) // ХЗ как дать выбор пользователю, кроме этого костыля
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
 
    public static string GeneratePassword(User user)
    {
        var wordsGenerator = new WordsGenerator();

        user.PasswordWithoutTranslite = wordsGenerator.GeneratePasswordWithoutTranslite();
        user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);

        var result = DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite);
        Console.WriteLine(result);
        return result;
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
                    Console.WriteLine("Введите Имя на русском языке");
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
    public class SetUserData
    {
        public static void SetUser()
        {
            var user = new User { Guid = Guid.NewGuid() };

            user.FirstName = GetUserFirstName();
            user.LastName = GetUserLastName();

            SetUserCredentials(user);

            _outputStrings.Add($"Guid:{user.Guid}\nLogin:{user.Login}\nFirstName:{user.FirstName}\nLastName:{user.LastName}\nPassword:{user.PasswordWithFormatting}");
            Console.WriteLine(_outputStrings.LastOrDefault());
        }

        private static string GetUserFirstName()
        {
            Console.WriteLine("Введите Имя");
            return CheckInputLogin();
        }

        private static string GetUserLastName()
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

        private static string CheckInputLogin()
        {
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Пожалуйста, введите корректные данные.");
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
