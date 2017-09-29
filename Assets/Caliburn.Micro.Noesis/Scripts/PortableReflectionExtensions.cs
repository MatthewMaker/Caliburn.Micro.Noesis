#if UNITY_5_5_OR_NEWER
#define NOESIS
#endif
#if !NOESIS || (ENABLE_MONO_BLEEDING_EDGE_EDITOR || ENABLE_MONO_BLEEDING_EDGE_STANDALONE)
#define ENABLE_TASKS
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Caliburn.Micro
{
    /// <summary>
    /// A collection of extension methods to help with differing reflection between the portable library and SL5
    /// </summary>
    internal static class PortableReflectionExtensions {

        public static bool IsAssignableFrom(this Type t, Type c) {
            return t.GetTypeInfo().IsAssignableFrom(c.GetTypeInfo());
        }

        public static Type[] GetGenericArguments(this Type t) {
#if NOESIS
            var typeInfo = t.GetTypeInfo();

            if (typeInfo.IsGenericType && !typeInfo.IsGenericTypeDefinition)
            {
                return typeInfo.GetGenericArguments();
            }
            else
            {
                return Type.EmptyTypes;
            }
#else
            return t.GetTypeInfo().GenericTypeArguments;
#endif
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type t) {
#if NOESIS
            return t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
#else
            return t.GetRuntimeProperties();
#endif
        }

        public static IEnumerable<ConstructorInfo> GetConstructors(this Type t)
        {
#if NOESIS
            return t.GetTypeInfo().GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
#else
            return t.GetTypeInfo().DeclaredConstructors;
#endif
        }

        public static IEnumerable<Type> GetInterfaces(this Type t)
        {
#if NOESIS
            return t.GetTypeInfo().GetInterfaces();
#else
            return t.GetTypeInfo().ImplementedInterfaces;
#endif
        }

        public static IEnumerable<Type> GetTypes(this Assembly a)
        {
#if NOESIS
            return a.GetDefinedTypes();
#else
            return a.DefinedTypes.Select(t => t.AsType());
#endif
        }

#if NOESIS
        public static IEnumerable<Type> GetDefinedTypes(this Assembly a)
        {
            Type[] types = a.GetTypes();

            Type[] typeinfos = new Type[types.Length];

            for (int i = 0; i < types.Length; i++)
            {

                Type typeinfo = types[i].GetTypeInfo();

                if (typeinfo == null)
                    throw new NotSupportedException();

                typeinfos[i] = typeinfo;
            }

            return typeinfos;
        }
#endif

        public static bool IsAbstract(this Type t) {
            return t.GetTypeInfo().IsAbstract;
        }

        public static bool IsInterface(this Type t) {
            return t.GetTypeInfo().IsInterface;
        }

        public static bool IsGenericType(this Type t) {
            return t.GetTypeInfo().IsGenericType;
        }

        public static MethodInfo GetMethod(this Type t, string name, Type[] parameters)
        {
#if NOESIS
            return t.GetMethod(name, parameters);
#else
            return t.GetRuntimeMethod(name, parameters);
#endif
        }
    }
}
