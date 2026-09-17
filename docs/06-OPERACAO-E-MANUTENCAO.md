# Operação e manutenção

## Banco de dados

O banco SQLite é criado automaticamente em `%LOCALAPPDATA%\CaixaModernizado\caixa.db`.

Não copie o arquivo aberto como método de backup. Use as rotinas da própria aplicação.

## Backup

Em **Ferramentas → Criar backup compactado do SQLite**, escolha uma pasta de destino. A aplicação cria um snapshot consistente, valida o banco e grava um arquivo ZIP.

## Restauração

Em **Ferramentas → Restaurar backup do SQLite**, selecione um ZIP criado pela aplicação. Antes de substituir os dados atuais, o programa gera automaticamente um backup preventivo.

## Manutenção

A tela **Manutenção do banco SQLite** disponibiliza:

- `PRAGMA quick_check`;
- `PRAGMA integrity_check`;
- `PRAGMA optimize`;
- `VACUUM`.

O `VACUUM` cria backup preventivo antes de regravar o banco.

## Distribuição

Para gerar uma versão Release self-contained para Windows x64, execute `publish-win-x64.cmd`.
