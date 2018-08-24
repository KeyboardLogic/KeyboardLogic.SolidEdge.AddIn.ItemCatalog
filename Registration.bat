@echo off

set ADDIN_PATH="%~dp0\KeyboardLogic.SolidEdge.AddIn.ItemCatalog.dll"
set REGASM_X86="C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"
set REGASM_X64="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe"

CLS

echo This batch file must be executed with administrator privileges!
echo. 

:menu
echo [Options]
echo 1 Register
echo 2 Unregister
echo 3 Quit

:choice
set /P C=Enter selection:
if "%C%"=="1" goto register
if "%C%"=="2" goto unregister
if "%C%"=="3" goto end
goto choice

:register
echo.
%REGASM_X86% /codebase %ADDIN_PATH%
echo.
%REGASM_X64% /codebase %ADDIN_PATH%
goto end

:unregister
echo.
%REGASM_X86% /u %ADDIN_PATH%
echo.
%REGASM_X64% /u %ADDIN_PATH%
goto end

:end
pause