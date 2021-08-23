FROM mcr.microsoft.com/dotnet/core/sdk:3.1

WORKDIR /build

COPY . .
RUN chmod +x ./Migrations/runScript.sh

RUN dotnet restore ./Migrations/Migrations.csproj
RUN dotnet publish ./Migrations/Migrations.csproj --output ./out

ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.2.1/wait /wait
RUN chmod +x /wait

CMD /wait && ./Migrations/runScript.sh