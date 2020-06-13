FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS builder

ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install -g Cake.Tool

ENTRYPOINT [ "dotnet-cake" ]