using System.Collections.Generic;

namespace RandomJokes.Data
{
    public class Joke
    {
        public int Id { get; set; }
        public string SetUp { get; set; }
        public string PunchLine { get; set; }
        public string Type { get; set; }
        public List<UserLikedJokes> UserLikedJokes { get; set; }
    }

}
