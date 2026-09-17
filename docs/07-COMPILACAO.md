# 07 - Compilação

## Visual Studio

Abra `CaixaModernizado.sln` e use **Compilar > Compilar Solução**.

A primeira compilação restaura automaticamente os pacotes NuGet.

## Linha de comando

Na raiz do projeto:

```bat
build.cmd
```

Ou manualmente:

```bat
dotnet restore CaixaModernizado.sln
dotnet build CaixaModernizado.sln -c Release
```

## Dependência externa

A aplicação usa:

- `Microsoft.Data.Sqlite` 10.0.11

## Banco de dados

O arquivo não faz parte da solução compilada. É criado na primeira execução em:

`%LOCALAPPDATA%\CaixaModernizado\caixa.db`
