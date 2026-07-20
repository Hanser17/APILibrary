# API Library - Prueba Técnica

## Descripción

API REST desarrollada en ASP.NET Core utilizando una arquitectura en capas inspirada en los principios de Clean Architecture y Arquitectura Hexagonal.

La solución implementa autenticación mediante JWT, autorización basada en roles, patrón repositorio, pruebas de integración y documentación automática mediante Swagger/OpenAPI.

---

# Instrucciones de Ejecución del Proyecto

## 1. Clonar el repositorio

El primer paso consiste en clonar el repositorio desde el servidor remoto hacia la máquina local.

```bash
git clone <url-del-repositorio>
```

Una vez completado el proceso, abrir la solución utilizando Visual Studio o el IDE de preferencia.

---

## 2. Restaurar las dependencias

Restaurar todos los paquetes NuGet requeridos por la solución.

```bash
dotnet restore
```

También es posible realizar esta operación desde Visual Studio mediante la opción:

```text
Restore NuGet Packages
```

---

## 3. Configurar la cadena de conexión

El proyecto utiliza **SQL Server** como motor de base de datos.

Es necesario modificar el archivo `appsettings.json` y configurar la cadena de conexión correspondiente al entorno local.

Ejemplo:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

## 4. Ejecutar las migraciones

La solución contiene dos contextos independientes:

* Contexto de Persistencia de la aplicación.
* Contexto de Identity para autenticación y autorización.

Debido a esto, las migraciones deben ejecutarse de manera independiente para cada contexto.

### Migraciones del contexto de Persistencia

```powershell
Add-Migration InitialMigration -Context API_LibraryContext
Update-Database -Context API_LibraryContext
```

### Migraciones del contexto de Identity

```powershell
Add-Migration InitialIdentityMigration -Context IdentityContext
Update-Database -Context IdentityContext
```

Una vez completado este proceso, las tablas necesarias serán creadas automáticamente en la base de datos.

---

## 5. Ejecutar la aplicación

Al iniciar la aplicación por primera vez se ejecutará automáticamente el proceso de inicialización de datos (*Seed Data*).

Este proceso crea automáticamente:

### Datos de prueba del dominio

* Autores.
* Libros.
* Préstamos.

### Datos de seguridad

* Roles del sistema.
* Usuario Administrador.
* Usuario Básico.

Esto permite comenzar a utilizar la aplicación inmediatamente sin necesidad de insertar información manualmente.

---

## 6. Usuarios de prueba

La aplicación crea automáticamente dos usuarios:

* Usuario con rol **Admin**.
* Usuario con rol **Basic**.

El usuario con rol **Admin** posee permisos para realizar operaciones de escritura sobre el sistema:

* Crear registros.
* Actualizar registros.
* Eliminar registros.

El usuario con rol **Basic** únicamente posee permisos de consulta.

---

## 7. Autenticación mediante Swagger

Para consumir los endpoints protegidos es necesario autenticarse previamente utilizando el endpoint de Login.

Una vez obtenido el JWT, deberá utilizarse el botón **Authorize** disponible en Swagger.

En el cuadro de autenticación debe introducirse únicamente el token generado.

Ejemplo:

