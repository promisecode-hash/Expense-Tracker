# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything so ProjectReferences to sibling folders work
COPY ExpenseTracker.Api/ ExpenseTracker.Api/
COPY ExpenseTracker.Application/ ExpenseTracker.Application/
COPY ExpenseTracker.Domain/ ExpenseTracker.Domain/
COPY ExpenseTracker.Infrastructure/ ExpenseTracker.Infrastructure/
COPY ExpenseTracker.slnx ./

RUN dotnet restore ExpenseTracker.Api/ExpenseTracker.Api.csproj
RUN dotnet publish ExpenseTracker.Api/ExpenseTracker.Api.csproj -c Release -o /app/out --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-3000}
EXPOSE 3000

ENTRYPOINT ["dotnet", "ExpenseTracker.Api.dll"]
