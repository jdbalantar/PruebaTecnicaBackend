# Prueba Tecnica Backend

Este proyecto es una aplicación backend desarrollada en **.NET Core** que gestiona entidades como **Usuarios**, **Estudiantes**, **Profesores**, **Cursos** y **Calificaciones**. Proporciona una API para interactuar con estas entidades y maneja la auditoría de cambios en la base de datos.

## Tabla de Contenidos

- [Estructura del Proyecto](#estructura-del-proyecto)
- [Configuración Inicial](#configuracion-inicial)
- [Uso](#uso)
- [Entidades Principales](#entidades-principales)
- [Auditoría](#auditoria)
- [Consideraciones Adicionales](#consideraciones-adicionales)

## Estructura del Proyecto

El proyecto está organizado en las siguientes capas:

- **Domain**: Contiene las entidades principales y sus configuraciones.
- **Infrastructure**: Maneja la configuración de la base de datos, repositorios y la lógica de acceso a datos.
- **Presentation**: Incluye los controladores de la API y la configuración de la aplicación.

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

3. **Aplicar migraciones y actualizar la base de datos**:

   ```bash
   dotnet ef database update
   ```

   Esto creará las tablas necesarias en la base de datos configurada.

## Uso

Para ejecutar la aplicación:

```bash
dotnet run --project Presentation
```

La API estará disponible en `https://localhost:5001` o `http://localhost:5000`, según la configuración.

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

Para más detalles, revisa los archivos fuente y la configuración en el repositorio.

---

_Desarrollado por [jdbalantar](https://github.com/jdbalantar)._