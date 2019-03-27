# Compiler
Introduction to compiler

## Regex - NFA - DFA
@ Regex-to-NFA ([Link](https://en.wikipedia.org/wiki/Thompson%27s_construction)) - transforming a regular expression into an equivalent nondeterministic finite automaton.

@ NFA-to-DFA ([Link 1](http://web.cecs.pdx.edu/~harry/compilers/slides/LexicalPart3.pdf) | [Link 2](https://er.yuvayana.org/nfa-to-dfa-conversion-algorithm-with-solved-example/)) - converting a nondeterministic finite automaton into a equivalent deterministic finite automaton.

@ DFA minimization ([Link](https://en.wikipedia.org/wiki/DFA_minimization)) - transforming a given deterministic finite automaton (DFA) into an equivalent DFA that has a minimum number of states.

**References**

The following links can help you when dealing with regexes:
1. [Regular-Expressions.info](http://www.regular-expressions.info) - Regex Tutorial, Examples and Reference - Regex Patterns
2. Regular Expression Library (great site with lots of regexes and an excellent regex tester) ([Link 1](http://regexlib.com/Default.aspx) | [Link 2](http://regexlib.com/RETester.aspx))
3. DFA minimization ([ITMO Link](http://neerc.ifmo.ru/wiki/index.php?title=%D0%A2%D0%B5%D0%BE%D1%80%D0%B8%D1%8F_%D1%84%D0%BE%D1%80%D0%BC%D0%B0%D0%BB%D1%8C%D0%BD%D1%8B%D1%85_%D1%8F%D0%B7%D1%8B%D0%BA%D0%BE%D0%B2) | [Youtube example](https://www.youtube.com/watch?v=0XaGAkY09Wc))

@ [**Graphviz**](https://www.graphviz.org/) - Drawing graphs with dot.


## CFG

@ What is a CFG? ([Read more](https://www.cs.rochester.edu/~nelson/courses/csc_173/grammars/cfg.html))

@ Left recursion in CFG problem
```
    In the formal language theory of computer science, left recursion is a special case of recursion where a string is recognized as part of a language by the fact that it decomposes into a string from that same language (on the left) and a suffix (on the right).
```
@ Types of left recursion    

1. Direct left recursion
    ```
        A → Aα
    ```
2. Indirect left recursion
    ```
        A → BC
        B → A
    ```
3. Hidden left recursion
    ```
        A → BA
        B → 𝜀
    ```
@ Elimination of Left-Recursion

<p align="center">
  <img width="460" height="380" src="https://web.cs.wpi.edu/~kal/images/PLT/PLTelralgorithm.gif">
</p>

+ External links:
1. https://en.wikipedia.org/wiki/Left_recursion
2. https://web.cs.wpi.edu/~kal/PLT/PLT4.1.2.html
3. [ELR - ITMO University](https://neerc.ifmo.ru/wiki/index.php?title=%D0%A3%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B5%D0%BD%D0%B8%D0%B5_%D0%BB%D0%B5%D0%B2%D0%BE%D0%B9_%D1%80%D0%B5%D0%BA%D1%83%D1%80%D1%81%D0%B8%D0%B8)

+ See also
1. [CYK Algorithm](https://en.wikipedia.org/wiki/CYK_algorithm)
2. [LL Parser](https://en.wikipedia.org/wiki/LL_parser)
