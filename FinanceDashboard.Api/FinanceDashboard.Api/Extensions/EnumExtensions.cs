using AutoMapper;

namespace FinanceDashboard.Api.Extensions
{
    // Extension for enum conversion
    public static class EnumExtensions
    {
        public static string ToDisplayString(this Enum value)
        {
            return value.ToString();
        }
    }


    public class EnumToStringConverter<TEnum> : IValueConverter<TEnum, string> where TEnum : Enum
    {
        public string Convert(TEnum source, ResolutionContext context)
        {
            return source.ToString();
        }
    }

    public class StringToEnumConverter<TEnum> : IValueConverter<string, TEnum> where TEnum : Enum
    {
        public TEnum Convert(string source, ResolutionContext context)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), source);
        }
    }



}
