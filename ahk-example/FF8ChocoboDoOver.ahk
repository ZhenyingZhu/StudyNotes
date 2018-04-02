#Persistent  ; Keep this script running until the user explicitly exits it. / not needed since hotkeys are used, making it persistent

DpadUp = Up

u::
Send {x down}
Sleep, 50
Send {x up}

Sleep, 100

Send {x down}
Sleep, 50
Send {x up}

Sleep, 100

Send {%DpadUp% down}
Sleep, 50
Send {%DpadUp% up}


