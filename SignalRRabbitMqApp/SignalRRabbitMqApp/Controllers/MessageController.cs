using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using SignalRRabbitMqApp.Models;

namespace SignalRRabbitMqApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] User userModel)
        {

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost"; 
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare("messagequeue", false, false, false);
            var userModelJson = JsonSerializer.Serialize(userModel);
            var byteData = Encoding.UTF8.GetBytes(userModelJson);
            channel.BasicPublish("","messagequeue",body:byteData);
            return Ok();
        }
    }
}