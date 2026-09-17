# 01 - Arquitetura

## Objetivo

Separar a interface da regra financeira e do acesso ao banco. A separação reduz acoplamento entre interface, persistência e regras financeiras.

```text
WinForms
   │
   ▼
CaixaApplication
   │
   ├── Repositories ─── SQLite
   │
   └── Services
       ├── BalanceService
       ├── ReportService
       └── BackupService
```

## Caixa.WinForms

Responsável apenas por interação com o usuário:

- menus;
- formulários;
- grades (`DataGridView`);
- escolha de datas/contas/históricos;
- apresentação de erros e resultados;
- exportação CSV;
- seleção do destino de backup e do ZIP a restaurar.

A tela não conhece SQL.

## Caixa.Core

### Models

Classes que representam as entidades: `Account`, `HistoryItem`, `CashTransaction` etc.

### Data

`Database.cs` cria o arquivo SQLite, tabelas, constraints e índices.

### Repositories

Concentram o SQL CRUD. Uma tela nunca deve escrever `SELECT`, `INSERT`, `UPDATE` ou `DELETE` diretamente.

### Services

Implementam regras que envolvem mais do que um simples CRUD.

- `BalanceService`: cálculo dos saldos;
- `ReportService`: extratos e relatórios;
- `BackupService`: snapshot consistente, ZIP, validação, restauração e rollback preventivo.
