    .text
    # fn foo: long (...)
foo:
    pushl %ebp
    movl %esp, %ebp
    movl $0, %eax
    lea 0(%ebp), %esp
    jmp .L2
.L2:
    leave
    ret

    # fn bar: long (...)
    .globl bar
bar:
    pushl %ebp
    movl %esp, %ebp
    movl $0, %eax
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

    # Before pushing the arguments, stack size = 0.
    # Arguments take 0 bytes.
    subl $0, %esp

    lea 0(%ebp), %esp
    lea foo, %eax
    call *%eax
    # Function returned.

    lea 0(%ebp), %esp

    # Before pushing the arguments, stack size = 0.
    # Arguments take 0 bytes.
    subl $0, %esp

    lea 0(%ebp), %esp
    lea bar, %eax
    call *%eax
    # Function returned.

    lea 0(%ebp), %esp
    movl $0, %eax
    lea 0(%ebp), %esp
    jmp .L4
.L4:
    leave
    ret

    .section .rodata
