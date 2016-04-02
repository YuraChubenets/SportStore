using SportStore.Domain.Abstract;
using SportStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public ViewResult List(int page =1)
        {
            SportListViewModel model;
            model = new SportListViewModel
            {
                Games = repository.Games
                .OrderBy(game => game.GameId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Games.Count()
                }
            };
            return View(model);
        }
    }
}