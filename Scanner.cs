using System.Collections.Generic;

namespace Clox;

public class Scanner
{
  private readonly string _source;
  private List<Token> _tokens = new();
  private int _start = 0;
  private int _current = 0;
  private int _line = 1;

  private static readonly Dictionary<string, TokenType> _keywords = new()
  {
    {"and", TokenType.AND},
    {"class", TokenType.CLASS},
    {"else", TokenType.ELSE},
    {"false", TokenType.FALSE},
    {"for", TokenType.FOR},
    {"fun", TokenType.FUN},
    {"if", TokenType.IF},
    {"nil", TokenType.NIL},
    {"or", TokenType.OR},
    {"print", TokenType.PRINT},
    {"return", TokenType.RETURN},
    {"super", TokenType.SUPER},
    {"this", TokenType.THIS},
    {"true", TokenType.TRUE},
    {"var", TokenType.VAR},
    {"while", TokenType.WHILE},
  };

  public Scanner(string source)
  {
    _source = source;
  }

  public List<Token> ScanTokens()
  {
    while (!IsAtEnd())
    {
      _start = _current;
      ScanToken();
    }

    _tokens.Add(new Token(TokenType.EOF, "", null, _line));
    return _tokens;

  }

  private bool IsAtEnd()
  {
    return _current >= _source.Length;
  }

  private void ScanToken()
  {
    char c = Advance();
    switch (c)
    {
      case '(': AddToken(TokenType.LEFT_PAREN); break;
      case ')': AddToken(TokenType.RIGHT_PAREN); break;
      case '{': AddToken(TokenType.LEFT_BRACE); break;
      case '}': AddToken(TokenType.RIGHT_BRACE); break;
      case ',': AddToken(TokenType.COMMA); break;
      case '.': AddToken(TokenType.DOT); break;
      case '-': AddToken(TokenType.MINUS); break;
      case '+': AddToken(TokenType.PLUS); break;
      case ';': AddToken(TokenType.SEMICOLON); break;
      case '*': AddToken(TokenType.STAR); break;
      case '!':
        AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
        break;
      case '=':
        AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
        break;
      case '<':
        AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
        break;
      case '>':
        AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
        break;
      case '/':
        if (Match('/'))
        {
          while (Peek() != '\n' && !IsAtEnd())
          {
            Advance();
          }
        }
        else
        {
          AddToken(TokenType.SLASH);
        }
        break;
      case ' ':
      case '\r':
      case '\t':
        break;
      case '\n':
        _line++;
        break;
      case '"':
        String();
        break;
      default:
        if (IsDigit(c))
        {
          Number();
        }
        else if (IsAlpha(c))
        {
          Identifier();
        }
        else
        {
          Program.Error(_line, "Unexpected character.");
        }
        break;
    }
  }

  private bool IsDigit(char c)
  {
    return c >= '0' && c <= '9';
  }

  private void Number()
  {
    while (IsDigit(Peek()))
    {
      Advance();
    }

    if (Peek() == '.' && IsDigit(PeekNext()))
    {
      // consume .
      Advance();
      while (IsDigit(Peek()))
      {
        Advance();
      }
    }

    AddToken(TokenType.NUMBER, Double.Parse(_source.Substring(_start, _current - _start)));
  }

  private bool IsAlpha(char c)
  {
    return
     (c >= 'a' && c <= 'z') ||
     (c >= 'A' && c <= 'Z') ||
     (c == '_');
  }

  private bool IsAlphaNumeric(char c)
  {
    return IsAlpha(c) || IsDigit(c);
  }

  public void Identifier()
  {
    while (IsAlphaNumeric(Peek()))
    {
      Advance();
    }

    var text = _source.Substring(_start, _current - _start);
    var isKeyword = _keywords.TryGetValue(text, out var type);
    if (!isKeyword)
    {
      type = TokenType.IDENTIFIER;
    }

    AddToken(type);
  }

  private void String()
  {
    while (Peek() != '"' && !IsAtEnd())
    {
      if (Peek() == '\n')
      {
        _line++;
      }
      Advance();
    }

    if (IsAtEnd())
    {
      Program.Error(_line, "Unterminated string.");
      return;
    }
    // closing "
    Advance();
    var value = _source.Substring(_start + 1, _current - 2 - _start);
    AddToken(TokenType.STRING, value);
  }

  private bool Match(char expected)
  {
    if (IsAtEnd())
    {
      return false;
    }

    if (_source.ElementAt(_current) != expected)
    {
      return false;
    }

    _current++;
    return true;
  }

  private char Peek()
  {
    if (IsAtEnd())
    {
      return '\0';
    }
    return _source.ElementAt(_current);
  }

  private char PeekNext()
  {
    if (_current + 1 > _source.Length)
    {
      return '\0';
    }

    return _source.ElementAt(_current + 1);
  }

  private char Advance()
  {
    _current++;
    return _source.ElementAt(_current - 1);
  }

  private void AddToken(TokenType type)
  {
    AddToken(type, null);
  }

  private void AddToken(TokenType type, object literal)
  {
    string text = _source.Substring(_start, _current - _start);
    _tokens.Add(new Token(type, text, literal, _line));
  }
}