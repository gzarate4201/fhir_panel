/**
 * (c) QA Ingenieros Ltda., Bogotá Colombia
 * 
 * 
 * Contains code authored by:    
 *   
 *   Alejandro Mejia 
 *   Santiago Urueña
 *   and others as recnognizad or listed in the code.
 */
using System;
using System.IO;
// using System.DrawingCore;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

// Librerias para manejo de MQTT
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

using Json.Net;

// Logging
using Microsoft.Extensions.Logging;

// Librerias para manejo de tareas asincronicas
using System.Threading;
using System.Threading.Tasks;

// Librerias para manejo de Notificaciones Cliente - Servidor
using Microsoft.AspNetCore.SignalR.Client;


// Modelos para Base de datos
using AspStudio.Models;
using AspStudio.Data;

/// <summary>
/// Esta función crea las clases con los parámetros entregados por el dispositivo
/// Se crean otras subclases para clasificar dichos parámetros
/// </summary>
/// <param name="">
/// La función no tiene parámetros, por lo cual realiza la configuración sin retornar nada
/// </param>

namespace Mqtt.Client.AspNetCore.Services
{
    public class MqttObj {
        public string topic {get; set;}
        public string msg {get; set;}
    }

    public class deviceObj {
        public int code {get; set;}
        public string msg {get; set;}
        public string device_id {get; set;}
        public Int64 dev_cur_pts {get; set;}
        public string tag {get; set;}
    }

    public class deviceDataObj {
        public string device_token {get; set;}
    }

    public class deviceDataBasicObj {
        public string dev_pwd {get; set;}
        public string dev_name {get; set;}
    }

    public class deviceDataNetConfObj {
        
        public string ip_addr {get; set;}
        public string net_mask {get; set;}
        public string gateway {get; set;}
        public string DDNS1 {get; set;}
        public string DDNS2 {get; set;}
        public int DHCP {get; set;}
    }

    public class deviceDataFaceRecObj {
        
        public int dec_face_num_cur {get; set;}
        public int dec_interval_cur {get; set;}
        public int dec_face_num_min {get; set;}
        public int dec_face_num_max {get; set;}
        public int dec_interval_min {get; set;}
        public int dec_interval_max {get; set;}
    }

    public class deviceDataVerInfoObj {
        
        public string dev_model {get; set;}
        public string firmware_ver {get; set;}
        public string firmware_date {get; set;}
    }

    public class deviceDataFunParObj {
        
        public bool temp_dec_en {get; set;}
        public bool stranger_pass_en {get; set;}
        public bool make_check_en {get; set;}
        public float alarm_temp {get; set;}
        public float temp_comp {get; set;}
        public int record_save_time {get; set;}

        public bool save_record {get; set;}
        public bool save_jpeg {get; set;}

    }

    public class deviceDataMqttProObj {
        
        public bool enable {get; set;}
        public bool retain {get; set;}
        public int pqos {get; set;}
        public int sqos {get; set;}
        public int port {get; set;}
        public string server {get; set;}
        public string username {get; set;}
        public string passwd {get; set;}
        public string topic2pulish {get; set;}
        public string topic2subscribe {get; set;}
        public int heartbeat {get; set;}
        

    }
    /// <summary>
    /// Instancia principal para la comunicación MQTT del backend del proyecto
    /// 
    /// </summary>
    /// <param name="">
    /// La función no tiene parámetros, por lo cual realiza la configuración sin retornar nada
    /// </param>
    public class MqttClientService : IMqttClientService
    {
        // Instancias para manejo de la libreria de MQTTnet
        private IMqttClient mqttClient;
        private IMqttClientOptions options;
        

        // Inyeccion clase para manejo de la conexion a BD
        private readonly IServiceScopeFactory _scopeFactory;


       
        // private readonly ApplicationDbContext dbContext;
        HubConnection connection;
        
