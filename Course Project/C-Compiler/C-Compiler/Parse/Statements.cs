using System;
using System.Collections.Generic;

// statement: labeled_statement
//          | compound_statement
//          | expression_statement
//          | selection_statement
//          | iteration_statement
//          | jump_statement
public class _statement : IPTNode
{
    public static int Parse(List<Token> src, int begin, out Statement stmt)
    {
        stmt = null;
        int current = _labeled_statement.Parse(src, begin, out stmt);
        if (current != -1)
        {
            return current;
        }

        current = _compound_statement.Parse(src, begin, out stmt);
        if (current != -1)
        {
            return current;
        }

        current = _expression_statement.Parse(src, begin, out stmt);
        if (current != -1)
        {
            return current;
        }

        current = _selection_statement.Parse(src, begin, out stmt);
        if (current != -1)
        {
            return current;
        }

        current = _iteration_statement.Parse(src, begin, out stmt);
        if (current != -1)
        {
            return current;
        }

        current = _jump_statement.Parse(src, begin, out stmt);
        if (current != -1)
        {
            return current;
        }

        return -1;
    }
}

public class Statement : IASTNode { }


// jump_statement: goto identifier ;
//               | continue ;
//               | break ;
//               | return <expression>? ;
public class _jump_statement : IPTNode
{
    public static int Parse(List<Token> src, int begin, out Statement stmt)
    {
        stmt = null;

        if (src[begin].type != TokenType.KEYWORD)
        {
            return -1;
        }

        KeywordValues val = ((TokenKeyword)src[begin]).val;

        int current = begin + 1;
        switch (val)
        {
            case KeywordValues.GOTO:
                if (src[current].type != TokenType.IDENTIFIER)
                {
                    return -1;
                }
                stmt = new GotoStatement(((TokenIdentifier)src[current]).val);
                current++;
                break;
            case KeywordValues.CONTINUE:
                current++;
                break;
            case KeywordValues.BREAK:
                current++;
                break;
            case KeywordValues.RETURN:
                int saved = current;
                Expression expr;
                current = _expression.Parse(src, current, out expr);
                if (current == -1)
                {
                    current = saved;
                    stmt = new ReturnStatement(null);
                }
                else
                {
                    stmt = new ReturnStatement(expr);
                }
                break;
            default:
                return -1;
        }

        if (!Parser.IsSEMICOLON(src[current]))
        {
            stmt = null;
            return -1;
        }

        return ++current;
    }
}

public class GotoStatement : Statement
{
    public GotoStatement(string _label)
    {
        label = _label;
    }
    public string label;
}

public class ContinueStatement : Statement { }

public class BreakStatement : Statement { }

public class ReturnStatement : Statement
{
    public ReturnStatement(Expression _expr)
    {
        expr = _expr;
    }
    public Expression expr;
}


// compound_statement : { <declaration_list>? <statement_list>? }
public class _compound_statement : IPTNode
{
    public static int Parse(List<Token> src, int begin, out Statement stmt)
    {
        stmt = null;
        if (!Parser.IsLCURL(src[begin]))
        {
            return -1;
        }
        int current = begin + 1;

        int saved = current;
        current = _declaration_list.Parse(src, current, out List<Declaration> decl_list);
        if (current == -1)
        {
            decl_list = new List<Declaration>();
            current = saved;
        }

        saved = current;
        current = _statement_list.Parse(src, current, out List<Statement> stmt_list);
        if (current == -1)
        {
            stmt_list = new List<Statement>();
            current = saved;
        }

        if (!Parser.IsRCURL(src[current]))
        {
            return -1;
        }
        current++;

        stmt = new CompoundStatement(decl_list, stmt_list);
        return current;
    }
}

public class CompoundStatement : Statement
{
    public CompoundStatement(List<Declaration> _decl_list, List<Statement> _stmt_list)
    {
        decl_list = _decl_list;
        stmt_list = _stmt_list;
    }

    readonly List<Declaration> decl_list;
    readonly List<Statement> stmt_list;
}


// declaration_list: declaration
//                 | declaration_list declaration
// [ note: my solution ]
// declaration_list: <declaration>+
public class _declaration_list : IPTNode
{
    public static int Parse(List<Token> src, int begin, out List<Declaration> decl_list)
    {
        decl_list = new List<Declaration>();
        int current = _declaration.Parse(src, begin, out Declaration decl);
        if (current == -1)
        {
            return -1;
        }
        decl_list.Add(decl);

        int saved;
        while (true)
        {
            saved = current;
            current = _declaration.Parse(src, current, out decl);
            if (current == -1)
            {
                return saved;
            }
            decl_list.Add(decl);
        }
    }
}


