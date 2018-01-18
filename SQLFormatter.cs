using System;
using System.Text;
using PM = PoorMansTSqlFormatterLib;
using System.IO;
using CommandLine;

namespace SQLFormatter
{
    class SQLFormatter
    {
        static void Main(string[] args)
        {
            string SQL = string.Empty;

            //var result = CommandLine.Parser.Default.ParseArguments<Options>(args);
            Options CommandArgs = new Options();

            bool isError = false;
            Parser.Default.ParseArguments<Options>(args)
                    .WithNotParsed<Options>(errorObj =>
                    {
                        isError = true;
                    })
                    .WithParsed<Options>(optionObj =>
                    {
                        CommandArgs = optionObj;
                    });

            if (isError)
            {
                Console.WriteLine("Error: Please check SQL formatter settings.");
                return;
            }

            //Read the SQL in from the standard input
            using (Stream InStream = Console.OpenStandardInput())
            using (StreamReader StrReader = new StreamReader(InStream))
            {
                while (StrReader.Peek() != 0x04)
                {
                    SQL += Convert.ToChar(StrReader.Read());
                }
            }

            SQL = Encoding.UTF8.GetString(Convert.FromBase64String(SQL));

            using (Stream OutStream = Console.OpenStandardOutput())
            using (StreamWriter StrWriter = new StreamWriter(OutStream))
            {
                PM.Formatters.TSqlStandardFormatterOptions FormatOptions =
                    new PM.Formatters.TSqlStandardFormatterOptions();

                FormatOptions.SpacesPerTab = CommandArgs.SpacesPerTab;
                FormatOptions.IndentString = CommandArgs.IndentString;
                FormatOptions.TrailingCommas = CommandArgs.TrailingCommas;
                FormatOptions.SpaceAfterExpandedComma = CommandArgs.SpaceAfterExpandedComma;
                FormatOptions.NewStatementLineBreaks = CommandArgs.NewStatementLineBreaks;
                FormatOptions.NewClauseLineBreaks = CommandArgs.NewClauseLineBreaks;
                FormatOptions.MaxLineWidth = CommandArgs.MaxLineWidth;
                FormatOptions.ExpandCommaLists = CommandArgs.ExpandCommaLists;
                FormatOptions.ExpandBooleanExpressions = CommandArgs.ExpandBooleanExpressions;
                FormatOptions.ExpandCaseStatements = CommandArgs.ExpandCaseStatements;
                FormatOptions.ExpandBetweenConditions = CommandArgs.ExpandBetweenConditions;
                FormatOptions.BreakJoinOnSections = CommandArgs.BreakJoinOnSections;
                FormatOptions.UppercaseKeywords = CommandArgs.UppercaseKeywords;
                FormatOptions.HTMLColoring = CommandArgs.HTMLColoring;
                FormatOptions.KeywordStandardization = CommandArgs.KeywordStandardization;
                FormatOptions.ExpandInLists = CommandArgs.ExpandInLists;
                
                PM.Formatters.TSqlStandardFormatter Formatter =
                    new PM.Formatters.TSqlStandardFormatter(FormatOptions);

                PM.SqlFormattingManager Manager = new PM.SqlFormattingManager(Formatter);

                //Write out the formatted SQL
                StrWriter.Write(
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(Manager.Format(SQL))));
            }
        }
    }

    class Options 
    {
        /*
         * 
         * TrailingCommas
         * SpaceAfterComma 
         * IndentString
         * SpacesPerTab
         */

        [Option("trailingcommas", Required = false)]
        public bool TrailingCommas { get; set; }

        [Option("spaceaftercomma", Required = false)]
        public bool SpaceAfterExpandedComma { get; set; }

        [Option("indentstring", Required = false)]
        public string IndentString { get; set; }

        [Option("spacespertab", Required = false)]
        public int SpacesPerTab { get; set; }

        [Option("newstatementlinebreaks", Required = false)]
        public int NewStatementLineBreaks { get; set; }

        [Option("newclauselinebreaks", Required = false)]
        public int NewClauseLineBreaks { get; set; }

        [Option("maxlinewidth", Required = false)]
        public int MaxLineWidth { get; set; }

        [Option("expandcommalists", Required = false)]
        public bool ExpandCommaLists { get; set; }

        [Option("expandbooleanexpressions", Required = false)]
        public bool ExpandBooleanExpressions { get; set; }

        [Option("expandcasestatements", Required = false)]
        public bool ExpandCaseStatements { get; set; }

        [Option("expandbetweenconditions", Required = false)]
        public bool ExpandBetweenConditions { get; set; }
        
        [Option("breakjoinonsections", Required = false)]
        public bool BreakJoinOnSections { get; set; }
        
        [Option("uppercasekeywords", Required = false)]
        public bool UppercaseKeywords { get; set; }
        
        [Option("htmlcoloring", Required = false)]
        public bool HTMLColoring { get; set; }
        
        [Option("keywordstandardization", Required = false)]
        public bool KeywordStandardization { get; set; }

        [Option("expandinlists", Required = false)]
        public bool ExpandInLists { get; set; }
    }
    
}
