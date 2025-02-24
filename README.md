# ProdutoAPI

Este projeto implementa uma API RESTful simples para gerenciar produtos.
O projeto utiliza o Code-First no Entity Framework Core. Com o Code-First, a estrutura do banco de dados é gerada a partir das classes C# (entidades) que definem a estrutura dos dados. Isso significa que a criação do banco de dados é feita a partir de migrações que refletem as mudanças no modelo de dados.

## Estrutura do Projeto

Este projeto possui os seguintes componentes principais:

- **Produto**: A entidade de dados que representa os produtos no banco de dados.
- **ProdutoDbContext**: O contexto de banco de dados do Entity Framework Core, que gerencia as entidades e a comunicação com o banco de dados.
- **Controllers**: Implementação dos endpoints da API para lidar com as requisições HTTP.
- **Migrations**: Arquivos de criação e atualização do banco de dados.
- **ProdutoAPI.Tests**: Projeto de testes unitários.

## Endpoints Disponíveis
- GET /produtos: Retorna todos os produtos ordenados.
- GET /produtos/{id}: Retorna um produto pelo Id.
- GET /produtos/por-nome?nome={nome}: Retorna produtos filtrados pelo nome.
- POST /produtos: Cadastra um novo produto.
- PUT /produtos/{id}: Atualiza um produto existente pelo Id.
- DELETE /produtos/{id}: Deleta um produto pelo Id.
