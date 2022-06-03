using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RandomJokes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RandomJokes.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace RandomJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Jokes : ControllerBase
    {
        private string _connectionString;

        public Jokes(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
      
        [HttpGet]
        [Route("getrandomjoke")]
        public JokeVM GetRandomJoke()
        {
            using var client = new HttpClient();
            var json = client.GetStringAsync("https://jokesapi.lit-projects.com/jokes/programming/random").Result;
            var joke= JsonConvert.DeserializeObject<List<Joke>>(json).FirstOrDefault();

            var repo = new JokesRepository(_connectionString);
           joke= repo.AddJoke(joke);
         
            var vm = new JokeVM
            {
                Id=joke.Id,
                Type=joke.Type,
                PunchLine=joke.PunchLine,
                SetUp=joke.SetUp,
                Dislikes = repo.GetJokeDislikes(joke.Id),
                Likes = repo.GetJokeLikes(joke.Id)
        };

            return vm;
        }
        [HttpGet]
        [Route("getlikes")]
        public LikesDisLikeVM GetLikes(int id)
        {
           
            var repo = new JokesRepository(_connectionString);
           

            var vm = new LikesDisLikeVM();
            vm.Dislike = repo.GetJokeDislikes(id);
            vm.Like = repo.GetJokeLikes(id);


            return vm;
        }
        [HttpGet]
        [Route("getjokes")]
        public List<JokeVM> GetJokes()
        {
            var repo = new JokesRepository(_connectionString);
            List<Joke> jokes = repo.GetJokes();
            return jokes.Select(joke=> new JokeVM
            {
               
                Type = joke.Type,
                PunchLine = joke.PunchLine,
                SetUp = joke.SetUp,
                Dislikes = repo.GetJokeDislikes(joke.Id),
                Likes = repo.GetJokeLikes(joke.Id),
                 Id = joke.Id

            })
              .ToList();
            
            
        }
        [Authorize]
        [HttpPost]
        [Route("addupdatelike")]
        public UserLikedJokes AddUpdateLike(UserLikedJokes userLikedJokes)
        {
            var repo = new JokesRepository(_connectionString);
            string email = User.FindFirst("user")?.Value;
            var userRepo = new UserRepository(_connectionString);
            userLikedJokes.UserId = userRepo.GetByEmail(email).Id;
           
            UserLikedJokes ulj = repo.AddUpdateLikes(userLikedJokes);
            return ulj;

        }



    }
}
