using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    // <ArithmeticExpression>   := <Expression> | "(" <Expression> ")"
    // <Expression>             := <Term> | <Term> <AdditionOperator> <Expression>
    // <Term>                   := <Factor> | <Factor> <MultiplicationOperator> <Term>
    // <Factor>                 := <Identification> | <Number> | <ArithmeticExpression>
    // <AdditionOperator>       := "+" | "-"
    // <MultiplicationOperator> := "*" | "/"
    class ArithmeticExpressionParser
    {
        private readonly IEnumerator<Token> _tokens;
        private int count;
        private int Length;
        public int Result;

        public ArithmeticExpressionParser(IEnumerable<Token> tokens)
        {
            _tokens = tokens.GetEnumerator();
            _tokens.MoveNext();

            count = 0;
            Length = tokens.ToList().Count;
            Result = 0;
        }

        public int Parse() => (count == Length) ? Result : ParseArithmeticExpression();
        
        // <ArithmeticExpression>   := <Expression> | "(" <Expression> ")"
        private  ParseArithmeticExpression()
        {
            if (count == Length)
                return 

            if (_tokens.Current is OpenParenthesisToken)
            {
                while (_tokens.Current != null)
                {
                    var expression = ParseExpression();
                    _tokens.MoveNext();

                    if (_tokens.Current is CloseParenthesisToken)
                    {
                        return expression;
                    }
                    else
                    {
                        throw new Exception("Exception: Expected Close parenthesis but get " + _tokens.Current);
                    }
                }
            }
            
            return ParseExpression();   // by default
        }

        // <Expression>             := <Term> | <Term> <AdditionOperator> <Expression>
        private int ParseExpression()
        {
            var number = ParseTerm();

            _tokens.MoveNext();
            
            while (_tokens.Current is AdditionOperatorToken)
            {
                var op = _tokens.Current;
                _tokens.MoveNext();

                var secondNumber = ParseExpression();
                if (op is PlusToken)
                    number = number + secondNumber;

                if (op is MinusToken)
                    number = number - secondNumber;

                _tokens.MoveNext();
            }

            return number;
        }

        // <Term>                   := <Factor> | <Factor> <MultiplicationOperator> <Term>
        private int ParseTerm()
        {
            var number = ParseFactor();

            _tokens.MoveNext();

            while (_tokens.Current is MultiplicationOperatorToken)
            {
                var op = _tokens.Current;
                _tokens.MoveNext();

                var secondNumber = ParseTerm();

                if (op is MultiplyToken)
                {
                    number = number * secondNumber;
                }

                if (op is DiviveToken)
                {
                    if (secondNumber == 0)
                        throw new Exception("Exception: Unsupported operator division by zero.");

                    number = number / secondNumber;
                }

                _tokens.MoveNext();
            }

            return number;
        }

        // <Factor>                 := <Identification> | <Number> | <ArithmeticExpression>
        private int ParseFactor()
        {
            if (_tokens.Current is NumberConstantToken)
            {
                return ParseNumber();
            }

            return ParseArithmeticExpression();
        }


        private int ParseNumber()
        {
            if (_tokens.Current is NumberConstantToken)
            {
                return (_tokens.Current as NumberConstantToken).Value;
            }

            throw new Exception("Exception: Expected a number constant but found " + _tokens.Current);
        }
    }
}
