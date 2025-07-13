#RequireAdmin
#Region ;**** Directives created by AutoIt3Wrapper_GUI ****
#AutoIt3Wrapper_Outfile=statbot.Exe
#AutoIt3Wrapper_UseUpx=y
#EndRegion ;**** Directives created by AutoIt3Wrapper_GUI ****
#include <GUIConstantsEx.au3>
#include <WindowsConstants.au3>
#include <WinAPIEx.au3>
#include <NomadMemory.au3>

Global Const $SS_RIGHT = 2

Global $aProcList[1][3] = [[0, "", 0]]
Global $iCurrentPID = 0
Global $bReadingStats = False ; Prevent re-entrancy
Global $sCurrentSel = ""

; Stat function addresses for STR, DEX, CON, INT, WIS, CHA (from your post)
Global $statFuncAddr[6] = [0x50C890, 0x50C980, 0x50CA70, 0x50CB60, 0x50CC50, 0x50CD40]

; GUI Layout
Local $width = 380, $height = 340
Local $gui = GUICreate("Character Stats", $width, $height)
GUICtrlCreateLabel("Select process:", 20, 20, 90, 22)
Global $cmbProc = GUICtrlCreateCombo("No process found", 115, 18, 150, 24)
Global $btnRefresh = GUICtrlCreateButton("Refresh", 275, 18, 75, 24)
Local $group = GUICtrlCreateGroup("Stats", 20, 55, $width-40, 255)

; Layout positions
Local $statBlockX = 40, $yStart = 80, $rowGap = 35

Global $cb[6], $lbl[6], $names[6] = ["STR","DEX","CON","INT","WIS","CHA"]
For $i = 0 To 5
    $cb[$i] = GUICtrlCreateCheckbox("", $statBlockX, $yStart + $i * $rowGap, 28, 28)
    GUICtrlSetFont($cb[$i], 12, 800, 0, "Segoe UI")
    GUICtrlCreateLabel($names[$i], $statBlockX+35, $yStart+$i*$rowGap+3, 40, 24)
    GUICtrlSetFont(-1, 12, 800, 0, "Segoe UI")
    $lbl[$i] = GUICtrlCreateLabel("-", $statBlockX+100, $yStart+$i*$rowGap+3, 50, 24, $SS_RIGHT)
    GUICtrlSetFont($lbl[$i], 12, 800, 0, "Segoe UI")
Next
GUICtrlCreateGroup("", -99, -99, 1, 1)
GUISetState(@SW_SHOW)

_RefreshProcessList()
AdlibRegister("_AutoUpdateStats", 100)
AdlibRegister("_AutoRefreshProcList", 5000)
AdlibRegister("_RepeatStatCalls", 250)

While 1
    Local $msg = GUIGetMsg()
    Switch $msg
        Case $GUI_EVENT_CLOSE
            ExitLoop
        Case $btnRefresh
            _RefreshProcessList(True)
        Case $cmbProc
            _SelectPIDFromDropdown()
    EndSwitch
WEnd

AdlibUnRegister("_AutoUpdateStats")
AdlibUnRegister("_AutoRefreshProcList")
AdlibUnRegister("_RepeatStatCalls")

; --- Functions ---

Func _RefreshProcessList($bKeepSel = False)
    Local $prevSel = GUICtrlRead($cmbProc), $foundIdx = -1
    Local $tmpProcList[64][3], $count = 0
    Local $wins = WinList()
    For $i = 1 To $wins[0][0]
        If $wins[$i][0] <> "" Then
            Local $hWnd = $wins[$i][1]
            If _GetWindowClassName($hWnd) = "KNG_KProject" Then
                Local $pid = WinGetProcess($hWnd)
                Local $pname = _ProcessGetName($pid)
                If $count >= UBound($tmpProcList) Then ReDim $tmpProcList[$count + 8][3]
                $tmpProcList[$count][0] = $pid
                $tmpProcList[$count][1] = $pname
                $tmpProcList[$count][2] = $hWnd
                $count += 1
            EndIf
        EndIf
    Next

    If $count > 0 Then
        $aProcList = 0 ; Remove old
        Global $aProcList[$count][3]
        For $i = 0 To $count - 1
            $aProcList[$i][0] = $tmpProcList[$i][0]
            $aProcList[$i][1] = $tmpProcList[$i][1]
            $aProcList[$i][2] = $tmpProcList[$i][2]
        Next
    Else
        $aProcList = 0 ; Remove old
        Global $aProcList[1][3]
        $aProcList[0][0] = 0
        $aProcList[0][1] = ""
        $aProcList[0][2] = 0
    EndIf

    Local $comboItems = ""
    For $i = 0 To UBound($aProcList) - 1
        If $aProcList[$i][0] <> 0 Then
            Local $item = $aProcList[$i][1] & " (" & $aProcList[$i][0] & ")"
            $comboItems &= $item & "|"
            If $bKeepSel And $item = $prevSel Then $foundIdx = $i
        EndIf
    Next
    If $comboItems <> "" Then
        $comboItems = StringTrimRight($comboItems, 1)
        Local $selItem
        If $foundIdx <> -1 Then
            $selItem = $aProcList[$foundIdx][1] & " (" & $aProcList[$foundIdx][0] & ")"
            $iCurrentPID = $aProcList[$foundIdx][0]
        Else
            $selItem = $aProcList[0][1] & " (" & $aProcList[0][0] & ")"
            $iCurrentPID = $aProcList[0][0]
        EndIf
        GUICtrlSetData($cmbProc, $comboItems, $selItem)
        _EnableStatControls(True)
    Else
        GUICtrlSetData($cmbProc, "No process found", "No process found")
        _EnableStatControls(False)
        $iCurrentPID = 0
        _ShowNoStats()
    EndIf
