using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportStore.Domain.Abstract;
using Moq;
using SportStore.Domain.Entities;
using System.Collections.Generic;
using SportStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using SportStore.WebUI.Models;
using SportStore.WebUI.HtmlHelpers;

namespace SportStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            //arrange
            Mock<IGameRepository> mock =
                new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId=1, Name="Game1" },
                new Game {GameId=2,  Name="Game2" },
                new Game {GameId=3,  Name="Game3" },
                new Game {GameId=4,  Name="Game4" },
                new Game {GameId=5,  Name="Game5" }
            });

            GameController gamectrl = new GameController(mock.Object);
            gamectrl.pageSize = 2;

            //act
            SportListViewModel result =
                (SportListViewModel)gamectrl.List(2).Model;

            //assert
            List<Game> games = result.Games.ToList();

            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Game3");
            Assert.AreEqual(games[1].Name, "Game4");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            //arrange
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // setup lambda
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            // act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
            // assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //arrange
            Mock<IGameRepository> mock =
                new Mock<IGameRepository>();
                         mock.Setup(m => m.Games).Returns(new List<Game>
                         {
                             new Game { GameId=1, Name="Game1" },
                             new Game {GameId=2,  Name="Game2" },
                             new Game {GameId=3,  Name="Game3" },
                             new Game {GameId=4,  Name="Game4" },
                             new Game {GameId=5,  Name="Game5" }
                         });

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            //Act
            SportListViewModel result = (SportListViewModel)controller.List(2).Model;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
    }
}
