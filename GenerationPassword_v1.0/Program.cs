using GenerationPassword_v1._0;

public class Program
{
    public static void Main(string[] args)
    {
        string passwordResultWithoutFormatting, passwordResultTranslited;

        WordsGenerator wordsGenerator = new WordsGenerator();

        wordsGenerator.GeneratePasswordWithoutTranslite(out passwordResultWithoutFormatting,out passwordResultTranslited);
       
        Console.WriteLine(passwordResultWithoutFormatting,passwordResultTranslited);
    }
}