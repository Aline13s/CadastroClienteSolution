# Scripts de Banco de Dados — SistemaClientesDB

Este diretório contém os scripts SQL responsáveis por criar e configurar toda a estrutura do banco de dados utilizado pelo sistema **CadastroCliente**.

## 📁 Estrutura

- `Scripts/SistemaClientesDB.sql`  
  Script principal que:
  - Cria o banco de dados `SistemaClientesDB` (caso não exista)
  - Cria tabelas: `Clientes`, `Categorias`, `Menu`, `Paginas`, `ControlesPagina`, `FiltrosPagina`
  - Cria views: `vw_MenuCompleto`, `vw_Clientes`
  - Cria procedures: `sp_CriarCliente`, `sp_AtualizarCliente`, `sp_ExcluirCliente`, `sp_ObterClientePorId`, `sp_CarregarMenuInicial`
  - Cria função: `fn_ListarClientes`
  - Popula dados iniciais: categorias, menus e páginas

## 🚀 Execução

Você pode executar o script manualmente utilizando o **SQL Server Management Studio (SSMS)** ou via linha de comando com `sqlcmd`.
