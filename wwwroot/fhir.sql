-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- fhir.dbo.device_site definition

-- Drop table

-- DROP TABLE fhir.dbo.device_site GO

CREATE TABLE fhir.dbo.device_site (
	id int IDENTITY(1,1) NOT NULL,
	device_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	latitud float NULL,
	longitud float NULL,
	ciudad_registro varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	sitio_registro varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	sitio_registro_id int NULL,
	nit varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_device_site PRIMARY KEY (id)
) GO;


-- fhir.dbo.devices definition

-- Drop table

-- DROP TABLE fhir.dbo.devices GO

CREATE TABLE fhir.dbo.devices (
	id int IDENTITY(1,1) NOT NULL,
	device_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	device_tkn varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	device_tag varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	device_pwd varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	device_name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ip_addr varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	net_mask varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	gateway varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ddns1 varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ddns2 varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	dhcp bit NULL,
	dev_model varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	firmware_ver varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	firmware_date varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	temp_dec_en bit NULL,
	stranger_pass_en bit NULL,
	mask_check_en bit NULL,
	alarm_temp float NULL,
	temp_comp float NULL,
	record_time_save int NULL,
	save_record bit NULL,
	save_jpeg bit NULL,
	mqtt_enable int NULL,
	mqtt_retain int NULL,
	pqos int NULL,
	sqos int NULL,
	mqtt_port int NULL,
	mqtt_server varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	mqtt_username varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	mqtt_password varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	topic2publish varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	topic2subscribe varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	heartbeat int NULL,
	bound bit NOT NULL,
	CONSTRAINT PK_devices PRIMARY KEY (id)
) GO;


-- fhir.dbo.devices_employees definition

-- Drop table

-- DROP TABLE fhir.dbo.devices_employees GO

CREATE TABLE fhir.dbo.devices_employees (
	id int IDENTITY(1,1) NOT NULL,
	device_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	user_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	status bit NOT NULL,
	created datetime NULL,
	CONSTRAINT PK_devices_employees PRIMARY KEY (id)
) GO;


-- fhir.dbo.employees definition

-- Drop table

-- DROP TABLE fhir.dbo.employees GO

