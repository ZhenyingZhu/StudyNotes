#Persistent  ; Keep this script running until the user explicitly exits it. / not needed since hotkeys are used, making it persistent

; -Button Assignments-
; (if you want a button to not do anything, leave it blank)
Joy1Button = a  ; Select                    / Default: x  Select                    / Button: A
Joy2Button = b  ; Cancel                    / Default: v  Menu                      / Button: B
Joy3Button = x  ; Card Game                 / Default: s  Card Game                 / Button: X
Joy4Button = y  ; Menu                      / Default: c  Cancel                    / Button: Y
Joy5Button = 1  ; Escape (left)             / Default: h  Rotate Left               / Button: LB
Joy6Button = 2  ; Escape/Switch POV (right) / Default: g  Rotate Right              / Button: RB
Joy7Button = k  ; Toggle Display (Map)      / Default: j  Toggle Display (Map)      / Button: Back/Select
Joy8Button = s  ; Pause                     / Default: a  Pause                     / Button: Start
Joy9Button = l  ; Rotate Left               / Default: d  Escape (left)             / Button: Left Thumbstick
Joy10Button = r ; Rotate Right/Trigger      / Default: f  Escape/Switch POV (right) / Button: Right Thumbstick

; -Trigger Assignments-
LTrigger = F1   ; Speed Boost (I prefer having Speed Boost here for handiness, but l for Rotate Left {h default} may be desired instead)
RTrigger = r    ; Rotate Right/Trigger

; -Dpad & Thumbsticks Assignments-
; diagonals (UpLeft, UpRight, DownLeft, and DownRight) may have 1 input left blank to have only 1 input
DpadUp = Up
DpadDown = Down
DpadLeft = Left
DpadRight = Right
DpadUpRight1 = Up
DpadUpRight2 = Right
DpadDownRight1 = Down
DpadDownRight2 = Right
DpadDownLeft1 = Down
DpadDownLeft2 = Left
DpadUpLeft1 = Up
DpadUpLeft2 = Left

LeftThumbstickUp = Up
LeftThumbstickDown = Down
LeftThumbstickLeft = Left
LeftThumbstickRight = Right
LeftThumbstickUpRight1 = Up
LeftThumbstickUpRight2 = Right
LeftThumbstickDownRight1 = Down
LeftThumbstickDownRight2 = Right
LeftThumbstickDownLeft1 = Down
LeftThumbstickDownLeft2 = Left
LeftThumbstickUpLeft1 = Up
LeftThumbstickUpLeft2 = Left

RightThumbstickUp = Up
RightThumbstickDown = Down
RightThumbstickLeft = Left
RightThumbstickRight = Right
RightThumbstickUpRight1 = Up
RightThumbstickUpRight2 = Right
RightThumbstickDownRight1 = Down
RightThumbstickDownRight2 = Right
RightThumbstickDownLeft1 = Down
RightThumbstickDownLeft2 = Left
RightThumbstickUpLeft1 = Up
RightThumbstickUpLeft2 = Left

; -End of Controller Assignments-


; Montitor non button press inputs
SetTimer, WatchPOV, 5    ; Dpad
SetTimer, WatchXYAxis, 5 ; Left Thumbstick
SetTimer, WatchZAxis, 5  ; Triggers
SetTimer, WatchRUAxis, 5 ; Right Thumbstick
return


; -Dpad-
;
WatchPOV:
GetKeyState, POV, JoyPOV  ; Get position of the POV control.
KeyToHoldDownPrev = %KeyToHoldDown%  ; Prev now holds the key that was down before (if any).
KeyToHoldDownPrev2 = %KeyToHoldDown2%  ; Prev2 now holds the other key that was down before (if any).

