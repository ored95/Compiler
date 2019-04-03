namespace Engine
{
    public abstract class Token { }

    public class ParenthesisToken : Token { }
    public class OpenParenthesisToken : ParenthesisToken { }
    public class CloseParenthesisToken : ParenthesisToken { }

    public class BooleanOperatorToken : Token { }
    public class SmallerToken : BooleanOperatorToken { }
    public class SmallerOrEqualToken : BooleanOperatorToken { }
    public class EqualToken : BooleanOperatorToken { }
    public class NotEqualToken : BooleanOperatorToken { }
    public class BiggerOrEqualToken : BooleanOperatorToken { }
    public class BiggerToken : BooleanOperatorToken { }

    public class AdditionOperatorToken : Token { }
    public class PlusToken : AdditionOperatorToken { }
    public class MinusToken : AdditionOperatorToken { }

    public class MultiplicationOperatorToken : Token { }
    public class MultiplyToken : MultiplicationOperatorToken { }
    public class DiviveToken : MultiplicationOperatorToken { }

    public class BooleanValueToken : Token { }
    public class TrueToken : BooleanValueToken { }
    public class FalseToken : BooleanValueToken { }

    public class NumberConstantToken : Token
    {
        public NumberConstantToken(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class ErrorToken : Token { }
}
