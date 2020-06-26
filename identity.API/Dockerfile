#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1809 AS build
WORKDIR /src
COPY ["complaint_solution.sln", "complaint_solution.sln"]
COPY ["complaint.Test/complaint.Test.csproj", "complaint.Test/"]
COPY ["complaint_API/complaint_API.csproj", "complaint_API/"]
COPY ["complaint.Data/complaint.Data.csproj", "complaint.Data/"]
COPY ["EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus.RabbitMQ/"]
COPY ["identity.API/identity.API.csproj", "identity.API/"]
COPY ["identity.Data/identity.Data.csproj", "identity.Data/"]
COPY ["identity.test/identity.test.csproj", "identity.test/"]
RUN dotnet restore "complaint_solution.sln"
COPY . .
WORKDIR "/src/identity.API"
RUN dotnet build "identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "identity.API.dll"]