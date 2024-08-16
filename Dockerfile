# Sử dụng image của .NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Sao chép các tệp .csproj với đường dẫn chính xác
COPY ["BOs/BOs.csproj", "BOs/"]
COPY ["DAOs/DAOs.csproj", "DAOs/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Jewelry-BE/Jewelry-BE.csproj", "Jewelry-BE/"]

# Khôi phục các package nuget (nếu có)
RUN dotnet restore "./Jewelry-BE/Jewelry-BE.csproj"

# Sao chép toàn bộ mã nguồn
COPY . .

# Thiết lập thư mục làm việc là thư mục dự án API
WORKDIR "/src/Jewelry-BE"

# Build ứng dụng
RUN dotnet build "./Jewelry-BE.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish ứng dụng
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Jewelry-BE.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Chuẩn bị để chạy ứng dụng
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jewelry-BE.dll"]
