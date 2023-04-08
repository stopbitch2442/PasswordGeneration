using GenerationPassword_v1._0;
using GenerationPassword_v1._0.Model;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
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
        SaveResult = 1,
        GoBack = 2
    }

    

    public static ChoiceMethod ValidateChoiceMethod([AllowNull] string choiceString)
    {
        if (!int.TryParse(choiceString, out int choiceInt))
        {
            Console.WriteLine();
            throw new Exception("Разрешается вводить только числа");
        }

        try
        {
            return (ChoiceMethod)choiceInt;
        }

        catch (Exception)
        {
            throw new Exception("Такого варианта еще нет в программе :(");
        }
    }
    
    public static void Main()
    {
         while (true)
        {
            _outputStrings.Clear();
            // Загуглить как получить текстовое представление енамок и подстаавлять их сюда типо "Выберите функцию:\n 1 {ChoiceMethod.GenerateUser.Description}"  +
            // Следующий шаг - выводить их через for Типо. cw("Выберите функцию:") и потом в форе или фориче выводить описание енамок +
            // Следующий шаг - сделать общий метод для всех енамок, чтобы автоматически выводились все их варианты
            // Все нужно автоматизировать и повторяющегося кода быть не должно)
            
            Console.WriteLine($"Выберите функцию:");
            foreach (ChoiceMethod value in Enum.GetValues(typeof(ChoiceMethod)))
            {
                Console.WriteLine((int)value + " - " + value.GetDescription());
            }
            ChoiceMethod? choice;
            // То же самое - общий метод для выбора варианта из любой енамки должен быть
            // TryException тоже желательно утащить в другой метод. До нас должно дойти только значение енамки
            try
            {
                choice = ValidateChoiceMethod(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }
            // Впоследствии - нуужно будет сделать метод, который сначала выведет описание енамки, затем будет ожидать ввода пользователя и возвращать нам уже проверенную хорошую енам очку
            
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
                // Любые входные данные в приложение должны быть проверены. Пример был выше
                int countPassword = Convert.ToInt32(Console.ReadLine());

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
        // Эту штуку тоже надо перегнать в енамку с выборами (впоследствии переведем все это в ООП)
        Console.WriteLine("Сохранить информацию в блокнот?\n1 - Да\n2 - Вернуться назад");
        string choice = Console.ReadLine();
        // Никакого ForEach, только вся введенная строка и попытка получить из них енамки
        foreach (char c in choice)
        {
            if (c != '1' && c != '2')
            {
                Console.WriteLine("Некорректный ввод! Пожалуйста, введите только цифры 1 или 2.");
            }
            else if (c == '1')
            {
                FlowSave(FileNaming());
            }
            else
            {
                Console.WriteLine("");
                // Плохо бесконечно вызывать рекурсию, почитай про переполнение CallStack'а, ведь при вызове рекурсии снова и снова, память засоряется и очистке не подлежит
                // Выше уже было решение без рекурсии, его, кстати, можно оптимизировать)
                Main();
            }
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

    public static void SetUser()
    {
        var user = new User();
        user.Guid = Guid.NewGuid();

        Console.WriteLine("Введите Имя");
        string firstName = Console.ReadLine();
        user.FirstName = firstName;

        Console.WriteLine("Введите Фамилию");
        string lastName = Console.ReadLine();
        user.LastName = lastName;

        var dictionaryTranslite = new DictionaryTranslite();
        user.Login = dictionaryTranslite.BuildLogin(firstName, lastName);

        var wordsGenerator = new WordsGenerator();
        user.PasswordWithoutTranslite = wordsGenerator.GeneratePasswordWithoutTranslite();
        user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);
        user.PasswordWithFormatting = DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite);

        // Такого быть не должно, это к вопросу об ООП ранее. Это просто две разных команды, должно быть что-то общее
        _outputStrings.Add($"Guid:{user.Guid}\nLogin:{user.Login}\nFirstName:{user.FirstName}\nLastName:{user.LastName}\nPassword:{user.PasswordWithFormatting}");
        Console.WriteLine(_outputStrings.LastOrDefault());
    }
}
