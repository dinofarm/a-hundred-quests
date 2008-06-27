@echo off

REM Currently the build fails on the first build but succeeds
REM on the second. I assume that something is built in the
REM wrong order.
msbuild.exe OHQ.sln /p:Configuration=Release
msbuild.exe OHQ.sln /p:Configuration=Release
msbuild.exe OHQ.sln /p:Configuration=Debug
msbuild.exe OHQ.sln /p:Configuration=Debug

cd OHQ\bin\x86
..\..\..\zip ..\..\..\Release_%DATE%.zip -r Release\*
..\..\..\zip ..\..\..\Debug_%DATE%.zip -r Debug\*
cd ..\..\..
