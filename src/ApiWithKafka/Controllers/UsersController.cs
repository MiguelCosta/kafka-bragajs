using System.Collections.Generic;
using System.Linq;
using System.Text;
using Confluent.Kafka;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithKafka.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IProducer<int, UserMessage> _producer;

        public UsersController(IProducer<int, UserMessage> producer)
        {
            _producer = producer;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return DataBase.Users;
        }

        [HttpGet("{id:int}")]
        public User Get(int id)
        {
            return DataBase.Users.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public User Post(User user)
        {
            user.Id = DataBase.Users.Count + 1;

            //DataBase.Users.Add(user);

            _producer.Produce(
                "topic-users",
                new Message<int, UserMessage>
                {
                    Key = user.Id,
                    Value = new UserMessage
                    {
                        Id = user.Id,
                        Name = user.Name
                    },
                    Headers = new Headers
                    {
                        new Header("Type", Encoding.UTF8.GetBytes(typeof(User).FullName))
                    }
                });

            return user;
        }
    }
}
