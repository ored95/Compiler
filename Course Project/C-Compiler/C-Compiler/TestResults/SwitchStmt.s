
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
    lea -4(%ebp), %esp
    lea -4(%ebp), %esp
    cmpl $1, %eax
    jz .L3
    cmpl $20, %eax
    jz .L4
    cmpl $300, %eax
    jz .L5
    jmp .L7
.L3:

    # Before pushing the arguments, stack size = 4.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -8
    lea .LC0, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -4(%ebp), %esp
    jmp .L6
.L4:

    # Before pushing the arguments, stack size = 4.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -8
    lea .LC1, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -4(%ebp), %esp
    jmp .L6
.L5:

    # Before pushing the arguments, stack size = 4.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -8
    lea .LC2, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -4(%ebp), %esp
    jmp .L6
.L7:

    # Before pushing the arguments, stack size = 4.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -8
    lea .LC3, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -4(%ebp), %esp
    jmp .L6
.L6:
    lea -4(%ebp), %esp
    movl $0, %eax
    lea -4(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    .section .rodata
.LC0:
    .String "1\n"
.LC1:
    .String "2\n"
.LC2:
    .String "3\n"
.LC3:
    .String "default\n"
