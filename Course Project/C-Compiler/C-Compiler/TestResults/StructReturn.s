    .text
    # fn foo: struct S (...)
    .globl foo
foo:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] s : struct S
    movl $1, %eax
    movl %eax, -4(%ebp)
    lea -4(%ebp), %esp
    lea -4(%ebp), %eax
    movl %eax, %esi
    movl 8(%ebp), %edi
    movl $4, %ecx
    movb %cl, %al
    shrl $2, %ecx
    cld
    rep movsl
    movb %al, %cl
    andb $3, %cl
    rep movsb
    movl 8(%ebp), %eax
    lea -4(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret


    # fn main: long (...)
    .globl main
main:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] s : struct S

    # Before pushing the arguments, stack size = 4.
    # Allocate space for returning stack.
    subl $4, %esp
    movl %esp, %eax
    # Arguments take 4 bytes.
    subl $4, %esp

    # Putting extra argument for struct return address.
    movl %eax, 0(%esp)

    lea -12(%ebp), %esp
    lea foo, %eax
    call *%eax
    # Function returned.

    movl %eax, %esi
    lea -4(%ebp), %edi
    movl $4, %ecx
    movb %cl, %al
    shrl $2, %ecx
    cld
    rep movsl
    movb %al, %cl
    andb $3, %cl
    rep movsb
    lea -4(%ebp), %esp

    # Before pushing the arguments, stack size = 4.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -8
    lea -4(%ebp), %eax
    movl 0(%eax), %eax
    movl %eax, -8(%ebp)

    # Argument 0 is at -12
    lea .LC0, %eax
    movl %eax, -12(%ebp)

    lea -12(%ebp), %esp
    lea printf, %eax
    call *%eax
    # Function returned.

    lea -4(%ebp), %esp
.L3:
    leave
    ret

    .section .rodata
.LC0:
    .String "%d"
