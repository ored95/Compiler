using System;
using System.Collections.Generic;

public class Parser
{
    public static bool IsSizeof(Token token)
    {
        if (token.type == TokenType.KEYWORD)
        {
            if (((TokenKeyword)token).val == KeywordValues.SIZEOF)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsLPAREN(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.LPAREN)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsRPAREN(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.RPAREN)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsCOLON(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.COLON)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsQuestionMark(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.QUESTION)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsAssignment(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.ASSIGN)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsCOMMA(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.COMMA)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsLCURL(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.LCURL)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsRCURL(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.RCURL)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsLBRACKET(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.LBRACKET)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsRBRACKET(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.RBRACKET)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsPERIOD(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.PERIOD)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsEllipsis(List<Token> src, int begin)
    {
        if (IsPERIOD(src[begin]))
        {
            begin++;
            if (IsPERIOD(src[begin]))
            {
                begin++;
                if (IsPERIOD(src[begin]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsSEMICOLON(Token token)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == OperatorValues.SEMICOLON)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsKeyword(Token token, KeywordValues val)
    {
        if (token.type == TokenType.KEYWORD)
        {
            if (((TokenKeyword)token).val == val)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsOperator(Token token, OperatorValues val)
    {
        if (token.type == TokenType.OPERATOR)
        {
            if (((TokenOperator)token).val == val)
            {
                return true;
            }
        }
        return false;
    }

    public static string GetIdentifierValue(Token token)
    {
        if (token.type == TokenType.IDENTIFIER)
        {
            return ((TokenIdentifier)token).val;
        }
        else
        {
            return null;
        }
    }

    public static List<Token> GetTokensFromString(string src)
    {
        LexicalAnalysis lex = new LexicalAnalysis();
        lex.src = src;
        lex.Lex();
        return lex.tokens;
    }

}

public class ParserEnvironment
{
    public static bool debug = false;
}

public class Scope
{
    public Scope()
    {
        vars = new List<string>();
        typedef_names = new List<string>();
    }

    public bool HasVariable(string var)
    {
        return vars.FindIndex(x => x == var) != -1;
    }

    public bool HasTypedefName(string type)
    {
        return typedef_names.FindIndex(x => x == type) != -1;
    }

    public bool HasIdentifier(string id)
    {
        return HasVariable(id) || HasTypedefName(id);
    }

    public void AddTypedefName(string type)
    {
        typedef_names.Add(type);
    }

    public List<string> typedef_names;
    public List<string> vars;
}

public class ScopeSandbox
{
    public ScopeSandbox()
    {
        scopes = new Stack<Scope>();
        scopes.Push(new Scope());
    }

    public void InScope()
    {
        scopes.Push(new Scope());
    }

    public void OutScope()
    {
        scopes.Pop();
    }

    public bool HasVariable(string var)
    {
        return scopes.Peek().HasVariable(var);
    }

    public bool HasTypedefName(string type)
    {
        return scopes.Peek().HasTypedefName(type);
    }

    public void AddTypedefName(string type)
    {
        scopes.Peek().AddTypedefName(type);
    }

    public bool HasIdentifier(string id)
    {
        return scopes.Peek().HasIdentifier(id);
    }

    public Stack<Scope> scopes;
}

static class ScopeEnvironment
{
    static ScopeEnvironment()
    {
        sandboxes = new Stack<ScopeSandbox>();
        sandboxes.Push(new ScopeSandbox());
    }

    public static void PushSandbox()
    {
        if (sandboxes.Count == 0)
        {
            return;
        }
        sandboxes.Push(sandboxes.Peek());
    }

    public static void PopSandbox()
    {
        if (sandboxes.Count < 2)
        {
            return;
        }
        ScopeSandbox top = sandboxes.Pop();
        sandboxes.Pop();
        sandboxes.Push(top);
    }

    public static void InScope()
    {
        sandboxes.Peek().InScope();
    }

    public static void OutScope()
    {
        sandboxes.Peek().OutScope();
    }

    public static bool HasVariable(string var)
    {
        return sandboxes.Peek().HasVariable(var);
    }

    public static bool HasTypedefName(string type)
    {
        return sandboxes.Peek().HasTypedefName(type);
    }

    public static void AddTypedefName(string type)
    {
        sandboxes.Peek().AddTypedefName(type);
    }

    public static bool HasIdentifier(string id)
    {
        return sandboxes.Peek().HasIdentifier(id);
    }

    public static Stack<ScopeSandbox> sandboxes;
}

public interface IPTNode { }

public interface IASTNode { }