**Instruções:**

Rodar as migrations para criação de banco de dados (projeto Infrastructure contém o contexto do EF).

Configurar as variáveis de ambiente abaixo, de acordo com o ambiente que o projeto será executado:
 - DB_SA_PASSWORD (senha do usuario do sql server, caso não exista, criar o usuário de aplicação "sa")
 - DB_NAME (nome do banco de dados criado via migration)
 - DB_HOST (servidor sql server)

   
Exemplo de configuração para execução local em ambiente de desenvolvimento via Visual Studio: ASPNETCORE_ENVIRONMENT=Development,DB_SA_PASSWORD=senha,DB_NAME=PlanoDeContas,DB_HOST=(localdb)\Local
