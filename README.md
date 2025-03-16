# **Sistema de Análise de Logs**

## **Visão Geral**

Este projeto consiste em uma API para armazenamento e análise de logs com base em agregações utilizando **C#/.NET** e **MongoDB**. O objetivo é centralizar os logs de diferentes serviços, permitindo consultas para análise de desempenho,
erros e comportamentos suspeitos.

## **Tecnologias Utilizadas**

- **ASP.NET Core**
- **MongoDB**
- **Docker**
- **Swagger**

## **Como Executar o Projeto**

### **1. Clonar o Repositório**

```sh
git clone https://github.com/murilonicemento/log-analyser.git
cd log-analyser
```

### **2. Configurar as Variáveis de Ambiente**

Crie um arquivo **`appsettings.json`** e configure a string de conexão com o MongoDB:

```json
{
  "LogAnalyserConfiguration": {
    "Database": "logAnalyser",
    "Host": "localhost",
    "Port": 27017,
    "User": "",
    "Password": ""
  }
}
```

### **3. Executar com Docker**

Certifique-se de ter o **Docker** instalado e execute:

```sh
docker-compose up --build
```

Isso criará os containers para a API e o MongoDB.

Para parar os containers:

```sh
docker-compose down
```

### **4. Testar e acessar a API**

Acesse a documentação do Swagger em:

```
http://localhost:5232/swagger
```

## **Contribuição**

Fique à vontade para abrir uma **issue** ou enviar um **pull request**.

## Licença

Este projeto está licenciado sob a [MIT License](LICENSE).