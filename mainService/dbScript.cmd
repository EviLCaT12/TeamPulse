docker-compose up -d

dotnet-ef database drop -f -c WriteDbContext -p  src\Performances\src\TeamPulse.Performances.Infrastructure\ -s src\TeamPulse.Web\
dotnet-ef database drop -f -c WriteDbContext -p  src\Teams\src\TeamPulse.Teams.Infrastructure\ -s src\TeamPulse.Web\

dotnet-ef migrations remove -c WriteDbContext -p src\Performances\src\TeamPulse.Performances.Infrastructure\ -s src\TeamPulse.Web\
dotnet-ef migrations remove -c WriteDbContext -p src\Teams\src\TeamPulse.Teams.Infrastructure\ -s src\TeamPulse.Web\

dotnet-ef migrations add PerformancesInit -c WriteDbContext -p src\Performances\src\TeamPulse.Performances.Infrastructure\ -s src\TeamPulse.Web\
dotnet-ef migrations add PerformancesInit -c WriteDbContext -p src\Teams\src\TeamPulse.Teams.Infrastructure\ -s src\TeamPulse.Web\

dotnet-ef database update -c WriteDbContext -p  src\Performances\src\TeamPulse.Performances.Infrastructure\ -s src\TeamPulse.Web\
dotnet-ef database update -c WriteDbContext -p  src\Teams\src\TeamPulse.Teams.Infrastructure\ -s src\TeamPulse.Web\


pause