# 💼 Advance Request - Sistema de Antecipação de Parcelas

Este projeto foi desenvolvido como parte de um desafio técnico, com o objetivo de construir um sistema onde um **usuário pode se cadastrar**, **cadastrar contratos**, e **solicitar antecipações de parcelas** — seguindo os requisitos propostos.

## ✨ Funcionalidades

### 🧑 Cadastro e Autenticação
- Registro de novos usuários via e-mail e senha
- 
### 📄 Contratos
- Criação de contratos com valor total e número de parcelas (máx. 12)
- Cálculo automático do valor de cada parcela
- Listagem de contratos do usuário
- Visualização das parcelas vinculadas

### ⏩ Solicitação de Antecipação
- Usuário pode solicitar a antecipação de parcelas com vencimento maior que 30 dias
- Exibição de parcelas disponíveis para antecipar por contrato
- Histórico de solicitações realizadas

### ✅ Aprovação de Antecipações
- Visualização de todas as solicitações de antecipação
- Seleção múltipla via `DataGrid` para aprovar solicitações
- Atualização automática do status das antecipações aprovadas

## 🧱 Tecnologias Utilizadas

- **Backend**: ASP.NET Core (.NET 8), Entity Framework, PostgreSQL
- **Frontend**: React + TypeScript, Material UI, Axios, React Router
- **Swagger** para testes de API

## 🚀 Como Rodar o Projeto

### Backend (.NET)

```bash
cd ./backend
dotnet restore
dotnet run
