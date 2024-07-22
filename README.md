# GerenciarEnderecos

Projeto Crud de endereços com login/cadastro de usuario

banco de dados:
Configure a connection string no appsettings e as chaves de criptografia da senha:
Exemplo de chaves a serem adicionas:

AppSettings.json :
```json
  "ConnectionStrings": {
    "DefaultConnection": "<conn string>"
  },
  "AllowedHosts": "*",
  "EncryptionKeys": {
    "PASSWORDHASH": "this is my hash",
    "SALTKEY": "this is my salt",
    "VIKEY": "this is my VIKEY"
  },
  "JwtKey": "SBGJZ8zK4LTTYMfHp5lCnvdIY520IieIPjLthUrn"
```
Para executar o projeto:

Crie o banco de dados executanto uma migration:
##Migrations
#### no console do gerenciador de pacotes selecione o dal referente:
EntityFrameworkCore\Add-Migration "Init" -Context AppDbContext
EntityFrameworkCore\update-database -Context AppDbContext

#### Caso queira remover o ultimo snapshot
//EntityFrameworkCore\Remove-Migration -Context AppDbContext

##Navegação

Crie um usuário, acesse o gerencimento de endereços utilizando-o.




        
