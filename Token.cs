namespace Clox;

public class Token
{
  internal readonly TokenType _type;
  internal readonly string _lexeme;
  internal readonly object _literal;
  internal readonly int _line;

  public Token(TokenType type, string lexeme, object literal, int line)
  {
    _type = type;
    _lexeme = lexeme;
    _literal = literal;
    _line = line;
  }

    public override string ToString()
    {
        return _type + " " + _lexeme + " " + _literal;
    }

}