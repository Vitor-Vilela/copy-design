# Repositório de Referência de Design Patterns

## Orientações Gerais

Este repositório é o **projeto de referência** para a disciplina **Design Patterns**.  
Cada equipe deve realizar um **fork** deste repositório com o nome da equipe/projeto fornecido pelo professor.

### `Carrocinha do bem`


O objetivo é **desenvolver um microsserviço de domínio** utilizando **código limpo** (servidor nativo da linguagem escolhida), aplicando **refatoração, código limpo** e pelo menos **um Design Pattern por integrante**.

---

## Atividades Avaliativas

### N1 – Atividade em Equipe
- Desenvolver e apresentar a **reconstrução arquitetural** de um microsserviço.  
- Microsserviço deve rodar em **servidor puro** da linguagem escolhida pela equipe.
- Mostrar o microsserviço persistindo dados em pelo menos um banco de dados. (Ex. **SQLite ou MySQL**).  
- Justificar tecnicamente os **padrões aplicados** (Corporativos, arquitetura limpa e GoF).  

### N2 – Atividade em Equipe e Individual
- Desenvolver e apresentar a **reconstrução arquitetural** de um microsserviço.
- Microsserviço deve rodar em **servidor puro** da linguagem escolhida pela equipe.
- Aplicar e justificar no código o uso de padrões corporativos, arquitetura limpa e GoF.
- Cada integrante deve versionar pelo menos **um design pattern** e **explicar no código** sua aplicação.
- Persistir dados em dois banco de dados distintos. (Ex. **SQLite e MySQL**).

### N3 – Apresentação em Equipe
- Apresentar a **reconstrução arquitetural** de um microsserviço.
- Apresentar o projeto e uma análise crítica das decisões arquiteturais tomadas.  

---

## Requisitos Obrigatórios

- Desenvolver **apenas um microsserviço de domínio**, na linguagem escolhida pela equipe.  
- **Não é necessário frontend** — apenas uma ferramenta de requisições HTTP (ex.: Postman, curl, HTTPie) será utilizada.  
- O microsserviço deve:
  - Aplicar **técnicas de refatoração** e **código limpo**.
  - Utilizar **mínimo de um Design Pattern por integrante** (GoF).
  - Suportar **alternância entre bancos** (Ex. SQLite e MySQL via variáveis de ambiente).
  - Usar **servidor nativo da linguagem** (ex.: `HttpServer` em Java, `http` no Node.js, `http.server` em Python, etc.).
  - Seguir **Arquitetura Limpa** (Clean Architecture / Ports & Adapters), sem frameworks.
- Cada integrante da equipe deve realizar **commits identificáveis** com seu usuário.

---

## Equipe

- Nome do Projeto: **[preencher com o nome definido pelo professor]**  
- Integrantes:
  - Vinicius da costa pereira - @vinicosper
  - Vinicius viana gomes – @vini-vg
  - Marcos vinicius maximo – @Marcos Vinicius Maximo Da Silva
  - Vitor vilela – @Vítor Vilela

---

## Contexto Comercial da Aplicação

Com certeza! Baseado no código e na estrutura do seu projeto, aqui está uma apresentação no mesmo estilo que você pediu:

Este microsserviço é um componente central da plataforma "Carrocinha do bem", possuindo a responsabilidade única de gerenciar o catálogo de animais disponíveis para adoção. Ele centraliza todas as operações essenciais, como o cadastro de novos pets, a consulta de animais por perfil (espécie, raça, idade) e a atualização de seu status — de "disponível" para "adotado".

Sua arquitetura como um serviço dedicado garante que a lógica de gerenciamento dos animais seja totalmente desacoplada de outros domínios, como o cadastro de usuários ou o controle de doações. Essa separação permite que o catálogo evolua e escale de forma independente, garantindo a performance e a integridade dos dados dos pets.

---

## Stack Tecnológica

