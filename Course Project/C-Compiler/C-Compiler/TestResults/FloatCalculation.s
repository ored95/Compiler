
    .text
    # fn main: long (...)
    .globl main
main:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] f1 : float
    subl $4, %esp # [AUTO] f2 : float
    subl $8, %esp # [AUTO] d1 : double
    subl $8, %esp # [AUTO] d2 : double
    lea -4(%ebp), %eax
    pushl %eax
    flds .LC0
    popl %ebx
    fsts 0(%ebx)
    lea -24(%ebp), %esp
    lea -8(%ebp), %eax
    pushl %eax
    flds .LC1
    popl %ebx
    fsts 0(%ebx)
    lea -24(%ebp), %esp
    lea -16(%ebp), %eax
    pushl %eax
    fldl .LC2
    popl %ebx
    fstl 0(%ebx)
    lea -24(%ebp), %esp
    lea -24(%ebp), %eax
    pushl %eax
    fldl .LC3
    popl %ebx
    fstl 0(%ebx)
    lea -24(%ebp), %esp

    # Before pushing the arguments, stack size = 24.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -28
    flds -4(%ebp)
    subl $4, %esp
    fstps 0(%esp)
    flds -8(%ebp)
    fldl -36(%ebp)
    addl $4, %esp
    faddp
    fstpl -28(%ebp)

    # Argument 0 is at -32
    lea .LC4, %eax
    movl %eax, -32(%ebp)

    lea -32(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -24(%ebp), %esp

    # Before pushing the arguments, stack size = 24.
    # Arguments take 12 bytes.
    subl $12, %esp

    # Argument 1 is at -32
    fldl -16(%ebp)
    subl $8, %esp
    fstpl 0(%esp)
    fldl -24(%ebp)
    fldl -44(%ebp)
    addl $8, %esp
    fdivp
    fstpl -32(%ebp)

    # Argument 0 is at -36
    lea .LC5, %eax
    movl %eax, -36(%ebp)

    lea -36(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -24(%ebp), %esp

    # Before pushing the arguments, stack size = 24.
    # Arguments take 12 bytes.
    subl $12, %esp

    # Argument 1 is at -32
    flds -4(%ebp)
    subl $8, %esp
    fstpl 0(%esp)
    fldl -16(%ebp)
    fldl -44(%ebp)
    addl $8, %esp
    fmulp
    fstpl -32(%ebp)

    # Argument 0 is at -36
    lea .LC6, %eax
    movl %eax, -36(%ebp)

    lea -36(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -24(%ebp), %esp

    # Before pushing the arguments, stack size = 24.
    # Arguments take 12 bytes.
    subl $12, %esp

    # Argument 1 is at -32
    flds -8(%ebp)
    subl $8, %esp
    fstpl 0(%esp)
    fldl -24(%ebp)
    fldl -44(%ebp)
    addl $8, %esp
    fsubp
    fstpl -32(%ebp)

    # Argument 0 is at -36
    lea .LC7, %eax
    movl %eax, -36(%ebp)

    lea -36(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -24(%ebp), %esp
    movl $0, %eax
    lea -24(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    .section .rodata
    .align 4
.LC0:
    .long 1075138527
    .align 4
.LC1:
    .long 1107637043
    .align 8
.LC2:
    .long 1115938332
    .long 1072939210
    .align 8
.LC3:
    .long 1284724362
    .long 1073923059
.LC4:
    .String "lf\n"
.LC5:
    .String "lf\n"
.LC6:
    .String "lf\n"
.LC7:
    .String "lf\n"
