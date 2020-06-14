<div align="center">
	<a href="#">
		<img src="https://next.salud.gob.sv/index.php/apps/gallery/preview/136477">
	</a>
</div>
<center><h1>Universal Identity</h1></center>
<center><h3>Ministerio de Salud de El Salvador (MINSAL)</h3></center>


## Tabla de Contenido

* [Descripción](#descripción)
* [Instalación](#instalación)
* [Colaboradores](#colaboradores)



## Descripción

El Ministerio de Salud necesita mostrar información de Consumos y Existencias de todos los sistemas que registran esos datos, CONEX es la Api que se encarga de almacenar y servir este dato. Su función es ser el banco de datos de los sistemas que requieren no solo registrar su propia información sino mostrar el conjunto de datos de consumos y existencias (por medio de reportes, tableros, etc.)

Para este propocito CONEX es una API Rest con autenticación JWT que expone los EndPoints para las diferentes operaciones que se requieren:

- **Get:** Para obtener la data de consumos y existencias por fecha determinada
- **Post:** Para que los clientes con usuarios autorizados puedan enviar su información diaria a CONEX
- **Put:** Para modificar un dato de consumos y existencias en una fecha dada.



## Instalación

Los requerimientos y forma de instalación de este proyecto se detallan en el siguiente enlace [**INSTALL.md**](http://codigo.salud.gob.sv/abastecimiento/UniversalIdentity/tree/master/INSTALL.md). 



## Colaboradores
<div align="center">
                <div align="center">
                    <a href="http://codigo.salud.gob.sv/faldana"  target="_blank"><img  style="width: 90px; height: 90px;" width="90" src="http://codigo.salud.gob.sv/uploads/-/system/user/avatar/49/avatar.png"></a><br />
                   Farid Antonio Pérez Aldana<br/>
                    <a href="mailto:faldana@salud.gob.sv">faldana@salud.gob.sv</a>
                </div>                  
</div>

<div align="center">
    <b>Dirección de Tecnologías de Información y Comunicaciones (DTIC).</b><br />
    <b>Ministerio de Salud</b><br />
    <a href="http://www.salud.gob.sv" alt="minsal" target="_blank">www.salud.gob.sv</a>
</div>
