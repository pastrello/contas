-- Referência didática. O esquema efetivamente executado está em Caixa.Core/Data/Database.cs.
CREATE TABLE accounts (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    code TEXT NOT NULL UNIQUE,
    number TEXT NOT NULL DEFAULT '',
    name TEXT NOT NULL,
    opening_balance_cents INTEGER NOT NULL DEFAULT 0,
    -- Campos de compatibilidade com bancos criados por versões anteriores.
    legacy_stored_real_cents INTEGER,
    legacy_stored_cleared_cents INTEGER,
    is_active INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE histories (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    code TEXT NOT NULL UNIQUE,
    description TEXT NOT NULL,
    direction INTEGER NOT NULL CHECK(direction IN (-1,1)),
    is_active INTEGER NOT NULL DEFAULT 1
);

CREATE TABLE transactions (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    account_id INTEGER NOT NULL REFERENCES accounts(id),
    date TEXT NOT NULL,
    document_number TEXT NOT NULL DEFAULT '',
    history_id INTEGER NOT NULL REFERENCES histories(id),
    amount_cents INTEGER NOT NULL CHECK(amount_cents >= 0),
    is_cleared INTEGER NOT NULL DEFAULT 0,
    is_archived INTEGER NOT NULL DEFAULT 0,
    -- Campo interno de compatibilidade com bancos existentes.
    legacy_rolled_into_opening_balance INTEGER NOT NULL DEFAULT 0,
    description TEXT NOT NULL DEFAULT '',
    legacy_source TEXT,
    legacy_record_number INTEGER
);
