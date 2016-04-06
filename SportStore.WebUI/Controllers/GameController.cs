using SportStore.Domain.Abstract;
using SportStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class GameController : Controller
    {
        private IGameRepository repository;
        public int pageSize = 4;
        public GameController(IGameRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page =1)
        {
            SportListViewModel model;
            model = new SportListViewModel
            {
                Games = repository.Games
                .Where(p=> category==null || p.Category==category )
                .OrderBy(item => item.GameId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                      repository.Games.Count() :
                      repository.Games.Where(game => game.Category == category).Count()
               
                   },
                       CurrentCategory =category                
                   };
            return View(model);
        }
    }
}