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
