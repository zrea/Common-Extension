﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
/// Test Clone 2
    public static class Enums
    {
/// Test 21
        public static T Parse<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }

        public static T? NullableParse<T>(string name) where T: struct
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return Parse<T>(name);
        }

        public static T? GetNullableEnum<T>(int value) where T: struct
        {
            T result = (T)(object)value;

            if (Enum.IsDefined(typeof(T), result))
            {
                return result;
            }

            return null;
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        static Random random = new Random();

        public static IEnumerable<T> Range<T>(int start, int stop) where T : class
        {
            return Enumerable.Range(start, stop - start + 1).Select(x => x as T);
        }

        public static T Random<T>(this T[] items)
        {
            return items[random.Next(items.Length)];
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            return items.OrderBy(x => random.Next());
        }
    }
}
