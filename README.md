Hotel Management API

📌 Visão Geral

Este é um sistema para gestão de Hotéis e Suítes, desenvolvido seguindo os princípios da Clean Architecture e utilizando diversas tecnologias modernas para garantir escalabilidade e desempenho.

🚀 Tecnologias Utilizadas

.NET Core

Clean Architecture

MediatR

CQRS (Command Query Responsibility Segregation)

Response Pattern

Unit of Work + Repository Pattern

Hangfire (para jobs em background)

Redis & MemoryCache (para caching)

IdentityCore (autenticação e gestão de usuários)

FluentValidation (validação de inputs)

Entity Framework Core + PostgreSQL + Fluent API (para manipulação do banco de dados)

Docker (para conteinerização e orquestração dos serviços)

🏗️ Estrutura do Projeto

O projeto está organizado seguindo os princípios da Clean Architecture, separando bem as responsabilidades entre camadas:

Application: Regras de negócio e casos de uso.

Domain: Entidades e interfaces.

Infrastructure: Acesso a dados, serviços externos e integrações.

Presentation (HM.API): Controladores e endpoints da API.

🔧 Configuração e Execução

Clonar o repositório:

git clone https://github.com/seu-usuario/HotelManagement.git
cd HotelManagement

Criar e subir os containers Docker:

cd HM.API
docker-compose up --build

A API será executada nas portas 80 e 443.

🔑 Credenciais Padrão

Ao iniciar a aplicação, um usuário administrador será gerado automaticamente:

E-mail: admin@hotel.com

Senha: Admin@123

📌 Funcionalidades

1️⃣ Administração de Hotéis

Usuários administradores podem:

Criar novos Hotéis

Gerenciar os usuários e permissões

Ao criar um hotel, um usuário HOTEL_ADMIN é gerado automaticamente com:

Senha Padrão: Hotel@123

E-mail: Informado no request

2️⃣ Gestão de Hotéis

Usuários HOTEL_ADMIN podem:

Criar funcionários

Criar categorias de Suítes

Criar Suítes

3️⃣ Reserva de Quartos

Usuários finais podem se cadastrar via api/sign-up

Usuários cadastrados podem criar reservas

📜 Endpoints Principais

🔐 Autenticação

POST /api/sign-up - Cadastro de usuários finais

POST /api/login - Login de usuário

🏨 Hotéis

POST /api/hotels - Criar um novo hotel

GET /api/hotels - Listar todos os hotéis

🏢 Suítes

POST /api/suites - Criar uma nova suíte

GET /api/suites - Listar todas as suítes

📅 Reservas

POST /api/reservations - Criar uma reserva

GET /api/reservations - Listar reservas

📌 Considerações Finais

Este sistema foi projetado para ser escalável e performático, utilizando tecnologias modernas para caching, background jobs e uma arquitetura limpa.

Caso tenha alguma dúvida ou sugestão, fique à vontade para contribuir! 🚀
