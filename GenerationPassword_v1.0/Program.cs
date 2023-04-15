using GenerationPassword_v1._0;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

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
    public static IEnumerable<Type> GetTypesWithMyAttribute(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (Attribute.IsDefined(type, typeof(CommandAttribute)))
                yield return type;
        }
    }
    public static void Main()
    {
        var outputStrings = new List<string>();
        while (true)
        {
            Console.WriteLine($"Выберите функцию:");
            PrintEnumValue<ChoiceMethod>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> typesWithMyAttribute = GetTypesWithMyAttribute(assembly);

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                continue;
            }

            Type type = null;

            switch ((ChoiceMethod)choice)
            {
                case ChoiceMethod.GenerateUser:
                    type = typesWithMyAttribute.FirstOrDefault(t => t.Name == "GenerateUserCommand");
                    break;
                case ChoiceMethod.GeneratePassword:
                    type = typesWithMyAttribute.FirstOrDefault(t => t.Name == "GeneratePasswordCommand");
                    break;
                default:
                    Console.WriteLine($"Функция с номером {choice} не найдена. Попробуйте еще раз.");
                    continue;
            }

            if (type != null)
            {
                var typeInstance = Activator.CreateInstance(type, outputStrings);
                if (typeInstance is ICommand)
                {
                    ((ICommand)typeInstance).Execute();
                }
                else
                {
                    throw new InvalidOperationException("Тип не реализует интерфейс ICommand.");
                }
            }
            else
            {
                Console.WriteLine($"Класс команды для функции {((ChoiceMethod)choice).ToString()} не найден.");
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
