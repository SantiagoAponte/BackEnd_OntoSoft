# BackEnd_OntoSoft

Este es el BackEnd del proyecto de grado "OntoSoft" en pro de obtener el titulo de Ingeniero Informático en la Universidad Autónoma de Occidente realizado en .NetCore, realizado con finalidades de administrar consultorios odontológicos. Si necesitas correr el proyecto, descargarlo y utiliza en el terminal "dotnet build" en la carpeta raíz del proyecto para compilarlo completamente, luego para runearlo, cambiar a la carpeta "WebApi" y escribir el comando dotnet watch run, una vez este comando se correra el ultimo archivo de migración de la base de datos y creara automáticamente todas las tablas con sus relaciones.

## Comenzando 🚀

Mira **Instalación** para conocer como instalar al detalle el proyecto.

## Herramientas utilizadas 📋

- [Dotnet CLI](https://docs.microsoft.com/es-es/dotnet/core/tools/?tabs=netcore2x)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Nuget](https://www.nuget.org/)
- [Git por línea de comandos](https://git-scm.com/download/win)
- [Git desde Visual Studio Code](https://code.visualstudio.com/docs/editor/versioncontrol)
- [Github](https://github.com/)
- [Postman](https://www.getpostman.com/)
- [Sqlserver] (https://www.microsoft.com/es-es/sql-server/sql-server-downloads)

### .NET Core

- [Introducción a .NET Framework](https://msdn.microsoft.com/es-es/library/hh425099%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396)
- [Introducción a .NET Core](https://docs.microsoft.com/es-es/dotnet/core/index)
- [Los lenguajes de programación disponibles en .NET Core](https://www.microsoft.com/net/learn/languages)
- [Diferencias entre .NET Framework y .NET Core](https://docs.microsoft.com/es-es/dotnet/standard/choosing-core-framework-server)
- [NET Standard](https://blogs.msdn.microsoft.com/dotnet/2016/09/26/introducing-net-standard/)
- [Instalación entorno de trabajo para .NET Core (SDK)](https://www.microsoft.com/net/learn/get-started/windows)
- [Crear proyectos de consola](https://docs.microsoft.com/es-es/dotnet/core/tutorials/using-with-xplat-cli), [librería NET Standard](https://docs.microsoft.com/es-es/dotnet/core/tutorials/library-with-visual-studio) y [Web API](https://docs.microsoft.com/es-es/aspnet/core/tutorials/first-web-api) con .NET Core desde línea de comandos (CLI) y desde Visual Studio
- [Instalación, configuración y uso de Visual Studio Code para editar proyectos C#](https://docs.microsoft.com/es-es/dotnet/core/tutorials/with-visual-studio-code)
- [Instalación, configuración y uso de Visual Studio Community para editar proyectos C#](https://docs.microsoft.com/es-es/dotnet/core/tutorials/with-visual-studio)
- [Crear dependencias entre proyectos dentro de una misma solución](https://msdn.microsoft.com/es-es/library/f3st0d45.aspx)
- [Añadir paquetes nuget a un proyecto](https://docs.microsoft.com/es-es/nuget/quickstart/use-a-package)
- [Utilización de librerías de terceros a través de nuget](https://docs.microsoft.com/es-es/nuget/quickstart/use-a-package)
- [Depurar código C# en Visual Studio Code](https://docs.microsoft.com/es-es/dotnet/core/tutorials/with-visual-studio-code)
- [Depurar código C# en Visual Studio Community](https://docs.microsoft.com/es-es/dotnet/core/tutorials/debugging-with-visual-studio?tabs=csharp)
- [Interacción de usuario (entrada y salida de datos) en un proyecto de Consola](https://docs.microsoft.com/es-es/dotnet/csharp/tutorials/console-teleprompter)

### Git

- [Comandos básicos para flujo de trabajo con Git](http://rogerdudler.github.io/git-guide/index.es.html)
- [Operaciones básicas de Git desde Visual Studio](https://blogs.msdn.microsoft.com/esmsdn/2016/03/04/utilizando-git-en-visual-studio/)
- [Crear y trabajar con un repositorio Git en GitHub](https://desarrolloweb.com/articulos/crear-repositorio-git-codigo.html)

### REST y Web API

- [Introducción a REST](https://dosideas.com/noticias/java/314-introduccion-a-los-servicios-web-restful)
- [Métodos básicos REST (get, post, put, delete)](http://asiermarques.com/2013/conceptos-sobre-apis-rest/)
- [Probar una API REST con Postman](https://www.getpostman.com/docs/)
- [Consumir una API REST desde C# con `HttpClient`](https://docs.microsoft.com/es-es/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client)
- [Crear una API REST con Asp.NET Core y Visual Studio](https://docs.microsoft.com/es-es/aspnet/core/tutorials/first-web-api)
- [Usar respuestas de Web API](http://hamidmosalla.com/2017/03/29/asp-net-core-action-results-explained/) y sus [códigos de error](https://apigee.com/about/blog/technology/restful-api-design-what-about-errors)
- [Crear un modelo de datos relacional](http://www.learnentityframeworkcore.com/relationships)
- [Usar inyección de dependencias para el uso de servicios en controladores Web API](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/dependency-injection)
- [Introducción a ORM (Object Relational Mapper)](https://es.wikipedia.org/wiki/Mapeo_objeto-relacional)
- [Introducción al ORM Entity Framework](https://docs.microsoft.com/en-us/ef/core/)
- [Usar base de datos en memoria](https://stormpath.com/blog/tutorial-entity-framework-core-in-memory-database-asp-net-core)
- [Consultas a la base de datos de forma síncrona con Entity Framework y LINQ](https://docs.microsoft.com/en-us/ef/core/querying/basic)
- [Consultas a la base de datos de forma asíncrona con Entity Framework y LINQ](https://docs.microsoft.com/en-us/ef/core/querying/async)



### Instalación 🔧

_Primero lo bajamos el proyecto de github_
```
-git clone https://github.com/SantiagoAponte/BackEnd_OntoSoft.git
```

_Compilamos el proyecto de manera sencilla_

```
dotnet build (en carpeta raiz)
```

_Cambiamos a la carpeta "WebApi" para runear el proyecto_
```
cd WebApi/
```

_Runeamos el proyecto_
```
dotnet watch run
```

No olvide cambiar la cadena de conexión local en el archivo appsettings.json
```
"ConnectionStrings": {
    "DefaultConnection" : "Data Source=localhost\\SQLEXPRESS; Initial Catalog=bd_ontosoft; Integrated Security=True"
  }
```

## Ejecutando las pruebas ⚙️

_Se utilizo Postman para realizar pruebas en las apis_

```
https://app.getpostman.com/join-team?invite_code=0277b35485e99c540fc6793d8bfc1146&ws=d07c0560-8d4c-41ac-b517-1bf3297d1ece
```

## Despliegue 📦

_El Proyecto se desplego en un servidor con imagen Centos8, usando Nginx y SqlServer_


## Autores ✒️

* **Santiago Aponte Marin** - *Desarrollador BackEnd* - [SantiagoAponte](https://github.com/SantiagoAponte)
* **Fernando Jose Martinez Velez** - *Desarrollador frontend* - [FernandoMartinez](fernando.martinez@uao.edu.co)

## Licencia 📄

Este proyecto está bajo la Licencia de la Universidad Autonoma de Occidente, el uso de este aplicativo como plagio, venta, entre otros, equivale a una denuncia.

## Expresiones de Gratitud 🎁

* Comenta a otros sobre este proyecto 📢
* Invita una cerveza 🍺 o un café ☕ a tu amigo desarrollador. 
* Da las gracias públicamente 🤓.
