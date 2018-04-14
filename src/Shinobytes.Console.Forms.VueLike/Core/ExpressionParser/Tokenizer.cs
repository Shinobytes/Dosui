using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.Console.Forms.Views
{
    internal class Tokenizer
    {
        public CollectionStream<Token> Tokenize(string expression)
        {
            var tokens = new List<Token>();

            if (!string.IsNullOrEmpty(expression))
            {
                var index = 0;
                while (index < expression.Length)
                {
                    var token = expression[index];
                    var identifier = "";
                    var number = "";
                    switch (token)
                    {
                        case ':': tokens.Add(Token(token, TokenType.Colon)); break;
                        case ';': tokens.Add(Token(token, TokenType.SemiColon)); break;
                        case '=': tokens.Add(Token(token, TokenType.Equals)); break;
                        case '/': tokens.Add(Token(token, TokenType.Divide)); break;
                        case '+': tokens.Add(Token(token, TokenType.Plus)); break;
                        case '-': tokens.Add(Token(token, TokenType.Minus)); break;
                        case '%': tokens.Add(Token(token, TokenType.Modulus)); break;
                        case ',': tokens.Add(Token(token, TokenType.Comma)); break;
                        case '.': tokens.Add(Token(token, TokenType.Dot)); break;
                        case '{': tokens.Add(Token(token, TokenType.OpenCurlyBracket)); break;
                        case '}': tokens.Add(Token(token, TokenType.CloseCurlyBracket)); break;
                        case '(': tokens.Add(Token(token, TokenType.OpenParenthesis)); break;
                        case ')': tokens.Add(Token(token, TokenType.CloseParenthesis)); break;
                        case '[': tokens.Add(Token(token, TokenType.OpenBracket)); break;
                        case ']': tokens.Add(Token(token, TokenType.CloseBracket)); break;
                        case '<': tokens.Add(Token(token, TokenType.LessThan)); break;
                        case '>': tokens.Add(Token(token, TokenType.GreaterThan)); break;
                        case '&': tokens.Add(Token(token, TokenType.And)); break;
                        case '|': tokens.Add(Token(token, TokenType.Or)); break;

                        case '\n': break;
                        case '\t': break;
                        case ' ': break; // tokens.Add(Token(token, TokenType.Whitespace)); 

                        case '"': break; // TODO: im lazy
                        case '\'': break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            do number += expression[index];
                            while (++index < expression.Length && IsNumber(number, expression[index]));
                            tokens.Add(Token(number, TokenType.Number));
                            continue;
                        default:
                            do identifier += expression[index];
                            while (++index < expression.Length && IsIdentifier(expression[index]));
                            tokens.Add(Token(identifier, TokenType.Identifier));
                            continue;
                    }
                    ++index;
                }
            }

            return new CollectionStream<Token>(tokens);
        }

        private bool IsIdentifier(char token)
        {
            var allowed = "qwertyuiopåasdfghjklöäzxcvbnm_1234567890";
            return allowed.Contains(char.ToLower(token).ToString());
        }

        private bool IsNumber(string previous, char token)
        {
            if (!string.IsNullOrEmpty(previous) && previous.Last() != '.' && token == '.')
                return true;
            return char.IsDigit(token);
        }

        public Token Token(string token, TokenType type)
        {
            return new Token(token, type);
        }

        public Token Token(char token, TokenType type)
        {
            return new Token(token.ToString(), type);
        }
    }
}