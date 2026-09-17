@echo off
setlocal
cd /d "%~dp0"
echo Restaurando pacotes NuGet...
dotnet restore CaixaModernizado.sln || goto :erro
echo.
echo Compilando em Release...
dotnet build CaixaModernizado.sln -c Release --no-restore || goto :erro
echo.
echo Compilacao concluida.
exit /b 0
:erro
echo.
echo Falha na compilacao. Verifique se o .NET 10 SDK esta instalado.
exit /b 1
