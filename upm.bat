@echo off
set /p version=Enter Version: 
echo %version%
git subtree split --prefix="Assets/Android Build Pipeline" --branch upm
git tag %version% upm
pause