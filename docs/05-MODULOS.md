# 05 - Módulos da aplicação

## Painel principal

Mostra cada conta e quatro informações:

- saldo inicial;
- saldo bancário (somente contabilizados);
- saldo real;
- diferença pendente.

## Contas

Cadastro, alteração e exclusão de contas financeiras.

## Históricos

CRUD dos tipos de lançamento e sua direção Entrada/Saída.

## Lançamentos

Permite incluir, alterar, excluir e contabilizar/descontabilizar.

## Extrato

Calcula o saldo acumulado linha a linha. O saldo anterior é obtido a partir do saldo inicial + movimentos anteriores ao período.

## Consistência

Lista lançamentos ainda não contabilizados.

## Previsão de cheques

Lista movimentos não contabilizados classificados como cheques pela regra atual do sistema.

## Baixa / arquivamento

O lançamento continua participando do saldo e permanece auditável.

## Consulta de baixados

Consulta registros arquivados e permite restaurá-los quando necessário.

## Relatório por histórico

Permite marcar vários históricos e consultar por período.

## Backup

Cria um snapshot consistente do SQLite, valida a integridade, compacta em ZIP e permite restauração segura.
