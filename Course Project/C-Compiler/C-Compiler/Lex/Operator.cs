using System.Collections.Generic;
using System.Linq;

public class TokenOperator : Token
{
    public TokenOperator()
    {
        type = TokenType.OPERATOR;
    }
    public OperatorValues val;
    public static Dictionary<string, OperatorValues> ops = new Dictionary<string, OperatorValues>() {
        { "[",    OperatorValues.LBRACKET     },
        { "]",    OperatorValues.RBRACKET     },
        { "(",    OperatorValues.LPAREN       },
        { ")",    OperatorValues.RPAREN       },
        { ".",    OperatorValues.PERIOD       },
        { ",",    OperatorValues.COMMA        },
        { "?",    OperatorValues.QUESTION     },
        { ":",    OperatorValues.COLON        },
        { "~",    OperatorValues.TILDE        },
        { "-",    OperatorValues.SUB          },
        { "->",   OperatorValues.RARROW       },
        { "--",   OperatorValues.DEC          },
        { "-=",   OperatorValues.SUBASSIGN    },
        { "+",    OperatorValues.ADD          },
        { "++",   OperatorValues.INC          },
        { "+=",   OperatorValues.ADDASSIGN    },
        { "&",    OperatorValues.BITAND       },
        { "&&",   OperatorValues.AND          },
        { "&=",   OperatorValues.ANDASSIGN    },
        { "*",    OperatorValues.MULT         },
        { "*=",   OperatorValues.MULTASSIGN   },
        { "<",    OperatorValues.LT           },
        { "<=",   OperatorValues.LEQ          },
        { "<<",   OperatorValues.LSHIFT       },
        { "<<=",  OperatorValues.LSHIFTASSIGN },
        { ">",    OperatorValues.GT           },
        { ">=",   OperatorValues.GEQ          },
        { ">>",   OperatorValues.RSHIFT       },
        { ">>=",  OperatorValues.RSHIFTASSIGN },
        { "=",    OperatorValues.ASSIGN       },
        { "==",   OperatorValues.EQ           },
        { "|",    OperatorValues.BITOR        },
        { "||",   OperatorValues.OR           },
        { "|=",   OperatorValues.ORASSIGN     },
        { "!",    OperatorValues.NOT          },
        { "!=",   OperatorValues.NEQ          },
        { "/",    OperatorValues.DIV          },
        { "/=",   OperatorValues.DIVASSIGN    },
        { "%",    OperatorValues.MOD          },
        { "%=",   OperatorValues.MODASSIGN    },
        { "^",    OperatorValues.XOR          },
        { "^=",   OperatorValues.XORASSIGN    },
        { ";",    OperatorValues.SEMICOLON    },
        { "{",    OperatorValues.LCURL        },
        { "}",    OperatorValues.RCURL        }
    };

    public override string ToString()
    {
        return type.ToString() + " [" + val.ToString() + "]: " + ops.First(pair => pair.Value == val).Key;
    }
}

public class FSAOperator : FSA
{
    public enum OpState
    {
        START,
        END,
        ERROR,
        FINISH,
        SUB,
        ADD,
        AMP,
        MULT,
        LT,
        LTLT,
        GT,
        GTGT,
        EQ,
        OR,
        NOT,
        DIV,
        MOD,
        XOR
    };

    public static List<char> opchars = new List<char>() {
        '[',
        ']',
        '(',
        ')',
        '.',
        ',',
        '?',
        ':',
        '-',
        '>',
        '+',
        '&',
        '*',
        '~',
        '!',
        '/',
        '%',
        '<',
        '=',
        '^',
        '|',
        ':',
        ';',
        '{',
        '}'
    };

    public OpState state;
    public FSAOperator()
    {
        state = OpState.START;
        str = "";
    }

    public void Reset()
    {
        state = OpState.START;
        str = "";
    }

    public FSAStatus GetStatus()
    {
        switch (state)
        {
            case OpState.START:
                return FSAStatus.NONE;
            case OpState.END:
                return FSAStatus.END;
            case OpState.ERROR:
                return FSAStatus.ERROR;
            default:
                return FSAStatus.RUN;
        }
    }

    private string str;
    public Token RetrieveToken()
    {
        TokenOperator token = new TokenOperator();
        token.type = TokenType.OPERATOR;
        //Console.WriteLine("str = [{0}]", str.Substring(0, str.Length - 1));
        //Console.WriteLine(TokenOperator.ops.ContainsKey(str.Substring(0, str.Length - 1)));
        token.val = TokenOperator.ops[str.Substring(0, str.Length - 1)];
        return token;
    }

