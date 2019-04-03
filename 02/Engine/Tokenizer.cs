using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine
{
    class Tokenizer
    {
        private readonly StringReader _reader;
        private string _text;

        public Tokenizer(string text)
        {
            _text = text;
            _reader = new StringReader(text);
        }

        public IEnumerable<Token> Tokenize()
        {
            var tokens = new List<Token>();
            while (_reader.Peek() != -1)
            {
                while (char.IsWhiteSpace((char)_reader.Peek()))
                {
                    _reader.Read();
                }

                if (_reader.Peek() == -1)
                    break;

                if (char.IsDigit((char)_reader.Peek()))
                {
                    var nr = ParseNumber();
                    tokens.Add(new NumberConstantToken(nr));
                }

                var c = (char)_reader.Peek();
                switch (c)
                {
                    case '(':
                        tokens.Add(new OpenParenthesisToken());
                        _reader.Read();
                        break;
                    case ')':
                        tokens.Add(new CloseParenthesisToken());
                        _reader.Read();
                        break;
                    case '<':
                        tokens.Add(new SmallerToken());
                        _reader.Read();
                        c = (char)_reader.Peek();
                        
                        // notice: case SmallerOrEqual
                        if (c == '=')
                        {
                            tokens.RemoveAt(tokens.Count - 1);
                            tokens.Add(new SmallerOrEqualToken());
                            _reader.Read();
                        }
                        break;
                    case '=':
                        _reader.Read();
                        c = (char)_reader.Peek();

                        if (c == '=')
                        {
                            tokens.Add(new EqualToken());
                            _reader.Read();
                        }
                        else
                        {
                            tokens.Add(new ErrorToken());
                            // exit
                            var remainingText = _reader.ReadToEnd() ?? string.Empty;
                            throw new Exception(string.Format("Unknown grammar found at position {0} : '{1}'", _text.Length - remainingText.Length, remainingText));
                        }
                        break;
                    case '!':
                        _reader.Read();
                        c = (char)_reader.Peek();

                        if (c == '=')
                        {
                            tokens.Add(new NotEqualToken());
                            _reader.Read();
                        }
                        else
                        {
                            tokens.Add(new ErrorToken());
                            // exit
                            var remainingText = _reader.ReadToEnd() ?? string.Empty;
                            throw new Exception(string.Format("Unknown grammar found at position {0} : '{1}'", _text.Length - remainingText.Length, remainingText));
                        }
                        break;
                    case '>':
                        tokens.Add(new BiggerToken());
                        _reader.Read();
                        c = (char)_reader.Peek();

                        // notice: case BiggerOrEqual
                        if (c == '=')
                        {
                            tokens.RemoveAt(tokens.Count - 1);
                            tokens.Add(new BiggerOrEqualToken());
                            _reader.Read();
                        }
                        break;
                    case '+':
                        tokens.Add(new PlusToken());
                        _reader.Read();
                        break;
                    case '-':
                        tokens.Add(new MinusToken());
                        _reader.Read();
                        break;
                    case '*':
                        tokens.Add(new MultiplyToken());
                        _reader.Read();
                        break;
                    case '/':
                        tokens.Add(new DiviveToken());
                        _reader.Read();
                        break;
                    default:
                        while (char.IsWhiteSpace((char)_reader.Peek()))
                        {
                            _reader.Read();
                        }

                        if (_reader.Peek() == -1)
                            break;

                        if (char.IsLetter(c))
                        {
                            var remainingText = _reader.ReadToEnd() ?? string.Empty;
                            if (remainingText.Length > 0)
                                throw new Exception(string.Format("Unknown grammar found at position {0} : '{1}'", _text.Length - remainingText.Length, remainingText));
                        }
                        break;
                }
            }

            return tokens;
        }

        private int ParseNumber()
        {
            var digits = new List<int>();
            while (char.IsDigit((char)_reader.Peek()))
            {
                var digit = (char)_reader.Read();
                int i;
                if (int.TryParse(char.ToString(digit), out i))
                {
                    digits.Add(i);
                }
                else
                    throw new Exception("Could not parse integer number when parsing digit: " + digit);
            }

            var nr = 0;
            var mul = 1;
            digits.Reverse();
            digits.ForEach(d =>
            {
                nr += d * mul;
                mul *= 10;
            });

            return nr;
        }

        private Token ParseKeyword()
        {
            var text = new StringBuilder();
            while (char.IsLetter((char)_reader.Peek()))
            {
                text.Append((char)_reader.Read());
            }

            var potentialKeyword = text.ToString().ToLower();

            switch (potentialKeyword)
            {
                case "true":
                    return new TrueToken();
                case "false":
                    return new FalseToken();
                default:
                    throw new Exception("Expected keyword (True, False) but found " + potentialKeyword);
            }
        }
    }
}
