# Prueba Tecnica Backend

Este proyecto es una aplicación backend desarrollada en **.NET Core** que gestiona entidades como **Usuarios**, **Estudiantes**, **Profesores**, **Cursos** y **Calificaciones**. Proporciona una API para interactuar con estas entidades y maneja la auditoría de cambios en la base de datos.

## Tabla de Contenidos

- [Estructura del Proyecto](#estructura-del-proyecto)
- [Arquitectura del Proyecto](#arquitectura-del-proyecto)
- [Paquetes NuGet Utilizados](#paquetes-nuget-utilizados)
- [Configuración Inicial](#configuracion-inicial)
- [Uso](#uso)
- [Configuración de la Base de Datos](#configuracion-de-la-base-de-datos)
- [Entidades Principales](#entidades-principales)
- [Auditoría](#auditoria)
- [Consideraciones Adicionales](#consideraciones-adicionales)

## Estructura del Proyecto

El proyecto está organizado en las siguientes capas:

- **Domain**: Contiene las entidades principales y sus configuraciones.
- **Application**: Contiene la lógica de negocio y los casos de uso
- **Infrastructure**: Maneja la configuración de la base de datos, repositorios y la lógica de acceso a datos.
- **Presentation**: Incluye los controladores de la API y la configuración de la aplicación.


## Arquitectura del Proyecto

Este proyecto sigue una arquitectura basada en **capas** con separación de responsabilidades:

1. **Domain Layer (Capa de Dominio)**: Define las entidades y la lógica de negocio.
2. **Infrastructure Layer (Capa de Infraestructura)**: Contiene la configuración de Entity Framework Core y los repositorios.
3. **Application Layer (Capa de Aplicación)**: Contiene los servicios de aplicación y la lógica específica de la API.
4. **Presentation Layer (Capa de Presentación)**: Contiene los controladores de la API y las configuraciones de ASP.NET Core.

## Paquetes NuGet Utilizados

El proyecto usa los siguientes paquetes NuGet:

- **Microsoft.EntityFrameworkCore** - ORM para gestionar la base de datos.
- **Microsoft.EntityFrameworkCore.SqlServer** - Proveedor de SQL Server para EF Core.
- **Microsoft.EntityFrameworkCore.Tools** - Herramientas de migraciones y gestión de la base de datos.
- **Microsoft.AspNetCore.Identity** - Manejo de autenticación y usuarios.
- **Swashbuckle.AspNetCore** - Generación de documentación con Swagger.
- **Serilog** - Registro de logs.
- **MediatR** - Implementación del patrón Mediator para desacoplar la lógica de negocio.
- **AutoMapper** - Mapeo automático entre objetos DTO y entidades.

## Configuración Inicial

1. **Clonar el repositorio**:

   ```bash
   git clone https://github.com/jdbalantar/PruebaTecnicaBackend.git
   cd PruebaTecnicaBackend
   ```

2. **Restaurar paquetes NuGet**:

   ```bash
   dotnet restore
   ```

3. **Configurar la cadena de conexión en `appsettings.json`**:

   Ubica el archivo `appsettings.json` en el proyecto **Presentation (Api)** y modifica la sección `ConnectionStrings` con la configuración de tu base de datos:

   ```json
   "ConnectionStrings": {
     "Default": "Server=TU_SERVIDOR;Database=TU_BASE_DE_DATOS;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;"
   }
   ```

   ⚠ **Importante**: No olvides reemplazar `TU_SERVIDOR`, `TU_BASE_DE_DATOS`, `TU_USUARIO` y `TU_CONTRASEÑA` con los valores adecuados.

4. **Aplicar migraciones y actualizar la base de datos**:

   ```bash
   dotnet ef database update
   ```

   Esto creará las tablas necesarias en la base de datos configurada.

## Uso

Para ejecutar la aplicación:

```bash
dotnet run --project Api
```

La API estará disponible en `https://localhost:7119` o `http://localhost:5079`, según la configuración.

## Configuración de la Base de Datos

El proyecto utiliza **Entity Framework Core** para la gestión de la base de datos. Asegúrate de que el servidor SQL configurado en `appsettings.json` esté activo antes de ejecutar las migraciones.

Si necesitas regenerar las migraciones, puedes ejecutar:

```bash
dotnet ef migrations add NombreDeMigracion
```

O desde el package manager

```bash
Add-Migration "NombreMigración" -Context ApplicationDbContext
```

Para actualizar la base de datos:

```bash
dotnet ef update-database
```

O desde el package manager


```bash
Update-Database -Context ApplicationDbContext
```

## Entidades Principales

El proyecto gestiona las siguientes entidades:

- **User**: Representa a un usuario del sistema, con propiedades como `FirstName`, `LastName` y `Identification`.
- **Teacher**: Hereda de `User` y contiene información adicional específica de los profesores.
- **Student**: Hereda de `User` y contiene información adicional específica de los estudiantes.
- **Course**: Representa un curso, con propiedades como `Name`, `Description` y una relación con `Teacher` y `Student`.
- **Qualification**: Almacena las calificaciones de los estudiantes en los cursos.

## Auditoría

El sistema implementa una auditoría para rastrear los cambios en las entidades. Cada vez que se realiza una operación de creación, actualización o eliminación, se registra una entrada en la tabla `AuditLogs` con información relevante, como el usuario que realizó la acción, la entidad afectada y los valores anteriores y nuevos.

## Consideraciones Adicionales

- **Manejo de Relaciones**: Se ha configurado el comportamiento de eliminación en las relaciones para evitar conflictos de eliminación en cascada múltiple.
- **Datos Semilla**: Se han definido datos iniciales para las entidades principales en el método `SeedData` del `ApplicationDbContext`.
- **Autenticación y Autorización**: El proyecto usa **ASP.NET Core Identity** para la gestión de usuarios y roles.
- **Manejo de Errores**: Se ha implementado middleware para capturar y manejar errores en la API.
- **Documentación API**: Se recomienda utilizar **Swagger** para explorar los endpoints disponibles. Puedes habilitarlo en `Startup.cs` agregando:

  ```csharp
  services.AddSwaggerGen();
  ```

  Y acceder a la documentación en `https://localhost:7119/swagger/index.html`.

Para más detalles, revisa los archivos fuente y la configuración en el repositorio.

---

_Desarrollado por [jdbalantar](https://github.com/jdbalantar)._

