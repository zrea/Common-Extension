using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;
using System.Runtime.Caching;
using System.Globalization;

namespace System
{
    public static class Extensions
    {
        public static StringBuilder AppendLine(this StringBuilder builder,
            string str, params object[] parameters)
        {
            return builder.AppendLine(string.Format(str, parameters));
        }

        public static string GetRootMessage(this Exception exception)
        {
            return exception.GetBaseException().Message;
        }

        public static Exception GetRoot(this Exception exception)
        {
            if (exception.InnerException == null)
                return exception;
            else return exception.InnerException.GetRoot();
        }

        public static DateTime ToDateTime(this string value, string format, IFormatProvider provider)
        {
            return DateTime.ParseExact(value, format, provider);
        }

        public static DateTime? ToNullDateTime(this string value, string format,
            IFormatProvider provider, DateTimeStyles style = DateTimeStyles.None)
        {
            DateTime result;
            return DateTime.TryParseExact(value, format, provider, style, out result) ? result : (DateTime?)null;
        }

        public static DateTime? ToNullDateTime(this string value)
        {
            DateTime result;
            return DateTime.TryParse(value, out result) ? result : (DateTime?)null;
        }

        public static QueryHelper<TQuery, T> Filter<TQuery, T>
            (this TQuery query, Expression<Func<T, bool>> expr)
            where TQuery : IQueryable<T>
        {
            return new QueryHelper<TQuery, T>(query, expr);
        }

        public static Func<T, TResult> Memorize<T, TResult>(this Func<T, TResult> func)
        {
            Dictionary<T, TResult> dictionary = new Dictionary<T, TResult>();

            Func<T, TResult> f = input =>
                {
                    TResult result;

                    if (dictionary.TryGetValue(input, out result))
                    {
                        return result;
                    }
                    else
                    {
                        result = func(input);
                        dictionary.Add(input, result);
                        return result;
                    }
                };

            return f;
        }

        public static bool NotNullAnd<T>(this T item, Func<T, bool> predicate)
        {
            if (item == null) return false;
            else return predicate(item);
        }

        public static U ExpectFor<T, U>(this T item, Func<T, U> selector, U defaultValue)
        {
            if (item != null)
                return selector(item);
            else
                return defaultValue;
        }

        public static int ToIntOrDefaultInt(this string value, int result = 0)
        {
            int.TryParse(value, out result);

            return result;
        }

        public static int? ToNullInt(this string value)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        public static short ToShortOrDefaultShort(this string value, short result = (short) 0)
        {
            short.TryParse(value, out result);

            return result;
        }

        public static bool ToBoolOrDefaultBool(this string value, bool result = false)
        {
            bool.TryParse(value, out result);

            return result;
        }

