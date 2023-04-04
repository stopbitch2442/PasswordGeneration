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

            var firstWord = random.Next(PasswordFirstWord.Count);
            var password = PasswordFirstWord[firstWord];

            var index1 = random.Next(PasswordSecondWord.Count);
            var password1 = PasswordSecondWord[index1];
            
            var index2 = random.Next(PasswordThirdWord.Count);
            var password2 = PasswordThirdWord[index2];

            var index3 = random.Next(PasswordFourWord.Count);
            var password3 = PasswordFourWord[index3];

            string passwordWithoutFormatting = number + password + password1 + password2 + password3;
            return passwordWithoutFormatting;

       
           
        }

    }
}
