using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationPassword_v1._0.Model
{
    public class ChoiceMethod
    {
        public int Value { get; set; }
        public string Description { get; set; }

        public static readonly ChoiceMethod GenerateUser = new ChoiceMethod { Value = 1, Description = "Сгенерировать пользователя"};
        public static readonly ChoiceMethod GeneratePassword = new ChoiceMethod { Value = 2, Description= "сгенерировать несколько паролей"};
    }

    public class SaveChoiceMethod
    {
        public int Value { get; set; }
        public string Description { get; set; }

        public static readonly SaveChoiceMethod SaveResult = new SaveChoiceMethod { Value = 1, Description = "Сохранить результат" };
        public static readonly SaveChoiceMethod GoBack = new SaveChoiceMethod { Value = 2, Description = "Вернуться назад" };
    }

}
