using GenerationPassword_v1._0;
using GenerationPassword_v1._0.Model;

public class Program
{
    public static void Main(string[] args)
    {
        var wordsGenerator = new WordsGenerator();
       string passwordWithoutFormatting = wordsGenerator.GeneratePasswordWithoutTranslite();

        string passwordTranslited = DictionaryTranslite.ConvertToLatin(passwordWithoutFormatting);

        Console.WriteLine(passwordWithoutFormatting, passwordTranslited);
    }
}