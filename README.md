# CRUD de Usuários

Aplicação web para gerenciamento de usuários, desenvolvida como Atividade Prática Individual.

O sistema permite realizar o cadastro, consulta, atualização e exclusão de usuários por meio de uma API REST integrada a um banco de dados SQL Server.

---

## Tecnologias utilizadas

### Front-end

- HTML5
- CSS3
- JavaScript
- Live Server

### Back-end

- C#
- .NET 10
- ASP.NET Core
- Entity Framework Core
- Swagger / OpenAPI

### Banco de Dados

- Microsoft SQL Server

### Controle de versão

- Git
- GitHub

---

## Funcionalidades

O sistema possui as seguintes funcionalidades:

- Cadastro de usuários
- Listagem de usuários cadastrados
- Consulta de usuário por ID
- Atualização de usuários
- Exclusão de usuários
- Confirmação antes da exclusão
- Validação dos campos obrigatórios
- Validação do formato do e-mail
- Validação das regras de negócio
- Verificação de e-mail duplicado
- Verificação de CPF duplicado
- Validação da data de nascimento
- Tratamento de erros
- Retorno de códigos HTTP adequados
- Comunicação entre Front-end e Back-end por meio de uma API REST

---

## Dados do usuário

A tabela `Usuarios` possui os seguintes campos:

| Campo | Tipo | Restrições |
|---|---|---|
| Id | INT | Chave primária, Identity |
| Nome | NVARCHAR(100) | NOT NULL |
| Email | NVARCHAR(150) | NOT NULL, UNIQUE |
| CPF | NVARCHAR(14) | NOT NULL, UNIQUE |
| Telefone | NVARCHAR(20) | Pode ser NULL |
| DataNascimento | DATE | NOT NULL |
| DataCadastro | DATETIME | NOT NULL, valor padrão GETDATE() |

---

## Banco de Dados

O sistema utiliza o Microsoft SQL Server como banco de dados.

O script para criação do banco e da tabela está disponível no arquivo:

```
database.sql
```

O script contém a criação do banco de dados, da tabela `Usuarios`, da chave primária e das restrições de unicidade para e-mail e CPF.

---

## Estrutura do projeto

```
CRUD-USUARIOS/
│
├── Backend/
│   └── CrudUsuarios/
│       │
│       ├── Controllers/
│       │   └── UsuariosController.cs
│       │
│       ├── Data/
│       │   └── AppDbContext.cs
│       │
│       ├── DTOs/
│       │   ├── UsuarioCreateDto.cs
│       │   └── UsuarioUpdateDto.cs
│       │
│       ├── Models/
│       │   └── Usuario.cs
│       │
│       ├── Services/
│       │   └── UsuarioService.cs
│       │
│       ├── Properties/
│       │   └── launchSettings.json
│       │
│       ├── Program.cs
│       ├── appsettings.json
│       └── CrudUsuarios.csproj
│
├── Frontend/
│   │
│   ├── css/
│   │   └── style.css
│   │
│   ├── js/
│   │   └── app.js
│   │
│   └── index.html
│
├── database.sql
├── README.md
└── .gitignore
```

---

## Como executar o projeto

### 1. Banco de Dados

Primeiramente, é necessário possuir o Microsoft SQL Server instalado e configurado.

Execute o arquivo `database.sql` no SQL Server para criar o banco de dados e a tabela `Usuarios`.

Depois, verifique se a string de conexão presente no arquivo `Backend/CrudUsuarios/appsettings.json` está configurada corretamente para o seu ambiente.

### 2. Executar o Back-end

Abra um terminal na pasta:

```
Backend/CrudUsuarios
```

Execute:

```
dotnet restore
```

Depois:

```
dotnet run
```

A API será executada em:

```
http://localhost:5065
```

### 3. Executar o Front-end

Abra o arquivo `Frontend/index.html` utilizando o Live Server.

O Front-end será aberto pelo navegador.

> É necessário manter o Back-end executando enquanto o Front-end estiver sendo utilizado, pois o Front-end realiza requisições para a API.

---

## API REST

A API disponibiliza os seguintes endpoints:

### Listar usuários

```
GET /api/usuarios
```

Retorna todos os usuários cadastrados.

### Consultar usuário por ID

```
GET /api/usuarios/{id}
```

Exemplo:

```
GET /api/usuarios/1
```

Retorna os dados do usuário correspondente ao ID informado.

### Cadastrar usuário

```
POST /api/usuarios
```

Exemplo de dados enviados:

```json
{
    "nome": "João da Silva",
    "email": "joao@email.com",
    "cpf": "123.456.789-00",
    "telefone": "79999999999",
    "dataNascimento": "2000-01-01"
}
```

### Atualizar usuário

```
PUT /api/usuarios/{id}
```

Exemplo:

```
PUT /api/usuarios/1
```

### Excluir usuário

```
DELETE /api/usuarios/{id}
```

Exemplo:

```
DELETE /api/usuarios/1
```

A exclusão é realizada após a confirmação do usuário no Front-end.

---

## Códigos HTTP utilizados

A API utiliza códigos HTTP de acordo com o resultado das operações:

| Código | Significado |
|---|---|
| 200 | Operação realizada com sucesso |
| 201 | Recurso criado com sucesso |
| 204 | Operação realizada sem conteúdo para retorno |
| 400 | Dados inválidos ou erro de validação |
| 404 | Usuário não encontrado |
| 500 | Erro interno do servidor |

---

## Validações e regras de negócio

O sistema realiza validações tanto no Front-end quanto no Back-end.

Entre as validações implementadas estão:

- Nome obrigatório
- E-mail obrigatório
- Validação do formato do e-mail
- CPF obrigatório
- E-mail não pode ser duplicado
- CPF não pode ser duplicado
- Data de nascimento obrigatória
- Data de nascimento não pode ser futura
- Limitação do tamanho dos campos
- Usuário deve existir para ser atualizado ou excluído

As validações do Back-end garantem que dados inválidos não sejam inseridos diretamente no banco de dados.

---

## Arquitetura da aplicação

A aplicação foi organizada separando as responsabilidades entre Front-end, Back-end e Banco de Dados.

```
┌─────────────────────────────┐
│          FRONT-END          │
│                             │
│ HTML + CSS + JavaScript     │
└──────────────┬──────────────┘
               │
               │ HTTP / JSON
               ↓
┌─────────────────────────────┐
│          BACK-END           │
│                             │
│ ASP.NET Core                │
│ Controllers                 │
│ Services                    │
│ DTOs                        │
│ Entity Framework Core       │
└──────────────┬──────────────┘
               │
               │ SQL
               ↓
┌─────────────────────────────┐
│        BANCO DE DADOS       │
│                             │
│ Microsoft SQL Server        │
│ Tabela Usuarios             │
└─────────────────────────────┘
```

---

## Controle de versão

O projeto utiliza Git para controle de versão.

O código-fonte está armazenado em um repositório do GitHub.

Os commits foram utilizados durante o desenvolvimento para registrar as atualizações e alterações realizadas no projeto.

---

## Objetivo do projeto

O objetivo deste projeto é desenvolver uma aplicação web completa de gerenciamento de usuários, integrando conhecimentos de Front-end, Back-end e Banco de Dados por meio de uma API REST e de um banco de dados relacional.

---

## Autor

Projeto desenvolvido individualmente como atividade acadêmica.

**Victor Felipe Santos Melo**
Análise e Desenvolvimento de Sistemas