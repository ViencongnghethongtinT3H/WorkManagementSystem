using System.ComponentModel;
using System.Globalization;

namespace WorkManagementSystem.Shared.Extensions;

public static class StringEnumExtension
{
    public static string GetDescription<T>(this T e) where T : IConvertible
    {
        string description = null;

        if (e is Enum)
        {
            Type type = e.GetType();
            Array values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val == e.ToInt32(CultureInfo.InvariantCulture))
                {
                    var memInfo = type.GetMember(type.GetEnumName(val));
                    var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (descriptionAttributes.Length > 0)
                    {
                        description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                    }

                    break;
                }
            }
        }
        return description;
    }

    public static T GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
        return attributes.Length > 0
          ? (T)attributes[0]
          : null;
    }
    public static string ToName(this Enum value)
    {
        try
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }
        catch
        {
            return string.Empty;
        }
    }
}
