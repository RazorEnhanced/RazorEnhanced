
using System;
using System.Reflection;
using Assistant;

namespace RazorEnhanced.Helpers
{
    public static class Reflection
    {
        public static T GetTypePropertyValue<T>(Type type, string property, object obj = null,
            BindingFlags bindingFlags = BindingFlags.Default)
        {
            PropertyInfo propertyInfo = type.GetProperty(property);

            T val = (T)propertyInfo?.GetValue(obj, null);

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (val == null)
            {
                return default;
            }

            return val;
        }

        public static T GetTypePropertyValue<T>(string type, string property, object obj,
            BindingFlags bindingFlags = BindingFlags.Default, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = ClassicUOClient.CUOAssembly;
            }

            Type t = assembly?.GetType(type);

            return t == null ? default : GetTypePropertyValue<T>(t, property, obj, bindingFlags);
        }

        public static T GetTypeFieldValue<T>(Type type, string property, object obj = null,
            BindingFlags bindingFlags = BindingFlags.Default)
        {
            FieldInfo fieldInfo = type.GetField(property);

            T val = (T)fieldInfo?.GetValue(obj);

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (val == null)
            {
                return default;
            }

            return val;
        }

        public static T GetTypeFieldValue<T>(string type, string property, object obj,
            BindingFlags bindingFlags = BindingFlags.Default, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = ClassicUOClient.CUOAssembly;
            }

            Type t = assembly?.GetType(type);

            return t == null ? default : GetTypeFieldValue<T>(t, property, obj, bindingFlags);
        }

        public static MethodInfo GetTypeMethod(string type, string methodName, object obj = null,
            Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = ClassicUOClient.CUOAssembly;
            }

            Type t = assembly?.GetType(type);

            return GetTypeMethod(t, methodName, obj);
        }

        public static MethodInfo GetTypeMethod(Type type, string methodName, object obj = null)
        {
            return type.GetMethod(methodName);
        }

        public static object CreateInstanceOfType(string type, Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = ClassicUOClient.CUOAssembly;
            }

            Type t = assembly?.GetType(type);

            return Activator.CreateInstance(t);
        }

        public static object CreateInstanceOfType(string type, Assembly assembly = null, params object[] args)
        {
            if (assembly == null)
            {
                assembly = ClassicUOClient.CUOAssembly;
            }

            Type t = assembly?.GetType(type);

            return t == null ? null : Activator.CreateInstance(t, args);
        }

        public static T CreateInstanceOfType<T>(string type, Assembly assembly = null, params object[] args)
        {
            if (assembly == null)
            {
                assembly = ClassicUOClient.CUOAssembly;
            }

            Type t = assembly?.GetType(type);

            if (t == null)
            {
                return default;
            }

            return (T)Activator.CreateInstance(typeof(T), Activator.CreateInstance(t, args));
        }

        /*
         * https://stackoverflow.com/questions/6961781/reflecting-a-private-field-from-a-base-class
         */
        public static T GetTypeFieldValueRecurse<T>(Type t, string name, object obj)
        {
            FieldInfo fi;

            while ((fi = t.GetField(name,
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)) == null &&
                    (t = t.BaseType) != null)
            {
            }

            if (fi != null)
            {
                return (T)fi.GetValue(obj);
            }

            return default;
        }

        public static void SetTypeFieldValueRecurse(Type t, string name, object obj, object val)
        {
            FieldInfo fi;

            while ((fi = t.GetField(name,
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)) == null &&
                    (t = t.BaseType) != null)
            {
            }

            if (fi != null)
            {
                fi.SetValue(obj, val);
            }
        }
    }
}