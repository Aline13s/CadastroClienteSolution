-- =======================================================================================
-- Banco de Dados: SistemaClientesDB
-- Este script cria (caso não exista) toda a estrutura de banco de dados para o sistema de clientes.
-- Cada bloco de criação verifica existência antes de criar (idempotência).
-- Comentários explicam propósito de cada seção e parâmetro.
-- =======================================================================================

-- =============================================
-- 0. Criação da Base de Dados (se não existir)
-- =============================================
IF DB_ID(N'SistemaClientesDB') IS NULL
    CREATE DATABASE [SistemaClientesDB];
GO

USE [SistemaClientesDB];
GO


-- =============================================
-- 1. Esquema de Tabelas
-- =============================================

-- 1.1 Tabela de Categorias
IF OBJECT_ID(N'dbo.Categorias', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categorias (
        CategoriaId INT           IDENTITY(1,1) PRIMARY KEY,  -- PK auto-incremental
        Nome        NVARCHAR(50)  NOT NULL UNIQUE            -- Nome único da categoria
    );
END
GO

-- 1.2 Tabela de Clientes
IF OBJECT_ID(N'dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes (
        ClienteId    INT           IDENTITY(1,1) PRIMARY KEY,  -- PK do cliente
        Nome         NVARCHAR(100) NOT NULL,                    -- Nome completo
        Email        NVARCHAR(200) NULL,                        -- E-mail opcional
        Telefone     NVARCHAR(50)  NULL,                        -- Telefone opcional
        CategoriaId  INT           NOT NULL,                   -- FK para dbo.Categorias
            CONSTRAINT FK_Clientes_Categorias FOREIGN KEY(CategoriaId)
            REFERENCES dbo.Categorias(CategoriaId),
        Ativo        BIT           NOT NULL DEFAULT 1,         -- 1 = ativo, 0 = inativo
        DataCadastro DATETIME      NOT NULL DEFAULT GETDATE()  -- Data/hora da criação
    );
END
GO

-- 1.3 Tabela de Menu
IF OBJECT_ID(N'dbo.Menu', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Menu (
        MenuId   INT           IDENTITY(1,1) PRIMARY KEY,  -- PK do menu
        Chave    NVARCHAR(50)  NOT NULL UNIQUE,            -- Identificador técnico
        Titulo   NVARCHAR(100) NOT NULL                    -- Texto exibido no menu
    );
END
GO

-- 1.4 Tabela de Páginas
IF OBJECT_ID(N'dbo.Paginas', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Paginas (
        PaginaId INT           IDENTITY(1,1) PRIMARY KEY,  -- PK da página
        MenuId   INT           NOT NULL,
            CONSTRAINT FK_Paginas_Menu FOREIGN KEY(MenuId)
            REFERENCES dbo.Menu(MenuId),
        Chave    NVARCHAR(50)  NOT NULL,   -- Identificador técnico
        Titulo   NVARCHAR(100) NOT NULL,   -- Texto exibido na UI
        Rota     NVARCHAR(200) NOT NULL    -- URL da página
    );
END
GO

-- 1.5 Tabela de ControlesPagina
IF OBJECT_ID(N'dbo.ControlesPagina', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ControlesPagina (
        ControleId INT           IDENTITY(1,1) PRIMARY KEY,  -- PK do controle
        PaginaId   INT           NOT NULL,
            CONSTRAINT FK_Controles_Paginas FOREIGN KEY(PaginaId)
            REFERENCES dbo.Paginas(PaginaId),
        Chave      NVARCHAR(50)  NOT NULL,  -- Identificador do botão
        Label      NVARCHAR(50)  NOT NULL   -- Texto do botão
    );
END
GO

-- 1.6 Tabela de FiltrosPagina
IF OBJECT_ID(N'dbo.FiltrosPagina', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.FiltrosPagina (
        FiltroId INT           IDENTITY(1,1) PRIMARY KEY,  -- PK do filtro
        PaginaId INT           NOT NULL,
            CONSTRAINT FK_Filtros_Paginas FOREIGN KEY(PaginaId)
            REFERENCES dbo.Paginas(PaginaId),
        Campo    NVARCHAR(100) NOT NULL,  -- Coluna a filtrar
        Label    NVARCHAR(100) NOT NULL   -- Texto do filtro
    );
END
GO


-- =============================================
-- 2. Procedure sp_CarregarMenuInicial
-- =============================================
-- Remove se existir e recria:
IF OBJECT_ID(N'dbo.sp_CarregarMenuInicial','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CarregarMenuInicial;
GO

CREATE PROCEDURE dbo.sp_CarregarMenuInicial
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @menuId    INT,
            @paginaLst INT,
            @paginaEdt INT;

    -- 2.1) Inserção inicial de categorias
    IF NOT EXISTS(SELECT 1 FROM dbo.Categorias WHERE Nome = 'Ouro')
        INSERT INTO dbo.Categorias(Nome) VALUES('Ouro');
    IF NOT EXISTS(SELECT 1 FROM dbo.Categorias WHERE Nome = 'Prata')
        INSERT INTO dbo.Categorias(Nome) VALUES('Prata');
    IF NOT EXISTS(SELECT 1 FROM dbo.Categorias WHERE Nome = 'Bronze')
        INSERT INTO dbo.Categorias(Nome) VALUES('Bronze');

    -- 2.2) Menu Cliente
    IF NOT EXISTS(SELECT 1 FROM dbo.Menu WHERE Chave = 'Cliente')
    BEGIN
        INSERT INTO dbo.Menu(Chave, Titulo) VALUES('Cliente','Cliente');
        SET @menuId = SCOPE_IDENTITY();
    END
    ELSE
        SET @menuId = (SELECT MenuId FROM dbo.Menu WHERE Chave = 'Cliente');

    -- 2.3) Páginas
    IF NOT EXISTS(SELECT 1 FROM dbo.Paginas WHERE MenuId=@menuId AND Chave='ListarClientes')
        INSERT INTO dbo.Paginas(MenuId,Chave,Titulo,Rota)
        VALUES(@menuId,'ListarClientes','Listar Clientes','/clientes/listar');

    IF NOT EXISTS(SELECT 1 FROM dbo.Paginas WHERE MenuId=@menuId AND Chave='NovoCliente')
        INSERT INTO dbo.Paginas(MenuId,Chave,Titulo,Rota)
        VALUES(@menuId,'NovoCliente','Novo Cliente','/clientes/editar');

    SELECT @paginaLst = PaginaId FROM dbo.Paginas WHERE MenuId=@menuId AND Chave='ListarClientes';
    SELECT @paginaEdt = PaginaId FROM dbo.Paginas WHERE MenuId=@menuId AND Chave='NovoCliente';

    -- 2.4) Controles na listagem
    IF NOT EXISTS(SELECT 1 FROM dbo.ControlesPagina WHERE PaginaId=@paginaLst AND Chave='Novo')
    BEGIN
        INSERT INTO dbo.ControlesPagina(PaginaId,Chave,Label)
        VALUES
          (@paginaLst,'Novo','Novo registro'),
          (@paginaLst,'Pesquisar','Pesquisar'),
          (@paginaLst,'Editar','Editar'),
          (@paginaLst,'Excluir','Excluir');
    END

    -- 2.5) Controles na edição
    IF NOT EXISTS(SELECT 1 FROM dbo.ControlesPagina WHERE PaginaId=@paginaEdt AND Chave='Gravar')
    BEGIN
        INSERT INTO dbo.ControlesPagina(PaginaId,Chave,Label)
        VALUES
          (@paginaEdt,'Gravar','Gravar'),
          (@paginaEdt,'Voltar','Voltar'),
          (@paginaEdt,'Excluir','Excluir');
    END

    -- 2.6) Filtros na listagem
    IF NOT EXISTS(SELECT 1 FROM dbo.FiltrosPagina WHERE PaginaId=@paginaLst AND Campo='Nome')
    BEGIN
        INSERT INTO dbo.FiltrosPagina(PaginaId,Campo,Label)
        VALUES
          (@paginaLst,'Nome','Nome'),
          (@paginaLst,'Categoria','Categoria');
    END
