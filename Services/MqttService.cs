// Clase que me permite inyectarla en otras clases
namespace Mqtt.Client.AspNetCore.Services
{
    public class MqttService
    {
        private readonly IMqttClientService mqttClientService;
        public MqttService(MqttClientServiceProvider provider)
        {
            mqttClientService = provider.MqttClientService;
        }
    }
}