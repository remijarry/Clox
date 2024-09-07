namespace Clox;

using static Clox.TokenType;

public class Parser
{
  private readonly List<Token> _tokens;
  private int current = 0;

  public Parser(List<Token> tokens)
  {
    _tokens = tokens;
  }

  private Expr Expression()
  {
    return Equality();
  }

  private Expr Equality()
  {
    var expr = Comparison();

    while (Match(BANG_EQUAL) || Match(EQUAL_EQUAL))
    {
      Token op = Previous();
      var right = Comparison();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr Comparison()
  {
    var expr = Term();

    while (Match(GREATER, GREATER_EQUAL, LESS, LESS_EQUAL))
    {
      var op = Previous();
      var right = Term();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr Term()
  {
    var expr = Factor();

    while (Match(MINUS, PLUS))
    {
      var op = Previous();
      var right = Factor();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr Factor()
  {
    var expr = Unary();

    while (Match(SLASH, STAR))
    {
      var op = Previous();
      var right = Unary();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr Unary()
  {
    if (Match(BANG, MINUS))
    {
      var op = Previous();
      var right = Unary();
      return new Expr.Unary(op, right);
    }

    return Primary();
  }

  private Expr Primary()
  {
    if (Match(FALSE))
    {
      return new Expr.Literal(false);
    }
    if (Match(TRUE))
    {
      return new Expr.Literal(true);
    }
    if (Match(NIL))
    {
      return new Expr.Literal(null);
    }
    if (Match(NUMBER, STRING))
    {
      return new Expr.Literal(Previous().Literal);
    }
    if (Match(LEFT_PAREN))
    {
      var expr = Expression();
      Consume(RIGHT_PAREN, "Expect ')' after expression.");
      return new Expr.Grouping(expr);
    }

    return null;
  }

  private Token Consume(TokenType type, string message)
  {
    if (Check(type))
    {
      return Advance();
    }

    throw Error(Peek(), message);
  }

  private ParseError Error(Token token, string message)
  {
    Program.Error(token, message);
  }


  private bool Match(params TokenType[] types)
  {
    foreach (var type in types)
    {
      if (Check(type))
      {
        Advance();
        return true;
      }
    }
    return false;
  }

  private bool Check(TokenType type)
  {
    if (IsAtEnd())
    {
      return false;
    }

    return Peek().Type == type;
  }

  private bool IsAtEnd()
  {
    return Peek().Type == EOF;
  }

  private Token Peek()
  {
    return _tokens.ElementAt(current);
  }

  private Token Previous()
  {
    return _tokens.ElementAt(current - 1);
  }

  private Token Advance()
  {
    if (!IsAtEnd())
    {
      current++;
    }

    return Previous();
  }
}