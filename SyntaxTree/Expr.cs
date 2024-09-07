using System.Collections.Generic;

namespace Clox;

public abstract class Expr
{
  public abstract T Accept<T>(IVisitor<T> visitor);
  public interface IVisitor<T>
  {
    T VisitBinaryExpr(Binary Expr);
    T VisitGroupingExpr(Grouping Expr);
    T VisitLiteralExpr(Literal Expr);
    T VisitUnaryExpr(Unary Expr);
  }
  public class Binary : Expr
  {
    public Expr Left { get; }
    public Token Op { get; }
    public Expr Right { get; }
    public Binary(Expr left, Token op, Expr right)
    {
      Left = left;
      Op = op;
      Right = right;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.VisitBinaryExpr(this);
    }
  }
  public class Grouping : Expr
  {
    public Expr Expression { get; }
    public Grouping(Expr expression)
    {
      Expression = expression;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.VisitGroupingExpr(this);
    }
  }
  public class Literal : Expr
  {
    public Object Value { get; }
    public Literal(object value)
    {
      Value = value;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.VisitLiteralExpr(this);
    }
  }
  public class Unary : Expr
  {
    public Token Op { get; }
    public Expr Right { get; }
    public Unary(Token op, Expr right)
    {
      Op = op;
      Right = right;
    }
    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.VisitUnaryExpr(this);
    }
  }
}
