using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationPassword_v1._0
{
    public class DictionaryTranslite
    {
        public static readonly Dictionary<char, string> ConvertedLetters = new()
        {
        {'а', "a"},
        {'б', "b"},
        {'в', "v"},
        {'г', "g"},
        {'д', "d"},
        {'е', "e"},
        {'ё', "yo"},
        {'ж', "zh"},
        {'з', "z"},
        {'и', "i"},
        {'й', "j"},
        {'к', "k"},
        {'л', "l"},
        {'м', "m"},
        {'н', "n"},
        {'о', "o"},
        {'п', "p"},
        {'р', "r"},
        {'с', "s"},
        {'т', "t"},
        {'у', "u"},
        {'ф', "f"},
        {'х', "h"},
        {'ц', "c"},
        {'ч', "ch"},
        {'ш', "sh"},
        {'щ', "sch"},
        {'ъ', "j"},
        {'ы', "i"},
        {'ь', "j"},
        {'э', "e"},
        {'ю', "yu"},
        {'я', "ya"},
        {'А', "A"},
        {'Б', "B"},
        {'В', "V"},
        {'Г', "G"},
        {'Д', "D"},
        {'Е', "E"},
        {'Ё', "Yo"},
        {'Ж', "Zh"},
        {'З', "Z"},
        {'И', "I"},
        {'Й', "J"},
        {'К', "K"},
        {'Л', "L"},
        {'М', "M"},
        {'Н', "N"},
        {'О', "O"},
        {'П', "P"},
        {'Р', "R"},
        {'С', "S"},
        {'Т', "T"},
        {'У', "U"},
        {'Ф', "F"},
        {'Х', "H"},
        {'Ц', "C"},
        {'Ч', "Ch"},
        {'Ш', "Sh"},
        {'Щ', "Sch"},
        {'Ъ', "J"},
        {'Ы', "I"},
        {'Ь', "J"},
        {'Э', "E"},
        {'Ю', "Yu"},
        {'Я', "Ya"},
        {'1', "1"},
        {'2', "2"},
        {'3', "3"},
        {'4', "4"},
        {'5', "5"},
        {'6', "6"},
        {'7', "7"},
        {'8', "8"},
        {'9', "9"},
        {'0', "0"}
    };

        public static string ConvertToLatin(string source)
        {
            var result = new StringBuilder();
            foreach (var letter in source)
            {
                result.Append(ConvertedLetters[letter]);
            }
            return result.ToString();

        }

        public static string SplitConvertPassword(string sourse) //Output: 99AaaBbbCccDdd
        {
            string input = sourse;
            string output = sourse.Substring(0, 2);

            for (int i = 2; i < input.Length; i++)
            {
                if (i == 2 || (i > 2 && char.IsUpper(input[i]) && !char.IsUpper(input[i - 1])))
                {
                    output += input.Substring(i, 3);
                }
            }
            return output;
        }

        public  string BuildLogin(string firstName,string lastName)
        {
            string login = ConvertToLatin(lastName).ToLower() + "_" + ConvertToLatin(firstName).Substring(0,2).ToLower();
        
            return login;
        }

    }
}

