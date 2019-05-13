using System;
using System.Collections.Generic;

public class TokenKeyword : Token
{
    public TokenKeyword()
    {
        type = TokenType.KEYWORD;
    }
    public KeywordValues val;

    public static Dictionary<string, KeywordValues> keywords = new Dictionary<string, KeywordValues>(StringComparer.InvariantCultureIgnoreCase) {
        { "AUTO",        KeywordValues.AUTO      },
        { "DOUBLE",      KeywordValues.DOUBLE    },
        { "INT",         KeywordValues.INT       },
        { "STRUCT",      KeywordValues.STRUCT    },
        { "BREAK",       KeywordValues.BREAK     },
        { "ELSE",        KeywordValues.ELSE      },
        { "LONG",        KeywordValues.LONG      },
        { "SWITCH",      KeywordValues.SWITCH    },
        { "CASE",        KeywordValues.CASE      },
        { "ENUM",        KeywordValues.ENUM      },
        { "REGISTER",    KeywordValues.REGISTER  },
        { "TYPEDEF",     KeywordValues.TYPEDEF   },
        { "CHAR",        KeywordValues.CHAR      },
        { "EXTERN",      KeywordValues.EXTERN    },
        { "RETURN",      KeywordValues.RETURN    },
        { "UNION",       KeywordValues.UNION     },
        { "CONST",       KeywordValues.CONST     },
        { "FLOAT",       KeywordValues.FLOAT     },
        { "SHORT",       KeywordValues.SHORT     },
        { "UNSIGNED",    KeywordValues.UNSIGNED  },
        { "CONTINUE",    KeywordValues.CONTINUE  },
        { "FOR",         KeywordValues.FOR       },
        { "SIGNED",      KeywordValues.SIGNED    },
        { "VOID",        KeywordValues.VOID      },
        { "DEFAULT",     KeywordValues.DEFAULT   },
        { "GOTO",        KeywordValues.GOTO      },
        { "SIZEOF",      KeywordValues.SIZEOF    },
        { "VOLATILE",    KeywordValues.VOLATILE  },
        { "DO",          KeywordValues.DO        },
        { "IF",          KeywordValues.IF        },
        { "STATIC",      KeywordValues.STATIC    },
        { "WHILE",       KeywordValues.WHILE     }
    };

    public override string ToString()
    {
        return type.ToString() + ": " + val.ToString();
    }

}
