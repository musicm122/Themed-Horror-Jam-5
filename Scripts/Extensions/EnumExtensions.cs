using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ThemedHorrorJam5.Scripts.GDUtils
{
    public static class EnumExtensions
    {
        public static List<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(System.Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = System.Enum.GetValues(enumType);

            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)System.Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
        
        public static int Count<T>(this System.Enum val) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return System.Enum.GetNames(typeof(T)).Length;
        }

        public static string GetDescription(this System.Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }
    }
}