# Controle de Gastos Residenciais

Projeto separado em web-api e front

###### Tecnologias usadas
- .net 9
- react
- sqlite (banco de dados)

###### Para executar em ambiente de desenvolvimento (Windows - Powershell)

###### web-api 

Dentro da pasta raiz execute o comando:

`$env:ASPNETCORE_ENVIRONMENT="Development"` </br>
`dotnet run --project .\web-api\src\Controle.Gastos.Residenciais.Api\Controle.Gastos.Residenciais.Api.csproj -c Debug -- --help`

###### front

Dentro da pasta front execute o comando:

`npm run dev` 

###### Para executar no docker com configurações do ambiente de desenvolvimento 

Dentro da pasta raiz execute o comando:

`docker compose up --build`

Abra o navegador na url: http://localhost:3000/

###### Sugestão de futuras melhorias

- Autenticação com cadastro e tela de login
- Adicionar filtros nas telas de listagem de dados, quando necessário.
- Melhorar a paginação
- Adicionar log de erros