        public static bool? ToNullBool(this string value)
        {
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        public static float ToFloatOrDefaultFloat(this string value, float result = 0)
        {
            float.TryParse(value, out result);

            return result;
        }

        public static long ToLongOrDefaultLong(this string value, long result = 0)
        {
            long.TryParse(value, out result);

            return result;
        }

        public static decimal ToDecimalOrDefaultDecimal(this string value, decimal result = 0)
        {
            decimal.TryParse(value, out result);

            return result;
        }

        public static decimal? ToNullDecimal(this string value)
        {
            decimal result;
            if (decimal.TryParse(value, out result))
            {
                return result;
            }
            return null;
        }

        public static bool ExpectEquals(this object obj, object compare)
        {
            if (obj == null)
                return compare == null;
            else return obj.Equals(compare);
        }

        public static U ExpectFor<T, U>(this T item, Func<T, U> selector)
        {
            if (item != null)
                return selector(item);
            else
                return default(U);
        }

        public static IEnumerable<T> Where<T>(this IEnumerable list, Func<T, bool> predicate)
        {
            foreach (T item in list)
            {
                if (predicate(item)) yield return item;
            }
        }

        public static IEnumerable<T> AsEnumerable<T>(this IEnumerable list)
        {
            foreach (T item in list)
            {
                yield return item;
            }
        }

        public static T FirstOrDefault<T>(this IEnumerable list) where T : class
        {
            var enumerate = list.GetEnumerator();

            if (enumerate.MoveNext())
            {
                return enumerate.Current as T;
            }
            else return default(T);
        }

        public static T FirstOrDefault<T>(this IEnumerable list, Func<T, bool> predicate) where T : class
        {
            var enumerate = list.GetEnumerator();

            while (enumerate.MoveNext())
            {
                var item = enumerate.Current as T;

                if (item != null && predicate(item))
                    return item;
            }

            return default(T);
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }

        public static T First<T>(this IEnumerable<T> list, Exception e)
        {
            try
            {
                return list.First();
            }
            catch
            {
                throw e;
            }
        }

        public static T First<T>(this IEnumerable<T> list, Func<T, bool> predicate, Exception e)
        {
            try
            {
                return list.First(predicate);
            }
            catch
            {
                throw e;
            }
        }

        public static T Single<T>(this IEnumerable<T> list, Exception e)
        {
            try
            {
                return list.Single();
            }
            catch
            {
                throw e;
            }
        }

        public static T Single<T>(this IEnumerable<T> list, Func<T, bool> predicate, Exception e)
        {
            try
            {
                return list.Single(predicate);
            }
            catch
            {
                throw e;
            }
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> list, Exception e)
        {
            try
            {
                return list.SingleOrDefault();
            }
            catch
            {
                throw e;
            }
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> list, Func<T, bool> predicate, Exception e)
        {
            try
            {
                return list.SingleOrDefault(predicate);
            }
            catch
            {
                throw e;
            }
        }

        public static T WithConfiguration<T>(this T item, params Action<T>[] actions)
        {
            actions.ForEach(action => action(item));

            return item;
        }

        public static List<T> ToList<T>(this IQueryable<T> query,
            Expression<Func<T, bool>> condition) where T : class
        {
            return query.Where(condition).ToList();
        }

        public static string ToDefaultString<T>(this Nullable<T> item, string defaultMessage)
            where T : struct
        {
            return item.HasValue ? item.Value.ToString() : defaultMessage;
        }

        public static string ToStringYesNo(this bool value)
        {
            return value ? "Yes" : "No";
        }

        public static string ToStringYesNo(this bool? value, string message = "-")
        {
            return value.HasValue ? value.Value ? "Yes" : "No" : message;
        }

        public static string ToDefaultFormat(this DateTime? item, string defaultMessage
            , string format = "dd/MM/yyyy")
        {
            return item.HasValue ? item.Value.ToString(format) : defaultMessage;
        }

        public static string ToDefaultFormat(this DateTime item, string format = "dd/MM/yyyy")
        {
            return item.ToString(format);
        }

        public static string ToFormatedString(this int attr)
        {
            return attr.ToString("#,##0");
        }

        public static string ToFormatedString(this double attr)
        {
            return attr.ToString("#,##0.00");
        }

        public static string ToFormatString(this decimal attr, string format = "#,##0.00")
        {
            return attr.ToString(format);
        }

        public static string ToFormatString(this decimal? value, string format = "#,##0.00", string emptyMessage = "")
        {
            return value.HasValue ? value.Value.ToFormatString(format) : emptyMessage;
        }

        public static int ToInt(this long source)
        {
            return Convert.ToInt32(source);
        }
    }

    public class QueryHelper<TQuery, T>
        where TQuery : IQueryable<T>
    {
        TQuery _query;
        Expression<Func<T, bool>> _expr;

        internal QueryHelper(TQuery query, Expression<Func<T, bool>> expr)
        {
            _query = query;
            _expr = expr;
        }

        public TQuery When(bool fact)
        {
            return fact ? (TQuery)_query.Where(_expr) : _query;
        }
    }

    class MemoryCache<T> : MemoryCache, IMemoryCache<T>
    {
        static MemoryCache<T> _instant;

        public TimeSpan Interval { get; set; }
        public string RegionName { get; set; }

        private MemoryCache()
            : base(typeof(T).Name)
        {
            Interval = new TimeSpan(24, 0, 0);
        }

        public static MemoryCache<T> Instant 
        { 
            get 
            { 
                return _instant ?? (_instant = new MemoryCache<T>()); 
            } 
        }

        public T AddOrGetExisting(string key, T value)
        {
            return (T)this.AddOrGetExisting(new CacheItem(key, value),
                new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now.Add(Interval),
                    SlidingExpiration = Interval
                }).Value;
        }

        public bool Add(string key, T value)
        {
            return base.Add(key, value, new CacheItemPolicy
            {
                SlidingExpiration = Interval
            }, RegionName);
        }

        public T this[string key]
        {
            get
            {
                return (T)base[key];
            }
            set
            {
                base[key] = value;
            }
        }

        public bool TryGetValue(string key, out T value)
        {
            try
            {
                value = this[key];
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }
    }
}
