# 03 - Banco SQLite

## accounts

Armazena as contas financeiras.

Campos principais:

- `code`;
- `number`;
- `name`;
- `opening_balance_cents`;
- `is_active`.

Existem também campos internos de compatibilidade mantidos para bancos já criados em versões anteriores. Eles não fazem parte do fluxo normal da aplicação.

## histories

Armazena os históricos/tipos de lançamento.

Além de código e descrição, contém `direction`:

- `1` = entrada;
- `-1` = saída.

## transactions

Armazena todos os lançamentos financeiros em uma única tabela.

Campos principais:

- `account_id`;
- `date`;
- `document_number`;
- `history_id`;
- `amount_cents`;
- `is_cleared`;
- `is_archived`;
- `description`.

## Por que valores em centavos?

Em vez de armazenar dinheiro como ponto flutuante, R$ 123,45 é gravado como `12345`. Isso evita diferenças binárias de arredondamento.

## Índices

O banco possui índices para os filtros mais frequentes, incluindo conta/data, documento, histórico e estado de contabilização/arquivamento.
