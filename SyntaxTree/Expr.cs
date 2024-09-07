using System.Collections.Generic;

namespace Clox;

public abstract class Expr
{
  public class Binary : Expr
  {
    private Expr _left;
    private Token _op;
    private Expr _right;
    public Binary(Expr left, Token op, Expr right)
    {
      _left = left;
      _op = op;
      _right = right;
    }
  }
  public class Grouping : Expr
  {
    private Expr _expression;
    public Grouping(Expr expression)
    {
      _expression = expression;
    }
  }
  public class Literal : Expr
  {
    private Object _value;
    public Literal(Object value)
    {
      _value = value;
    }
  }
  public class Unary : Expr
  {
    private Token _op;
    private Expr _right;
    public Unary(Token op, Expr right)
    {
      _op = op;
      _right = right;
    }
  }
}