        public MqttClientService(IMqttClientOptions options, IServiceScopeFactory scopeFactory)
        {
            this.options = options;
            mqttClient = new MqttFactory().CreateMqttClient();

            // Creamos el objeto dbContext
            // dbContext = _dbContext;
            _scopeFactory = scopeFactory;

            // Configuracion del servicio MQTTClient
            ConfigureMqttClient();

            // Configuracion del servicio de mensajeria Cliente-Servidor
            ConfigureHub();
        }
        /// <summary>
        /// Esta función realiza la configuración pára realizar la comunicación entre el backend y el frontend
        /// 
        /// </summary>
        /// <param name="">
        /// La función no tiene parámetros, por lo cual realiza la configuración sin retornar nada
        /// </param>

        private void ConfigureHub() 
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/note")
                .Build();
            // Thread.Sleep(10000);
            
        }
        /// <summary>
        /// Esta función realiza la configuración del cliente MQTT asingándolo a la misma plataforma
        /// 
        /// </summary>
        /// <param name="">
        /// La función no tiene parámetros, por lo cual realiza la configuración sin retornar nada
        /// </param>
        private void ConfigureMqttClient()
        {
            mqttClient.ConnectedHandler = this;
            mqttClient.DisconnectedHandler = this;
            mqttClient.ApplicationMessageReceivedHandler = this;
        }
        /// <summary>
        /// Esta función realiza el tratamiento del mensaje recibido por la aplicación para luego enviarla al cliente y así poder ser visualizado
        /// </summary>
        /// <param name="eventArgs">
        /// El parámetro es el mensaje recibido por la plataforma, realizar el tratamiento de dicho mensaje y ser enviado al frontend
        /// </param>

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            
            System.Console.WriteLine("Mensaje recibido");

            // De acuerdo al mensaje recibido se ejecuta alguna accion 
            
            
            // Funciones para decodificar los mensajes de llegada por MQTT y realizar las acciones adecuadas
            HandleReceivedMessagePayload(eventArgs.ApplicationMessage.ConvertPayloadToString());
            
            // Reconstruccion del String Json para envio por el Hub de mensajeria interna (Cliente-Servidor)
            // msg.Property("imageFile").Remove();
            string JsonMsg = eventArgs.ApplicationMessage.ConvertPayloadToString();
            var msg = JsonConvert.DeserializeObject<dynamic>(JsonMsg);
            if (msg.has("imageFile"))
            {
                ((JArray)msg.Property("imageFile")).Remove();
            }            
            string JsonSend = msg.ToString();

            
            System.Console.WriteLine(JsonSend);
            //JsonMsg = JsonConvert.DeserializeObject(msg);

            // Se imprime el objeto por consola
            // System.Console.WriteLine("El mensaje recibido es: ");
            // System.Console.WriteLine(JsonMsg);