CREATE TABLE fhir.dbo.employees (
	id int IDENTITY(1,1) NOT NULL,
	id_lenel varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	lastname varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	firstname varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ssno varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	id_status varchar(5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	documento varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	empresa varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	imageUrl varchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	start_time datetime NULL,
	end_time datetime NULL,
	nit varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	hasPhoto bit NULL,
	created datetime NULL,
	ciudadEnroll varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_employees PRIMARY KEY (id)
) GO;


-- fhir.dbo.fhir_data definition

-- Drop table

-- DROP TABLE fhir.dbo.fhir_data GO

CREATE TABLE fhir.dbo.fhir_data (
	id int IDENTITY(1,1) NOT NULL,
	tipo_documento varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	numero_documento bigint NOT NULL,
	fecha_registro datetime NOT NULL,
	temperatura float NOT NULL,
	ciudad_registro varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	sitio_registro varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	sitio_registro_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	latitud float NOT NULL,
	longitud float NOT NULL,
	nit varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	reportado int NOT NULL,
	instrumento varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	tipo_calibracion varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	tipo_medicion varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	valor_calibracion float NOT NULL,
	id_lenel bigint NOT NULL,
	CONSTRAINT PK_fhir_data PRIMARY KEY (id)
) GO;


-- fhir.dbo.upload_person definition

-- Drop table

-- DROP TABLE fhir.dbo.upload_person GO

CREATE TABLE fhir.dbo.upload_person (
	id int IDENTITY(1,1) NOT NULL,
	msgType int NOT NULL,
	[similar] float NOT NULL,
	user_id int NULL,
	name varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[time] datetime NOT NULL,
	temperature float NOT NULL,
	mask int NOT NULL,
	[matched] int NOT NULL,
	device_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	imageUrl varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_uload_person PRIMARY KEY (id)
) GO;


-- fhir.dbo.usuario definition

-- Drop table

-- DROP TABLE fhir.dbo.usuario GO

CREATE TABLE fhir.dbo.usuario (
	id int IDENTITY(1,1) NOT NULL,
	login nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_usuario PRIMARY KEY (id)
) GO;


-- dbo.RECO_DIAS source

CREATE VIEW RECO_DIAS AS
SELECT CONVERT(VARCHAR, dbo.upload_person.time,111) AS Fecha, MAX(dbo.upload_person.[time]) as time, dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, COUNT(*) AS reconocimientosDia, COUNT(DISTINCT(user_id)) As Personas, SUM(CASE WHEN dbo.upload_person.temperature >= 37.3 THEN 1 ELSE 0 END)  AS Alertas  FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY CONVERT(VARCHAR, dbo.upload_person.time,111), dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro;


-- dbo.REC_ECOPETROL source

CREATE VIEW  REC_ECOPETROL AS
SELECT dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, COUNT(*) AS reconocimientosDia, COUNT(DISTINCT(user_id)) As Personas, SUM(CASE WHEN EM.Empresa LIKE '%Ecopetrol%' THEN 1 ELSE 0 END)  AS Funcionarios, SUM(CASE WHEN dbo.upload_person.temperature > 37.5 THEN 1 ELSE 0 END)  AS Alertas  FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
WHERE dbo.upload_person.[time] > '2020-07-22 12:00:00' AND dbo.upload_person.[time] < '2020-07-22 12:00:00'
AND dbo.upload_person.user_id > 0 AND EM.empresa LIKE '%Ecopetrol%' AND dbo.upload_person.temperature > 37.5
GROUP BY dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro;


-- dbo.REC_GENERAL source

CREATE VIEW REC_GENERAL AS
SELECT dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, CONVERT(VARCHAR,dbo.upload_person.time,111) AS Fecha, MAX(dbo.upload_person.time) as time, COUNT(*) AS reconocimientosDia, COUNT(DISTINCT(user_id)) As Personas, SUM(CASE WHEN dbo.upload_person.temperature > 37.5 THEN 1 ELSE 0 END)  AS Alertas  FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, CONVERT(VARCHAR,dbo.upload_person.time,111), CONVERT(VARCHAR(10),dbo.upload_person.time,111);


-- dbo.REPO_ENROLAMIENTO source

CREATE VIEW REPO_ENROLAMIENTO AS
SELECT  Cast(CONVERT(varchar(10), created, 111) as date) as Fecha, created as time,  documento, CONCAT(firstname, ' ' , lastname) AS name, empresa , (CASE WHEN info_fotos.Fotos > 1 THEN CAST(1 AS BIT) ELSE Cast(0 AS BIT) END) As hasPhoto
FROM dbo.employees
LEFT JOIN (SELECT dbo.devices_employees.user_id, COUNT(*) AS Fotos FROM dbo.devices_employees WHERE dbo.devices_employees.status = 1 GROUP BY  dbo.devices_employees.user_id) AS info_fotos ON (info_fotos.user_id = dbo.employees.documento );


-- dbo.REPO_ENROLL_DEVICE source

CREATE VIEW REPO_ENROLL_DEVICE AS
SELECT DBO.devices_employees.user_id, CONCAT(EM.firstname, ' ' , EM.lastname) AS Nombre, DS.ciudad_registro, DS.sitio_registro, dbo.devices_employees.device_id,  count(*) intentos, SUM(CASE WHEN dbo.devices_employees.status=1 THEN 1 ELSE 0 END) as exitos, (CASE WHEN  count(*) > SUM(CASE WHEN dbo.devices_employees.status=1 THEN 1 ELSE 0 END) THEN 1 ELSE 0 END) AS Problemas, ( CASE WHEN SUM(CASE WHEN dbo.devices_employees.status=1 THEN 1 ELSE 0 END) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END ) AS hasPhoto
FROM dbo.devices_employees
RIGHT JOIN dbo.device_site AS DS ON (DS.device_id = dbo.devices_employees.device_id)
LEFT JOIN dbo.employees AS EM ON (EM.documento = dbo.devices_employees.user_id)
GROUP BY DBO.devices_employees.user_id, DS.ciudad_registro, DS.sitio_registro, dbo.devices_employees.device_id, EM.firstname, EM.lastname;


-- dbo.REPO_ENROLL_PHOTO source

CREATE VIEW REPO_ENROLL_PHOTO AS
SELECT DS.ciudad_registro, DS.sitio_registro, DE.device_id,DE.created, EM.documento, DE.status
FROM dbo.devices_employees AS DE
JOIN dbo.device_site AS DS ON (DS.device_id = DE.device_id)
JOIN dbo.employees AS EM ON (EM.documento = DE.user_id)
GROUP BY DS.ciudad_registro, DS.sitio_registro, DE.device_id,DE.created, EM.documento, DE.status;


-- dbo.SOPO_EVRECON_DIAS source

CREATE VIEW SOPO_EVRECON_DIAS as
SELECT dbo.upload_person.time, EM.documento,  dbo.upload_person.name, EM.empresa, dbo.upload_person.device_id,  DS.ciudad_registro, DS.sitio_registro, dbo.upload_person.similar, dbo.upload_person.temperature  
FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY dbo.upload_person.time, EM.documento,  dbo.upload_person.name,  EM.empresa, dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, dbo.upload_person.similar, dbo.upload_person.temperature;


-- dbo.SOPO_RECON_DIAS source

CREATE VIEW SOPO_RECON_DIAS AS
SELECT CONVERT(VARCHAR,dbo.upload_person.time,111) as Fecha, dbo.upload_person.time, EM.documento,  dbo.upload_person.name, EM.empresa, dbo.upload_person.device_id,  DS.ciudad_registro, DS.sitio_registro, dbo.upload_person.similar, dbo.upload_person.temperature  
FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0;


-- dbo.SOPO_RECO_PERSONA_DEV source

CREATE VIEW SOPO_RECO_PERSONA_DEV AS
SELECT DISTINCT(dbo.upload_person.user_id), dbo.upload_person.time as Fecha,  dbo.upload_person.name, EM.empresa, dbo.upload_person.device_id,  DS.ciudad_registro, DS.sitio_registro FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY dbo.upload_person.user_id,CONVERT(VARCHAR(10),dbo.upload_person.time,111), dbo.upload_person.time,   dbo.upload_person.name,  EM.empresa, dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro;


-- dbo.SOPO_RECO_PERSONA_DIA source

CREATE VIEW SOPO_RECO_PERSONA_DIA AS
SELECT dbo.upload_person.user_id, dbo.upload_person.name, MAX(dbo.upload_person.time) AS time, CONVERT(VARCHAR,dbo.upload_person.time,111 )  AS Fecha, EM.empresa,  DS.ciudad_registro
FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY dbo.upload_person.user_id, CONVERT(VARCHAR,dbo.upload_person.time,111 ), dbo.upload_person.name,  EM.empresa, DS.ciudad_registro;
-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- fhir.dbo.device_site definition

-- Drop table

-- DROP TABLE fhir.dbo.device_site GO

CREATE TABLE fhir.dbo.device_site (
	id int IDENTITY(1,1) NOT NULL,
	device_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	latitud float NULL,
	longitud float NULL,
	ciudad_registro varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	sitio_registro varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	sitio_registro_id int NULL,
	nit varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_device_site PRIMARY KEY (id)
) GO;


-- fhir.dbo.devices definition

-- Drop table

-- DROP TABLE fhir.dbo.devices GO

CREATE TABLE fhir.dbo.devices (
	id int IDENTITY(1,1) NOT NULL,
	device_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	device_tkn varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	device_tag varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	device_pwd varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	device_name varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ip_addr varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	net_mask varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	gateway varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ddns1 varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ddns2 varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	dhcp bit NULL,
	dev_model varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	firmware_ver varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	firmware_date varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	temp_dec_en bit NULL,
	stranger_pass_en bit NULL,
	mask_check_en bit NULL,
	alarm_temp float NULL,
	temp_comp float NULL,
	record_time_save int NULL,
	save_record bit NULL,
	save_jpeg bit NULL,
	mqtt_enable int NULL,
	mqtt_retain int NULL,
	pqos int NULL,
	sqos int NULL,
	mqtt_port int NULL,
	mqtt_server varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	mqtt_username varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	mqtt_password varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	topic2publish varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	topic2subscribe varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	heartbeat int NULL,
	bound bit NOT NULL,
	CONSTRAINT PK_devices PRIMARY KEY (id)
) GO;


-- fhir.dbo.devices_employees definition

-- Drop table

-- DROP TABLE fhir.dbo.devices_employees GO

CREATE TABLE fhir.dbo.devices_employees (
	id int IDENTITY(1,1) NOT NULL,
	device_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	user_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	status bit NOT NULL,
	created datetime NULL,
	CONSTRAINT PK_devices_employees PRIMARY KEY (id)
) GO;


-- fhir.dbo.employees definition

-- Drop table

-- DROP TABLE fhir.dbo.employees GO

CREATE TABLE fhir.dbo.employees (
	id int IDENTITY(1,1) NOT NULL,
	id_lenel varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	lastname varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	firstname varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ssno varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	id_status varchar(5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	status varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	documento varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	empresa varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	imageUrl varchar(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	start_time datetime NULL,
	end_time datetime NULL,
	nit varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	hasPhoto bit NULL,
	created datetime NULL,
	ciudadEnroll varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_employees PRIMARY KEY (id)
) GO;


-- fhir.dbo.fhir_data definition

-- Drop table

-- DROP TABLE fhir.dbo.fhir_data GO

CREATE TABLE fhir.dbo.fhir_data (
	id int IDENTITY(1,1) NOT NULL,
	tipo_documento varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	numero_documento bigint NOT NULL,
	fecha_registro datetime NOT NULL,
	temperatura float NOT NULL,
	ciudad_registro varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	sitio_registro varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	sitio_registro_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	latitud float NOT NULL,
	longitud float NOT NULL,
	nit varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	reportado int NOT NULL,
	instrumento varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	tipo_calibracion varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	tipo_medicion varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	valor_calibracion float NOT NULL,
	id_lenel bigint NOT NULL,
	CONSTRAINT PK_fhir_data PRIMARY KEY (id)
) GO;


-- fhir.dbo.upload_person definition

-- Drop table

-- DROP TABLE fhir.dbo.upload_person GO

CREATE TABLE fhir.dbo.upload_person (
	id int IDENTITY(1,1) NOT NULL,
	msgType int NOT NULL,
	[similar] float NOT NULL,
	user_id int NULL,
	name varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[time] datetime NOT NULL,
	temperature float NOT NULL,
	mask int NOT NULL,
	[matched] int NOT NULL,
	device_id varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	imageUrl varchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_uload_person PRIMARY KEY (id)
) GO;


-- fhir.dbo.usuario definition

-- Drop table

-- DROP TABLE fhir.dbo.usuario GO

CREATE TABLE fhir.dbo.usuario (
	id int IDENTITY(1,1) NOT NULL,
	login nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	nombre nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_usuario PRIMARY KEY (id)
) GO;


-- dbo.RECO_DIAS source

CREATE VIEW RECO_DIAS AS
SELECT CONVERT(VARCHAR, dbo.upload_person.time,111) AS Fecha, MAX(dbo.upload_person.[time]) as time, dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, COUNT(*) AS reconocimientosDia, COUNT(DISTINCT(user_id)) As Personas, SUM(CASE WHEN dbo.upload_person.temperature >= 37.3 THEN 1 ELSE 0 END)  AS Alertas  FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
WHERE dbo.upload_person.user_id > 0
GROUP BY CONVERT(VARCHAR, dbo.upload_person.time,111), dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro;


-- dbo.REC_ECOPETROL source

CREATE VIEW  REC_ECOPETROL AS
SELECT dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, COUNT(*) AS reconocimientosDia, COUNT(DISTINCT(user_id)) As Personas, SUM(CASE WHEN EM.Empresa LIKE '%Ecopetrol%' THEN 1 ELSE 0 END)  AS Funcionarios, SUM(CASE WHEN dbo.upload_person.temperature > 37.5 THEN 1 ELSE 0 END)  AS Alertas  FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
WHERE dbo.upload_person.[time] > '2020-07-22 12:00:00' AND dbo.upload_person.[time] < '2020-07-22 12:00:00'
AND dbo.upload_person.user_id > 0 AND EM.empresa LIKE '%Ecopetrol%' AND dbo.upload_person.temperature > 37.5
GROUP BY dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro;


-- dbo.REC_GENERAL source

CREATE VIEW REC_GENERAL AS
SELECT dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, CONVERT(VARCHAR,dbo.upload_person.time,111) AS Fecha, MAX(dbo.upload_person.time) as time, COUNT(*) AS reconocimientosDia, COUNT(DISTINCT(user_id)) As Personas, SUM(CASE WHEN dbo.upload_person.temperature > 37.5 THEN 1 ELSE 0 END)  AS Alertas  FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, CONVERT(VARCHAR,dbo.upload_person.time,111), CONVERT(VARCHAR(10),dbo.upload_person.time,111);


-- dbo.REPO_ENROLAMIENTO source

CREATE VIEW REPO_ENROLAMIENTO AS
SELECT  Cast(CONVERT(varchar(10), created, 111) as date) as Fecha, created as time,  documento, CONCAT(firstname, ' ' , lastname) AS name, empresa , (CASE WHEN info_fotos.Fotos > 1 THEN CAST(1 AS BIT) ELSE Cast(0 AS BIT) END) As hasPhoto
FROM dbo.employees
LEFT JOIN (SELECT dbo.devices_employees.user_id, COUNT(*) AS Fotos FROM dbo.devices_employees WHERE dbo.devices_employees.status = 1 GROUP BY  dbo.devices_employees.user_id) AS info_fotos ON (info_fotos.user_id = dbo.employees.documento );


-- dbo.REPO_ENROLL_DEVICE source

CREATE VIEW REPO_ENROLL_DEVICE AS
SELECT DBO.devices_employees.user_id, CONCAT(EM.firstname, ' ' , EM.lastname) AS Nombre, DS.ciudad_registro, DS.sitio_registro, dbo.devices_employees.device_id,  count(*) intentos, SUM(CASE WHEN dbo.devices_employees.status=1 THEN 1 ELSE 0 END) as exitos, (CASE WHEN  count(*) > SUM(CASE WHEN dbo.devices_employees.status=1 THEN 1 ELSE 0 END) THEN 1 ELSE 0 END) AS Problemas, ( CASE WHEN SUM(CASE WHEN dbo.devices_employees.status=1 THEN 1 ELSE 0 END) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END ) AS hasPhoto
FROM dbo.devices_employees
RIGHT JOIN dbo.device_site AS DS ON (DS.device_id = dbo.devices_employees.device_id)
LEFT JOIN dbo.employees AS EM ON (EM.documento = dbo.devices_employees.user_id)
GROUP BY DBO.devices_employees.user_id, DS.ciudad_registro, DS.sitio_registro, dbo.devices_employees.device_id, EM.firstname, EM.lastname;


-- dbo.REPO_ENROLL_PHOTO source

CREATE VIEW REPO_ENROLL_PHOTO AS
SELECT DS.ciudad_registro, DS.sitio_registro, DE.device_id,DE.created, EM.documento, DE.status
FROM dbo.devices_employees AS DE
JOIN dbo.device_site AS DS ON (DS.device_id = DE.device_id)
JOIN dbo.employees AS EM ON (EM.documento = DE.user_id)
GROUP BY DS.ciudad_registro, DS.sitio_registro, DE.device_id,DE.created, EM.documento, DE.status;


-- dbo.SOPO_EVRECON_DIAS source

CREATE VIEW SOPO_EVRECON_DIAS as
SELECT dbo.upload_person.time, EM.documento,  dbo.upload_person.name, EM.empresa, dbo.upload_person.device_id,  DS.ciudad_registro, DS.sitio_registro, dbo.upload_person.similar, dbo.upload_person.temperature  
FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY dbo.upload_person.time, EM.documento,  dbo.upload_person.name,  EM.empresa, dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro, dbo.upload_person.similar, dbo.upload_person.temperature;


-- dbo.SOPO_RECON_DIAS source

CREATE VIEW SOPO_RECON_DIAS AS
SELECT CONVERT(VARCHAR,dbo.upload_person.time,111) as Fecha, dbo.upload_person.time, EM.documento,  dbo.upload_person.name, EM.empresa, dbo.upload_person.device_id,  DS.ciudad_registro, DS.sitio_registro, dbo.upload_person.similar, dbo.upload_person.temperature  
FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0;


-- dbo.SOPO_RECO_PERSONA_DEV source

CREATE VIEW SOPO_RECO_PERSONA_DEV AS
SELECT DISTINCT(dbo.upload_person.user_id), dbo.upload_person.time as Fecha,  dbo.upload_person.name, EM.empresa, dbo.upload_person.device_id,  DS.ciudad_registro, DS.sitio_registro FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY dbo.upload_person.user_id,CONVERT(VARCHAR(10),dbo.upload_person.time,111), dbo.upload_person.time,   dbo.upload_person.name,  EM.empresa, dbo.upload_person.device_id, DS.ciudad_registro, DS.sitio_registro;


-- dbo.SOPO_RECO_PERSONA_DIA source

CREATE VIEW SOPO_RECO_PERSONA_DIA AS
SELECT dbo.upload_person.user_id, dbo.upload_person.name, MAX(dbo.upload_person.time) AS time, CONVERT(VARCHAR,dbo.upload_person.time,111 )  AS Fecha, EM.empresa,  DS.ciudad_registro
FROM dbo.upload_person 
JOIN dbo.device_site AS DS ON (DS.device_id = dbo.upload_person.device_id)
JOIN dbo.employees AS EM ON (dbo.upload_person.user_id = EM.documento)
AND dbo.upload_person.user_id > 0
GROUP BY dbo.upload_person.user_id, CONVERT(VARCHAR,dbo.upload_person.time,111 ), dbo.upload_person.name,  EM.empresa, DS.ciudad_registro;
