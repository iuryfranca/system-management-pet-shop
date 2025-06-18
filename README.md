# Sistema de Gerenciamento para Pet Shop

Sistema desenvolvido em Blazor.NET com Tailwind CSS para gerenciamento completo de pet shops, incluindo cadastros, relatórios e dashboard para análise de dados.

## Funcionalidades

### Cadastros

- **Funcionário**: Gerenciamento completo de funcionários do pet shop
- **Animal**: Cadastro de animais clientes com informações detalhadas
- **Produto**: Controle de produtos e estoque
- **Serviço**: Cadastro e agendamento de serviços

### Listagem com Filtros

- **Funcionário**: Listagem com filtros personalizados e opção de download
- **Animal**: Filtros por tipo, raça, idade e outros critérios
- **Produto**: Filtros por categoria, marca e disponibilidade
- **Serviço**: Filtros por status, funcionário e tipo de animal

### Relatórios

- **Relatório de serviços por tipo de animal**: Análise de serviços mais demandados por espécie
- **Relatório de maior e menor ocorrências de visita de animais**: Identificação de padrões de frequência
- **Relatório de maior e menor ocorrências de serviços por funcionário**: Análise de produtividade

### Dashboard

Dashboard interativo com dados gerais e estatísticas do sistema para tomada de decisões.

### Sistema de Login

Sistema de autenticação seguro para funcionários com diferentes níveis de acesso.

### Tela de Usuário

Interface para visualização e edição de dados pessoais do usuário logado.

## Estrutura do Banco de Dados

### Tabelas

- `funcionario`: Dados dos funcionários do pet shop
- `animal`: Informações dos animais clientes
- `produto`: Catálogo de produtos e controle de estoque
- `servico`: Serviços prestados e agendamentos
- `servico_produto`: Relacionamento entre serviços e produtos utilizados

### Script de Criação do Banco

```sql
CREATE TABLE funcionario(
    id_func int not null auto_increment primary key,
    nome_func varchar(300) not null,
    cargo_func varchar(50) not null,
    telefone_func varchar(11) not null,
    email_func varchar(300) not null unique,
    senha_func varchar(255) not null
);

CREATE TABLE animal(
    id_animal int not null auto_increment primary key,
    nome_ani varchar(300) not null,
    tipo_ani ENUM('Cachorro', 'Gato', 'Pássaro', 'Peixe', 'Roedor', 'Réptil', 'Outro') not null,
    raca_ani varchar(50) not null,
    idade_ani int,
    peso_ani double,
    dono_nome varchar(300),
    dono_telefone varchar(11)
);

CREATE TABLE produto(
    id_produto int not null auto_increment primary key,
    nome_prod varchar(300) not null,
    marca_prod varchar(300),
    valor_prod decimal(10,2) not null,
    estoque int default 0,
    categoria ENUM('Alimento', 'Medicamento', 'Higiene', 'Acessório', 'Outro') not null
);

CREATE TABLE servico(
    id_servico int not null auto_increment primary key,
    nome_serv varchar(300) not null,
    descricao_serv text,
    duracao_serv time,
    valor_serv decimal(10,2) not null,
    id_func_fk int,
    id_animal_fk int,
    data_servico datetime not null,
    status ENUM('Agendado', 'Em andamento', 'Concluído', 'Cancelado') default 'Agendado',
    foreign key (id_func_fk) references funcionario (id_func),
    foreign key (id_animal_fk) references animal (id_animal)
);

CREATE TABLE servico_produto (
    id_servico_fk int not null,
    id_produto_fk int not null,
    quantidade int not null DEFAULT 1,
    valor_unitario decimal(10,2) not null,
    primary key (id_servico_fk, id_produto_fk),
    foreign key (id_servico_fk) references servico(id_servico),
    foreign key (id_produto_fk) references produto(id_produto)
);
```

## Startando o projeto

Certifique-se de ter o NPM e Node.js instalados no seu projeto. Também certifique-se de ter instalado o .NET SDK para conseguir desenvolver aplicações .NET. Execute o seguinte comando para instalar todas as dependências:

```Bash
npm install
```

Em seguida, execute este comando para compilar o código fonte e observar mudanças:

```Bash
dotnet watch
```

Certifique-se também de executar o seguinte script para compilar o código fonte do Tailwind CSS:

```Bash
npx tailwindcss -i wwwroot/css/input.css -o wwwroot/css/output.css --watch
```

Execute este comando para compilar seu projeto e todas suas dependências:

```Bash
dotnet build
```