    public void ReadChar(char ch)
    {
        str = str + ch;

        switch (state)
        {
            case OpState.END:
            case OpState.ERROR:
                state = OpState.ERROR;
                break;
            case OpState.START:
                if (opchars.Exists(x => x == ch))
                {
                    switch (ch)
                    {
                        case '-':
                            state = OpState.SUB;
                            break;
                        case '+':
                            state = OpState.ADD;
                            break;
                        case '&':
                            state = OpState.AMP;
                            break;
                        case '*':
                            state = OpState.MULT;
                            break;
                        case '<':
                            state = OpState.LT;
                            break;
                        case '>':
                            state = OpState.GT;
                            break;
                        case '=':
                            state = OpState.EQ;
                            break;
                        case '|':
                            state = OpState.OR;
                            break;
                        case '!':
                            state = OpState.NOT;
                            break;
                        case '/':
                            state = OpState.DIV;
                            break;
                        case '%':
                            state = OpState.MOD;
                            break;
                        case '^':
                            state = OpState.XOR;
                            break;
                        default:
                            state = OpState.FINISH;
                            break;
                    }
                }
                else
                {
                    state = OpState.ERROR;
                }
                break;
            case OpState.FINISH:
                state = OpState.END;
                break;
            case OpState.SUB:
                switch (ch)
                {
                    case '>':
                    case '-':
                    case '=':
                        state = OpState.FINISH;
                        break;
                    default:
                        state = OpState.END;
                        break;
                }
                break;
            case OpState.ADD:
                switch (ch)
                {
                    case '+':
                    case '=':
                        state = OpState.FINISH;
                        break;
                    default:
                        state = OpState.END;
                        break;
                }
                break;
            case OpState.AMP:
                switch (ch)
                {
                    case '&':
                    case '=':
                        state = OpState.FINISH;
                        break;
                    default:
                        state = OpState.END;
                        break;
                }
                break;
            case OpState.MULT:
                if (ch == '=')
                {
                    state = OpState.FINISH;
                }
                else
                {
                    state = OpState.END;
                }
                break;
            case OpState.LT:
                switch (ch)
                {
                    case '=':
                        state = OpState.FINISH;
                        break;
                    case '<':
                        state = OpState.LTLT;
                        break;
                    default:
                        state = OpState.END;
                        break;
                }
                break;
            case OpState.GT:
                switch (ch)
                {
                    case '=':
                        state = OpState.FINISH;
                        break;
                    case '>':
                        state = OpState.GTGT;
                        break;
                    default:
                        state = OpState.END;
                        break;
                }
                break;
            case OpState.EQ:
                if (ch == '=')
                {
                    state = OpState.FINISH;
                }
                else
                {
                    state = OpState.END;
                }
                break;
            case OpState.OR:
                switch (ch)
                {
                    case '|':
                    case '=':
                        state = OpState.FINISH;
                        break;
                    default:
                        state = OpState.END;
                        break;
                }
                break;
            case OpState.NOT:
                if (ch == '=')
                {
                    state = OpState.FINISH;
                }
                else
                {
                    state = OpState.END;
                }
                break;
            case OpState.DIV:
                if (ch == '=')
                {
                    state = OpState.FINISH;
                }
                else
                {
                    state = OpState.END;
                }
                break;
            case OpState.MOD:
                if (ch == '=')
                {
                    state = OpState.FINISH;
                }
                else
                {
                    state = OpState.END;
                }
                break;
            case OpState.XOR:
                if (ch == '=')
                {
                    state = OpState.FINISH;
                }
                else
                {
                    state = OpState.END;
                }
                break;
            case OpState.LTLT:
                if (ch == '=')
                {
                    state = OpState.FINISH;
                }
                else
                {
                    state = OpState.END;
                }
                break;
            case OpState.GTGT:
                if (ch == '=')
                {
                    state = OpState.FINISH;
                }
                else
                {
                    state = OpState.END;
                }
                break;
            default:
                state = OpState.ERROR;
                break;
        }
    }

    public void ReadEOF()
    {
        str = str + '0';
        switch (state)
        {
            case OpState.FINISH:
            case OpState.SUB:
            case OpState.ADD:
            case OpState.AMP:
            case OpState.MULT:
            case OpState.LT:
            case OpState.LTLT:
            case OpState.GT:
            case OpState.GTGT:
            case OpState.EQ:
            case OpState.OR:
            case OpState.NOT:
            case OpState.DIV:
            case OpState.MOD:
            case OpState.XOR:
                state = OpState.END;
                break;
            default:
                state = OpState.ERROR;
                break;
        }
    }
}