    .text
    # fn foo: long (...)
    .globl foo
foo:
    pushl %ebp
    movl %esp, %ebp
    movl $0, %eax
    lea 0(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    # fn main: long (...)
    .globl main
main:
    pushl %ebp
    movl %esp, %ebp

    # Before pushing the arguments, stack size = 0.
    # Arguments take 0 bytes.
    subl $0, %esp

    lea 0(%ebp), %esp
    lea foo, %eax
    call *%eax
    # Function returned.

    lea 0(%ebp), %esp
    movl $0, %eax
    lea 0(%ebp), %esp
    jmp .L3
.L3:
    leave
    ret

    .section .rodata
