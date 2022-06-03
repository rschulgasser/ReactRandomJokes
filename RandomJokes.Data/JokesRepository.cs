using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomJokes.Data
{
    public class JokesRepository
    {
        private readonly string _connectionString;

        public JokesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Joke AddJoke(Joke joke)
        {

            using var ctx = new JokesContext(_connectionString);
            var jokes = ctx.Jokes;
            joke.Id = 0;
            if (!jokes.Any(a => a.SetUp == joke.SetUp) && !jokes.Any(a => a.PunchLine == joke.PunchLine)) {

                ctx.Jokes.Add(joke);
                ctx.SaveChanges();
            }
            else
            {
                joke = ctx.Jokes.Where(a => a.SetUp == joke.SetUp && a.PunchLine == joke.PunchLine).FirstOrDefault();
            }
            joke = ctx.Jokes.Where(a => a.SetUp == joke.SetUp && a.PunchLine == joke.PunchLine).FirstOrDefault();
            return joke;
        }
        //public List<int> GetJokeLike(int jokeId)
        //{
        //    using var ctx = new JokesContext(_connectionString);
        //    var userLikedJokes = ctx.UserLikedJokes;

        //    List<int> likes = new List<int>();
        //    likes.Add(userLikedJokes.Count(c => c.JokeId==jokeId && c.Liked==false));
        //    likes.Add(userLikedJokes.Count(c => c.JokeId == jokeId && c.Liked == true));
        //    return likes;
        //}
        public int GetJokeLikes(int jokeId)
        {
            using var ctx = new JokesContext(_connectionString);

            return ctx.UserLikedJokes.Count(c => c.JokeId == jokeId && c.Liked == true);
        }
        public int GetJokeDislikes(int jokeId)
        {
            using var ctx = new JokesContext(_connectionString);

            return ctx.UserLikedJokes.Count(c => c.JokeId == jokeId && c.Liked == false);
        }
        public List<Joke> GetJokes()
        {
            using var ctx = new JokesContext(_connectionString);
            return ctx.Jokes.ToList();
        }
        public UserLikedJokes AddUpdateLikes(UserLikedJokes userLikedJokes)
        {
            using var ctx = new JokesContext(_connectionString);
            var list = ctx.UserLikedJokes;
            if (!list.Any(a => a.JokeId == userLikedJokes.JokeId && a.UserId == userLikedJokes.UserId)){
                userLikedJokes.DateTime = DateTime.Now;
                ctx.UserLikedJokes.Add(userLikedJokes);
                ctx.SaveChanges();
                return userLikedJokes;
            }
            else
            {
                var like = ctx.UserLikedJokes.FirstOrDefault(a => a.JokeId == userLikedJokes.JokeId && a.UserId == userLikedJokes.UserId);
                if (userLikedJokes.DateTime.AddMinutes(5) >= DateTime.Now)
                {
                    ctx.Database.ExecuteSqlInterpolated($"Update UserLikedJokes Set liked={userLikedJokes.Liked} WHERE UserId = {userLikedJokes.UserId} And JokeId={userLikedJokes.JokeId}");
                    return userLikedJokes;
                }
            }
            return null;
        }
    
    }
}
