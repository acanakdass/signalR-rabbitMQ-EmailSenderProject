using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRRabbitMqApp.Hubs
{
    public class MessageHub:Hub
    {
        public async Task SendMessageAsync(string message,string conId)
        {
            Console.WriteLine($"conId: {conId}");
            await Clients.Client(conId).SendAsync("receiveMessage", message);
        }
    }
}