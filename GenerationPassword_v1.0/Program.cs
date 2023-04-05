using GenerationPassword_v1._0;
using GenerationPassword_v1._0.Model;
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

    }



}