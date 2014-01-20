namespace MonoKle.Script
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class TokenStream
    {
        private Dictionary<string, int> tokenByString = new Dictionary<string, int>();

        private LinkedList<int> tokens = new LinkedList<int>();

        public TokenStream()
        {
            tokenByString.Add("return", 1);
            tokenByString.Add("set", 2);
        }

        public void Tokenize(string[] source)
        {
            for(int i = 0; i < source.Length; i++)
            {
                if(tokenByString.ContainsKey(source[i]))
                {
                    tokens.AddLast(tokenByString[source[i]]);
                }
                else
                {
                    tokens.AddLast(0);
                }
            }
        }

        public int[] GetTokens()
        {
            return tokens.ToArray();
        }
    }
}
