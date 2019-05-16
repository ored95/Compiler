

    .text
    # fn cons: struct Node *(long, struct Node *)
    .globl cons
cons:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] hd : struct Node *

    # Before pushing the arguments, stack size = 4.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -8
    movl $8, %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea malloc, %eax
    call *%eax
    # Function returned.

    movl %eax, -4(%ebp)
    lea -4(%ebp), %esp
    movl -4(%ebp), %eax
    addl $0, %eax
    pushl %eax
    movl 8(%ebp), %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -4(%ebp), %esp
    movl -4(%ebp), %eax
    addl $4, %eax
    pushl %eax
    movl 12(%ebp), %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -4(%ebp), %esp
    movl -4(%ebp), %eax
    lea -4(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    # fn print_list: void (struct Node *)
    .globl print_list
print_list:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] cur : struct Node *
    movl 8(%ebp), %eax
    movl %eax, -4(%ebp)
    lea -4(%ebp), %esp
.L4:
    movl -4(%ebp), %eax
    pushl %eax
    movl $0, %eax
    movl %eax, %ebx
    popl %eax
    cmpl %ebx, %eax
    setne %al
    movzbl %al, %eax
    lea -4(%ebp), %esp
    testl %eax, %eax
    jz .L5

    # Before pushing the arguments, stack size = 4.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -8
    movl -4(%ebp), %eax
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
.L6:
    lea -4(%ebp), %eax
    pushl %eax
    movl -4(%ebp), %eax
    movl 4(%eax), %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -4(%ebp), %esp
    jmp .L4
.L5:
.L3:
    leave
    ret

    # fn print_list_recursive: void (struct Node *)
    .globl print_list_recursive
print_list_recursive:
    pushl %ebp
    movl %esp, %ebp
    movl 8(%ebp), %eax
    pushl %eax
    movl $0, %eax
    movl %eax, %ebx
    popl %eax
    cmpl %ebx, %eax
    setne %al
    movzbl %al, %eax
    lea 0(%ebp), %esp
    testl %eax, %eax
    jz .L8

    # Before pushing the arguments, stack size = 0.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -4
    movl 8(%ebp), %eax
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
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -4
    movl 8(%ebp), %eax
    movl 4(%eax), %eax
    movl %eax, -4(%ebp)

    lea -4(%ebp), %esp
    lea print_list_recursive, %eax
    call *%eax
    # Function returned.

    lea 0(%ebp), %esp
.L8:
.L7:
    leave
    ret

    # fn flip_list: void *(struct Node *)
    .globl flip_list
flip_list:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] cur : struct Node *
    movl 8(%ebp), %eax
    movl %eax, -4(%ebp)
    lea -4(%ebp), %esp
    lea 8(%ebp), %eax
    pushl %eax
    movl $0, %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -4(%ebp), %esp
.L10:
    movl -4(%ebp), %eax
    pushl %eax
    movl $0, %eax
    movl %eax, %ebx
    popl %eax
    cmpl %ebx, %eax
    setne %al
    movzbl %al, %eax
    lea -4(%ebp), %esp
    testl %eax, %eax
    jz .L11
    subl $4, %esp # [AUTO] next : struct Node *
    movl -4(%ebp), %eax
    movl 4(%eax), %eax
    movl %eax, -8(%ebp)
    lea -8(%ebp), %esp
    movl -4(%ebp), %eax
    addl $4, %eax
    pushl %eax
    movl 8(%ebp), %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -8(%ebp), %esp
    lea 8(%ebp), %eax
    pushl %eax
    movl -4(%ebp), %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -8(%ebp), %esp
    lea -4(%ebp), %eax
    pushl %eax
    movl -8(%ebp), %eax
    popl %ebx
    movl %eax, 0(%ebx)
    lea -8(%ebp), %esp
    jmp .L10
.L11:
    movl 8(%ebp), %eax
    lea -8(%ebp), %esp
    jmp .L9
.L9:
    leave
    ret

    # fn main: long (...)
    .globl main
main:
    pushl %ebp
    movl %esp, %ebp
    subl $4, %esp # [AUTO] hd : struct Node *

    # Before pushing the arguments, stack size = 4.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -8

    # Before pushing the arguments, stack size = 12.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -16

    # Before pushing the arguments, stack size = 20.
    # Arguments take 8 bytes.
    subl $8, %esp

    # Argument 1 is at -24
    movl $0, %eax
    movl %eax, -24(%ebp)

    # Argument 0 is at -28
    movl $0, %eax
    movl %eax, -28(%ebp)

    lea -28(%ebp), %esp
    lea cons, %eax
    call *%eax
    # Function returned.

    movl %eax, -16(%ebp)

    # Argument 0 is at -20
    movl $1, %eax
    movl %eax, -20(%ebp)

    lea -20(%ebp), %esp
    lea cons, %eax
    call *%eax
    # Function returned.

    movl %eax, -8(%ebp)

    # Argument 0 is at -12
    movl $2, %eax
    movl %eax, -12(%ebp)

    lea -12(%ebp), %esp
    lea cons, %eax
    call *%eax
    # Function returned.

    movl %eax, -4(%ebp)
    lea -4(%ebp), %esp

    # Before pushing the arguments, stack size = 4.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -8
    movl -4(%ebp), %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea print_list, %eax
    call *%eax
    # Function returned.

    lea -4(%ebp), %esp
    lea -4(%ebp), %eax
    pushl %eax

    # Before pushing the arguments, stack size = 8.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -12
    movl -4(%ebp), %eax
    movl %eax, -12(%ebp)

    lea -12(%ebp), %esp
    lea flip_list, %eax
    call *%eax
    # Function returned.

    movl -8(%ebp), %ebx
    movl %eax, 0(%ebx)
    lea -4(%ebp), %esp

    # Before pushing the arguments, stack size = 4.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -8
    movl -4(%ebp), %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea print_list, %eax
    call *%eax
    # Function returned.

    lea -4(%ebp), %esp

    # Before pushing the arguments, stack size = 4.
    # Arguments take 4 bytes.
    subl $4, %esp

    # Argument 0 is at -8
    movl -4(%ebp), %eax
    movl %eax, -8(%ebp)

    lea -8(%ebp), %esp
    lea print_list_recursive, %eax
    call *%eax
    # Function returned.

    lea -4(%ebp), %esp
    movl $0, %eax
    lea -4(%ebp), %esp
    jmp .L12
.L12:
    leave
    ret

    .section .rodata
.LC0:
    .String "%d\t"
.LC1:
    .String "%d\t"
