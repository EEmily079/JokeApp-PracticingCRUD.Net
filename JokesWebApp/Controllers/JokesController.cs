
using JokesWebApp.Data;
using JokesWebApp.Models;
using JokesWebApp.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JokesWebApp.Controllers
{
    public class JokesController : Controller
    {
        private readonly JokesDbContext jokesDbContext;

        public JokesController(JokesDbContext jokesDbContext)
        {
            this.jokesDbContext = jokesDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var jokes = await jokesDbContext.Jokes.ToListAsync();
            return View(jokes);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddJokeViewModel addJokeRequest)
        {
            var joke = new Joke()
            {

                JokeQuestion = addJokeRequest.JokeQuestion,
                JokeAnswer = addJokeRequest.JokeAnswer
            };

            await jokesDbContext.Jokes.AddAsync(joke);
            await jokesDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var joke = await jokesDbContext.Jokes.FirstOrDefaultAsync(x => x.Id == id);

            if (joke != null)
            {
                var veiwModel = new UpdateJokeViewModel()
                {
                    Id = joke.Id,
                    JokeQuestion = joke.JokeQuestion,
                    JokeAnswer = joke.JokeAnswer
                };
                return await Task.Run(() => View("View", veiwModel));
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> View (UpdateJokeViewModel model)
        {
            var joke = await jokesDbContext.Jokes.FindAsync(model.Id);
            if (joke != null)
            {
                joke.JokeQuestion = model.JokeQuestion;
                joke.JokeAnswer = model.JokeAnswer;

                await jokesDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Delete( UpdateJokeViewModel model)
        {
            var joke = await jokesDbContext.Jokes.FindAsync (model.Id);
            if(joke != null)
            {
                jokesDbContext.Jokes.Remove(joke);
                jokesDbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }








    }
}
