using System;
using System.Collections.Generic;
using System.Data;

namespace Shinobytes.Console.Forms.Views
{
    public class ViewTemplateExpressionParser : IViewTemplateExpressionParser
    {
        private readonly Tokenizer tokenizer;

        public ViewTemplateExpressionParser()
        {
            this.tokenizer = new Tokenizer();
        }

        public IViewTemplateNodeExpression Parse(string expression)
        {
            var tokens = tokenizer.Tokenize(expression);
            var nodes = new List<SyntaxNode>();
            if (tokens.Available > 0)
            {
                var token = tokens.Current;
                if (token.Type == TokenType.OpenCurlyBracket)
                {
                    tokens.ConsumeExpected(t => t.Type == TokenType.OpenCurlyBracket);
                    tokens.ConsumeExpected(t => t.Type == TokenType.OpenCurlyBracket);

                    while (tokens.Available > 0 && tokens.Current.Type != TokenType.CloseCurlyBracket)
                    {
                        nodes.Add(ParseNode(tokens));
                    }
                }
                else
                {
                    throw new InvalidExpressionException($"The expression '{expression}' could not be parsed, expression needs to start with '{{'");
                }
            }

            return new ViewTemplateNodeExpression(nodes);
        }

        private SyntaxNode ParseNode(CollectionStream<Token> tokens)
        {
            var current = tokens.Current;

            if (current.Type == TokenType.Identifier)
            {
                return ParseIdentifier(tokens);
            }
            else if (current.Type == TokenType.Number)
            {
                return ParseNumber(tokens);
            }
            else if (current.Type == TokenType.DoubleQuouteString)
            {
                return ParseString(tokens);
            }
            else if (current.Type == TokenType.SingleQuouteString)
            {
                return ParseChar(tokens);
            }
            else
            {
                throw new NotImplementedException($"Unexpected token '{current.Value}' found!");
            }
        }

        private SyntaxNode ParseNumber(CollectionStream<Token> tokens)
        {
            var number = tokens.ConsumeExpected(x => x.Type == TokenType.Number);
            if (double.TryParse(number.Value, out var result))
            {
                var syntaxNode = new Number(result);
                return ParsePostIdentifier(syntaxNode, tokens);
            }
            throw new NotSupportedException($"'{number.Value}' is not a supported number format.");
        }

        private SyntaxNode ParseString(CollectionStream<Token> tokens)
        {
            var text = tokens.ConsumeExpected(x => x.Type == TokenType.DoubleQuouteString);
            var syntaxNode = new Text(text.Value);
            return ParsePostIdentifier(syntaxNode, tokens);
        }

        private SyntaxNode ParseChar(CollectionStream<Token> tokens)
        {
            var text = tokens.ConsumeExpected(x => x.Type == TokenType.SingleQuouteString);
            var syntaxNode = new TextChar(text.Value[0]);
            return ParsePostIdentifier(syntaxNode, tokens);
        }

        private SyntaxNode ParseIdentifier(CollectionStream<Token> tokens)
        {
            var identifier = tokens.ConsumeExpected(x => x.Type == TokenType.Identifier);
            var node = new Identifier(identifier.Value);
            return ParsePostIdentifier(node, tokens);
        }

        private SyntaxNode ParsePostIdentifier(SyntaxNode node, CollectionStream<Token> tokens)
        {
            var next = tokens.Current;
            switch (next.Type)
            {
                case TokenType.Dot:
                    var dot = tokens.Consume();
                    return ParseMemberAccess(node, dot, tokens);
                case TokenType.OpenParenthesis:
                    var open = tokens.Consume();
                    return ParseMemberInvocation(node, open, tokens);
                case TokenType.OpenBracket:
                    var bracket = tokens.Consume();
                    return ParseIndexer(node, bracket, tokens);
            }

            return node;
        }

        private SyntaxNode ParseIndexer(SyntaxNode node, Token bracket, CollectionStream<Token> tokens)
        {
            var indexerExpression = ParseExpressionList(tokens, TokenType.Comma, TokenType.CloseBracket);
            tokens.ConsumeExpected(x => x.Type == TokenType.CloseBracket);
            return new IndexedMemberAccess(node, new Indexer(indexerExpression));
        }

        private SyntaxNode ParseMemberInvocation(SyntaxNode node, Token open, CollectionStream<Token> tokens)
        {
            var member = node;
            var arguments = ParseExpressionList(tokens, TokenType.Comma, TokenType.CloseParenthesis);
            tokens.ConsumeExpected(x => x.Type == TokenType.CloseParenthesis);
            return new MemberInvocation(member, arguments);
        }

        private SyntaxNode[] ParseExpressionList(CollectionStream<Token> tokens, TokenType separator, TokenType closer)
        {
            var expressions = new List<SyntaxNode>();

            while (tokens.Available > 0 && tokens.Current.Type != closer)
            {
                if (tokens.Current.Type == separator)
                {
                    tokens.Consume();
                }
                else
                {
                    var expr = ParseNode(tokens);
                    expressions.Add(expr);
                }
            }

            return expressions.ToArray();
        }

        private SyntaxNode ParseMemberAccess(SyntaxNode node, Token dot, CollectionStream<Token> tokens)
        {
            var identifier = ParseIdentifier(tokens);
            return new MemberAccess(node, identifier);
        }

    }
}