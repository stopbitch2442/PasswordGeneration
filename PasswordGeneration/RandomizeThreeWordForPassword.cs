using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGeneration
{
    public class RandomizeFourWordForPassword
    {
        private readonly List<string> PasswordFirstWord = new List<string>
        {
    "Дикие",
    "Грязные",
    "Жестокие",
    "Маленькие",
    "Старые",
    "Варварские",
    "Интересные",
    "Одинокие",
    "Беспощадные",
    "Русские"
        };

        private readonly List<string> PasswordSecondWord = new List<string>
        {
    "Гризли",
    "Суслики",
    "Коровы",
    "Кроты",
    "Обезьяны",
    "Ювелиры",
    "Макаки",
    "Свиньи",
    "Афиканцы",
    "Медведи"
        };

        private readonly List<string> PasswordThirdWord = new List<string>
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

        private readonly List<string> PasswordFourWord = new List<string>
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


        private readonly Random random = new Random();

        public string GenerateWordsForPassword()
        {
            var index = random.Next(PasswordFirstWord.Count);
            var password = PasswordFirstWord[index];

            var index1 = random.Next(PasswordSecondWord.Count);
            var password1 = PasswordSecondWord[index1];

            var index2 = random.Next(PasswordThirdWord.Count);
            var password2 = PasswordThirdWord[index2];

            var index3 = random.Next(PasswordFourWord.Count);
            var password3 = PasswordFourWord[index3];

            var passwordResult = password + password1 + password2 + password3;

            return passwordResult;
        }
    }
}
