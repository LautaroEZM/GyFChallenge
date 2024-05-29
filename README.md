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

Todos los requests a la api llevan autenticación tipo Bearer Token. Se deberá utilizar el token obtenido del login y el mismo expira 10 minutos luego de su emisión.


## Estructura del Proyecto

- **GyFChallenge.csproj**: Archivo del proyecto que contiene dependencias y el framework objetivo.
- **Program.cs**: Punto de entrada de la aplicación.
- **Data/AppDBContext.cs**: Contexto de la base de datos de Entity Framework Core.
- **Models/Products.cs**: Clases de modelo para `Product` y `User`.
- **Pages**: Razor Pages para la interfaz web.
- **wwwroot**: Archivos estáticos (CSS, JS, etc.).


## Ejecutar la Aplicación

La aplicación bien puede ejecutarse desde el IDE o usando el siguiente comando:
```sh
dotnet run
```

La aplicación estará disponible en `http://localhost:5257`

## Endpoints de la API

### Users

- **POST /register**: Crear un nuevo usuario.
Ejemplo de los datos:
```
{
    "Username": "Lautaro",
    "Email": "lautaro.mongelo@gmail.com",
    "Password": "12345"
}
```
- **POST /login**: Loguearse en un usuario existente.
Ejemplo de los datos:
```
{
    "Email": "lautaro.mongelo@gmail.com",
    "Password": "12345"
}
```


### Products

- **GET /product**: Listar todos los productos.
- **GET /product/{id}**: Obtener el detalle de un producto por ID.
- **POST /product**: Crear un nuevo producto.
Ejemplo de los datos:
```
{
    "Price": 50,
    "name": "Auriculares2",
    "stock": 1,
    "category": "Category2"
}
```

- **PUT /product/{id}**: Actualizar un producto existente.
Ejemplo de los datos:
```
{
    "Price": 50,
    "name": "Auriculares2",
    "stock": 1,
    "category": "Category2"
}
```

- **DELETE /product/{id}**: Eliminar un producto por ID.
- **GET /product/budget?budget={Presupuesto}**: Devuelve los productos filtrados con las indicaciones del challenge, validando anteriormente que cumplan los requisitos.