// statement_list: statement
//               | statement_list statement
// [ note: my solution ]
// statement_list: <statement>+
public class _statement_list : IPTNode
{
    public static int Parse(List<Token> src, int begin, out List<Statement> stmt_list)
    {
        stmt_list = new List<Statement>();
        int current = _statement.Parse(src, begin, out Statement stmt);
        if (current == -1)
        {
            return -1;
        }
        stmt_list.Add(stmt);

        int saved;
        while (true)
        {
            saved = current;
            current = _statement.Parse(src, current, out stmt);
            if (current == -1)
            {
                return saved;
            }
            stmt_list.Add(stmt);
        }
    }
}


// expression_statement: <expression>? ;
public class _expression_statement : IPTNode
{
    public static int Parse(List<Token> src, int begin, out Statement stmt)
    {
        stmt = null;
        int current = _expression.Parse(src, begin, out Expression expr);
        if (current == -1)
        {
            expr = null;
            current = begin;
        }

        if (!Parser.IsSEMICOLON(src[current]))
        {
            return -1;
        }
        current++;

        stmt = new ExpressionStatement(expr);
        return current;
    }
}

public class ExpressionStatement : Statement
{
    public ExpressionStatement(Expression _expr)
    {
        expr = _expr;
    }
    public Expression expr;
}


// iteration_statement: while ( expression ) statement
//                    | do statement while ( expression ) ;
//                    | for ( <expression>? ; <expression>? ; <expression>? ) statement
public class _iteration_statement : IPTNode
{
    private static int ParseExpression(List<Token> src, int begin, out Expression expr)
    {
        expr = null;
        if (!Parser.IsLPAREN(src[begin]))
        {
            return -1;
        }
        int current = begin + 1;
        current = _expression.Parse(src, current, out expr);
        if (current == -1)
        {
            return -1;
        }
        if (!Parser.IsRPAREN(src[current]))
        {
            return -1;
        }
        current++;
        return current;
    }

    public static int Parse(List<Token> src, int begin, out Statement stmt)
    {
        stmt = null;
        int current;
        if (Parser.IsKeyword(src[begin], KeywordValues.WHILE))
        {
            // while
            current = begin + 1;

            current = ParseExpression(src, current, out Expression cond);
            if (current == -1)
            {
                return -1;
            }

            current = _statement.Parse(src, current, out Statement body);
            if (current == -1)
            {
                return -1;
            }

            stmt = new WhileStatement(cond, body);
            return current;

        }
        else if (Parser.IsKeyword(src[begin], KeywordValues.DO))
        {
            // do
            current = begin + 1;

            current = _statement.Parse(src, current, out Statement body);
            if (current == -1)
            {
                return -1;
            }

            current = ParseExpression(src, current, out Expression cond);
            if (current == -1)
            {
                return -1;
            }

            stmt = new DoWhileStatement(body, cond);
            return current;

        }
        else if (Parser.IsKeyword(src[begin], KeywordValues.FOR))
        {
            // for
            current = begin + 1;

            // match '('
            if (!Parser.IsLPAREN(src[current]))
            {
                return -1;
            }
            current++;

            // match init
            int saved = current;
            current = _expression.Parse(src, current, out Expression init);
            if (current == -1)
            {
                init = null;
                current = saved;
            }

            // match ';'
            if (!Parser.IsSEMICOLON(src[current]))
            {
                return -1;
            }
            current++;

            // match cond
            saved = current;
            current = _expression.Parse(src, current, out Expression cond);
            if (current == -1)
            {
                init = null;
                current = saved;
            }

            // match ';'
            if (!Parser.IsSEMICOLON(src[current]))
            {
                return -1;
            }
            current++;

            // match loop
            Expression loop;
            saved = current;
            current = _expression.Parse(src, current, out loop);
            if (current == -1)
            {
                init = null;
                current = saved;
            }

            // match ')'
            if (!Parser.IsRPAREN(src[current]))
            {
                return -1;
            }
            current++;

            current = _statement.Parse(src, current, out Statement body);
            if (current == -1)
            {
                return -1;
            }

            stmt = new ForStatement(init, cond, loop, body);
            return current;

        }
        else
        {
            return -1;
        }
    }
}

public class WhileStatement : Statement
{
    public WhileStatement(Expression _cond, Statement _body)
    {
        cond = _cond;
        body = _body;
    }
    public Expression cond;
    public Statement body;
}

public class DoWhileStatement : Statement
{
    public DoWhileStatement(Statement _body, Expression _cond)
    {
        body = _body;
        cond = _cond;
    }
    public Statement body;
    public Expression cond;
}

public class ForStatement : Statement
{
    public ForStatement(Expression _init, Expression _cond, Expression _loop, Statement _body)
    {
        init = _init;
        cond = _cond;
        loop = _loop;
        body = _body;
    }
    public Expression init;
    public Expression cond;
    public Expression loop;
    public Statement body;
}


