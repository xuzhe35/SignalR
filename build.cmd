@echo off
cd %~dp0

IF "%BUILD_SYSTEM_VERSION%" == "" (
  SET BUILD_SYSTEM_VERSION=0.1.0
)

SET BUILD_SYSTEM_PATH=packages\BuildCore.%BUILD_SYSTEM_VERSION%\tools


:::: Download NuGet.exe

IF EXIST .nuget\NuGet.exe GOTO after_download_nuget
echo Downloading latest version of NuGet.exe...
mkdir .nuget
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "((new-object net.webclient).DownloadFile('https://nuget.org/nuget.exe', '.nuget\NuGet.exe'))"
:after_download_nuget


:::: Download BuildCore.nupkg

SET EnableNuGetPackageRestore=true
IF EXIST %HOMEDRIVE%%HOMEPATH%\.nuget\BuildCore.%BUILD_SYSTEM_VERSION%.nupkg (

  :::: Local "build install" copy of BuildCode.nupkg is always used if it exists
  DEL /S /Q packages\BuildCore.%BUILD_SYSTEM_VERSION%
  .nuget\NuGet.exe install BuildCore -version %BUILD_SYSTEM_VERSION% -o packages -Source %HOMEDRIVE%%HOMEPATH%\.nuget

) ELSE (

  :::: Otherwise restore from default sources
  .nuget\NuGet.exe install BuildCore -version %BUILD_SYSTEM_VERSION% -o packages

)


:::: Running build.proj

msbuild %BUILD_SYSTEM_PATH%\tasks\_prepare.proj

IF "%1" == "" (
  msbuild build.proj 
) ELSE (
  msbuild build.proj /t:%*
)
