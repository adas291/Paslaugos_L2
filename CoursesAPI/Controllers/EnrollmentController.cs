using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using RabbitMQ.Client;
using CoursesAPI.Models;
using Newtonsoft.Json;
using Plain.RabbitMQ;

namespace CoursesAPI.Controllers
{
  [ApiController, Route("api/[controller]")]
  public class EnrollmentController : ControllerBase
  {

    private readonly IPublisher _publisher;

    public EnrollmentController(IPublisher publisher)
    {
      _publisher = publisher;
    }

    [HttpPost("send")]
    public void Post([FromBody] Enrollment enrollment)
    //public IActionResult Post()
    {
      _publisher.Publish(JsonConvert.SerializeObject(enrollment), "enrollment_topic", null);
      //return Ok("message sent");
    }

    //private void SendMessage(Enrollment enrollment)
    //{
    //    var factory = new ConnectionFactory() { HostName = "localhost" };
    //    using (var connection = factory.CreateConnection())
    //    using (var channel = connection.CreateModel())
    //    {
    //        channel.QueueDeclare(queue: "hello",
    //                             durable: false,
    //                             exclusive: false,
    //                             autoDelete: false,
    //                             arguments: null);


    //        //var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(enrollment));
    //        var body = Encoding.UTF8.GetBytes("hello world");

    //        channel.BasicPublish(exchange: "",
    //                             routingKey: "hello",
    //                             basicProperties: null,
    //                             body: body);

    //        Console.WriteLine(" [x] Sent {0}", body);
    //    }
    //}
  }
}
