
    .text
    # fn main: long (...)
    .globl main
main:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] a : long
    movl $3, %eax
    movl %eax, -4(%ebp)
    lea -4(%ebp), %esp
    movl -4(%ebp), %eax
    pushl %eax
    movl $3, %eax
    movl %eax, %ebx
    popl %eax
    cmpl %ebx, %eax
    sete %al
    movzbl %al, %eax
    lea -4(%ebp), %esp
    testl %eax, %eax
    jz .L4
    subl $4, %esp # [AUTO] b : long
    movl $4, %eax
    movl %eax, -8(%ebp)
    lea -8(%ebp), %esp
    jmp .L3
.L4:
.L3:
    lea -8(%ebp), %esp

    # Before pushing the arguments, stack size = 8.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -12
    movl -4(%ebp), %eax
    movl %eax, -12(%ebp)

    # Argument 0 is at -16
    lea .LC0, %eax
    movl %eax, -16(%ebp)

    lea -16(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -8(%ebp), %esp
    movl $0, %eax
    lea -8(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    .section .rodata
.LC0:
    .String "%d\n"
