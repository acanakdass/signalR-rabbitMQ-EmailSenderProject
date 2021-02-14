using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SignalRRabbitMqApp.Models;

namespace EmailSenderClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.UserName = "guest";
            factory.Password = "guest";
            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare("messagequeue", false, false, false);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("messagequeue", true, consumer);

            
            HubConnection signalRConnection = new HubConnectionBuilder().WithUrl("https://localhost:5001/messageHub").Build();
            await signalRConnection.StartAsync();
            
            consumer.Received += async (sender, e) =>
            {
                //Send Email 
                //EmailSender.Send();
                //e.Body.Span : message from queue
                var serializedData = Encoding.UTF8.GetString(e.Body.Span);
                var userModel = JsonSerializer.Deserialize<User>(serializedData);
                var emailSendResult = EmailSender.Send(userModel?.Email,userModel?.Message);
                if (emailSendResult)
                {
                    await signalRConnection.InvokeAsync("SendMessageAsync", $"Mail sent to : {userModel?.Email}",userModel?.CallerConnectionId);
                    Console.WriteLine($"Mail sent to : {userModel?.Email}");
                }
                else
                {
                    await signalRConnection.InvokeAsync("receiveMessage", "An error occured while trying to send email!");
                    Console.WriteLine("An error occured while trying to send email!");
                }
                
            };
            Console.Read();
        }
    }
}