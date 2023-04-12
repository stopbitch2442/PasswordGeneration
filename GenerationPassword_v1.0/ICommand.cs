using GenerationPassword_v1._0.Model;
using NTextCat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using static Program;

namespace GenerationPassword_v1._0
{
    public interface ICommand
    {
        void Execute();
    }

    public class GenerateUserCommand : ICommand //a command that creates a user
    {
        private readonly List<string> _outputStrings;

        public GenerateUserCommand(List<string> outputStrings)
        {
            _outputStrings = outputStrings;
        }
        public void Execute()
        {
            var user = new User { Guid = Guid.NewGuid() };

            user.FirstName = GetUserFirstName();
            user.LastName = GetUserLastName();

            SetUserCredentials(user);

            _outputStrings.Add($"Guid:{user.Guid}\nLogin:{user.Login}\nFirstName:{user.FirstName}\nLastName:{user.LastName}\nPassword:{user.PasswordWithFormatting}");
            Console.WriteLine(_outputStrings.LastOrDefault());
            Program.SaveResult.SaveResultChoice();
        }
        public static string GetUserFirstName()
        {
            Console.WriteLine("Введите Имя");
            return CheckInputLogin();
        }

        public static string GetUserLastName()
        {
            Console.WriteLine("Введите Фамилию");
            return CheckInputLogin();
        }

        private static void SetUserCredentials(User user)
        {
            user.Login = new DictionaryTranslite().BuildLogin(user.FirstName, user.LastName);
            user.PasswordWithoutTranslite = new WordsGenerator().GeneratePasswordWithoutTranslite();
            user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);
            user.PasswordWithFormatting = DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite);
        }
        public static void ValidateLanguageLogin(string userInfo)
        {
            var factory = new RankedLanguageIdentifierFactory();
            var identifier = factory.Load("Core14.profile.xml");
            var language = identifier.Identify(userInfo).FirstOrDefault()?.Item1.Iso639_2T.ToLower();

            if (language != "rus")
            {
                throw new Exception("Вводить необходимо на русском языке.");
            }
        }

        public static string CheckInputLogin()
        {
            while (true)
            {
                try
                {
                    string userInfo = Console.ReadLine();
                    ValidateLanguageLogin(userInfo);

                    if (!Regex.IsMatch(userInfo, @"^[а-яА-ЯёЁ]+$"))
                    {
                        Console.WriteLine("Введите на русском языке");
                        continue;
                    }

                    return userInfo;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static string GeneratePassword(User user)
        {
            var wordsGenerator = new WordsGenerator();

            user.PasswordWithoutTranslite = wordsGenerator.GeneratePasswordWithoutTranslite();
            user.PasswordTranslite = DictionaryTranslite.ConvertToLatin(user.PasswordWithoutTranslite);

            var result = DictionaryTranslite.SplitConvertPassword(user.PasswordTranslite);
            Console.WriteLine(result);
            return result;
        }
    }

    public class GeneratePasswordCommand : ICommand
    {
        private readonly List<string> _outputStrings;
        public GeneratePasswordCommand(List<string> outputStrings)
        {
            _outputStrings = outputStrings;
        }
        public void Execute()
        {
            Console.WriteLine("Введите сколько паролей необходимо сгенерировать");
            int countPassword = (Int32)ValidateChoiceMethod<Program.ChoiceMethod>(Console.ReadLine());

            var generatedPasswords = new List<string>();
            for (int i = 0; i < countPassword; i++)
            {
                var user = new User();
                _outputStrings.Add(GenerateUserCommand.GeneratePassword(user));
            }
            SaveResult.SaveResultChoice();
        }

    }
}