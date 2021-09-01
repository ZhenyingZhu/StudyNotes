# Reverse Engineering for Beginners

## Resources

- <https://beginners.re/#lite>
- <https://beginners.re/RE4B-CN-vol1.pdf>
- <https://beginners.re/RE4B-CN-vol2.pdf>

## Vol 1

### Chapter 1 CPU

Concepts

- OP code: transfer data between registers, manage mem, calculation, etc. Each CPU has its ISA (Instruction Set Architecture).
- machine code: the program send to CPU to run.
- Assembly lang: above machine code.
- GPR(General Processor register): fast storage on CPU to store temp var. x64 has 16 64-bit GPRs.

ISA

- x64 inherits opcodes from 8086. They have different length.
- ARM is RISC (reduced instruction computing). opcodes are all 4 bytes (32 bits). But most opcodes don't used up all 4 bytes, so introduce thumb mode, which uses 2 bytes. ARM64 switch back to 4-byte opcodes.

### Chapter 2 Basic Function

```assembly
f:
    mov eax, 123
    ret
```

According to calling convention, the next commend reads the val in EAX reg as output.

mov is actually copy not move.

[registers](https://www.tutorialspoint.com/assembly_programming/assembly_registers.htm)

- **TODO**: Go through

### Chapter 3 Hello World

Compile

- Assembly listing file: compiler creates one listing file for the assembly code. The file extension: `*.asm`.
- Compiler also creates `*.obj` and link it to `*.exe`.

In ASM file:

- CONST segment: contains values
- _TEXT segment: contains code.

`DB`: [data byte](https://stackoverflow.com/questions/17387492/what-does-the-assembly-instruction-db-actually-do). allocate some space and fill it with a string.

```C
#include <stdio.h>

int main()
{
    printf("hello, world\n");
    return 0;
}
```

An implicit const array is created to hold the string by the compiler.

```C
#include <stdio.h>

const char *$SG3830[]="hello, world\n"; // there is a nil (00h) at the end to complete the string in C.

int main()
{
    printf($SG3830);
    return 0;
}
```

```assembly
CONST SEGMENT
$SG3830 DB 'hello World', 0AH, 00H ; $SG3830 is a memory space that stores the string as a char array.
CONST ENDS
PUBLIC _main
EXTRN _printf:PROC

_TEXT SEGMENT
_main PROC
    push ebp
    mov ebp, esp
    push OFFSET $SG3830
    call _printf
    add esp, 4
    xor eax, eax
    pop ebp
    ret 0
_main ENDP
_TEXT ENDS
```

- function prologue: `_main PROC`
- function epilogue: `_main ENDP`
- to call the func: `call`

- `push`: push to the stack. `esp` reg stores the stack pointer.
- `add esp, 4`, since this is x86, and esp has the stack pointer. add 4 bytes skip the pointer, so the pointer content is released.
- `xor eax, eax` is to calculate 0.

HERE: P27
