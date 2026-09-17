@echo off
setlocal
cd /d "%~dp0"

set OUT=%CD%\publish\win-x64

echo =====================================================
echo  Caixa Modernizado - Publicacao Windows x64
echo =====================================================
echo.
echo Destino: %OUT%
echo.

if exist "%OUT%" rmdir /s /q "%OUT%"

dotnet publish src\Caixa.WinForms\Caixa.WinForms.csproj ^
  -c Release ^
  -r win-x64 ^
  --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -p:DebugType=None ^
  -p:DebugSymbols=false ^
  -o "%OUT%"

if errorlevel 1 (
  echo.
  echo ERRO: a publicacao nao foi concluida.
  pause
  exit /b 1
)

echo.
echo Publicacao concluida.
echo Copie o conteudo de:
echo %OUT%
echo para a CPU de teste.
echo.
start "" "%OUT%"
pause
