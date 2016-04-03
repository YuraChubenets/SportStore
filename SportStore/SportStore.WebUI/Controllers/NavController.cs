using SportStore.Domain.Abstract;
using SportStore.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IGameRepository reposit;
        
        public NavController(IGameRepository repo)
        {
            reposit = repo;
        }

        public PartialViewResult Menu(string category = null, bool horizontalNav = false)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = reposit.Games
                .Select(game => game.Category)
                .Distinct()
                .OrderBy(x => x);

            return PartialView("FlexMenu", categories);

        }
    }
}