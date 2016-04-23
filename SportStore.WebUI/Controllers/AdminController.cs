using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SportStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IGameRepository repository;

        public AdminController(IGameRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Games);
        }
        public ViewResult Edit(int gameId)
        {
            Game game = repository.Games
                          .First(g => g.GameId == gameId);
            return View(game);
        }

        [HttpPost]
        public ActionResult Edit(Game game, HttpPostedFileBase image = null)
        {                      
           if (ModelState.IsValid)
            {
                game.ImageMimeType = image.ContentType;
                game.ImagePath = Path.GetFileName(Guid.NewGuid().ToString() + "_" + 
                                       Path.GetFileName(image.FileName));
                image.SaveAs(Server.MapPath("~/Image/" + game.ImagePath));                
                repository.SaveGame(game);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);

                return RedirectToAction("Index");
            }
            else
            {
                return View(game);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Game());
        }


        [HttpPost]
        public ActionResult Delete(int gameId)
        {
            Game deletedGame = repository.DeleteGame(gameId);
            if (deletedGame != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена",
                    deletedGame.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
