# ShoppingCartApp

![.NET 6](https://img.shields.io/badge/.NET-6.0-blue)
![MySQL](https://img.shields.io/badge/MySQL-8.0-orange)
![License](https://img.shields.io/badge/license-MIT-green)

## Descrição

O **ShoppingCartApp** é uma aplicação desenvolvida em C# usando .NET 6 e MySQL, que permite a gestão de carrinhos de compras. Com esta aplicação, é possível criar, atualizar, remover e listar itens de um carrinho de forma eficiente e organizada. A arquitetura é construída com princípios de Clean Architecture, promovendo modularidade e manutenibilidade do código.

## Funcionalidades

- Criar um novo carrinho de compras.
- Adicionar, atualizar e remover itens do carrinho.
- Listar todos os itens em um carrinho específico.
- Atualizar informações gerais do carrinho.
- Remover carrinhos de compras.

## Tecnologias Utilizadas

- **C#**
- **.NET 6**
- **MySQL**

## Pré-requisitos

Antes de começar, certifique-se de ter os seguintes itens instalados:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [MySQL](https://dev.mysql.com/downloads/mysql/)
- [MySQL Workbench](https://www.mysql.com/products/workbench/) (opcional, para gerenciamento do banco de dados)

## Configuração do Projeto

1. **Clone o repositório**:

   ```bash
   git clone https://github.com/seu-usuario/ShoppingCartApp.git
   cd ShoppingCartApp
   ```
2. **Configurar o Banco de Dados:**

* Crie um novo banco de dados no MySQL, por exemplo, ShoppingCartDb.
* Atualize a string de conexão no arquivo appsettings.json:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ShoppingCartDb;User=root;Password=sua_senha;"
}
```

3. **Instalar as dependências:**

Execute o seguinte comando na raiz do projeto para restaurar as dependências do NuGet:

```bash
dotnet restore
```

4. **Aplicar as Migrations:**

Para criar as tabelas no banco de dados, execute o comando:

```bash
dotnet ef database update
```

## Rodando o Projeto

1. **Iniciar a aplicação:**

No terminal, navegue até a pasta do projeto e execute:
```bash
dotnet run
```

2. **Testar os Endpoints:**

Use uma ferramenta como Postman ou curl para testar os endpoints disponíveis na API:

* **Criar um novo carrinho:** POST /api/Cart
* **Adicionar um item ao carrinho:** POST /api/Cart/{cartId}/items
* **Listar itens do carrinho:** GET /api/Cart/{cartId}/items
* **Atualizar um item do carrinho:** PUT /api/Cart/{cartId}/items/{itemId}
* **Remover um item do carrinho:** DELETE /api/Cart/{cartId}/items/{itemId}
* **Atualizar o carrinho:** PUT /api/Cart/{cartId}
* **Remover o carrinho:** DELETE /api/Cart/{cartId}
* **Consultar um carrinho específico:** GET /api/Cart/{cartId}
