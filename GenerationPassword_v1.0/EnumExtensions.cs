using System.ComponentModel;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var descriptionAttribute = value.GetType()
            .GetField(value.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;

        return descriptionAttribute?.Description ?? value.ToString();
    }
}