if POV < 0   ; No angle to report
{
    KeyToHoldDown =
    KeyToHoldDown2 =
}
else if POV = 0    ; Up
{
    KeyToHoldDown = %DpadUp%
    KeyToHoldDown2 =
}
else if POV = 4500    ; Up-Right diagonal
{
    KeyToHoldDown = %DpadUpRight1%
    KeyToHoldDown2 = %DpadUpRight2%
}
else if POV = 9000    ; Right
{
    KeyToHoldDown = %DpadRight%
    KeyToHoldDown2 =
}
else if POV = 13500    ; Down-Right diagonal
{
    KeyToHoldDown = %DpadDownRight1%
    KeyToHoldDown2 = %DpadDownRight2%
}
else if POV = 18000    ; Down
{
    KeyToHoldDown = %DpadDown%
    KeyToHoldDown2 =
}
else if POV = 22500    ; Down-Left diagonal
{
    KeyToHoldDown = %DpadDownLeft1%
    KeyToHoldDown2 = %DpadDownLeft2%
}
else if POV = 27000    ; Left
{
    KeyToHoldDown = %DpadLeft%
    KeyToHoldDown2 =
}
else    ; Up-Left diagonal
{
    KeyToHoldDown = %DpadUpLeft1%
    KeyToHoldDown2 = %DpadUpLeft2%
}

if KeyToHoldDown = %KeyToHoldDownPrev% && KeyToHoldDown2 = %KeyToHoldDownPrev2% ; The correct key(s) is/are already down (or no key is needed).
    return  ; Do nothing.

; Otherwise, release the previous key(s) and press down the new key(s):
SetKeyDelay -1  ; Avoid delays between keystrokes.
if KeyToHoldDownPrev   ; There is a previous key to release.
    Send, {%KeyToHoldDownPrev% up}  ; Release it.
if KeyToHoldDownPrev2   ; There is another previous key to release.
    Send, {%KeyToHoldDownPrev2% up}  ; Release it.
if KeyToHoldDown   ; There is a key to press down.
    Send, {%KeyToHoldDown% down}  ; Press it down.
if KeyToHoldDown2   ; There is another key to press down.
    Send, {%KeyToHoldDown2% down}  ; Press it down.
return


; -Left Thumbstick-
;
WatchXYAxis:
GetKeyState, JoyX, JoyX  ; Get position of X axis.
GetKeyState, JoyY, JoyY  ; Get position of Y axis.
KeyToHoldDownPrev3 = %KeyToHoldDown3%  ; Prev3 now holds the key that was down before (if any).
KeyToHoldDownPrev4 = %KeyToHoldDown4%  ; Prev4 now holds the other key that was down before (if any).

if (JoyY < 30 && JoyX > 30 && JoyX < 70)    ; Up
{
    KeyToHoldDown3 = %LeftThumbstickUp%
    KeyToHoldDown4 =
}
else if (JoyY < 30 && JoyX > 70)    ; Up-Right diagonal
{
    KeyToHoldDown3 = %LeftThumbstickUpRight1%
    KeyToHoldDown4 = %LeftThumbstickUpRight2%
}
else if (JoyX > 70 && JoyY > 30 && JoyY < 70)    ; Right
{
    KeyToHoldDown3 = %LeftThumbstickRight%
    KeyToHoldDown4 =
}
else if (JoyX > 70 && JoyY > 70)    ; Down-Right diagonal
{
    KeyToHoldDown3 = %LeftThumbstickDownRight1%
    KeyToHoldDown4 = %LeftThumbstickDownRight2%
}
else if (JoyY > 70 && JoyX > 30 && JoyX < 70)    ; Down
{
    KeyToHoldDown3 = %LeftThumbstickDown%
    KeyToHoldDown4 =
}
else if (JoyX < 30 && JoyY > 70)    ; Down-Left diagonal
{
    KeyToHoldDown3 = %LeftThumbstickDownLeft1%
    KeyToHoldDown4 = %LeftThumbstickDownLeft2%
}
else if (JoyX < 30 && JoyY > 30 && JoyY < 70)    ; Left
{
    KeyToHoldDown3 = %LeftThumbstickLeft%
    KeyToHoldDown4 =
}
else if (JoyX < 30 && JoyY < 30)   ; Up-Left diagonal
{
    KeyToHoldDown3 = %LeftThumbstickUpLeft1%
    KeyToHoldDown4 = %LeftThumbstickUpLeft2%
}
else  ; No angle to report
{
    KeyToHoldDown3 =
    KeyToHoldDown4 =
}

if KeyToHoldDown3 = %KeyToHoldDownPrev3% && KeyToHoldDown4 = %KeyToHoldDownPrev4% ; The correct key(s) is/are already down (or no key is needed).
    return  ; Do nothing.

