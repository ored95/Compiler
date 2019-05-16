    .data
    .align 4
s_init:
    .long 1
    .long 2

    .local s
    .comm s,8,4

    .globl e_init
    .align 4
e_init:
    .long 1
    .long 2

    .comm e,8,4


    .text
    # fn main: long (...)
    .globl main
main:
    pushl %ebp
    movl %esp, %ebp

    # Before pushing the arguments, stack size = 0.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -4
    movl $s_init, %eax
    movl 0(%eax), %eax
    movl %eax, -4(%ebp)

    # Argument 0 is at -8
    lea .LC0, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea 0(%ebp), %esp

    # Before pushing the arguments, stack size = 0.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -4
    movl $s, %eax
    movl 0(%eax), %eax
    movl %eax, -4(%ebp)

    # Argument 0 is at -8
    lea .LC1, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea 0(%ebp), %esp

    # Before pushing the arguments, stack size = 0.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -4
    movl $e_init, %eax
    movl 0(%eax), %eax
    movl %eax, -4(%ebp)

    # Argument 0 is at -8
    lea .LC2, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea 0(%ebp), %esp

    # Before pushing the arguments, stack size = 0.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -4
    movl $e, %eax
    movl 0(%eax), %eax
    movl %eax, -4(%ebp)

    # Argument 0 is at -8
    lea .LC3, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea 0(%ebp), %esp
    movl $0, %eax
    lea 0(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    .section .rodata
.LC0:
    .String "%d\n"
.LC1:
    .String "%d\n"
.LC2:
    .String "%d\n"
.LC3:
    .String "%d\n"