// selection_statement: if ( expression ) statement
//                    | if ( expression ) statement else statement
//                    | switch ( expression ) statement
public class _selection_statement : IPTNode
{
    private static int ParseExpression(List<Token> src, int begin, out Expression expr)
    {
        expr = null;
        if (!Parser.IsLPAREN(src[begin]))
        {
            return -1;
        }
        int current = begin + 1;
        current = _expression.Parse(src, current, out expr);
        if (current == -1)
        {
            return -1;
        }
        if (!Parser.IsRPAREN(src[current]))
        {
            return -1;
        }
        current++;
        return current;
    }

    public static int Parse(List<Token> src, int begin, out Statement stmt)
    {
        stmt = null;

        int current;
        Expression expr;
        if (Parser.IsKeyword(src[begin], KeywordValues.SWITCH))
        {
            // switch
            current = begin + 1;
            current = ParseExpression(src, current, out expr);
            if (current == -1)
            {
                return -1;
            }

            current = _statement.Parse(src, current, out stmt);
            if (current == -1)
            {
                return -1;
            }

            stmt = new SwitchStatement(expr, stmt);
            return current;

        }
        else if (Parser.IsKeyword(src[begin], KeywordValues.IF))
        {
            // if
            current = begin + 1;
            current = ParseExpression(src, current, out expr);
            if (current == -1)
            {
                return -1;
            }
            current = _statement.Parse(src, current, out Statement true_stmt);
            if (current == -1)
            {
                return -1;
            }
            if (!Parser.IsKeyword(src[current], KeywordValues.ELSE))
            {
                stmt = new IfStatement(expr, true_stmt);
                return current;
            }
            current++;
            current = _statement.Parse(src, current, out Statement false_stmt);
            if (current == -1)
            {
                return -1;
            }
            stmt = new IfElseStatement(expr, true_stmt, false_stmt);
            return current;

        }
        else
        {
            return -1;
        }
    }
}

public class SwitchStatement : Statement
{
    public SwitchStatement(Expression _expr, Statement _stmt)
    {
        expr = _expr;
        stmt = _stmt;
    }
    public Expression expr;
    public Statement stmt;
}

public class IfStatement : Statement
{
    public IfStatement(Expression _cond, Statement _stmt)
    {
        cond = _cond;
        stmt = _stmt;
    }
    public Expression cond;
    public Statement stmt;
}

public class IfElseStatement : Statement
{
    public IfElseStatement(Expression _cond, Statement _true_stmt, Statement _false_stmt)
    {
        cond = _cond;
        true_stmt = _true_stmt;
        false_stmt = _false_stmt;
    }
    public Expression cond;
    public Statement true_stmt;
    public Statement false_stmt;
}


// labeled_statement : identifier : statement
//                   | case constant_expression : statement
//                   | default : statement
public class _labeled_statement : IPTNode
{
    public static int Parse(List<Token> src, int begin, out Statement stmt)
    {
        stmt = null;

        int current;
        if (Parser.IsKeyword(src[begin], KeywordValues.DEFAULT))
        {
            current = begin + 1;

            // match ':'
            if (!Parser.IsCOLON(src[current]))
            {
                return -1;
            }
            current++;

            // match statement
            current = _statement.Parse(src, current, out stmt);
            if (current == -1)
            {
                return -1;
            }

            stmt = new CaseStatement(null, stmt);
            return current;

        }
        else if (Parser.IsKeyword(src[begin], KeywordValues.CASE))
        {
            current = begin + 1;

            // match expr
            current = _constant_expression.Parse(src, current, out Expression expr);
            if (current == -1)
            {
                return -1;
            }

            // match ':'
            if (!Parser.IsCOLON(src[current]))
            {
                return -1;
            }
            current++;

            // match statement
            current = _statement.Parse(src, current, out stmt);
            if (current == -1)
            {
                return -1;
            }

            stmt = new CaseStatement(expr, stmt);
            return current;

        }
        else if (src[begin].type == TokenType.IDENTIFIER)
        {
            String label = ((TokenIdentifier)src[begin]).val;
            current = begin + 1;

            // match ':'
            if (!Parser.IsCOLON(src[current]))
            {
                return -1;
            }
            current++;

            // match statement
            current = _statement.Parse(src, current, out stmt);
            if (current == -1)
            {
                return -1;
            }

            stmt = new LabeledStatement(label, stmt);
            return current;

        }
        else
        {
            return -1;
        }
    }
}

public class LabeledStatement : Statement
{
    public LabeledStatement(String _label, Statement _stmt)
    {
        label = _label;
        stmt = _stmt;
    }
    public String label;
    public Statement stmt;
}

public class CaseStatement : Statement
{
    public CaseStatement(Expression _expr, Statement _stmt)
    {
        expr = _expr;
        stmt = _stmt;
    }
    // expr == null means 'default'
    public Expression expr;
    public Statement stmt;
}