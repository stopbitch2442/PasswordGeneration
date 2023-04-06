using GenerationPassword_v1._0;
using GenerationPassword_v1._0.Model;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main(string[] args)
    {
      
        var wordsGenerator = new WordsGenerator();
        
        string passwordWithoutFormatting = wordsGenerator.GeneratePasswordWithoutTranslite();
        string passwordTranslited = DictionaryTranslite.ConvertToLatin(passwordWithoutFormatting);
        string passwordWithFormatting = DictionaryTranslite.SplitConvertPassword(passwordTranslited);

        Console.WriteLine($"{passwordWithoutFormatting} {passwordTranslited} {passwordWithFormatting}");

        SetUser();
        
       
    }

    public static User SetUser()
    {
        var user = new User();
        user.Guid= Guid.NewGuid();
        Console.WriteLine("Введите Имя");
        string name = Console.ReadLine();
        user.FirstName = name;

        return user;
        //LastName = user.LastName;
        //Login = user.Login;
        //PasswordWithoutTranslite = user.PasswordWithoutTranslite;
        //PasswordTranslite = user.PasswordTranslite;
    }



}