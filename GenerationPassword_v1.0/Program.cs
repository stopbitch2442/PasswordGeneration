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
        var outputStrings = new List<string>();
        ICommand command;
        while (true)
        {
            Console.WriteLine($"Выберите функцию:");
            PrintEnumValue<ChoiceMethod>();

            ChoiceMethod choice = CheckChoiceUsers();
            switch (choice)
            {
                case ChoiceMethod.GenerateUser:
                    command = new GenerateUserCommand(outputStrings);
                    break;
                //case ChoiceMethod.GeneratePassword:
                //    command = new GeneratePasswordCommand(outputStrings);
                //    break;
                default:
                    throw new NotSupportedException();
            }

            command.Execute();
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
 
    
}
