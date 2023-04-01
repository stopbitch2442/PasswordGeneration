using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace PasswordGeneration.Model
{
    public class TranslateName
    {
        public class Translit
        {
            // объявляем и заполняем словарь с заменами
            // при желании можно исправить словать или дополнить
            Dictionary<string, string> dictionaryChar = new Dictionary<string, string>()
            {
                {"а","a"},
                {"б","b"},
                {"в","v"},
                {"г","g"},
                {"д","d"},
                {"е","e"},
                {"ё","yo"},
                {"ж","zh"},
                {"з","z"},
                {"и","i"},
                {"й","y"},
                {"к","k"},
                {"л","l"},
                {"м","m"},
                {"н","n"},
                {"о","o"},
                {"п","p"},
                {"р","r"},
                {"с","s"},
                {"т","t"},
                {"у","u"},
                {"ф","f"},
                {"х","h"},
                {"ц","ts"},
                {"ч","ch"},
                {"ш","sh"},
                {"щ","sch"},
                {"ъ","'"},
                {"ы","yi"},
                {"ь",""},
                {"э","e"},
                {"ю","yu"},
                {"я","ya"}
            };
            /// <summary>
            /// метод делает транслит на латиницу
            /// </summary>
            /// <param name="source"> это входная строка для транслитерации </param>
            /// <returns>получаем строку после транслитерации</returns>
            public string TranslitFileName(string source)
            {
                var result = "";
                // проход по строке для поиска символов подлежащих замене которые находятся в словаре dictionaryChar
                foreach (var ch in source)
                {
                    var ss = "";
                    // берём каждый символ строки и проверяем его на нахождение его в словаре для замены,
                    // если в словаре есть ключ с таким значением то получаем true 
                    // и добавляем значение из словаря соответствующее ключу
                    if (dictionaryChar.TryGetValue(ch.ToString(), out ss))
                    {
                        result += ss;

                    }
                    // иначе добавляем тот же символ
                    else result += ch;
                }
                return result;
            }
        }
    }
}
