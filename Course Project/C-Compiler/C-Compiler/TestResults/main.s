
    .text
    # fn main: long (long, char **)
    .globl main
main:
    pushl %ebp
    movl %esp, %ebp

    # Before pushing the arguments, stack size = 0.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -4
    movl 8(%ebp), %eax
    movl %eax, -4(%ebp)

    # Argument 0 is at -8
    lea .LC0, %eax
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
    .String "%d"