; Otherwise, release the previous key(s) and press down the new key(s):
SetKeyDelay -1  ; Avoid delays between keystrokes.
if KeyToHoldDownPrev3   ; There is a previous key to release.
    Send, {%KeyToHoldDownPrev3% up}  ; Release it.
if KeyToHoldDownPrev4   ; There is another previous key to release.
    Send, {%KeyToHoldDownPrev4% up}  ; Release it.
if KeyToHoldDown3   ; There is a key to press down.
    Send, {%KeyToHoldDown3% down}  ; Press it down.
if KeyToHoldDown4   ; There is another key to press down.
    Send, {%KeyToHoldDown4% down}  ; Press it down.
return


; -Triggers-
;
WatchZAxis:
GetKeyState, JoyZ, JoyZ  ; Get position of Z axis.
KeyToHoldDownPrev5 = %KeyToHoldDown5%  ; Prev5 now holds the key that was down before (if any).

if JoyZ > 70 ; left trigger pressed
    KeyToHoldDown5 = %LTrigger%
else if JoyZ < 30 ; right trigger pressed
    KeyToHoldDown5 = %RTrigger%
else
    KeyToHoldDown5 =

if KeyToHoldDown5 = %KeyToHoldDownPrev5%  ; The correct key is already down (or no key is needed).
{
    if KeyToHoldDown5  ;  There is a key pressed
        Send, {%KeyToHoldDown5% down}  ; Repeat the keystroke
    return
}

; Otherwise, release the previous key and press down the new key:
SetKeyDelay -1  ; Avoid delays between keystrokes.
if KeyToHoldDownPrev5   ; There is a previous key to release.
    Send, {%KeyToHoldDownPrev5% up}  ; Release it.
if KeyToHoldDown5   ; There is a key to press down.
    Send, {%KeyToHoldDown5% down}  ; Press it down.
return


; -Right Thumbstick-
;
WatchRUAxis:
GetKeyState, JoyR, JoyR  ; Get position of R axis.
GetKeyState, JoyU, JoyU  ; Get position of U axis.
KeyToHoldDownPrev6 = %KeyToHoldDown6%  ; Prev6 now holds the key that was down before (if any).
KeyToHoldDownPrev7 = %KeyToHoldDown7%  ; Prev7 now holds the other key that was down before (if any).

if (JoyR < 30 && JoyU > 30 && JoyU < 70)    ; Up
{
    KeyToHoldDown6 = %RightThumbstickUp%
    KeyToHoldDown7 =
}
else if (JoyR < 30 && JoyU > 70)    ; Up-Right diagonal
{
    KeyToHoldDown6 = %RightThumbstickUpRight1%
    KeyToHoldDown7 = %RightThumbstickUpRight2%
}
else if (JoyU > 70 && JoyR > 30 && JoyR < 70)    ; Right
{
    KeyToHoldDown6 = %RightThumbstickRight%
    KeyToHoldDown7 =
}
else if (JoyR > 70 && JoyU > 70)    ; Down-Right diagonal
{
    KeyToHoldDown6 = %RightThumbstickDownRight1%
    KeyToHoldDown7 = %RightThumbstickDownRight2%
}
else if (JoyR > 70 && JoyU > 30 && JoyU < 70)    ; Down
{
    KeyToHoldDown6 = %RightThumbstickDown%
    KeyToHoldDown7 =
}
else if (JoyU < 30 && JoyR > 70)    ; Down-Left diagonal
{
    KeyToHoldDown6 = %RightThumbstickDownLeft1%
    KeyToHoldDown7 = %RightThumbstickDownLeft2%
}
else if (JoyU < 30 && JoyR > 30 && JoyR < 70)    ; Left
{
    KeyToHoldDown6 = %RightThumbstickLeft%
    KeyToHoldDown7 =
}
else if (JoyR < 30 && JoyU < 30)   ; Up-Left diagonal
{
    KeyToHoldDown6 = %RightThumbstickUpLeft1%
    KeyToHoldDown7 = %RightThumbstickUpLeft2%
}
else  ; No angle to report
{
    KeyToHoldDown6 =
    KeyToHoldDown7 =
}

