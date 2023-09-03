using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace SubredditStats.Shared.Utils
{
    public class CliArguments
    {
        private readonly static string[] ArgumentPrefixes = { "--", "/" };
        private readonly static string[] ArgumentControlCharacters = { "-", "/" };

        private readonly string[] _args;

        public CliArguments(string[] args)
        {
            _args = args;
            if (args.Length == 0)
            {
                //throw new EmptyArgumentsException();
            }
        }

        public T? GetArgumentValueForCallableName<T>([CallerMemberName] string? callableName = "") => GetValue<T>(callableName);
        public T? GetArgumentValueForPropertyName<T>([CallerMemberName] string? propertyName = "") => GetValue<T>(propertyName);
        public T? GetArgumentValueForMethodName<T>([CallerMemberName] string? methodName = "") => GetValue<T>(methodName);

        public int Count()
        {
            var count = 0;
            for (int i = 0; i < _args.Length; i++)
            {
                if (_args[i].StartsWithAny(ArgumentPrefixes))
                {
                    count++;
                }
            }
            return count;
        }

        public string? GetValue(string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                for (int i = 0; i < _args.Length; i++)
                {
                    var strippedAndTrimmed = _args[i].Trim();
                    foreach (var prefix in ArgumentControlCharacters)
                    {
                        strippedAndTrimmed = strippedAndTrimmed.RemoveAll(ArgumentControlCharacters);
                    }

                    if (strippedAndTrimmed.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (i + 1 >= _args.Length ||
                            _args[i + 1].StartsWithAny(ArgumentPrefixes))
                        {
                            // "value-less"/single argument
                            return "true";
                        }
                        else
                        {
                            // 'valued'/double argument 
                            if (string.IsNullOrWhiteSpace(_args[i + 1]))
                            {
                                throw new ArgumentException($"Value for \"{name}\" argument is null, empty, or whitespace.");
                            }

                            return _args[i + 1];
                        }
                    }
                }
            }

            return null;
        }

        public T? GetValue<T>(string? name)
        {
            if (GetValue(name) is string value)
            {
                return Convert.To<T>(value);
            }
            else
            {
                return default;
            }
        }

        public void PrintUsage()
        {
            Console.WriteLine("Usage: ");
        }

        [Serializable]
        private class ArgumentNotFoundException : Exception
        {
            public ArgumentNotFoundException()
            {
            }

            public ArgumentNotFoundException(string message) : base(message)
            {
            }

            public ArgumentNotFoundException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected ArgumentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        [Serializable]
        private class EmptyArgumentsException : Exception
        {
            public EmptyArgumentsException()
            {
            }

            public EmptyArgumentsException(string message) : base(message)
            {
            }

            public EmptyArgumentsException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected EmptyArgumentsException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    }
}