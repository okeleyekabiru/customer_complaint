#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-nanoserver-1809 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1809 AS build
WORKDIR /src
COPY ["gateway.Test/gateway.Test.csproj", "gateway.Test/"]
COPY ["complaint_gateway.APi/complaint_gateway.APi.csproj", "complaint_gateway.APi/"]
COPY ["gateway.Data/gateway.Data.csproj", "gateway.Data/"]
COPY ["EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus.RabbitMQ/"]
RUN dotnet restore "gateway.Test/gateway.Test.csproj"
COPY . .
WORKDIR "/src/gateway.Test"
RUN dotnet build "gateway.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gateway.Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gateway.Test.dll"]