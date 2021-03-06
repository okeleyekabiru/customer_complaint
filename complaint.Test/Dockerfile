#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-nanoserver-1809 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1809 AS build
WORKDIR /src
COPY ["complaint.Test/complaint.Test.csproj", "complaint.Test/"]
COPY ["complaint_API/complaint_API.csproj", "complaint_API/"]
COPY ["complaint.Data/complaint.Data.csproj", "complaint.Data/"]
COPY ["EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus.RabbitMQ/"]
RUN dotnet restore "complaint.Test/complaint.Test.csproj"
COPY . .
WORKDIR "/src/complaint.Test"
RUN dotnet build "complaint.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "complaint.Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "complaint.Test.dll"]