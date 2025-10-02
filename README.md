# API - Sistema de Caixa de Banco

Esta é uma API RESTful desenvolvida em .NET 8 como solução para o desafio técnico. O sistema permite o cadastro de contas bancárias, consulta, inativação, transferência de valores, consulta de histórico de transações e auditoria automática de operações críticas.

## Tecnologias Utilizadas

* **.NET 8:** A mais recente versão Long-Term Support (LTS) do framework da Microsoft.
* **C# 12:** Linguagem de programação principal.
* **ASP.NET Core 8:** Framework para construção da API web.
* **Entity Framework Core 8:** ORM para a persistência de dados.
* **SQL Server:** Sistema de gerenciamento de banco de dados.
* **MediatR:** Biblioteca para implementação do padrão Mediator, facilitando a aplicação do CQRS e de pipelines de comportamento.
* **xUnit & Moq:** Frameworks para a escrita e execução de testes unitários e para a criação de mocks de dependências.
* **Swagger (Swashbuckle):** Ferramenta para documentação e teste interativo dos endpoints da API.

## Arquitetura e Padrões de Projeto

A solução foi estruturada seguindo os princípios da **Clean Architecture (Arquitetura Limpa)**, garantindo um sistema desacoplado, testável e de fácil manutenção. O fluxo de dependências é sempre direcionado para o centro (Core/Domínio).

### Camadas da Arquitetura

1.  **`Vindi.Core` (Domínio):** O coração da aplicação. Contém as entidades de negócio (`ContaBancaria`, `LogAuditoria`, `HistoricoTransferencia`), enums e as interfaces dos repositórios e da `Unit of Work`. Esta camada não tem dependências de frameworks externos e encapsula toda a lógica de negócio, seguindo um padrão **Domain-Driven Design (DDD) simplificado**.

2.  **`Vindi.Application` (Aplicação):** Orquestra os casos de uso do sistema. [cite_start]Utiliza o padrão **CQRS (Command Query Responsibility Segregation)** para separar as operações de escrita (Commands) das de leitura (Queries). [cite: 923]

3.  **`Vindi.Infrastructure` (Infraestrutura):** Contém as implementações concretas de interfaces definidas nas camadas superiores, como o acesso a dados com Entity Framework Core e a implementação da `Unit of Work`.

4.  **`Vindi.API` (Apresentação):** A camada mais externa, responsável por expor a funcionalidade via uma API REST. Os controllers são "magros" (`thin controllers`), apenas delegando o trabalho para o MediatR.

### Padrões de Destaque

* **Repository & Unit of Work:** O acesso aos dados é abstraído pelo `Repository Pattern`. Para garantir a **atomicidade das transações**, foi implementado o padrão `Unit of Work`. Os repositórios agora apenas adicionam ou modificam entidades na memória (`DbContext`). É a `Unit of Work` que centraliza a responsabilidade de chamar `SaveChangesAsync()`, garantindo que múltiplas operações (ex: debitar de uma conta, creditar em outra e criar um registro de histórico) sejam salvas na mesma transação. Se qualquer parte falhar, nada é persistido.

* **MediatR Pipeline Behaviors para Auditoria:** Para requisitos transversais como **auditoria**, foi utilizada uma abordagem avançada para manter os handlers de negócio limpos. Um `AuditoriaBehavior` genérico intercepta apenas os comandos marcados com uma interface `IAuditavel` e, se a operação for bem-sucedida, cria e persiste o log de auditoria usando a `Unit of Work`.

* **Testes Unitários com xUnit e Moq:** O projeto inclui uma suíte de testes unitários que cobre as camadas de `Core` (Domínio) e `Application`.
    * **Testes de Domínio:** Validam as regras de negócio puras dentro das entidades (ex: `ContaBancaria`), garantindo que a lógica de débitos, créditos e validações de estado funcione corretamente.
    * **Testes de Aplicação:** Utilizam o Moq para simular dependências (repositórios, Unit of Work) e testar a lógica de orquestração dos `Command Handlers` e `Query Handlers`, garantindo que eles interajam corretamente com suas dependências.

* [cite_start]**SOLID & Clean Code:** Os princípios SOLID (Single Responsibility, Open/Closed, etc.) e as práticas de Clean Code foram a base para a organização das classes, a nomeação de variáveis e a estruturação dos métodos. [cite: 499, 500, 501]

## Estrutura do Projeto

A estrutura de pastas reflete a separação de responsabilidades da Clean Architecture:

```
/
├── Vindi.SistemaBancario.sln
├── Vindi.API/
├── Vindi.Application/
├── Vindi.Core/
├── Vindi.Infrastructure/
└── Vindi.Tests/
    ├── Application/
    └── Core/
```

## Como Executar o Projeto Localmente

### Pré-requisitos
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads)

### Passos
1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/jacksonccosta/Sistema_Caixa_de_Banco.git](https://github.com/jacksonccosta/Sistema_Caixa_de_Banco.git)
    cd Sistema_Caixa_de_Banco
    ```

2.  **Configure a Connection String:**
    Abra o arquivo `Vindi.API/appsettings.json` e altere a `DefaultConnection` para apontar para sua instância do SQL Server.

3.  **Aplique as Migrations do Banco de Dados:**
    Abra um terminal na raiz do projeto e execute o comando abaixo. Ele criará o banco de dados e todas as tabelas (`ContasBancarias`, `LogsAuditoria`, `HistoricoTransferencias`).
    ```bash
    dotnet ef database update --startup-project Vindi.API
    ```

4.  **Execute a API:**
    ```bash
    cd Vindi.API
    dotnet run
    ```

5.  **Execute os Testes (Opcional):**
    Em um novo terminal, na raiz da solução, execute o comando:
    ```bash
    dotnet test
    ```

6.  **Acesse a Documentação:**
    Com a aplicação em execução, abra seu navegador e acesse o endereço na porta indicada no seu terminal para visualizar a documentação interativa do Swagger.

## Endpoints da API

A documentação completa e interativa de todos os endpoints está disponível na interface do Swagger ao executar a aplicação. Os principais recursos são:

* `POST /api/Contas`: Cria uma nova conta bancária.
* `GET /api/Contas`: Lista todas as contas, com filtros por nome e documento.
* `GET /api/Contas/{documento}`: Busca uma conta específica pelo documento.
* `POST /api/Contas/transferencia`: Realiza uma transferência entre contas. **(Auditado e Histórico Gerado)**
* `PATCH /api/Contas/{documento}/inativar`: Inativa uma conta. **(Auditado)**
* `GET /api/Contas/{documento}/historico-transferencias`: Retorna o histórico de transferências enviadas e recebidas para uma conta.