if KeyToHoldDown6 = %KeyToHoldDownPrev6% && KeyToHoldDown7 = %KeyToHoldDownPrev7% ; The correct key(s) is/are already down (or no key is needed).
    return  ; Do nothing.

; Otherwise, release the previous key(s) and press down the new key(s):
SetKeyDelay -1  ; Avoid delays between keystrokes.
if KeyToHoldDownPrev6   ; There is a previous key to release.
    Send, {%KeyToHoldDownPrev6% up}  ; Release it.
if KeyToHoldDownPrev7   ; There is another previous key to release.
    Send, {%KeyToHoldDownPrev7% up}  ; Release it.
if KeyToHoldDown6   ; There is a key to press down.
    Send, {%KeyToHoldDown6% down}  ; Press it down.
if KeyToHoldDown7   ; There is another key to press down.
    Send, {%KeyToHoldDown7% down}  ; Press it down.
return


; -Buttons-
;
Joy1::
Send {%Joy1Button% down}
SetTimer, WaitForJoy1, 10
return

Joy2::
Send {%Joy2Button% down}
SetTimer, WaitForJoy2, 10
return

Joy3::
Send {%Joy3Button% down}
SetTimer, WaitForJoy3, 10
return

Joy4::
Send {%Joy4Button% down}
SetTimer, WaitForJoy4, 10
return

Joy5::
Send {%Joy5Button% down}
SetTimer, WaitForJoy5, 10
return

Joy6::
Send {%Joy6Button% down}
SetTimer, WaitForJoy6, 10
return

Joy7::
Send {%Joy7Button% down}
SetTimer, WaitForJoy7, 10
return

Joy8::
Send {%Joy8Button% down}
SetTimer, WaitForJoy8, 10
return

Joy9::
Send {%Joy9Button% down}
SetTimer, WaitForJoy9, 10
return

Joy10::
Send {%Joy10Button% down}
SetTimer, WaitForJoy10, 10
return


; Button Release Monitoring

WaitForJoy1:
if not GetKeyState("Joy1")  ; The button has been released
{
    Send {%Joy1Button% up}  ; Release button
    SetTimer, WaitForJoy1, off  ; Stop monitoring the button.
    return
}
; Since above didn't "return", the button is still being held down.
Send {%Joy1Button% down}  ; Send another keystroke.
return

WaitForJoy2:
if not GetKeyState("Joy2")
{
    Send {%Joy2Button% up}
    SetTimer, WaitForJoy2, off
    return
}
Send {%Joy2Button% down} 
return

WaitForJoy3:
if not GetKeyState("Joy3")
{
    Send {%Joy3Button% up}
    SetTimer, WaitForJoy3, off
    return
}
Send {%Joy3Button% down}
return

WaitForJoy4:
if not GetKeyState("Joy4")
{
    Send {%Joy4Button% up}
    SetTimer, WaitForJoy4, off
    return
}
Send {%Joy4Button% down}
return

WaitForJoy5:
if not GetKeyState("Joy5")
{
    Send {%Joy5Button% up}
    SetTimer, WaitForJoy5, off
    return
}
Send {%Joy5Button% down}
return

WaitForJoy6:
if not GetKeyState("Joy6")
{
    Send {%Joy6Button% up}
    SetTimer, WaitForJoy6, off
    return
}
Send {%Joy6Button% down}
return

WaitForJoy7:
if not GetKeyState("Joy7")
{
    Send {%Joy7Button% up}
    SetTimer, WaitForJoy7, off
    return
}
Send {%Joy7Button% down}
return

WaitForJoy8:
if not GetKeyState("Joy8")
{
    Send {%Joy8Button% up}
    SetTimer, WaitForJoy8, off
    return
}
Send {%Joy8Button% down}
return

WaitForJoy9:
if not GetKeyState("Joy9")
{
    Send {%Joy9Button% up}
    SetTimer, WaitForJoy9, off
    return
}
Send {%Joy9Button% down}
return

WaitForJoy10:
if not GetKeyState("Joy10")
{
    Send {%Joy10Button% up}
    SetTimer, WaitForJoy10, off
    return
}
Send {%Joy10Button% down}
return