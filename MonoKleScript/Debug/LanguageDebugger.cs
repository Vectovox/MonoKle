namespace MonoKleScript.Debug
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;
    using MonoKleScript.Grammar;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class used for debugging the MonoKleScript language.
    /// </summary>
    public static class LanguageDebugger
    {
        /// <summary>
        /// Prints the tokens identified in the given source.
        /// </summary>
        /// <param name="source"></param>
        public static void PrintLexerTokens(string source)
        {
            AntlrInputStream stream = new AntlrInputStream(source);
            MonoKleScriptLexer lexer = new MonoKleScriptLexer(stream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);

            Console.WriteLine("\n## LEXER TOKENS ##");
            foreach (var v in lexer.GetAllTokens())
            {
                Console.WriteLine("Line: " + v.Line + " | Text: " + v.Text + " | Rule: " + lexer.TokenNames[v.Type]);
            }
        }

        public static void PrintParserTree(string source)
        {
            AntlrInputStream stream = new AntlrInputStream(source);
            MonoKleScriptLexer lexer = new MonoKleScriptLexer(stream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            MonoKleScriptParser parser = new MonoKleScriptParser(tokenStream);
            MonoKleScriptParser.ScriptContext context = parser.script();
            
            ParseTreeWalker walker = new ParseTreeWalker();
            ParserTreePrinterListener listener = new ParserTreePrinterListener(parser.RuleNames);

            Console.WriteLine("\n## PARSER TREE ##");
            walker.Walk(listener, context);
        }

        public static void PrintParserTokens(string source)
        {
            AntlrInputStream stream = new AntlrInputStream(source);
            MonoKleScriptLexer lexer = new MonoKleScriptLexer(stream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            MonoKleScriptParser parser = new MonoKleScriptParser(tokenStream);
            MonoKleScriptParser.ScriptContext context = parser.script();

            ParseTreeWalker walker = new ParseTreeWalker();
            ParserTokenPrinterListener listener = new ParserTokenPrinterListener(parser.RuleNames);

            Console.WriteLine("\n## PARSER TOKENS ##");
            walker.Walk(listener, context);
        }

        private class ParserTokenPrinterListener : MonoKleScriptBaseListener
        {
            private string[] ruleNames;

            public ParserTokenPrinterListener(string[] ruleNames)
            {
                this.ruleNames = ruleNames;
            }

            public override void EnterEveryRule(ParserRuleContext context)
            {
                StringBuilder sb = new StringBuilder();

                if (context.children != null)
                {
                    foreach (var v in context.children)
                    {
                        if (v.ChildCount == 0)
                        {
                            sb.Append(v.ToString());
                            sb.Append(' ');
                        }
                    }
                }

                Console.Write(sb);
            }
        }

        private class ParserTreePrinterListener : MonoKleScriptBaseListener
        {
            private string[] ruleNames;

            public ParserTreePrinterListener(string[] ruleNames)
            {
                this.ruleNames = ruleNames;
            }

            public override void EnterEveryRule(ParserRuleContext context)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < context.Depth(); i++)
                {
                    sb.Append("  ");
                }

                sb.Append(context.Depth());
                sb.Append(") [");
                sb.Append(ruleNames[context.GetRuleIndex()]);
                sb.Append("] -> ");

                if (context.children != null)
                {
                    foreach (var v in context.children)
                    {
                        if (v.ChildCount == 0)
                        {
                            sb.Append(v.ToString());
                        }
                    }
                }

                Console.WriteLine(sb);
            }
        }

        //private class SubRulePrinterListener : MonoKleScriptBaseListener
        //{
        //    private string[] ruleNames;

        //    public SubRulePrinterListener(string[] ruleNames)
        //    {
        //        this.ruleNames = ruleNames;
        //    }

        //    public override void EnterEveryRule(ParserRuleContext context)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append(context.Depth());
        //        sb.Append(") [");
        //        sb.Append(ruleNames[context.GetRuleIndex()]);
        //        sb.Append("] -> ");

        //        foreach (var v in context.children)
        //        {
        //            if (v.ChildCount == 0)
        //            {
        //                sb.Append(v.ToString());
        //            }
        //        }

        //        Console.WriteLine(sb);
        //    }
        //}
    }
}
