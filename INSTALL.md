# Api Consumos y Existencias (CONEX)
### Ministerio de Salud de El Salvador (MINSAL)

La api CONEX almacena los datos diarios de Consumos y Existencias de los sistemas que registran dichas actividades, dichos datos son expuestos por la CONEX (endpoints) para ser consumidos por todo sistema que los requiera.

CONEX usa un contenedor Docker y para almacenar los datos utiliza una base PostgreSQL externa al contenedor. 

Al ser un sistema "Dockerizado" es multiplataforma por lo que puede instalarse en todo sistema operativo que sea compatible con Docker.



## Tabla de contenido

- [Api Consumos y Existencias (CONEX)](#api-consumos-y-existencias-conex)
    - [Ministerio de Salud de El Salvador (MINSAL)](#ministerio-de-salud-de-el-salvador-minsal)
  - [Tabla de contenido](#tabla-de-contenido)
  - [Requisitos](#requisitos)
    - [1. Software](#1-software)
    - [2. Sistema Operativo](#2-sistema-operativo)
    - [3. Navegador Web](#3-navegador-web)
  - [Instalación del Sistema](#instalaci%c3%b3n-del-sistema)
    - [1) Obtener el compilado de la última versión del código fuente](#1-obtener-el-compilado-de-la-%c3%baltima-versi%c3%b3n-del-c%c3%b3digo-fuente)
    - [2) Configurar la conexión a la Base de Datos](#2-configurar-la-conexi%c3%b3n-a-la-base-de-datos)
    - [3) Generar imagen Docker](#3-generar-imagen-docker)
    - [4) Correr imagen en un contender Docker](#4-correr-imagen-en-un-contender-docker)
    - [5) Creación y extracción de archivo Tar (opcional)](#5-creaci%c3%b3n-y-extracci%c3%b3n-de-archivo-tar-opcional)



## Requisitos

Para instalar CONEX se debe tener previamente instalado y configurado Docker, así como también PostgreSQL local o un servidor PostgreSQL donde poder correr el script Sql para generar la base de datos.



### 1. Software

| Software                                   | Versión     |
| ------------------------------------------ | ----------- |
| Docker                                     | Latest |
| IIS7+ / Apache u otro servidor web compatible |  |
| PostgreSQL (si es instalación local)       | 12.+      |
| Última versión del compilado de la API | -       |




### 2. Sistema Operativo

CONEX ha sido probado en Windows 10, en Ununtu 18 y en Debian, otros sistemas operativos no presentarán dificultades si son compatibles con Docker.



### 3. Navegador Web

Debido a la no estandarización en lo referente al motor de renderizado de los diferentes navegadores web, CONEX ha sido optimizado para los siguientes navegadores, por lo cual se recomienda su uso:

- Mozilla Firefox 26.0 o superior

- Google Chrome 31.0.1650.63-1 o superior

- Otros navegadores basados en Chromium 31.+

  

Instalación del Sistema
------------------------

Los pasos para instalar CONEX son los siguientes.

### 1) Obtener el compilado de la última versión del código fuente

CONEX ha sido desarrollado bajo .Net Core 3.+, por lo que si usted desea compilar el código puede hacerlo clonando el repositorio correspondiente.

    git@codigo.salud.gob.sv:abastecimiento/api_consumos-existencias.git

Luego de esto ya puede compilar el repositorio, antes deberá tener instalado y configurado .Net Core en su sistema operativo. Para mas referencias sobre instalación de .NET Core visite [Instalación de .NET Core](https://dotnet.microsoft.com/download).

Una vez dentro de la carpeta del repositorio clonado ejecute

```
> dotnet run
```

La aplicación deberá mostrarse en su navegador predeterminado.



### 2) Configurar la conexión a la Base de Datos

Para configurar su cadena de conexión debe editar el archivo "*appsettings.json*" que se encuentra en la carpeta raíz del proyecto, si no existe deberá crearlo, este es su contenido por defecto

    {
      "AuthOptions": {
        "SecureKey": "XXX",    
        "ExpiresInHours": 12
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "ConnectionStrings": {
        "DefaultConnection": "Server=XXX;Port=5432;Database=XXX;User Id=XXX;Password=XXX;"
      },
      "AllowedHosts": "*"
    }

PostgreSQL por defecto habilita el puerto 5432 para conexiones a la base de datos, si no es el caso también deberá cambiar este valor.

Por lo generar solo es necesario modificar la información pertinente al servidor, nombre de la base de datos e información de usuario y clave.

Una vez hechos los cambios respectivos ya puede generar la imagen Docker.



### 3) Generar imagen Docker

Para verificar los puertos y configuraciones adicionales de Docker dentro de CONEX consulte el Archivo Docker que se proporciona en el repositorio.

Para crear la imagen correspondiente ejecute 

```
# Docker build -t [nombre de imagen]:[tag de la imagen] .

Docker build -t conex:release .
```

> **Importante**: No olvide el punto (.) al final del nombre de la imagen



### 4) Correr imagen en un contender Docker

Ya ha generado la imagen Docker entonces solo falta correrla en un contenedor, para ello ejecute

```
# Docker run --name [nombre del contenedor] -p [puerto local]:[puerto expuesto] [nombre de imagen]:[tag] [otros parametros]

Docker run --name conex_container -p 8000:80 conex:release --restart always
```

La aplicación deberá correr en su navegador bajó http://localhost:8000/ 

*--restart always*: Reinicia el contenedor generado a partir de la imagen cuando el servidor huésped inicia debido a cualquier contingencia.



### 5) Creación y extracción de archivo Tar (opcional)

Cuando se necesita mover la imagen de un ambiente de desarrollo a un ambiente de producción es necesario generar un archivo tar de esa imagen para poder transportarlo de máquina a máquina.

Si se está trabajando directamente en el servidor donde se a compilado el proyecto entonces no es necesario generar un archivo tar.

Si usted está utilizando un sistema operativo diferente al sistema operativo donde desempaquetará su imagen, la nomenclatura Docker cambia para generar un archivo Tar. Deberá ejecutar las ordenes teniendo en cuenta esto.

**Creación**

Sistema operativo diferente:

```
# docker save [imagen]:[tag] -o [nombre del archivo].tar

docker save conex:release -o conexfile.tar
```
Mismo sistema operativo:
```
# docker save [imagen]:[tag] > [nombre del archivo].tar

docker save conex:release > conexfile.tar
```

**Extracción**

Sistema operativo diferente:

```
# docker load -i [nombre del archivo].tar

docker load -i conexfile.tar
```

Mismo sistema operativo:

```
# docker load < [nombre del archivo].tar

docker load < conexfile.tar
```





