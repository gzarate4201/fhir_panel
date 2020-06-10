using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

// Librerias para manejo de MQTT
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

// Librerias para manejo de tareas asincronicas
using System.Threading;
using System.Threading.Tasks;

// Librerias para manejo de Notificaciones Cliente - Servidor
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using SignalRChat.Hubs;

// Modeles para Base de datos
using AspStudio.Models;
using AspStudio.Data;


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

    public class MqttClientService : IMqttClientService
    {
        // Instancias para manejo de la libreria de MQTTnet
        private IMqttClient mqttClient;
        private IMqttClientOptions options;

        // Instancias para manejo de la conexion a BD
        private readonly ApplicationDbContext dbContext;


        HubConnection connection;
        
        public MqttClientService(IMqttClientOptions options)
        {
            this.options = options;
            mqttClient = new MqttFactory().CreateMqttClient();
            ConfigureMqttClient();
            ConfigureHub();
        }

        private async void ConfigureHub() 
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/note")
                .Build();
            // Thread.Sleep(10000);
            
        }
        private void ConfigureMqttClient()
        {
            mqttClient.ConnectedHandler = this;
            mqttClient.DisconnectedHandler = this;
            mqttClient.ApplicationMessageReceivedHandler = this;
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            
            System.Console.WriteLine("Mensaje recibido");

            // De acuerdo al mensaje recibido se ejecuta alguna accion 
            HandleReceivedMessagePayload(eventArgs.ApplicationMessage.ConvertPayloadToString());

            // Envio por SignalR paraa comunicacion con el Cliente
            try {
                System.Console.WriteLine("Conectando al Hub");
                await connection.StartAsync();
                System.Console.WriteLine("Enviando al Hub");
                await connection.InvokeAsync("SendMessage", eventArgs.ApplicationMessage.Topic, eventArgs.ApplicationMessage.ConvertPayloadToString());
                System.Console.WriteLine("DesConectando del Hub");
                await connection.StopAsync();
                System.Console.WriteLine("Mensaje enviado por el Hub");
            } catch (System.Exception e)
            {
                System.Console.WriteLine("Error sending Hub Message" + e.Message + e.StackTrace );
                // throw new System.NotImplementedException();
            }
            
        
        }

        public  void HandleReceivedMessagePayload(string JsonMsg) {
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

                // Se imprime el objeto por consola
                System.Console.WriteLine(mensaje);
            } catch (Exception e) {
                System.Console.WriteLine("Error : " + e.Message);
            }
            
        }

        // Subscribirse al Topic que envia la tableta
        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("connected");
            // await mqttClient.SubscribeAsync("PublishTest");
            await mqttClient.SubscribeAsync("PublishTest");
        }

        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("Broker Desconectado");
            throw new System.NotImplementedException();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await mqttClient.ConnectAsync(options);
            if (!mqttClient.IsConnected)
            {
                await mqttClient.ReconnectAsync();
            }
        }

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
    }
}
