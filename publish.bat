@echo off
setlocal
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0tools\maintenance\publish-zh.ps1" %*
exit /b %errorlevel%