END;
GO

-- Popula dados iniciais
EXEC dbo.sp_CarregarMenuInicial;
GO


-- =============================================
-- 3. View vw_MenuCompleto
-- =============================================
IF OBJECT_ID(N'dbo.vw_MenuCompleto','V') IS NOT NULL
    DROP VIEW dbo.vw_MenuCompleto;
GO

CREATE VIEW dbo.vw_MenuCompleto AS
SELECT
  m.MenuId,
  m.Chave    AS MenuChave,
  m.Titulo   AS MenuTitulo,
  p.PaginaId,
  p.Chave    AS PaginaChave,
  p.Titulo   AS PaginaTitulo,
  p.Rota,
  c.ControleId,
  c.Chave    AS ControleChave,
  c.Label    AS ControleLabel,
  f.FiltroId,
  f.Campo    AS FiltroCampo,
  f.Label    AS FiltroLabel
FROM dbo.Menu m
JOIN dbo.Paginas p              ON p.MenuId = m.MenuId
LEFT JOIN dbo.ControlesPagina c ON c.PaginaId = p.PaginaId
LEFT JOIN dbo.FiltrosPagina f   ON f.PaginaId = p.PaginaId;
GO


-- =============================================
-- 4. Procedures CRUD para Clientes
-- =============================================

