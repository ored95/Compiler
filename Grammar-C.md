```c
# FILE:         c.cfg
# PURPOSE:      The Grammar for Standard C with _opt factored out,
#               and the oversight for enumeration-constant corrected.
#
# LANGUAGE:     C
# TARGET:       C++
# Note: default the TITLE:  to Cfg
#
# NOTE:		This grammar is closely related to rc.cfg
#		which used as input to awk
#
# TRANSCRIBED:  McKeeman @ WangInst     1986
# MODIFIED:     {0} McKeeman -- 89.08.15 -- original
# 		{1} Aurenz   -- 89.09.07 -- rc parser complete
# 		{2} Aki      -- 92.01.06 -- support awk processing
# 		{3} McKeeman -- 92.02.25 -- restore ANSI details
# 		{4} McKeeman -- 93.01.08 -- removed : after lhs
#
#Input format for c.cfg:
#
#  1) comments must have a '#' in column 1, 
#     or be an entirely empty line
#
#  2) The format of a rule is:
#       lhs
#           rhs1
#           rhs2
#           ...
#           rhsN
#
#     The left hand side must start with a non-blank in column 1.
#
#     The right hand side(s) must start with a blank in column 1.
#     The r.h.s. must be on one line.
#     Blanks must be used to separate tokens in the r.h.s.
#
#  3) An empty rhs is specified with the predefined keyword:
#        _E_M_P_T_Y_R_U_L_E_
#     as the only token on the line.
#
#[begin example]
#
# ,---- column 1
# |
# v
#
# # rule xxx
# xxx
#     yyy + yyy
#     zzz xxx
#
# # rule yyy (1st alternative is empty)
# yyy
#     _E_M_P_T_Y_R_U_L_E_
#     yyy - xxx
#
# # rule zzz (empty)
# zzz
#     _E_M_P_T_Y_R_U_L_E_
#
#[end example]
# 


# 
# C expression rules
# 

primary-expression
    identifier
    constant
    string-literal
    ( expression )

postfix-expression
    primary-expression
    postfix-expression [ expression ]
    postfix-expression ( )
    postfix-expression ( argument-expression-list )
    postfix-expression . identifier
    postfix-expression -> identifier
    postfix-expression ++
    postfix-expression --

argument-expression-list
    assignment-expression
    argument-expression-list , assignment-expression

unary-expression
    postfix-expression
    ++ unary-expression
    -- unary-expression
    unary-operator cast-expression
    sizeof unary-expression
    sizeof ( type-name )

unary-operator
    &
    *
    +
    -
    ~
    !

cast-expression
    unary-expression
    ( type-name ) cast-expression

multiplicative-expression
    cast-expression
    multiplicative-expression * cast-expression
    multiplicative-expression / cast-expression
    multiplicative-expression % cast-expression

additive-expression
    multiplicative-expression
    additive-expression + multiplicative-expression
    additive-expression - multiplicative-expression

shift-expression
    additive-expression
    shift-expression << additive-expression
    shift-expression >> additive-expression

relational-expression
    shift-expression
    relational-expression < shift-expression
    relational-expression > shift-expression
    relational-expression <= shift-expression
    relational-expression >= shift-expression

equality-expression
    relational-expression
    equality-expression == relational-expression
    equality-expression != relational-expression

AND-expression
    equality-expression
    AND-expression & equality-expression

exclusive-OR-expression
    AND-expression
    exclusive-OR-expression ^ AND-expression

inclusive-OR-expression
    exclusive-OR-expression
    inclusive-OR-expression | exclusive-OR-expression

logical-AND-expression
    inclusive-OR-expression
    logical-AND-expression && inclusive-OR-expression

logical-OR-expression
    logical-AND-expression
    logical-OR-expression || logical-AND-expression

conditional-expression
    logical-OR-expression
    logical-OR-expression ? expression : conditional-expression

assignment-expression
    conditional-expression
    unary-expression assignment-operator assignment-expression

assignment-operator
    =
    *=
    /=
    %=
    +=
    -=
    <<=
    >>=
    &=
    ^=
    |=

expression
    assignment-expression
    expression , assignment-expression

constant-expression
    conditional-expression

#
# C declaration rules
#

declaration
    declaration-specifiers ;
    declaration-specifiers init-declarator-list ;

declaration-specifiers
    storage-class-specifier
    type-specifier
    type-qualifier
    storage-class-specifier declaration-specifiers
    type-specifier          declaration-specifiers
    type-qualifier          declaration-specifiers

init-declarator-list
    init-declarator
    init-declarator-list , init-declarator

init-declarator
    declarator
    declarator = initializer

storage-class-specifier
    typedef
    extern
    static
    auto
    register

type-specifier
    void
    char
    short
    int
    long
    float
    double
    signed
    unsigned
    struct-or-union-specifier
    enum-specifier
    typedef-name

struct-or-union-specifier
    struct-or-union { struct-declaration-list }
    struct-or-union identifier { struct-declaration-list }
    struct-or-union identifier

struct-or-union
    struct
    union

struct-declaration-list
    struct-declaration
    struct-declaration-list struct-declaration

struct-declaration
    specifier-qualifier-list struct-declarator-list ;

specifier-qualifier-list
    type-specifier
    type-qualifier
    type-specifier specifier-qualifier-list 
    type-qualifier specifier-qualifier-list 

struct-declarator-list
    struct-declarator
    struct-declarator-list , struct-declarator

struct-declarator
    declarator
    constant-expression
    declarator  constant-expression

enum-specifier
    enum { enumerator-list }
    enum identifier { enumerator-list }
    enum identifier

enumerator-list
    enumerator
    enumerator-list , enumerator

enumerator
    enumeration-constant
    enumeration-constant = constant-expression

enumeration-constant
    identifier

type-qualifier
    const
    volatile

declarator
    direct-declarator
    pointer direct-declarator

direct-declarator
    identifier
    ( declarator )
    direct-declarator [ ]
    direct-declarator [ constant-expression ]
    direct-declarator ( )
    direct-declarator ( parameter-type-list )
    direct-declarator ( identifier-list )

pointer
     *
     * pointer
     * type-qualifier-list
     * type-qualifier-list pointer

type-qualifier-list
    type-qualifier
    type-qualifier-list type-qualifier

parameter-type-list
    parameter-list
    parameter-list , ...

parameter-list
    parameter-declaration
    parameter-list , parameter-declaration

parameter-declaration
    declaration-specifiers declarator
    declaration-specifiers
    declaration-specifiers abstract-declarator

identifier-list
    identifier
    identifier-list , identifier

type-name
    specifier-qualifier-list
    specifier-qualifier-list abstract-declarator

abstract-declarator
    pointer
    direct-abstract-declarator
    pointer direct-abstract-declarator

direct-abstract-declarator
    ( abstract-declarator )
    [ ]
    [ constant-expression ]
    ( )
    ( parameter-type-list )
    direct-abstract-declarator [ ]
    direct-abstract-declarator [ constant-expression ]
    direct-abstract-declarator ( )
    direct-abstract-declarator ( parameter-type-list )

typedef-name
    identifier

initializer
    assignment-expression
    { initializer-list }
    { initializer-list , }

initializer-list
    initializer
    initializer-list , initializer

#
# C statement rules
#

statement
    labeled-statement
    compound-statement
    expression-statement
    selection-statement
    iteration-statement
    jump-statement

labeled-statement
    identifier : statement
    case constant-expression : statement
    default : statement

compound-statement
    { }
    { declaration-list }
    { statement-list }
    { declaration-list statement-list }

declaration-list
    declaration
    declaration-list declaration

statement-list
    statement
    statement-list statement

expression-statement
    ;
    expression ;

selection-statement
    if ( expression ) statement
    if ( expression ) statement else statement
    switch ( expression ) statement

iteration-statement
    while ( expression ) statement
    do statement while ( expression ) ;
    for (            ;            ;            ) statement
    for (            ;            ; expression ) statement
    for (            ; expression ;            ) statement
    for (            ; expression ; expression ) statement
    for ( expression ;            ;            ) statement
    for ( expression ;            ; expression ) statement
    for ( expression ; expression ;            ) statement
    for ( expression ; expression ; expression ) statement

jump-statement
    goto identifier ;
    continue ;
    break ;
    return ;
    return expression ;

translation-unit
    external-declaration
    translation-unit external-declaration

external-declaration
    function-definition
    declaration

function-definition
                           declarator                  compound-statement
    declaration-specifiers declarator                  compound-statement
                           declarator declaration-list compound-statement
    declaration-specifiers declarator declaration-list compound-statement

```