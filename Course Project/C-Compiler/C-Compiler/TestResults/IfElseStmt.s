
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
    subl $4, %esp # [AUTO] b : long
    movl $4, %eax
    movl %eax, -8(%ebp)
    lea -8(%ebp), %esp
    movl -4(%ebp), %eax
    pushl %eax
    movl -8(%ebp), %eax
    movl %eax, %ebx
    popl %eax
    cmpl %ebx, %eax
    setg %al
    movzbl %al, %eax
    lea -8(%ebp), %esp
    testl %eax, %eax
    jz .L3
    lea -4(%ebp), %eax
    pushl %eax
    movl $3, %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -8(%ebp), %esp

    # Before pushing the arguments, stack size = 8.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -12
    lea .LC0, %eax
    movl %eax, -12(%ebp)

    lea -12(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -8(%ebp), %esp
    jmp .L4
.L3:

    # Before pushing the arguments, stack size = 8.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -12
    lea .LC1, %eax
    movl %eax, -12(%ebp)

    lea -12(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -8(%ebp), %esp
.L4:
    movl -4(%ebp), %eax
    pushl %eax
    movl -8(%ebp), %eax
    movl %eax, %ebx
    popl %eax
    cmpl %ebx, %eax
    setle %al
    movzbl %al, %eax
    lea -8(%ebp), %esp
    testl %eax, %eax
    jz .L5

    # Before pushing the arguments, stack size = 8.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -12
    lea .LC2, %eax
    movl %eax, -12(%ebp)

    lea -12(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -8(%ebp), %esp
    jmp .L6
.L5:

    # Before pushing the arguments, stack size = 8.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -12
    lea .LC3, %eax
    movl %eax, -12(%ebp)

    lea -12(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -8(%ebp), %esp
.L6:
    movl $0, %eax
    lea -8(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    .section .rodata
.LC0:
    .String "a > b\n"
.LC1:
    .String "a <= b\n"
.LC2:
    .String "a <= b\n"
.LC3:
    .String "a > b\n"
