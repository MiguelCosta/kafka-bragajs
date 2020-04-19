using System.Collections.Generic;
using Contracts;

namespace ApiWithKafka
{
    public static class DataBase
    {
        public static List<User> Users { get; set; } = new List<User>();
    }
}
