using System.Text;
using System.Collections.Generic;

namespace clausewitz_savegame_tokenizer
{
    public static class Tokenizer
    {
        /// <summary>
        /// Builds tokens list from a string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <param name="keepEmptyQuotes">Indicates if empty tokens in quotes should be preserved.</param>
        /// <param name="delimiters">String containing delimiters.</param>
        /// <param name="quote">String contataining quote charaters.</param>
        /// <param name="delimetersPreserved">String containing delimiters to be preserved as tokens.</param>
        /// <param name="escape">String containing escape characters.</param>
        /// <returns>Tokens stored in a list of strings.</returns>
        public static List<string> Tokenize(string input, bool keepEmptyQuotes = true, string delimiters = " \t\n", string quote = "\"", string delimetersPreserved = "={}", string escape = "\\")
        {
            List<string> tokens = new List<string>();   // built tokens are stored here

            int index           = 0;                    // current character's index
            int length          = input.Length;         // string's length

            char character      = char.MinValue;        // current character is stored here
            char delimiter      = char.MinValue;        // current delmiter is stored here
            char startedQuote   = char.MinValue;        // current quote's opening character is stored here

            bool completed      = false;                // indicates that token is completed and can be added
            bool valid          = true;                 // indicates that character is a valid token character
            bool validDelimiter = false;                // indicates that delimiteris preserved
            bool quoted         = false;                // indicates that quote has started
            bool escaped        = false;                // indicates that escaped character is encountered
            bool emptyQuote     = false;                // indicates that quote is empty 

            StringBuilder token = new StringBuilder();  // current token is constructed here

            while (index < length)
            {
                // resetting values
                character       = input[index];
                delimiter       = char.MinValue;
                valid           = true;                 // assuming current character is apart of a token
                validDelimiter  = false;
                escaped         = false;
                emptyQuote      = false;

                // escape character check
                if (!string.IsNullOrEmpty(escape))
                {
                    if (escape.IndexOf(character) != -1)
                    {
                        ++index;                        // trying to get escaped character

                        if (index < length)             // boundaries check
                        {
                            character = input[index];   // getting escaped character
                            valid = true;
                        }
                        else
                        {
                            valid = false;
                        }

                        escaped = true;
                    }
                }

                // quote character check
                if (!string.IsNullOrEmpty(quote) && !escaped)
                {
                    if (quote.IndexOf(character) != -1)
                    {
                        if (!quoted)                    // opened quote check
                        {
                            quoted = true;
                            startedQuote = character;   // stroing opening quote character
                            valid = false;
                        }
                        else
                        {
                            if (startedQuote == character)
                            {
                                quoted = false;
                                startedQuote = char.MinValue;
                                valid = false;

                                if (token.Length == 0)
                                {
                                    emptyQuote = true;
                                }
                            }
                        }
                    }
                }

                // delimiter character check
                if (!string.IsNullOrEmpty(delimiters) && !escaped && !quoted)
                {
                    if (delimiters.IndexOf(character) != -1)
                    {
                        if (token.Length != 0)          // non-empty token check
                        {
                            completed = true;           // delimiter encountered, so token is finished
                        }

                        valid = false;
                    }
                }

                // preserved delimiter character check
                if (!string.IsNullOrEmpty(delimetersPreserved) && !escaped && !quoted)
                {
                    if (delimetersPreserved.IndexOf(character) != -1)
                    {
                        if (token.Length != 0)          // non-empty token check
                        {
                            completed = true;           // delimiter encountered, so token is finished
                        }

                        valid = false;
                        delimiter = character;
                        validDelimiter = true;
                    }
                }

                // constructing token
                if (valid)
                {
                    token.Append(character);
                }

                // adding token to list
                if (completed && token.Length != 0)
                {
                    tokens.Add(token.ToString());
                    token.Clear();
                    completed = false;
                }

                // adding delimiter
                if (validDelimiter)
                {
                    tokens.Add(delimiter.ToString());
                }

                // adding empty quote
                if (emptyQuote && keepEmptyQuotes)
                {
                    tokens.Add("");
                }

                ++index;
            }

            // end of string handling
            if (token.Length != 0)
            {
                tokens.Add(token.ToString());
            }

            return tokens;
        }
    }
}