            // Envio por SignalR paraa comunicacion con el Cliente
            try {
                System.Console.WriteLine("Conectando al Hub");
                await connection.StartAsync();
                System.Console.WriteLine("Enviando al Hub");
                await connection.InvokeAsync("SendMessage", eventArgs.ApplicationMessage.Topic, JsonSend);
                System.Console.WriteLine("DesConectando del Hub");
                await connection.StopAsync();
                System.Console.WriteLine("Mensaje enviado por el Hub");
            } catch (System.Exception e)
            {
                System.Console.WriteLine("Error sending Hub Message" + e.Message + e.StackTrace );
                // throw new System.NotImplementedException();
            }
            
        
        }
        /// <summary>
        /// Esta función realiza la extracción de los parámetros recibidos enviados por el dispositivo
        /// 
        /// </summary>
        /// <param name="JsonMsg">
        /// El parámetro es el JSON enviado por el dispositivo, capturado por MQTTClient y notificado al ForeGround por el servicio SignalR.
        /// </param>
        public  void HandleReceivedMessagePayload(string JsonMsg) {
            DateTime localDate = DateTime.Now;
            try {
                // Se convierte la cadena del JsonMsg en un objeto dinamico para identificar el tipo de respuesta
                // De acuerdo al contenido se realizan acciones
                var mensaje = JsonConvert.DeserializeObject<dynamic>(JsonMsg);

                // // Determinar las variables comunes a todos los mensajes
                // var code = mensaje.code;
                // var device_id = mensaje.device_id;
                // var cur_pts = mensaje.dev_cur_pts;
                // var tag = mensaje.tag;


                // De acuerdo al valor de datas, asignar los datos a un objeto

                // // Debug de las variables principales
                // System.Console.WriteLine(code, device_id, cur_pts, tag);

                

                System.Console.WriteLine("Identificando respuesta:");

                if (mensaje.code == -1)
                {
                    System.Console.WriteLine("Error de comando, el dispositivo entregó código de error ");
                    System.Console.WriteLine(mensaje.msg);
                }
                if (mensaje.code == 0)
                {
                    // Respuesta al evento Get All Params
                    if(mensaje.msg == "get param success")
                    {
                        System.Console.WriteLine("Los parámetros del device son los siguientes: ");

                        System.Console.WriteLine("Parámetros básicos: ");
                        System.Console.WriteLine("Nombre dispositivo: ");
                        System.Console.WriteLine(mensaje.datas.basic_parameters.dev_name);
                        System.Console.WriteLine("Contraseña dispositivo: ");
                        System.Console.WriteLine(mensaje.datas.basic_parameters.dev_pwd);
                        System.Console.WriteLine("Parámetros de red: ");
                        System.Console.WriteLine("Dirección IP: ");
                        System.Console.WriteLine(mensaje.datas.network_config.ip_addr);
                        System.Console.WriteLine("Máscara de Subred: ");
                        System.Console.WriteLine(mensaje.datas.network_config.net_mask);
                        System.Console.WriteLine("Puerta de enlace: ");
                        System.Console.WriteLine(mensaje.datas.network_config.gateway);
                        System.Console.WriteLine("DDNS1: ");
                        System.Console.WriteLine(mensaje.datas.network_config.DDNS1);
                        System.Console.WriteLine("DDNS2: ");
                        System.Console.WriteLine(mensaje.datas.network_config.DDNS2);
                        System.Console.WriteLine("Bandera DHCP: ");
                        System.Console.WriteLine(mensaje.datas.network_config.DHCP);

                        System.Console.WriteLine("Parámetros de reconocimiento facial: ");
                        System.Console.WriteLine("Número de rostros guardados: ");
                        System.Console.WriteLine(mensaje.datas.face_recognition_cfg.dec_face_num_cur);
                        System.Console.WriteLine("Intervalo de misma persona: ");
                        System.Console.WriteLine(mensaje.datas.face_recognition_cfg.dec_interval_cur);
                        System.Console.WriteLine("Mínimo número de rostros a capturar: ");
                        System.Console.WriteLine(mensaje.datas.face_recognition_cfg.dec_face_num_min);
                        System.Console.WriteLine("Máximo número de rostros a capturar: ");
                        System.Console.WriteLine(mensaje.datas.face_recognition_cfg.dec_face_num_max);
                        System.Console.WriteLine("Intervalo mínimo de misma persona: ");
                        System.Console.WriteLine(mensaje.datas.face_recognition_cfg.dec_interval_min);
                        System.Console.WriteLine("Intervalo máximo de misma persona: ");
                        System.Console.WriteLine(mensaje.datas.face_recognition_cfg.dec_interval_max);

                        System.Console.WriteLine("Información de versión: ");
                        System.Console.WriteLine("Versión del modélo: ");
                        System.Console.WriteLine(mensaje.datas.version_info.dev_model);
                        System.Console.WriteLine("Versión del firmware: ");
                        System.Console.WriteLine(mensaje.datas.version_info.firmware_ver);
                        System.Console.WriteLine("Fecha del firmware: ");
                        System.Console.WriteLine(mensaje.datas.version_info.firmware_date);

                        System.Console.WriteLine("Parámetros de temperatura: ");
                        System.Console.WriteLine("Detección de temperatura: ");
                        System.Console.WriteLine(mensaje.datas.fun_param.temp_dec_en);
                        System.Console.WriteLine("Detección de visitantes: ");
                        System.Console.WriteLine(mensaje.datas.fun_param.stranger_pass_en);
                        System.Console.WriteLine("Detección de tapabocas: ");
                        System.Console.WriteLine(mensaje.datas.fun_param.make_check_en);
                        System.Console.WriteLine("Temperatura de alarma: ");
                        System.Console.WriteLine(mensaje.datas.fun_param.alarm_temp);
                        System.Console.WriteLine("Compensación de temperatura: ");
                        System.Console.WriteLine(mensaje.datas.fun_param.temp_comp);
                        System.Console.WriteLine("Tiempo de grabación: ");
                        System.Console.WriteLine(mensaje.datas.fun_param.record_save_time);
                        System.Console.WriteLine("Grabación: ");
                        System.Console.WriteLine(mensaje.datas.fun_param.save_record);
                        System.Console.WriteLine("Guardado de imágenes: ");
                        System.Console.WriteLine(mensaje.datas.fun_param.save_jpeg);

                        System.Console.WriteLine("Parámetros MQTT: ");
                        System.Console.WriteLine("Enable: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.enable);
                        System.Console.WriteLine("Retain: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.retain);
                        System.Console.WriteLine("Publish QoS: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.pqos);
                        System.Console.WriteLine("Subscribe QoS: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.sqos);
                        System.Console.WriteLine("Puerto: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.port);
                        System.Console.WriteLine("Servidor: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.server);
                        System.Console.WriteLine("Usuario: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.username);
                        System.Console.WriteLine("Contraseña: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.passwd);                       
                        System.Console.WriteLine("Tópico para publicar: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.topic2publish);
                        System.Console.WriteLine("Tópico para suscripción: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.topic2subscribe);
                        System.Console.WriteLine("HeartBeat: ");
                        System.Console.WriteLine(mensaje.datas.mqtt_protocol_set.heartbeat);

                        // Creación del objeto device para ser ingresado a la base de datos
                        // Se hace la recolección de parámetros de la trama recibida. 
                        var device = new Device()
                        {
                            Id = 0,
                            DevId = mensaje.device_id,
                            DevTag = "2",
                            DevTkn = "3",
                            DevPwd = mensaje.datas.basic_parameters.dev_pwd,
                            DevName = mensaje.datas.basic_parameters.dev_name,
                            IpAddr = mensaje.datas.network_config.ip_addr,
                            NetMsk = mensaje.datas.network_config.net_mask,
                            NetGw = mensaje.datas.network_config.gateway,
                            DDNS1 = mensaje.datas.network_config.DDNS1,
                            DDNS2 = mensaje.datas.network_config.DDNS2,
                            DHCP = mensaje.datas.network_config.DHCP,
                            DecFaceNumCur = mensaje.datas.face_recognition_cfg.dec_face_num_cur,
                            DecIntCur = mensaje.datas.face_recognition_cfg.dec_interval_cur,
                            DecFaceNumMin = mensaje.datas.face_recognition_cfg.dec_face_num_min,
                            DecFaceNumMax = mensaje.datas.face_recognition_cfg.dec_face_num_max,
                            DecIntMin = mensaje.datas.face_recognition_cfg.dec_interval_min,
                            DecIntMax = mensaje.datas.face_recognition_cfg.dec_interval_max,
                            DevMdl = mensaje.datas.version_info.dev_model,
                            FwrVer = mensaje.datas.version_info.firmware_ver,
                            FwrDate = mensaje.datas.version_info.firmware_ver,
                            TempDecEn = mensaje.datas.fun_param.temp_dec_en,
                            StrPassEn = mensaje.datas.fun_param.stranger_pass_en,
                            MkeChkEn = mensaje.datas.fun_param.make_check_en,
                            AlarmTemp = mensaje.datas.fun_param.alarm_temp,
                            TempComp = mensaje.datas.fun_param.temp_comp,
                            RcrdTimeSv = mensaje.datas.fun_param.record_save_time,
                            SvRec = mensaje.datas.fun_param.save_record,
                            SvJpg = mensaje.datas.fun_param.save_jpeg,
                            MqttEn = mensaje.datas.mqtt_protocol_set.enable,
                            MqttRet = mensaje.datas.mqtt_protocol_set.retain,
                            PQos = mensaje.datas.mqtt_protocol_set.pqos,
                            SQos = mensaje.datas.mqtt_protocol_set.sqos,
                            MqttPrt = mensaje.datas.mqtt_protocol_set.port,
                            MqttSrv = mensaje.datas.mqtt_protocol_set.server,
                            MqttUsr = mensaje.datas.mqtt_protocol_set.username,
                            MqttPwd = mensaje.datas.mqtt_protocol_set.passwd,
                            Topic2Pub = mensaje.datas.mqtt_protocol_set.topic2publish,
                            Topic2Sub = mensaje.datas.mqtt_protocol_set.topic2subscribe,
                            HeartBt = mensaje.datas.mqtt_protocol_set.heartbeat
                        };

                        StoreDevice(device);
                    }

                    // Respuesta a la orden Bind

                    if (mensaje.msg == "mqtt bind ctrl success")
                    {
                        System.Console.WriteLine("Dispositivo enlazado correctamente.");
                    }
                    if (mensaje.msg == "mqtt unbind ctrl success")
                    {
                        System.Console.WriteLine("Dispositivo desenlazado correctamente.");
                    }
                    if (mensaje.msg == "basic param set success")
                    {
                        System.Console.WriteLine("Parámetros básicos cambiados exitosamente.");
                    }
                    if (mensaje.msg == "network param set success")
                    {
                        System.Console.WriteLine("Parámetros de red cambiados exitosamente.");
                    }
                    if (mensaje.msg == "The interface has changed and is no longer supported")
                    {
                        System.Console.WriteLine("Los parámetros de reconocimiento no pueden ser cambiados.");
                    }
                    if (mensaje.msg == "remote config set success")
                    {
                        System.Console.WriteLine("Parámetros remotos configurados exitosamente.");
                    }
                    if (mensaje.msg == "funtable param set success")
                    {
                        System.Console.WriteLine("Parámetros de temperatura cambiados exitosamente.");
                    }
                    if (mensaje.msg == "delete all piclib success!")
                    {
                        System.Console.WriteLine("Datos guardados del dispositivo borrados correctamente");
                    }
                    if (mensaje.msg == "delete lib piclib success")
                    {
                        System.Console.WriteLine("Librerías de datos borrados correctamente");
                    }
                    if (mensaje.msg == "delete users piclib success")
                    {
                        System.Console.WriteLine("Usuarios borrados correctamente");
                    }

                    if (mensaje.msg == "Upload Person Info!") {
                        System.Console.WriteLine("Registro de persona en el Dispositivo");
                        if(mensaje.datas.user_id == "") {
                            mensaje.datas.user_id = 0;
                        };

                        // Convierte la imagen a jpeg y la graba en el directorio wwwroot
                        var image_url = ExportToImage(mensaje.datas.imageFile.ToString());
                        // System.Console.WriteLine(mensaje.datas.imageFile);
                        // Se envia a la base de datos el nombre de la imagen
                        mensaje.datas.imageFile = image_url;


                        // Se carga en el modelo Person los datos a almacenar en la DB
                        var persona = new Person()
                        {                            
                            MsgType = (Int32)mensaje.datas.msgType,
                            Similar = (float)mensaje.datas.similar,
                            UserId = (Int32)mensaje.datas.user_id,
                            Name = mensaje.datas.name,
                            RegisterTime = localDate,
                            Temperature = (float)mensaje.datas.temperature,
                            Matched = (Int32)mensaje.datas.matched,
                            Mask = (Int32)mensaje.datas.mask,
                            DevId = mensaje.device_id.ToString(),
                            imageUrl = image_url
                        };
                        StorePerson(persona);

                    }

                }
                // Se imprime el objeto por consola
                System.Console.WriteLine("El mensaje recibido es: ");
                System.Console.WriteLine(mensaje);
                
            } catch (Exception e) {
                System.Console.WriteLine("Error leyendo mensaje de Mqtt : " + e.Message);
            }
            
        }

        /// <summary>
        /// Genera la insercion de datos en la BD SQL Server a partir de la clase Person
        /// </summary>
        /// <param name="persona">
        ///     Contiene los datos de la persona que fue detectada por la tableta
        ///     Ejemplo:
        ///     var persona = new Person()
        ///                {
        ///                    MsgType = "1",
        ///                    Similar = 98,
        ///                    UserId = 77,
        ///                    Name = "Santiago Urueña",
        ///                    RegisterTime = localDate,
        ///                    Temperature = 36,
        ///                    Matched = 1,
        ///                    imageUrl = "/upload/images/77.jpg"
        ///                };
        /// </param>
        public void StorePerson(Person persona) {
            
            using (var scope = _scopeFactory.CreateScope()) {

                // el servicio de base de datos a traves de ApplicationDbContext es del tipo singleton 
                // scoped por lo tanto se requiere crearlo antes de hacer el envio a la base de datos
                // de lo contrario da un error 
                // Para esto es necesario usar la clase Microsoft.Extensions.DependencyInjection
                // e instanciar un scope
                // revisar en el constructor la instanciacion de _scopeFactory
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try {
                    dbContext.Persons.Add(persona);
                    dbContext.SaveChanges();
                } catch (Exception e) {
                    System.Console.WriteLine("Error Guardando Persona en la base de datos:" + e.Message + e.StackTrace);
                }
            }
            
        }


        /// <summary>
        /// el servicio de base de datos a traves de ApplicationDbContext es del tipo singleton 
        /// scoped por lo tanto se requiere crearlo antes de hacer el envio a la base de datos
        /// de lo contrario da un error 
        /// Para esto es necesario usar la clase Microsoft.Extensions.DependencyInjection
        /// e instanciar un scope revisar en el constructor la instanciacion de _scopeFactory
        /// </summary>
        /// <param name="dev"></param>
        public void StoreDevice(Device dev)
        {

            using (var scope = _scopeFactory.CreateScope())
            {

                
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    dbContext.Devices.Add(dev);
                    dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                }
            }

        }

        /// <summary>
        /// Esta función realiza la subscripción del tópico MQTT donde se obtienen dichos parámetros
        /// 
        /// </summary>
        /// <param name="eventArgs">
        /// La función entra con el mensaje pero no retorna nada, sólo realiza la subscripción
        /// </param>
        
        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("connected");
            // await mqttClient.SubscribeAsync("PublishTest");
            await mqttClient.SubscribeAsync("PublishTest");
        }

        /// <summary>
        /// Esta función se ejecuta cuando se haya perdido la conexión con el broker MQTT generando la excepción
        /// 
        /// </summary>
        /// <param name="eventArgs">
        /// La función entra con el mensaje pero no retorna nada, sólo realiza la subscripción
        /// </param>

        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("Broker Desconectado");

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Esta función realiza la reconexión asíncrona con el broker 
        /// 
        /// </summary>
        /// <param name="cancellationToken">
        /// La función entra con el token y sólo realiza la reconexión
        /// </param>

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await mqttClient.ConnectAsync(options);
            // _logger.LogWarning("Broker conectado");
            if (!mqttClient.IsConnected)
            {
                await mqttClient.ReconnectAsync();
            }
        }

        /// <summary>
        /// Esta función realiza la desconexión asíncrona con el broker 
        /// 
        /// </summary>
        /// <param name="cancellationToken">
        /// La función entra con el token y sólo realiza la desconexión
        /// </param>


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
            {
                var disconnectOption = new MqttClientDisconnectOptions
                {
                    ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                    ReasonString = "NormalDisconnection"
                };
                await mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
            }
            await mqttClient.DisconnectAsync();
        }

        /// <summary>
        ///  Convert image comming in MQTT message in format base64 to JPEG and store in Registers directory
        /// </summary>
        /// <param name="base64">
        /// base64 : String que contiene los datos de la imagen
        /// </param>
        /// <returns>
        /// imagePath : Ruta en el sistema de archivos de la imagen jpeg, para ser almacenada en la base de datos
        /// </returns>
        protected string ExportToImage(string base64)
        {
            DateTime localDate = DateTime.Now;
            var image64 = base64.Substring(base64.LastIndexOf(',') + 1);
            //  Convierte la cadena base64 en un arreglo de bytes
            byte[] bytes = Convert.FromBase64String(image64);
            var imageName = "output" + localDate.ToString("yyyy_MM_dd_HH_mm_ss") + ".jpg";
            var folderPath = "Media/Registers/";
            var imagePath = folderPath + imageName;
            
            using(Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                try {
                    image.Save(imagePath, ImageFormat.Jpeg);  // Or Png
                } catch(System.Exception e){
                    System.Console.WriteLine("Error saving " + imagePath + " in filesystem" + e.Message + e.StackTrace );
                }
                
            }
            
            return (imagePath);
        }
    }
}
