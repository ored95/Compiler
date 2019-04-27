using System;
using System.Collections.Generic;

namespace Engine
{
    // <Expression>             := <AlgebraicExpression> <BooleanOperator> <AlgebraicExpression> | <AlgebraicExpression>
    // <AlgebraicExpression>    := <AlgebraicExpression> <AdditionOperator> <Term> | <Term>
    // <Term>                   := <Term> <MultiplicationOperator> <Factor> | <Factor>
    // <Factor>                 := <Identification> | <Constant> | "(" <AlgebraicExpression> ")"    
    // <BooleanOperator>        := "<" | "<=" | "==" | "!=' | ">=" | ">" 
    // <AdditionOperator>       := "+" | "-"
    // <MultiplicationOperator> := "*" | "/"

    // Extend: Production for BooleanConstant (true | false) -- issue
    class Parser
    {
        private readonly IEnumerator<Token> _tokens;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens.GetEnumerator();
            _tokens.MoveNext();
        }

        // <Expression> := <AlgebraicExpression> <BooleanOperator> <AlgebraicExpression> | <AlgebraicExpression>
        public bool Parse()
        {
            while (_tokens.Current != null)
            {
                var expression = ParseAlgebraicExpression();
                
                if (!_tokens.MoveNext())
                {
                    return expression != 0;
                }

                bool value = true;

                // <BooleanOperator> := "<" | "<=" | "==" | "!=' | ">=" | ">" 
                if (_tokens.Current is BooleanOperatorToken)
                {
                    var op = _tokens.Current;
                    _tokens.MoveNext();

                    var nextExpression = ParseAlgebraicExpression();

                    if (op is SmallerToken)
                        value = (expression < nextExpression);

                    if (op is SmallerOrEqualToken)
                        value = (expression <= nextExpression);

                    if (op is EqualToken)
                        value = (expression == nextExpression);

                    if (op is NotEqualToken)
                        value = (expression != nextExpression);

                    if (op is BiggerOrEqualToken)
                        value = (expression >= nextExpression);

                    if (op is BiggerToken)
                        value = (expression > nextExpression);
                }

                if (!_tokens.MoveNext())
                    throw new Exception("Unsupported more than 1 boolean operators!");

                return value;
            }

            throw new Exception("Empty expression");
        }

        // <AlgebraicExpression> := <AlgebraicExpression> <AdditionOperator> <Term> | <Term>
        private int ParseAlgebraicExpression()
        {
            var number = ParseTerm();
            if (!_tokens.MoveNext())
                return number;

            while (_tokens.Current is AdditionOperatorToken)
            {
                var op = _tokens.Current;
                _tokens.MoveNext();

                var secondNumber = ParseAlgebraicExpression();
                if (op is PlusToken)
                    number = number + secondNumber;

                if (op is MinusToken)
                    number = number - secondNumber;

                _tokens.MoveNext();
            }

            return number;
        }

        // <Term> := <Term> <MultiplicationOperator> <Factor> | <Factor>
        private int ParseTerm()
        {
            var firstNumber = ParseFactor();

            if (!_tokens.MoveNext())
                return firstNumber;

            while (_tokens.Current is MultiplicationOperatorToken)
            {
                var op = _tokens.Current;
                _tokens.MoveNext();

                var secondNumber = ParseTerm();

                if (op is MultiplyToken)
                {
                    firstNumber = firstNumber * secondNumber;
                }
                
                if (op is DiviveToken)
                {
                    if (secondNumber == 0)
                        throw new Exception("Unsupported operator division by zero.");

                    firstNumber = firstNumber / secondNumber;
                }

                _tokens.MoveNext();
            }

            return firstNumber;
        }

        // <Factor> := <Identification> | <NumberConstant> | "(" <AlgebraicExpression> ")"
        private int ParseFactor()
        {

            if (_tokens.Current is OpenParenthesisToken)
            {
                _tokens.MoveNext();

                var expInPars = ParseAlgebraicExpression();
                //_tokens.MoveNext();

                if (!(_tokens.Current is CloseParenthesisToken))
                    throw new Exception("Expecting Close Parenthesis");

                _tokens.MoveNext();

                return expInPars;
            }

            if (_tokens.Current is NumberConstantToken)
            {
                return ParseNumberConstant();
            }

            return 0;       // <Identification>
        }


        private int ParseNumberConstant()
        {
            if (_tokens.Current is NumberConstantToken)
            {
                return (_tokens.Current as NumberConstantToken).Value;
            }

            throw new Exception("zExpected a number constant but found " + _tokens.Current);
        }
    }
}