```text
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

No es necesario incluir manualmente el prefijo:

```text
Bearer
```

La aplicación se encargará automáticamente de agregarlo a las solicitudes autenticadas.

---

## 8. Probar la API

Una vez autenticado el usuario, podrá consumir todos los endpoints disponibles según los permisos asociados a su rol.

Todos los endpoints se encuentran documentados mediante Swagger/OpenAPI incluyendo:

* Descripción funcional.
* Parámetros de entrada.
* Modelos de respuesta.
* Posibles códigos HTTP.
* Restricciones de autorización.

---

# Decisiones Técnicas y Arquitectónicas

## Arquitectura utilizada

Para el desarrollo de esta solución se adoptó una arquitectura en capas inspirada en los principios de la Arquitectura Hexagonal y Clean Architecture, estableciendo una clara separación de responsabilidades entre cada componente del sistema y garantizando un bajo acoplamiento entre las distintas partes de la aplicación.

La jerarquía de dependencias fue diseñada de forma que las capas internas no dependan de las externas, siendo el dominio el núcleo del sistema y la única capa completamente independiente.

La solución se encuentra organizada en las siguientes capas:

* **Domain**
* **Application**
* **Infrastructure**

  * Persistence
  * Identity
* **API**
* **Integration Tests**

---

## Capa de Dominio (Domain)

La capa de dominio representa el núcleo del negocio y no posee dependencias hacia ninguna otra capa del sistema.

Dentro de esta capa se encuentran:

* Entidades de negocio como Autores, Libros y Préstamos.
* Interfaces de repositorios que definen contratos sin depender de implementaciones concretas.
* Enumeraciones y objetos de valor.
* Configuraciones compartidas del dominio.

Esta separación permite aislar completamente las reglas de negocio de tecnologías específicas como Entity Framework Core o SQL Server.

---

## Capa de Aplicación (Application)

La capa de aplicación depende exclusivamente del dominio y es responsable de orquestar los casos de uso del sistema.

### DTOs

Se utilizaron DTOs para evitar exponer directamente las entidades del dominio hacia los consumidores externos de la API.

Beneficios:

* Reduce el acoplamiento.
* Mejora la seguridad.
* Facilita la evolución del dominio.

### Interfaces de Servicios

Los controladores dependen de abstracciones y no de implementaciones concretas, aplicando el principio **Dependency Inversion** de SOLID.

Beneficios:

* Facilita el testing.
* Reduce el acoplamiento.
* Permite sustituir implementaciones futuras.

### Servicios de Aplicación

Los servicios contienen la lógica de aplicación y orquestan los distintos casos de uso.

### AutoMapper

Se utilizó AutoMapper para simplificar la conversión entre entidades y DTOs, reduciendo código repetitivo y centralizando las configuraciones de mapeo.

---

## Registro de Dependencias

Cada capa expone un método de extensión para registrar sus dependencias en el contenedor de inversión de control.

Ejemplos:

* `AddApplicationDependencies()`
* `AddPersistenceDependencies()`
* `AddIdentityDependencies()`
* `AddSwaggerDocumentation()`

Esto mantiene el archivo `Program.cs` limpio y fácil de mantener.

---

## Capa de Persistencia (Persistence)

La capa de persistencia es responsable del acceso a datos y de la comunicación con SQL Server mediante Entity Framework Core.

### DbContext

Representa la conexión con la base de datos y el punto central de acceso a las entidades persistidas.

### Patrón Repositorio

Se implementó el patrón repositorio para abstraer completamente la tecnología de acceso a datos.

Beneficios:

* Desacoplar el dominio de Entity Framework Core.
* Centralizar las operaciones de persistencia.
* Facilitar las pruebas.
* Permitir sustituir el proveedor de persistencia en el futuro.

Además se implementó un **Repositorio Genérico** para evitar duplicación de código en operaciones CRUD comunes.

Los métodos fueron definidos como `virtual` para permitir su extensión mediante repositorios específicos, aplicando el principio **Open/Closed Principle**.

### Seeds de Datos

Durante el primer arranque de la aplicación se crean automáticamente:

* Autores de prueba.
* Libros de prueba.
* Préstamos de prueba.

---

## Capa de Identity

La autenticación y autorización fueron aisladas en una capa independiente para desacoplar la solución del proveedor de identidad actual.

Aunque actualmente se utiliza ASP.NET Core Identity, el diseño permite migrar fácilmente a soluciones como:

* Auth0
* Azure AD
* Keycloak
* Identity Server

Dentro de esta capa se encuentran:

* Contexto de Identity.
* Entidad de usuario.
* Refresh Tokens.
* Configuración JWT.
* Servicios de autenticación.
* Registro de dependencias.

### Seeds de Seguridad

Durante la inicialización se crean automáticamente:

* Roles del sistema.
* Usuario administrador.
* Usuario básico.

---

## Capa API

La API representa la puerta de entrada del sistema y contiene exclusivamente responsabilidades relacionadas con HTTP.

### Controladores

Reciben las solicitudes HTTP y delegan la ejecución de los casos de uso hacia la capa de aplicación.

### Middleware Global de Excepciones

Se implementó un middleware para capturar excepciones globales y devolver respuestas consistentes al cliente.

Beneficios:

* Centralización del manejo de errores.
* Reducción de duplicación.
* Estandarización de respuestas.

### Swagger

Swagger/OpenAPI fue utilizado para documentar automáticamente:

* Endpoints.
* Modelos.
* Parámetros.
* Códigos HTTP.
* Requisitos de autenticación.

---

## Pruebas de Integración

Se implementó un proyecto independiente de pruebas de integración utilizando xUnit.

Las pruebas validan el flujo completo del sistema atravesando todas las capas de la aplicación:

* API
* Servicios
* Repositorios
* Base de datos
* Seguridad

Se implementó además un mecanismo de autenticación automática para evitar repetir el proceso de login en cada prueba.

---

## Principios y Patrones Aplicados

### Principios SOLID

* Single Responsibility Principle
* Open/Closed Principle
* Liskov Substitution Principle
* Interface Segregation Principle
* Dependency Inversion Principle

### Patrones Utilizados

* Repository Pattern
* Generic Repository Pattern
* Dependency Injection
* Middleware Pattern
* Extension Methods Pattern

---

## Conclusión

El objetivo principal de estas decisiones arquitectónicas fue construir una solución mantenible, escalable y desacoplada, permitiendo la evolución futura del sistema sin afectar sus componentes centrales.
