using GenerationPassword_v1._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationPassword_v1._0
{
    public class WordsGenerator
    {
        private readonly Random random = new();

        private readonly List<string> PasswordFirstWord = new()
        {
    "Диких",
    "Грязных",
    "Жестоких",
    "Маленьких",
    "Старых",
    "Варварских",
    "Интересных",
    "Одиноких",
    "Беспощадных",
    "Русских"
        };

        private readonly List<string> PasswordSecondWord = new()
        {
    "Гризли",
    "Сусликов",
    "Коров",
    "Кротов",
    "Обезьян",
    "Ювелиров",
    "Макак",
    "Свиней",
    "Афиканцец",
    "Медведей"
        };

        private readonly List<string> PasswordThirdWord = new()
        {
    "Съели",
    "Взяли",
    "Отобрали",
    "Забрали",
    "Подарили",
    "Купили",
    "Своровали",
    "Любили",
    "Усыновили",
    "Спрятали"
        };

        private readonly List<string> PasswordFourWord = new()
        {
    "Зуб",
    "Морковь",
    "Яблоко",
    "Ребёнка",
    "Кольцо",
    "Суп",
    "Машину",
    "Человека",
    "Цепочку",
    "Бриллиант"
        };
        
        public string GeneratePasswordWithoutTranslite()
        {
         
            var number = random.NextInt64(10, 99);

            var FirstWord = random.Next(PasswordFirstWord.Count);
            var passwordFirstWord = PasswordFirstWord[FirstWord];

            var SecondWord = random.Next(PasswordSecondWord.Count);
            var passwordSecondWord = PasswordSecondWord[SecondWord];
            
            var ThirdWord = random.Next(PasswordThirdWord.Count);
            var passwordThirdWord = PasswordThirdWord[ThirdWord];

            var FourWord = random.Next(PasswordFourWord.Count);
            var passwordFourWord = PasswordFourWord[FourWord];

            string passwordWithoutFormatting = number + passwordFirstWord + passwordSecondWord + passwordThirdWord + passwordFourWord;

            return passwordWithoutFormatting;
        }
    }
}
