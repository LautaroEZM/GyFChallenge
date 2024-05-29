# GyFChallenge API

Este proyecto es una API backend desarrollada usando .NET 8, diseñada como un desafío para desarrolladores backend. 
La API incluye operaciones CRUD básicas para la gestión de productos y usuarios.

## Tabla de Contenidos

- [Introducción](#introducción)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Configuración de la Base de Datos](#configuración-de-la-base-de-datos)
- [Ejecutar la Aplicación](#ejecutar-la-aplicación)
- [Endpoints de la API](#endpoints-de-la-api)

## Introducción

### Prerrequisitos

- .NET 8 SDK
- SQL Server (o cualquier otra base de datos compatible con Entity Framework Core)

### Instalación

1. Clona el repositorio:
```sh
git clone https://github.com/LautaroEZM/GyFChallenge.git
cd GyFChallenge
```

2. Restaura las dependencias:
```sh
   dotnet restore
```

3. Actualiza la cadena de conexión a la base de datos en `appsettings.json`:
```json
   {
   "ConnectionStrings": {
     "DefaultConnection": "Server={hostname enviado por email};Uid=gyfchallenge;Password={password enviado por email};Database=StockManagementDb;MultipleActiveResultSets=true;TrustServerCertificate=true"
   }
   }
```
4. Aplica las migraciones para crear el esquema de la base de datos: (Sólo si no se utiliza la base de datos enviada por email)
```
sh
   dotnet ef database update
```

5. Crearse un usuario utilizando el siguiente endpoint:
```
http://localhost:5257/register
```

Ejemplo de los datos:
```
{
    "Username": "Lautaro",
    "Email": "lautaro.mongelo@gmail.com",
    "Password": "12345"
}
```

6. Obtener un auth token del siguiente endpoint utilizando las mismas credenciales utilizadas al momento de registrarse:
```
   http://localhost:5257/login
```

Ejemplo de los datos:
```
{
    "Email": "lautaro.mongelo@gmail.com",
    "Password": "12345"
}
```
Todos los requests a la api llevan autenticación tipo Bearer Token. Se deberá utilizar el token obtenido del login y el mismo expira 10 minutos luego de su emisión.


## Estructura del Proyecto

- **GyFChallenge.csproj**: Archivo del proyecto que contiene dependencias y el framework objetivo.
- **Program.cs**: Punto de entrada de la aplicación.
- **Data/AppDBContext.cs**: Contexto de la base de datos de Entity Framework Core.
- **Models/Products.cs**: Clases de modelo para `Product` y `User`.
- **Pages**: Razor Pages para la interfaz web.
- **wwwroot**: Archivos estáticos (CSS, JS, etc.).

## Configuración de la Base de Datos

El contexto de la base de datos está definido en `Data/AppDBContext.cs`:
```csharp
public class AppDBContext : DbContext
{
public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } 
    public DbSet<User> Users { get; set; } 
 
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    { 
        modelBuilder.Entity<Product>(tb => 
        { 
            tb.HasKey(col => col.Id); 
            tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd(); 
        }); 
 
        modelBuilder.Entity<User>(tb => 
        { 
            tb.HasKey(col => col.Mail); 
            tb.Property(col => col.Mail).HasMaxLength(50); 
            tb.Property(col => col.Name).HasMaxLength(50); 
            tb.Property(col => col.Surname).HasMaxLength(50); 
        }); 
 
        modelBuilder.Entity<Product>().ToTable("Products"); 
        modelBuilder.Entity<User>().ToTable("Users"); 
    } 
}
```
## Ejecutar la Aplicación

La aplicación bien puede ejecutarse desde el IDE o usando el siguiente comando:
```sh
dotnet run
```

La aplicación estará disponible en `http://localhost:5257`

## Endpoints de la API

### Users

- **POST /register**: Crear un nuevo usuario.
- **POST /login**: Loguearse en un usuario existente.

### Products

- **GET /api/products**: Listar todos los productos.
- **GET /api/products/{id}**: Obtener el detalle de un producto por ID.
- **POST /api/products**: Crear un nuevo producto.
- **PUT /api/products/{id}**: Actualizar un producto existente.
- **DELETE /api/products/{id}**: Eliminar un producto por ID.
