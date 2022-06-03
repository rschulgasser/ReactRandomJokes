using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomJokes.Web.Models
{
    public class JokeVM
    {
        public int Id { get; set; }
        public string SetUp { get; set; }
        public string PunchLine { get; set; }
        public string Type { get; set; }
        public int Likes{ get; set; }
        public int Dislikes { get; set; }
    }
}
