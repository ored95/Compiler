    .text
    # fn awesome_add: long (long, long)
    .globl awesome_add
awesome_add:
    pushl %ebp
    movl %esp, %ebp
    movl 8(%ebp), %eax
    pushl %eax
    movl 12(%ebp), %eax
    movl %eax, %ebx
    popl %eax
    addl %ebx, %eax
    lea 0(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    # fn awesome_cmp: long (void *, void *)
    .globl awesome_cmp
awesome_cmp:
    pushl %ebp
    movl %esp, %ebp
    movl 8(%ebp), %eax
    movl 0(%eax), %eax
    pushl %eax
    movl 12(%ebp), %eax
    movl 0(%eax), %eax
    movl %eax, %ebx
    popl %eax
    subl %ebx, %eax
    lea 0(%ebp), %esp
    jmp .L3
.L3:
    leave
    ret



    # fn main: long (...)
    .globl main
main:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] fp : long (*)(long, long)
    subl $32, %esp # [AUTO] arr : long [8]
    movl $1, %eax
    movl %eax, -36(%ebp)
    lea -36(%ebp), %esp
    movl $6, %eax
    movl %eax, -32(%ebp)
    lea -36(%ebp), %esp
    movl $3, %eax
    movl %eax, -28(%ebp)
    lea -36(%ebp), %esp
    movl $8, %eax
    movl %eax, -24(%ebp)
    lea -36(%ebp), %esp
    movl $4, %eax
    movl %eax, -20(%ebp)
    lea -36(%ebp), %esp
    movl $5, %eax
    movl %eax, -16(%ebp)
    lea -36(%ebp), %esp
    movl $0, %eax
    movl %eax, -12(%ebp)
    lea -36(%ebp), %esp
    movl $2, %eax
    movl %eax, -8(%ebp)
    lea -36(%ebp), %esp
    subl $4, %esp # [AUTO] i : long
    lea -4(%ebp), %eax
    pushl %eax
    lea awesome_add, %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -40(%ebp), %esp
    lea -4(%ebp), %eax
    pushl %eax
    movl $awesome_add, %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -40(%ebp), %esp

    # Before pushing the arguments, stack size = 40.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -44
    movl $2, %eax
    movl %eax, -44(%ebp)

    # Argument 0 is at -48
    movl $1, %eax
    movl %eax, -48(%ebp)

    lea -48(%ebp), %esp
    movl -4(%ebp), %eax
    call *%eax
    # Function returned.

    lea -40(%ebp), %esp

    # Before pushing the arguments, stack size = 40.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -44
    movl $2, %eax
    movl %eax, -44(%ebp)

    # Argument 0 is at -48
    movl $1, %eax
    movl %eax, -48(%ebp)

    lea -48(%ebp), %esp
    movl -4(%ebp), %eax
    call *%eax
    # Function returned.

    lea -40(%ebp), %esp

    # Before pushing the arguments, stack size = 40.
    # Arguments take 16 bytes.
    subl $16, %esp

    # Argument 3 is at -44
    lea awesome_cmp, %eax
    movl %eax, -44(%ebp)

    # Argument 2 is at -48
    movl $4, %eax
    movl %eax, -48(%ebp)

    # Argument 1 is at -52
    movl $8, %eax
    movl %eax, -52(%ebp)

    # Argument 0 is at -56
    lea -36(%ebp), %eax
    movl %eax, -56(%ebp)

    lea -56(%ebp), %esp
    lea qsort, %eax
    call *%eax
    # Function returned.

    lea -40(%ebp), %esp
    lea -40(%ebp), %eax
    pushl %eax
    movl $0, %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -40(%ebp), %esp
.L5:
    movl -40(%ebp), %eax
    pushl %eax
    movl $8, %eax
    movl %eax, %ebx
    popl %eax
    cmpl %ebx, %eax
    setl %al
    movzbl %al, %eax
    lea -40(%ebp), %esp
    testl %eax, %eax
    jz .L6

    # Before pushing the arguments, stack size = 40.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -44
    lea -36(%ebp), %eax
    pushl %eax
    movl -40(%ebp), %eax
    pushl %eax
    movl $4, %eax
    movl %eax, %ebx
    popl %eax
    imul %ebx
    movl %eax, %ebx
    popl %eax
    addl %ebx, %eax
    movl 0(%eax), %eax
    movl %eax, -44(%ebp)

    # Argument 0 is at -48
    lea .LC0, %eax
    movl %eax, -48(%ebp)

    lea -48(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -40(%ebp), %esp
.L7:
    lea -40(%ebp), %eax
    pushl %eax
    movl -40(%ebp), %eax
    popl %ecx
    movl %eax, %ebx
    addl $1, %eax
    movb %al, 0(%ecx)
    lea -40(%ebp), %esp
    jmp .L5
.L6:
    movl $0, %eax
    lea -40(%ebp), %esp
    jmp .L4
.L4:
    leave
    ret

    .section .rodata
.LC0:
    .String "%d\t"
