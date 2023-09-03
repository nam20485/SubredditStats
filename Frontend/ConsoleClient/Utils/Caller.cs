using System;
using System.Runtime.CompilerServices;

namespace SubredditStats.Frontend.ConsoleClient.Utils
{
    public static class Caller
    {
        public static string GetMemberName([CallerMemberName] string callerMemberName = "") => callerMemberName;
        public static string GetFilePath([CallerFilePath] string callerFilePath = "") => callerFilePath;
        public static int GetLineNumber([CallerLineNumber] int callerLineNumber = -1) => callerLineNumber;

        public static string MemberNameAndMessage(string format,
                                                  [CallerMemberName] string callerMemberName = "",
                                                  params object[] formatArgs)
        {
            return $"{callerMemberName}() - {string.Format(format, formatArgs)}";
        }

        public static string MemberNameEnter([CallerMemberName] string callerMemberName = "")
        {
            return $"{callerMemberName}() - Enter";
        }

        public static string MemberNameExit([CallerMemberName] string callerMemberName = "")
        {
            return $"{callerMemberName}() - Exit";
        }

        //public static string MemberNameDegree2([CallerMemberName] string callerMemberName = "")
        //{
        //    // inception!
        //    return GetMemberName();
        //}

        public static (string Name, string path, int Line) MemberNameLocationValues([CallerMemberName] string callerMemberName = "",
                                                                     [CallerFilePath] string callerFilePath = "",
                                                                     [CallerLineNumber] int callerLineNumber = -1)
        {
            return (callerMemberName, callerFilePath, callerLineNumber);
        }

        public static string MemberNameLocation([CallerMemberName] string callerMemberName = "",
                                                [CallerFilePath] string callerFilePath = "",
                                                [CallerLineNumber] int callerLineNumber = -1,
                                                bool memberName = true,
                                                bool filePath = true,
                                                bool lineNumber = true)
        {
            return $"{(filePath ? callerFilePath : "")}: {(lineNumber ? callerLineNumber : "")} - {(memberName ? callerMemberName + "()" : "")}";
        }

        public static string MemberNameAndExpression<T>(T value,
                                                     [CallerMemberName] string callerMemberName = "",
                                                     [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            return $"{callerMemberName}() - \'{valueExpression}\' => \'{value}\'";
        }

        public static string MemberNameCalledWith<T>(T value,
                                                     [CallerMemberName] string callerMemberName = "",
                                                     [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            return $"{callerMemberName}({valueExpression} = \'{value}\')";
        }

        public static string MemberNameCalledWithEnter<T>(T value,
                                                         [CallerMemberName] string callerMemberName = "",
                                                         [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            return $"{callerMemberName}({valueExpression} = \'{value}\') - Enter";
        }

        public static string MemberNameCalledWithExit<T>(T value,
                                                        [CallerMemberName] string callerMemberName = "",
                                                        [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            return $"{callerMemberName}({valueExpression} = \'{value}\') - Exit";
        }

        public static string MemberNameCalledWithAndMessage<T>(T value,
                                                               string format = "",
                                                               [CallerMemberName] string callerMemberName = "",
                                                               [CallerArgumentExpression(nameof(value))] string valueExpression = "",
                                                               params object[] formatArgs)
        {
            return $"{callerMemberName}({valueExpression} = \'{value}\') - {string.Format(format, formatArgs)}";
        }

        public static string MemberNameCalledWithAndExpression<T, U>(T calledWith,
                                                                     U expression,
                                                                     [CallerMemberName] string callerMemberName = "",
                                                                     [CallerArgumentExpression(nameof(calledWith))] string calledWithExpression = "",
                                                                     [CallerArgumentExpression(nameof(expression))] string expressionExpression = "")

        {
            return $"{callerMemberName}({calledWithExpression} = '{calledWith}') -  \'{expressionExpression}\' => \'{expression}\'";
        }

        public static string ArgumentExpression<T>(T value,
                                                   [CallerArgumentExpression(nameof(value))] string valueExpression = "")
        {
            return $"{valueExpression} => {value}";
        }

        public static string ArgumentExpression<T, U>(T tValue, U uValue,
                                                      [CallerArgumentExpression(nameof(tValue))] string tValueExpression = "",
                                                      [CallerArgumentExpression(nameof(uValue))] string uValueExpression = "",
                                                      string separator = "\n")  // TODO: does this work?
        {
            return $"\'{tValueExpression}\' => \'{tValue}\'{separator}\'{uValueExpression}\' => \'{uValue}\'";
        }

        public static void ActionExpression(Action value,
                                            [CallerArgumentExpression(nameof(value))] string valueExpression = "",
                                            [CallerMemberName] string callerMemberName = "")
        {
            Console.Out.Write($"{callerMemberName}(...) - calling {valueExpression}... ");
            value();
            Console.Out.WriteLine("returned");

        }
    }
}
