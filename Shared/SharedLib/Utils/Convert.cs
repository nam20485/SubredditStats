using System;
using System.Runtime.Serialization;

namespace SubredditStats.Shared.Utils
{
    public static class Convert
    {
        public static T To<T>(string value)
        {
            if (typeof(T) == typeof(bool))
            {
                return (T)(bool.Parse(value) as object);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(short.Parse(value) as object);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(int.Parse(value) as object);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(long.Parse(value) as object);
            }
            else if (typeof(T) == typeof(short))
            {
                return (T)(long.Parse(value) as object);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(float.Parse(value) as object);
            }
            else if (typeof(T) == typeof(double))
            {
                return (T)(float.Parse(value) as object);
            }
            else if (typeof(T) == typeof(char))
            {
                return (T)(char.Parse(value) as object);
            }
            else if (typeof(T) == typeof(Enum))
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(value as object);
            }
            else
            {
                //return default;
                throw new TypeNotSupportedException(typeof(T));
            }
        }

        [Serializable]
        public class TypeNotSupportedException : Exception
        {
            public TypeNotSupportedException(Type t)
                : this($"Conversion for type parameter {t.GetType()} not supported")
            {
            }

            public TypeNotSupportedException(string message)
                : base(message)
            {
            }

            public TypeNotSupportedException(string message, Exception innerException)
                : base(message, innerException)
            {
            }

            protected TypeNotSupportedException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}
