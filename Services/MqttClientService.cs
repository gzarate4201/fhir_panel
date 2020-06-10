using System;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using System.Threading;

using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

using SignalRChat.Hubs;


namespace Mqtt.Client.AspNetCore.Services
{
    public class MqttClientService : IMqttClientService
    {
        private IMqttClient mqttClient;
        private IMqttClientOptions options;
        public IHubContext<ChatHub> _Hub;

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

            System.Console.WriteLine(eventArgs.ApplicationMessage.ConvertPayloadToString());
            // await Clients.All.SendAsync("ReceiveMessage", eventArgs.ApplicationMessage.Topic,eventArgs.ApplicationMessage.ConvertPayloadToString());
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
