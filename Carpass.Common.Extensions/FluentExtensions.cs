using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public class ConditionScope<T>
    {
        Func<T, bool> _predicate;

        T _value;

        internal ConditionScope(T value, Func<T, bool> predicate)
        {
            _value = value;
            _predicate = predicate;
        }

        public void Then(Action<T> action)
        {
            if (_predicate(_value))
            {
                action(_value);
            }
        }

        public void Then(params Action<T>[] actions)
        {
            if (_predicate(_value))
            {
                actions.ForEach(x => x(_value));
            }
        }
    }

    public interface IStringNullHelper
    {
        string IfNullOrEmpty { get; }
    }

    class StringNullHelper:IStringNullHelper
    {
        string _first;
        string _second;

        internal StringNullHelper(string first,string second)
        {
            _first = first;
            _second = second;
        }

        string IStringNullHelper.IfNullOrEmpty
        {
            get { return string.IsNullOrEmpty(_first) ? _second : _first; }
        }
    }

    public class ActionScope<T>
    {
        Action<T> _action;
        T _value;

        internal ActionScope(T value, Action<T> action)
        {
            _action = action;
            _value = value;
        }

        public void When(Func<T, bool> predicate)
        {
            if (predicate(_value)) _action(_value);
        }
    }

    public static class FluentExtensions
    {
        public static bool IsBetween<T>(this T value,  T minimum, T maximum) where T:IComparable
        {
            return value.CompareTo(minimum) >= 0 && value.CompareTo(maximum) <= 0;
        }

        public static ConditionScope<T> If<T>(this T value, Func<T, bool> predicate)
        {
            return new ConditionScope<T>(value,predicate);
        }

        public static ActionScope<T> Do<T>(this T value, Action<T> action)
        {
            return new ActionScope<T>(value, action);
        }

        public static IStringNullHelper Or(this string first, string optional)
        {
            return new StringNullHelper(first, optional);
        }
    }
}