- **Linguagem de Programação:** [ C#]  
- **Banco de Dados:** SQLite e MySQL (alternáveis via configuração)  
- **Arquitetura:** Clean Architecture / Ports & Adapters  

---

## Estrutura Recomendada

```
/docs/                 → documentação, diagramas, decisões (ADRs)
/src/                  → código do microsserviço
  /domain/             → entidades, regras de negócio, padrões (Strategy, State…)
  /application/        → casos de uso (Command, Observer…)
  /infrastructure/     → adapters (DB, outbox, etc.)
  /web/                → servidor HTTP nativo, roteamento
/tests/                → testes unitários e integrados
```

---

# Servidores Nativos por Linguagem

Este documento apresenta exemplos mínimos de servidores HTTP utilizando apenas recursos nativos de cada linguagem, sem frameworks adicionais.

---

## JavaScript / TypeScript (Node.js)

**Módulo nativo:** `http`

```javascript
const http = require('http');

const server = http.createServer((req, res) => {
  res.writeHead(200, {'Content-Type': 'text/plain'});
  res.end('Hello World\n');
});

server.listen(3000, () => {
  console.log('Servidor rodando em http://localhost:3000');
});
```

---

## Java

**Mais puro:** `com.sun.net.httpserver.HttpServer` (desde Java 6)

```java
import com.sun.net.httpserver.HttpServer;
import com.sun.net.httpserver.HttpHandler;
import com.sun.net.httpserver.HttpExchange;
import java.io.OutputStream;

public class Main {
    public static void main(String[] args) throws Exception {
        HttpServer server = HttpServer.create(new java.net.InetSocketAddress(8080), 0);
        server.createContext("/", new MyHandler());
        server.start();
    }

    static class MyHandler implements HttpHandler {
        public void handle(HttpExchange t) throws java.io.IOException {
            String response = "Hello World";
            t.sendResponseHeaders(200, response.length());
            OutputStream os = t.getResponseBody();
            os.write(response.getBytes());
            os.close();
        }
    }
}
```

---

## Python

**Mais puro:** módulo `http.server` (stdlib)

```python
from http.server import BaseHTTPRequestHandler, HTTPServer

class MyHandler(BaseHTTPRequestHandler):
    def do_GET(self):
        self.send_response(200)
        self.end_headers()
        self.wfile.write(b"Hello World")

server = HTTPServer(('localhost', 8000), MyHandler)
server.serve_forever()
```

---

## C# (.NET)

**Mais puro:** `HttpListener` (sem ASP.NET)

```csharp
using System;
using System.Net;
using System.Text;

class Program {
    static void Main() {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:8080/");
        listener.Start();
        Console.WriteLine("Servidor rodando...");

        while (true) {
            HttpListenerContext context = listener.GetContext();
            HttpListenerResponse response = context.Response;
            string responseString = "Hello World";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}
```

---

## PHP

**Mais puro:** servidor embutido (desde PHP 5.4)

Rodar no terminal:

```bash
php -S localhost:8000
```

E um `index.php` mínimo:

```php
<?php
echo "Hello World";
?>
```

---

## Go (Golang)

**Mais puro:** pacote `net/http`

```go
package main

import (
    "fmt"
    "net/http"
)

func handler(w http.ResponseWriter, r *http.Request) {
    fmt.Fprintln(w, "Hello World")
}

func main() {
    http.HandleFunc("/", handler)
    http.ListenAndServe(":8080", nil)
}
```

---

## Ruby

**Mais puro:** WEBrick (stdlib até Ruby 3.0; depois como gem)

```ruby
require 'webrick'

server = WEBrick::HTTPServer.new(:Port => 8000)
server.mount_proc '/' do |req, res|
  res.body = 'Hello World'
end
trap 'INT' do server.shutdown end
server.start
```



⚙️ Configuração do Banco de Dados

Este microsserviço foi configurado para suportar múltiplos bancos de dados de forma dinâmica através de variáveis de ambiente. A seleção é controlada por duas variáveis principais:

    DB_TYPE: Define o tipo de banco a ser usado (sqlite ou mysql).

    DB_CONNECTION_STRING: Define a string de conexão para o banco de dados.

Por padrão, se nenhuma variável for especificada, a aplicação utilizará SQLite.

💾 Usando SQLite (Padrão)

Esta é a configuração mais simples e não requer nenhum servidor de banco de dados externo. Um arquivo adoption.db será criado automaticamente no diretório do projeto.

Para rodar a aplicação com SQLite, basta executar o comando padrão, sem a necessidade de configurar variáveis de ambiente:
Bash

dotnet run --project src/Adocao.Web/Adocao.Web.csproj

🐬 Usando MySQL

Pré-requisito: Você precisa ter um servidor MySQL em execução e as credenciais de acesso (servidor, nome do banco, usuário e senha).

Para usar o MySQL, defina as variáveis de ambiente antes de executar a aplicação. Abaixo estão os comandos para diferentes sistemas operacionais.

No Windows (usando PowerShell)

PowerShell

# 1. Defina o tipo do banco
$env:DB_TYPE="mysql"

# 2. Defina a string de conexão (ATENÇÃO: substitua com seus dados)
$env:DB_CONNECTION_STRING="Server=localhost;Database=adocao_db;Uid=root;Pwd=sua_senha_secreta;"

# 3. Execute o projeto
dotnet run --project src/Adocao.Web/Adocao.Web.csproj

No Linux ou macOS

Bash

# 1. Defina o tipo do banco
export DB_TYPE="mysql"

# 2. Defina a string de conexão (ATENÇÃO: substitua com seus dados)
export DB_CONNECTION_STRING="Server=localhost;Database=adocao_db;Uid=root;Pwd=sua_senha_secreta;"

# 3. Execute o projeto
dotnet run --project src/Adocao.Web/Adocao.Web.csproj

✅ Verificando a Configuração

Ao iniciar a aplicação, verifique a primeira linha de log no console. Ela informará qual banco de dados está sendo utilizado:

[INFO] Usando banco de dados: sqlite

ou

[INFO] Usando banco de dados: mysql
# Padrões de Projetos

## 1. Command Pattern (Comando)
**O que é:** Encapsula uma ação ou solicitação como um objeto, separando quem pede da execução.

**Exemplo no projeto:**
- **Comando:** `ICommand<TResult>` define a "mensagem" da ação (ex.: `CreatePetCommand`).
- **Handler (Executor):** `CreatePetCommandHandler` implementa a lógica de negócio do comando.
- **Dispatcher (Distribuidor):** `CommandDispatcher` recebe o comando e envia para o handler correto.

---

## 2. Repository Pattern (Repositório)
**O que é:** Cria uma camada que esconde os detalhes do banco de dados da lógica de negócio. A aplicação interage com o repositório como se fosse uma coleção de objetos.

**Exemplo no projeto:**
- **Interface:** `IPetRepository` define operações CRUD da entidade `Pet`.
- **Implementação:** `SqLitePetRepository` ou `MySqlPetRepository` contém o SQL específico do banco.

**Benefício:** Handlers podem usar o repositório sem se preocupar com o tipo de banco (SQLite, MySQL, etc.).

---

## 3. Factory Pattern (Fábrica)
**O que é:** Centraliza a criação de objetos complexos ou que devem ser decididos em tempo de execução.

**Exemplo no projeto:**
- `DatabaseFactory` cria a instância correta de `IPetRepository` com base na configuração (`dbType` e `connectionString`).

**Benefício:** Evita espalhar a lógica de inicialização do banco pela aplicação.

---

## 4. Adapter Pattern (Adaptador)
**O que é:** Conecta interfaces incompatíveis, permitindo que trabalhem juntas.

**Exemplo no projeto:**
- `PetHttpAdapter` faz a ponte entre requisições HTTP (URL, método, corpo) e a arquitetura interna baseada em comandos e repositórios.
- **Função:** Traduz o formato da Web para o formato da aplicação (comando ou chamada de repositório).