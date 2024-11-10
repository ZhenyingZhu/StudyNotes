## PPSSPP
Build: http://forums.ppsspp.org/showthread.php?tid=5231

Start window: 
- PPSSPPWindows:main.cpp, EmuThread.cpp

Start PSP game: 
- Core:Core/System.PSP_InitStart
- PPSSPPWindows:Input/InputDevice.RunInputThread

PSP kernel:
- Core:HLE/HLE.__KernelInit
- Core:HLE/Kernel/sceKernel.__KernelDoState

Draw:
- GPU:Common/

GEDebugger:
- PPSSPPWindows:Windows/GE Debugger/GEDebugger