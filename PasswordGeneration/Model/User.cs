using PasswordGeneration;
using PasswordGeneration.Model;
using static PasswordGeneration.Model.TranslateName;

public  class User
{
    Guid Guid { get; set; }
    string FirstName{ get; set; }
    string LastName { get; set; }
    string Login { get; set; }
    
    int Age { get; set; }
    string Password { get; set; }
    string Email { get; set; }

    public static void GeneratedDataForUser()
    {
        Random random = new Random();
        var user = new User();

        user.Guid = Guid.NewGuid();

        Console.WriteLine("Введите Имя");
        user.FirstName = Console.ReadLine();
        Console.WriteLine("Введите Фамилию");
        user.LastName = Console.ReadLine();

        Translit translit= new Translit();
        string FirstNameTranslit = translit.TranslitFileName(user.FirstName);
        string LastNameTranslit = translit.TranslitFileName(user.LastName);

        user.Login = FirstNameTranslit + "_" + LastNameTranslit; // логин это транслит Имя + _ + фамилия

        RandomizeFourWordForPassword randomizeFourWordForPassword = new RandomizeFourWordForPassword();
        string WordsPassword = randomizeFourWordForPassword.GenerateWordsForPassword();
         


        user.Password =  random.NextInt64(10,99).ToString() + WordsPassword; // пароль это рандомное число + 4 рандомных слова транслитом

        Console.WriteLine($"Данные о пользователе:\n" +
            $"Guid:{user.Guid},Имя:{user.FirstName},Фамилия:{user.LastName},Логин:{user.Login},Пароль:{user.Password}");
    }
}
