using GenerationPassword_v1._0;
using GenerationPassword_v1._0.Model;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Выберите функцию:\n1 - сгенерировать пользователя\n2 - сгенерировать несколько паролей");
        string choice = Console.ReadLine();
        foreach (char c in choice)
        {
            if (c != '1' && c != '2')
            {
                Console.WriteLine("Некорректный ввод! Пожалуйста, введите только цифры 1 или 2.");
            }
            else if (c == '1')
            {
                
                SetUser();
              //  Console.WriteLine($"{( user.Login,user.FirstName)}");
            }
            else
            {
                Console.WriteLine("Введите сколько паролей необходимо сгенерировать");
                int countPassword = Convert.ToInt32(Console.ReadLine());

                for (int i = 0; i < countPassword; i++)
                {
                    GeneratePassword();
                }
            }
        }
    }

    public static void GeneratePassword()
    {
        var user = new User();
        var wordsGenerator = new WordsGenerator();

        user.PasswordWithoutTranslite = wordsGenerator.GeneratePasswordWithoutTranslite();
        user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);

        Console.WriteLine(DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite));
    }

    public static User SetUser()
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
        return user;
        
    }
}