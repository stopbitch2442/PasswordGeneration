using GenerationPassword_v1._0;
using GenerationPassword_v1._0.Model;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

public class Program
{
    public static readonly List<string> _outputStrings = new();
    public static void Main()
    {
        var outputStrings = new List<string>();
        while (true)
        {
            Console.WriteLine($"Выберите функцию:");
            PrintEnumValue<ChoiceMethod>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> typesWithMyAttribute = GetTypesWithMyAttribute(assembly);

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                continue;
            }

            Type type = null;

            if (choice == ChoiceMethod.GenerateUser.Value)
            {
                type = typesWithMyAttribute.FirstOrDefault(t => t.Name == "GenerateUserCommand");
            }
            else if (choice == ChoiceMethod.GeneratePassword.Value)
            {
                type = typesWithMyAttribute.FirstOrDefault(t => t.Name == "GeneratePasswordCommand");
            }
            else
            {
                Console.WriteLine($"Функция с номером {choice} не найдена. Попробуйте еще раз.");
                continue;
            }

            if (type != null)
            {
                var typeInstance = Activator.CreateInstance(type, outputStrings);
                if (typeInstance is ICommand)
                {
                    ((ICommand)typeInstance).Execute();
                }
                else
                {
                    throw new InvalidOperationException("Тип не реализует интерфейс ICommand.");
                }
            }
            else
            {
                Console.WriteLine($"Класс команды для функции {((ChoiceMethod)choice).ToString()} не найден.");
            }
        }
    }
}
