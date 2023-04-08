using GenerationPassword_v1._0;
using GenerationPassword_v1._0.Model;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Выберите функцию:\n1 - сгенерировать пользователя\n2 - сгенерировать несколько паролей");
        string choice = Console.ReadLine();

        foreach (char c in choice)
        {
            if (c != '1' && c != '2' && c != '3')
            {
                Console.WriteLine("Некорректный ввод! Пожалуйста, введите только цифры 1 или 2.");
                Main();
            }
            else if (c == '1')
            {
                SetUser();
                SaveResultChoice();
            }
            else if (c == '2')
            {
                Console.WriteLine("Введите сколько паролей необходимо сгенерировать");
                int countPassword = Convert.ToInt32(Console.ReadLine());

                TextWriter oldOut = Console.Out;

                for (int i = 0; i < countPassword; i++)
                {
                    GeneratePassword();
                }
                SaveResultChoice();
            }
            
        }
    }
    
    public static void SaveResultChoice()
    {
        Console.WriteLine("Сохранить информацию в блокнот?\n1 - Да\n2 - Вернуться назад");
        string choice = Console.ReadLine();
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
                Main();
            }
        }
    }

    public static string FileNaming()
    {
        Console.WriteLine("Введите имя файла для сохранения:");
        string fileName = Console.ReadLine();
        return fileName;
    }
    public static void FlowSave(string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName + ".txt", true))
        {
            Console.SetOut(writer);
            // чё тут написать чтобы выводимые значения мейн в streanwriter прилетали?
        }
        Console.WriteLine("Файл " + fileName + ".txt Успешно сохранён" );
    }



    public static void GeneratePassword()
    {
        var user = new User();
        var wordsGenerator = new WordsGenerator();

        user.PasswordWithoutTranslite = wordsGenerator.GeneratePasswordWithoutTranslite();
        user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);

        Console.WriteLine(DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite));
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

        Console.WriteLine($"Guid:{user.Guid}\nLogin:{user.Login}\nFirstName:{user.FirstName}\nLastName:{user.LastName}\nPassword:{user.PasswordWithFormatting}");

    }
}