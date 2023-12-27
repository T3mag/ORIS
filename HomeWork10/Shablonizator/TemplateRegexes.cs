using System.Text.RegularExpressions;

namespace Shablonizator
{
    public static partial class TemplateRegexes
    {
        public static readonly Regex PropertyTemplateRegex = MyPropertyTemplateRegex(); 
        public static readonly Regex IfTemplateRegex = MyIfTemplateRegex();
        public static readonly Regex ThenTemplateRegex = MyThenTemplateRegex();
        public static readonly Regex ElseTemplateRegex = MyElseTemplateRegex();
        public static readonly Regex FullIfTemplateRegex = MyFullIfTemplateRegex();
        public static readonly Regex FullForTemplateRegex = MyFullForTemplateRegex();
        public static readonly Regex ForTemplateRegex = MyForTemplateRegex();
        public static readonly Regex ForPropertyTemplate = MyForPropertyTemplate();
        public static readonly  Dictionary<string, Func<IComparable?, IComparable?, bool>> BoolExpressionDictionary = new() {
            {"<=", (x,y) => x?.CompareTo(y) <= 0},
            {">=", (x,y) => x?.CompareTo(y) >= 0},
            {"==", (x,y) => x?.CompareTo(y) == 0},
            { ">", (x,y) => x?.CompareTo(y) > 0},
            {"<", (x,y) => x?.CompareTo(y) < 0},
        };
        [GeneratedRegex("@{\\w*}")]
        private static partial Regex MyPropertyTemplateRegex();
        [GeneratedRegex(@"@{if\(.*\)}")]
        private static partial Regex MyIfTemplateRegex();
        [GeneratedRegex("@then{.\\w*}")]
        private static partial Regex MyThenTemplateRegex();
        [GeneratedRegex("@else{\\w*}")]
        private static partial Regex MyElseTemplateRegex();
        [GeneratedRegex("@{if(.*)}")]
        private static partial Regex MyFullIfTemplateRegex();
        [GeneratedRegex("@for(.*){.*}")]
        private static partial Regex MyFullForTemplateRegex();
        [GeneratedRegex(@"@for\(.*\)")]
        private static partial Regex MyForTemplateRegex();
        [GeneratedRegex(@"@{\w*.\w*}")]
        private static partial Regex MyForPropertyTemplate();
    }
}