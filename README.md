# 📚 API Gateway Books - CRUD com Lambda e DynamoDB

API REST serverless para gerenciar catálogo de livros usando AWS Lambda, API Gateway e DynamoDB.

---

## 🎯 Objetivo

Praticar **API Gateway HTTP API** integrando múltiplos Lambdas com DynamoDB, criando um CRUD completo serverless.

---

## 🏗️ Arquitetura

```
API Gateway (HTTP API)
    ├── GET /books → Lambda get-all-books → DynamoDB Scan
    ├── GET /books/{id} → Lambda get-book-by-id → DynamoDB Load
    └── POST /books → Lambda create-book → DynamoDB Save
```

**Serviços AWS:**
- **API Gateway HTTP API** - Endpoint público REST
- **Lambda (3 funções)** - Handlers das rotas
- **DynamoDB** - Banco NoSQL
- **CloudWatch Logs** - Logs das execuções

---

## 📁 Estrutura do Projeto

```
api-gateway-books/
├── src/
│   ├── BooksApi.Contracts/
│   │   └── Book.cs                    # Model compartilhado
│   ├── GetAllBooks/
│   │   ├── Function.cs                # GET /books
│   │   └── aws-lambda-tools-defaults.json
│   ├── GetBookById/
│   │   ├── Function.cs                # GET /books/{id}
│   │   └── aws-lambda-tools-defaults.json
│   └── CreateBook/
│       ├── Function.cs                # POST /books
│       └── aws-lambda-tools-defaults.json
└── api-gateway-books.sln
```

---

## 🚀 Deploy

### 1. Criar tabela DynamoDB

```bash
aws dynamodb create-table \
  --table-name Books \
  --attribute-definitions AttributeName=id,AttributeType=N \
  --key-schema AttributeName=id,KeyType=HASH \
  --billing-mode PAY_PER_REQUEST \
  --region us-east-1
```

### 2. Criar IAM Role

```bash
# Console: IAM → Roles → Create role
# - Trusted entity: Lambda
# - Policies: AWSLambdaBasicExecutionRole + AmazonDynamoDBFullAccess
# - Name: books-lambda-role
```

### 3. Deploy dos Lambdas

```bash
export DOTNET_ROOT=$HOME/.dotnet
export PATH=$PATH:$HOME/.dotnet:$HOME/.dotnet/tools

cd src/GetAllBooks
dotnet-lambda deploy-function get-all-books --function-role books-lambda-role --region us-east-1

cd ../GetBookById
dotnet-lambda deploy-function get-book-by-id --function-role books-lambda-role --region us-east-1

cd ../CreateBook
dotnet-lambda deploy-function create-book --function-role books-lambda-role --region us-east-1
```

### 4. Criar API Gateway

**Console AWS → API Gateway → Create API → HTTP API → Build**

- **Integrations:** Adicionar os 3 Lambdas
- **Routes:**
  - `GET /books` → `get-all-books`
  - `GET /books/{id}` → `get-book-by-id`
  - `POST /books` → `create-book`
- **Stage:** `$default`

---

## 🧪 Testes

```bash
# URL base (substitua pelo seu API ID)
API_URL="https://drnaql0h72.execute-api.us-east-1.amazonaws.com"

# Criar livro
curl -X POST $API_URL/books \
  -H "Content-Type: application/json" \
  -d '{"id":1,"title":"Clean Code","author":"Robert C. Martin","year":2008}'

# Listar todos
curl $API_URL/books

# Buscar por ID
curl $API_URL/books/1
```

**Respostas esperadas:**

```json
// GET /books
[
  {
    "Id": 1,
    "Title": "Clean Code",
    "Author": "Robert C. Martin",
    "Year": 2008
  }
]

// GET /books/1
{
  "Id": 1,
  "Title": "Clean Code",
  "Author": "Robert C. Martin",
  "Year": 2008
}

// POST /books
book with Id 1 created
```

---

## 🔑 Pontos-Chave

### API Gateway HTTP API
- Mais simples e barato que REST API
- Integração direta com Lambda
- Suporte a path parameters (`{id}`)

### Lambda Handlers
- `APIGatewayHttpApiV2ProxyRequest` - Input do API Gateway
- `APIGatewayHttpApiV2ProxyResponse` - Output com StatusCode e Body
- Deserialização case-insensitive para aceitar JSON minúsculo

### DynamoDB
- `DynamoDBContext` - ORM simplificado
- `ScanAsync` - Lista todos os itens
- `LoadAsync` - Busca por chave primária
- `SaveAsync` - Cria ou atualiza item

---

## 📊 Custas AWS

- **Lambda:** 1M invocações/mês grátis
- **API Gateway HTTP:** 1M chamadas/mês grátis (12 meses)
- **DynamoDB:** 25GB + 25 WCU/RCU grátis

---

## 🛠️ Tecnologias

- **.NET 8** - Runtime do Lambda
- **Amazon.Lambda.APIGatewayEvents** - Contratos do API Gateway
- **AWSSDK.DynamoDBv2** - Cliente DynamoDB
- **Amazon Lambda Tools** - Deploy CLI

---

## 📚 Aprendizados

✅ Criar API Gateway HTTP API  
✅ Integrar múltiplos Lambdas em uma API  
✅ Usar path parameters no API Gateway  
✅ CRUD completo com DynamoDB  
✅ Deserialização case-insensitive de JSON  
✅ Logs estruturados no CloudWatch