EndFunc

Func _EnableStatControls($bEnable)
    For $i = 0 To 5
        GUICtrlSetState($cb[$i], $bEnable ? $GUI_ENABLE : $GUI_DISABLE)
    Next
EndFunc

Func _SelectPIDFromDropdown()
    Local $sel = GUICtrlRead($cmbProc)
    If $sel = "No process found" Or $sel = "" Then
        $iCurrentPID = 0
        _ShowNoStats()
        _EnableStatControls(False)
        Return
    EndIf
    For $i = 0 To UBound($aProcList) - 1
        If StringInStr($sel, $aProcList[$i][0]) Then
            $iCurrentPID = $aProcList[$i][0]
            _EnableStatControls(True)
            Return
        EndIf
    Next
    $iCurrentPID = 0
    _ShowNoStats()
    _EnableStatControls(False)
EndFunc

Func _ShowNoStats()
    For $i = 0 To 5
        GUICtrlSetData($lbl[$i], "-")
    Next
EndFunc

Func _AutoUpdateStats()
    If $bReadingStats Then Return
    $bReadingStats = True

    If $iCurrentPID = 0 Or Not ProcessExists($iCurrentPID) Then
        _ShowNoStats()
        $bReadingStats = False
        Return
    EndIf

    Local $stats = _ReadAllStats($iCurrentPID)
    If IsArray($stats) Then
        For $i = 0 To 5
            GUICtrlSetData($lbl[$i], $stats[$i])
        Next
    Else
        _ShowNoStats()
    EndIf
    $bReadingStats = False
EndFunc

Func _AutoRefreshProcList()
    _RefreshProcessList(True)
EndFunc

Func _ReadAllStats($pid)
    Local $ah_Handle = _MemoryOpen($pid)
    If @error Or $ah_Handle = 0 Then
        Return SetError(1,0,0)
    EndIf

    Local $baseAddr = 0x0071A90C  ; dword_71A90C (holds pointer to stats struct)
    Local $statOffsets[6] = [131, 133, 136, 135, 132, 134] ; STR, DEX, CON, INT, WIS, CHA
    Local $ret[6]

    Local $pStats = _MemoryRead($baseAddr, $ah_Handle, "dword")
    If @error Or $pStats = 0 Then
        _MemoryClose($ah_Handle)
        Return SetError(1,0,0)
    EndIf

    For $i = 0 To 5
        Local $addr = $pStats + $statOffsets[$i]*2
        Local $val = _MemoryRead($addr, $ah_Handle, "ushort")
        If @error Then
            $ret[$i] = "?"
        Else
            $ret[$i] = $val
        EndIf
    Next
    _MemoryClose($ah_Handle)
    Return $ret
EndFunc

Func _ProcessGetName($pid)
    Local $objWMIService = ObjGet("winmgmts:\\.\root\cimv2")
    Local $colItems = $objWMIService.ExecQuery("Select * from Win32_Process Where ProcessId=" & $pid)
    For $objItem In $colItems
        Return $objItem.Name
    Next
    Return ""
EndFunc

Func _GetWindowClassName($hWnd)
    Local $aResult = DllCall("user32.dll", "int", "GetClassName", "hwnd", $hWnd, "str", "", "int", 256)
    If @error Or Not IsArray($aResult) Then Return ""
    Return $aResult[2]
EndFunc

Func _CallRemoteFunc($pid, $absoluteAddress)
    Local $hProc = _MemoryOpen($pid)
    If $hProc = 0 Then
        MsgBox(16, "Error", "Can't open process")
        Return
    EndIf
    Local $ret = DllCall("kernel32.dll", "handle", "CreateRemoteThread", _
        "handle", $hProc[1], _
        "ptr", 0, _
        "dword", 0, _
        "ptr", $absoluteAddress, _
        "ptr", 0, _
        "dword", 0, _
        "ptr", 0)
    _MemoryClose($hProc)
    ; Do not show error msgbox repeatedly, it's spammy!
EndFunc

; --- NEW: Adlib handler to repeat stat calls while checked ---
Func _RepeatStatCalls()
    If $iCurrentPID = 0 Or Not ProcessExists($iCurrentPID) Then Return
    For $i = 0 To 5
        If BitAND(GUICtrlRead($cb[$i]), $GUI_CHECKED) Then
            _CallRemoteFunc($iCurrentPID, $statFuncAddr[$i])
        EndIf
    Next
EndFunc