-- 4.1 Create
IF OBJECT_ID(N'dbo.sp_CriarCliente','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_CriarCliente;
GO
CREATE PROCEDURE dbo.sp_CriarCliente
  @Nome        NVARCHAR(100),       -- Nome do cliente (obrigatório)
  @Email       NVARCHAR(200) = NULL,-- E-mail (opcional)
  @Telefone    NVARCHAR(50)  = NULL,-- Telefone (opcional)
  @CategoriaId INT,                -- FK para dbo.Categorias
  @Ativo       BIT           = 1    -- Status inicial ativo
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Clientes(Nome,Email,Telefone,CategoriaId,Ativo)
  VALUES(@Nome,@Email,@Telefone,@CategoriaId,@Ativo);
  SELECT SCOPE_IDENTITY() AS ClienteId;  -- Retorna o ID gerado
END;
GO

-- 4.2 Read
IF OBJECT_ID(N'dbo.sp_ObterClientePorId','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ObterClientePorId;
GO
CREATE PROCEDURE dbo.sp_ObterClientePorId
  @ClienteId INT  -- ID do cliente
AS
BEGIN
  SET NOCOUNT ON;
  SELECT
    c.ClienteId, c.Nome, c.Email, c.Telefone, c.CategoriaId,
    cat.Nome AS Categoria, c.Ativo, c.DataCadastro
  FROM dbo.Clientes c
  JOIN dbo.Categorias cat ON c.CategoriaId = cat.CategoriaId
  WHERE c.ClienteId = @ClienteId;
END;
GO

-- 4.3 Update
IF OBJECT_ID(N'dbo.sp_AtualizarCliente','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_AtualizarCliente;
GO
CREATE PROCEDURE dbo.sp_AtualizarCliente
  @ClienteId   INT,                   -- ID do registro
  @Nome        NVARCHAR(100),
  @Email       NVARCHAR(200) = NULL,
  @Telefone    NVARCHAR(50)  = NULL,
  @CategoriaId INT,
  @Ativo       BIT
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Clientes
  SET Nome=@Nome,
      Email=@Email,
      Telefone=@Telefone,
      CategoriaId=@CategoriaId,
      Ativo=@Ativo
  WHERE ClienteId=@ClienteId;
END;
GO

-- 4.4 Delete
IF OBJECT_ID(N'dbo.sp_ExcluirCliente','P') IS NOT NULL
    DROP PROCEDURE dbo.sp_ExcluirCliente;
GO
CREATE PROCEDURE dbo.sp_ExcluirCliente
  @ClienteId INT  -- ID do cliente
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.Clientes WHERE ClienteId=@ClienteId;
END;
GO


-- =============================================
-- 5. Função fn_ListarClientes
-- =============================================
IF OBJECT_ID(N'dbo.fn_ListarClientes','IF') IS NOT NULL
    DROP FUNCTION dbo.fn_ListarClientes;
GO
CREATE FUNCTION dbo.fn_ListarClientes
(
  @FiltroNome NVARCHAR(100) = NULL,
  @FiltroCategoriaId INT = NULL
)
RETURNS TABLE
AS
RETURN
(
  SELECT
    c.ClienteId,
    c.Nome,
    c.Email,
    c.Telefone,
    c.CategoriaId,
    cat.Nome AS Categoria,
    c.Ativo,
    c.DataCadastro
  FROM dbo.Clientes c
  JOIN dbo.Categorias cat ON c.CategoriaId = cat.CategoriaId
  WHERE (@FiltroNome IS NULL OR c.Nome LIKE '%' + @FiltroNome + '%')
    AND (@FiltroCategoriaId IS NULL OR c.CategoriaId = @FiltroCategoriaId)
);
GO

-- Cria uma view que retorna todos os clientes com o nome da categoria
IF OBJECT_ID(N'dbo.vw_Clientes', 'V') IS NOT NULL
    DROP VIEW dbo.vw_Clientes;
GO

CREATE VIEW dbo.vw_Clientes AS
SELECT
  c.ClienteId,
  c.Nome,
  c.Email,
  c.Telefone,
  cat.Nome   AS Categoria,
  c.Ativo,
  c.DataCadastro
FROM dbo.Clientes c
JOIN dbo.Categorias cat
  ON c.CategoriaId = cat.CategoriaId;
GO

