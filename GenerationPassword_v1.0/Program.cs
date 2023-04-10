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
            Console.WriteLine();
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

    public static void PrintEnumValue<T>() where T: Enum
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
                
                SetUser();
                SaveResultChoice();
                
            }
            else if (choice == ChoiceMethod.GeneratePassword)
            {
                
                Console.WriteLine("Введите сколько паролей необходимо сгенерировать");
                int countPassword = (Int32)ValidateChoiceMethod<ChoiceMethod>(Console.ReadLine());

                var generatedPasswords = new List<string>();
                for (int i = 0; i < countPassword; i++)
                {
                    _outputStrings.Add(GeneratePassword());
                }
                SaveResultChoice();
            }
        }
    }



    // Подумай о разделении ответственности, метод должен делать что-то одно простое, или же вызывать много простых методов, передавая результаты каждого, создавая эдакий конвейер, как в факторио

    public static void SaveResultChoice()
    {
        Console.WriteLine("Сохранить информацию в блокнот?");
        PrintEnumValue<SaveChoiceMethod>();

        try
        {
            string userInput = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userInput))
            {
                throw new ArgumentException("Выбран неверный вариант. Попробуйте еще раз.");
            }
            var saveChoice = ValidateChoiceMethod<SaveChoiceMethod>(userInput);

            if (saveChoice == SaveChoiceMethod.SaveResult)
            {
                
               FlowSave(FileNaming());
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Main();
        }
    }

    public static string FileNaming()
    {
        Console.WriteLine("Введите имя файла для сохранения:");
        // Прооверяем входные данные)
        string fileName = Console.ReadLine();
        return fileName;
    }
    public static void FlowSave(string fileName)
    {
        // Не должно быть строчек типо ".txt". Почитай про Magic numbers и Magic strings
        // Подумай о том, как дать пользователю выбрать директорию записи файла 
        using (var file = new StreamWriter(fileName + ".txt", true))
        {
            foreach (var outputString in _outputStrings)
            {
                file.WriteLine(outputString);
            }
        }
        Console.WriteLine("Файл " + fileName + ".txt Успешно сохранён");
    }



    public static string GeneratePassword()
    {
        // Зачем GeneratePassword делать юзера?
        var user = new User();
        var wordsGenerator = new WordsGenerator();

        user.PasswordWithoutTranslite = wordsGenerator.GeneratePasswordWithoutTranslite();
        user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);

        var result = DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite);
        Console.WriteLine(result);
        return result;
    }
    
        public static void ValidateLanguageLogin(string userInfo) 
        {
            try
            {
                var factory = new RankedLanguageIdentifierFactory();
                var identifier = factory.Load("Core14.profile.xml");
                var languages = identifier.Identify(userInfo);
                    if (languages.FirstOrDefault()?.Item1.Iso639_2T.ToLower() != "rus")
                    {
                    throw new Exception("Вводить необходимо на русском языке.");
                
                    }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
        
            }
        }

    private static bool isFirstName = true;

    public static string CheckInputLogin()
    {
        while (true)
        {
            try
            {
                string userInfo = Console.ReadLine();
                ValidateLanguageLogin(userInfo);

                if (Regex.IsMatch(userInfo, @"^[а-яА-ЯёЁ]+$"))
                {
                    return userInfo;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public static void SetUser()
    {
        var user = new User();
        user.Guid = Guid.NewGuid();

        Console.WriteLine("Введите Имя");
        string firstName = CheckInputLogin();
        while (firstName == null)
        {
            Console.WriteLine("Введите Имя на русском языке");
            firstName = CheckInputLogin();
        }
        user.FirstName = firstName;

        Console.WriteLine("Введите Фамилию");
        string lastName = CheckInputLogin();
        while (lastName == null || !Regex.IsMatch(lastName, @"^[а-яА-ЯёЁ]+$"))
        {
            if (lastName != null && !Regex.IsMatch(lastName, @"^[а-яА-ЯёЁ]+$"))
            {
                Console.WriteLine("Введите Фамилию на русском языке");
            }
            else
            {
                Console.WriteLine("Введите Фамилию");
            }

            lastName = CheckInputLogin();
        }
        user.LastName = lastName;

        var dictionaryTranslite = new DictionaryTranslite();
        user.Login = dictionaryTranslite.BuildLogin(user.FirstName, user.LastName);

        var wordsGenerator = new WordsGenerator();
        user.PasswordWithoutTranslite = wordsGenerator.GeneratePasswordWithoutTranslite();
        user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);
        user.PasswordWithFormatting = DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite);

        // Такого быть не должно, это к вопросу об ООП ранее. Это просто две разных команды, должно быть что-то общее
        _outputStrings.Add($"Guid:{user.Guid}\nLogin:{user.Login}\nFirstName:{user.FirstName}\nLastName:{user.LastName}\nPassword:{user.PasswordWithFormatting}");
        Console.WriteLine(_outputStrings.LastOrDefault());
    }
}
