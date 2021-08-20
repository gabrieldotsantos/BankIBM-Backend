# BankIBM-Backend

Configurar conexão com o banco de dados PostgreSQL dentro do arquivo appsettings.json\
Dentro do objeto ConnectionStrings.BankIBM = "Host=192.168.18.179;Database=bankibm;Username=admin;Password=@1510"

\Para criação do banco de dados através do Entity Framework Core\
Abrir o Console do Gerenciador de Pacotes\
Executar: Add-Migration InitialCreate\
Executar:  Update-database\
