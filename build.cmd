@echo off

REM Gurtle - IBugTraqProvider for Google Code
REM Copyright (c) 2008 Atif Aziz. All rights reserved.
REM
REM  Author(s):
REM
REM      Atif Aziz, http://www.raboof.com
REM
REM This library is free software; you can redistribute it and/or modify it 
REM under the terms of the New BSD License, a copy of which should have 
REM been delivered along with this distribution.
REM
REM THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
REM "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
REM LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
REM PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
REM OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
REM SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
REM LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
REM DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
REM THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
REM (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
REM OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

pushd "%~dp0"
SetLocal EnableDelayedExpansion


:: get the revision of the working copy
SubWCRev . src\version.in src\version.txt
:: load the version numbers into variables
for /F "delims=, tokens=1,2,3,4" %%i in (src\version.txt) do (
	set majorversion=%%i
	set minorversion=%%j
	set microversion=%%k
	set wcversion=%%l
)
:: write the AssemblyInfoVersion.cs file with the version info
echo using System.Reflection; > src\Gurtle\Properties\AssemblyInfoVersion.cs
echo [assembly: AssemblyVersion("%majorversion%.%minorversion%.%microversion%.%wcversion%")] >> src\Gurtle\Properties\AssemblyInfoVersion.cs
echo [assembly: AssemblyFileVersion("%majorversion%.%minorversion%.%microversion%.%wcversion%")] >> src\Gurtle\Properties\AssemblyInfoVersion.cs
:: write the VersionNumberInclude.wxi file
echo ^<?xml version="1.0" encoding="utf-8"?^> > src\setup\VersionNumberInclude.wxi
echo ^<Include Id="VersionNumberInclude"^> >> src\setup\VersionNumberInclude.wxi
echo 	^<?define MajorVersion="%majorversion%" ?^> >> src\setup\VersionNumberInclude.wxi
echo 	^<?define MinorVersion="%minorversion%" ?^> >> src\setup\VersionNumberInclude.wxi
echo 	^<?define MicroVersion="%microversion%" ?^> >> src\setup\VersionNumberInclude.wxi
echo 	^<?define BuildVersion="%wcversion%" ?^> >> src\setup\VersionNumberInclude.wxi
echo ^</Include^> >> src\setup\VersionNumberInclude.wxi

for %%i in (Debug Release) do (
    "%SystemRoot%\Microsoft.NET\Framework\v3.5\msbuild" /p:Configuration=%%i /p:Platform=x86 src\Gurtle.sln
    "%SystemRoot%\Microsoft.NET\Framework\v3.5\msbuild" /p:Configuration=%%i /p:Platform=x64 src\Gurtle.sln
)

:: build the installer
pushd src\setup
for %%a in (x86 x64) do (
    echo Building setup for %%a platform
    set Platform=%%a
    ..\..\tools\WiX\candle -nologo -out ..\..\bin\Setup-%%a.wixobj Setup.wxs 
    ..\..\tools\WiX\light -nologo -sice:ICE08 -sice:ICE09 -sice:ICE32 -sice:ICE61 -out ..\..\bin\Gurtle-%majorversion%.%minorversion%.%microversion%.%wcversion%-%%a.msi ..\..\bin\Setup-%%a.wixobj -ext WixUIExtension -cultures:en-us
    ..\..\tools\WiX\candle -nologo -out ..\..\bin\MergeModule-%%a.wixobj MergeModule.wxs 
    ..\..\tools\WiX\light -nologo -sice:ICE08 -sice:ICE09 -sice:ICE32 -sice:ICE61 -out ..\..\bin\Gurtle-%majorversion%.%minorversion%.%microversion%.%wcversion%-%%a.msm ..\..\bin\MergeModule-%%a.wixobj
)
popd
del bin\*.wixobj
del bin\*.wixpdb

:end

popd
