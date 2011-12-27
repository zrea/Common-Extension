using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace System
{
    public static class ServiceLocator
    {
        public static IMemoryCache<T> GetCache<T>()
        {
            return MemoryCache<T>.Instant;
        }

        #region Registered Type
        static Dictionary<Type, Func<object>> _locators = new Dictionary<Type, Func<object>>();

        public static void Register<T, TImplement>() where TImplement : T
        {
            #region Self Implement
            if (!_locators.Any(x => x.Key == typeof(T)))
                _locators.Add(typeof(T), GetCreateMethod(typeof(TImplement)));
            #endregion
        }

        static Func<object> GetCreateMethod(Type type)
        {
            var constructor = type.GetConstructors().Concat(type.GetConstructors(BindingFlags.NonPublic| BindingFlags.Instance)).OrderBy(x => x.GetParameters().Count()).FirstOrDefault();
            if (constructor == null)
            {
                if (type.IsPublic)
                    return () => Activator.CreateInstance(type, BindingFlags.Instance, null,
                        new object[] { }, CultureInfo.CurrentCulture);
                else
                {
                    return () => Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null,
                        new object[] { }, CultureInfo.CurrentCulture);
                }
            }
            else if (!constructor.GetParameters().Any())
            {
                if (constructor.IsPublic)
                    return () => Activator.CreateInstance(type);
                else
                    return () => constructor.Invoke(null);
            }
            else
            {
                //var parameters = constructor.GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray();
                if (constructor.IsPublic)
                {
                    return () => constructor.Invoke(constructor.GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray());
                }
                else
                    return () => constructor.Invoke(constructor.GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray());
                    //return () => Activator.CreateInstance(type, BindingFlags.NonPublic | BindingFlags.Instance, null,
                    //    constructor.GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray(), CultureInfo.CurrentCulture);
            }
        }

        public static void Register<T>(Func<T> func)
        {
            #region Self Implement
            if (!_locators.Any(x => x.Key == typeof(T)))
                _locators.Add(typeof(T), () => func());
            #endregion
        }

        public static RegisterConfiguration<T> Register<T>()
        {
            return new RegisterConfiguration<T>();
        }

        static internal object GetInstance(Type type)
        {
            Func<object> func;
            if (_locators.TryGetValue(type, out func))
            {
                return func();
            }
            else throw new ArgumentOutOfRangeException("Type", type.FullName + " is not registered.");
        }

        public static T GetInstance<T>()
        {
            Func<object> func;
            if (_locators.TryGetValue(typeof(T), out func))
            {
                return (T)func();
            }
            else throw new ArgumentOutOfRangeException("Type", typeof(T).FullName + " is not registered.");
        }
        #endregion

        public static void RegisterModule<T>()
        {
            RegisterFromModule(typeof(T).Assembly);
        }

        public static void RegisterFromModule(Assembly assembly)
        {
            assembly.GetTypes().Select(x =>
                new
                {
                    DeriveType = x,
                    Attribute = x.GetCustomAttributes(true).FirstOrDefault(y =>
                        y.GetType() == typeof(InstanceAttribute))
                }).Where(x => x.Attribute != null).Select(x =>
                    new
                    {
                        x.DeriveType,
                        InstanceOfTypes = ((InstanceAttribute)x.Attribute).InstanceOfTypes
                    }).ForEach(d => d.InstanceOfTypes.ForEach(t =>
                                                                  {
                                                                      if (!_locators.Any(x => x.Key == t))
                                                                          _locators.Add(t, GetCreateMethod(d.DeriveType));
                                                                  }));
        }
    }

    public class RegisterConfiguration<T>
    {
        internal RegisterConfiguration()
        {
        }

        public void With<TImplement>() where TImplement : T
        {
            ServiceLocator.Register<T, TImplement>();
        }

        public void With(Func<T> func)
        {
            ServiceLocator.Register<T>(func);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class InstanceAttribute : Attribute
    {
        public InstanceAttribute(params Type[] instanceOfTypes)
        {
            InstanceOfTypes = instanceOfTypes;
        }

        internal Type[] InstanceOfTypes
        {
            get;
            set;
        }
    }
}
