namespace SignalRRabbitMqApp.Models
{
    public class User
    {
        public string Message { get; set; }
        public string Email { get; set; }
        
        public string CallerConnectionId { get; set; }
    }
}