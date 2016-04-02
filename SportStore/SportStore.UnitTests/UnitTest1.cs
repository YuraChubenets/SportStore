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
                            new Game {GameId=1,  Name="Game1", Category="Categ1" },
                            new Game {GameId=2,  Name="Game2", Category="Categ2"  },
                            new Game {GameId=3,  Name="Game3", Category="Categ1" },
                            new Game {GameId=4,  Name="Game4", Category="Categ2" },
                            new Game {GameId=5,  Name="Game5", Category="Categ3" }
               });

            GameController gamectrl = new GameController(mock.Object);
            gamectrl.pageSize = 3;

            //act
            var result =((SportListViewModel)gamectrl.List("Categ2",1)
                .Model).Games.ToList();
            //assert
            Assert.AreEqual(result.Count(), 2);
            Assert.AreEqual(result[0].Name, "Game2");
            Assert.AreEqual(result[1].Name, "Game4");
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
                            new Game { GameId=1, Name="Game1", Category="Categ1" },
                            new Game {GameId=2,  Name="Game2", Category="Categ2"  },
                            new Game {GameId=3,  Name="Game3", Category="Categ1" },
                            new Game {GameId=4,  Name="Game4", Category="Categ4" },
                            new Game {GameId=5,  Name="Game5", Category="Categ3" }
                        });

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            //Act
            SportListViewModel result = (SportListViewModel)controller.List("Categ2",2).Model;

            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 1);
            Assert.AreEqual(pageInfo.TotalPages, 1);
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //arrange
            Mock<IGameRepository> mock =
                new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
                       {
                            new Game { GameId=1, Name="Game1", Category="Categ1" },
                            new Game {GameId=2,  Name="Game2", Category="Categ2"  },
                            new Game {GameId=3,  Name="Game3", Category="Categ1" },
                            new Game {GameId=4,  Name="Game4", Category="Categ4" },
                            new Game {GameId=5,  Name="Game5", Category="Categ3" }
                        });

            NavController navCtrl = new NavController(mock.Object);

            //Act
            List<string> result = ((IEnumerable<string>)navCtrl.Menu().Model).ToList();

            //ASSERT
            Assert.AreEqual(result.Count(), 4);
            Assert.AreEqual(result[0], "Categ1");
            Assert.AreEqual(result[3], "Categ4");

        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //arrange
            Mock<IGameRepository> mock =
                new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
                       {
                            new Game { GameId=1, Name="Game1", Category="Categ1" },
                            new Game {GameId=2,  Name="Game2", Category="Categ2"  },
                            new Game {GameId=3,  Name="Game3", Category="Categ1" },
                            new Game {GameId=4,  Name="Game4", Category="Categ4" },
                            new Game {GameId=5,  Name="Game5", Category="Categ2" }
                        });

            NavController navCtrl = new NavController(mock.Object);
            string targetCateg = "Categ2";
            //Act
            var result = navCtrl.Menu(targetCateg).ViewData["SelectedCategory"];

            //ASSERT
            Assert.AreEqual(targetCateg, result);

        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            //arrange
            Mock<IGameRepository> mock =
                new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new Game[]
                       {
                            new Game { GameId=1, Name="Game1", Category="Categ1" },
                            new Game {GameId=2,  Name="Game2", Category="Categ2"  },
                            new Game {GameId=3,  Name="Game3", Category="Categ1" },
                            new Game {GameId=4,  Name="Game4", Category="Categ4" },
                            new Game {GameId=5,  Name="Game5", Category="Categ2" }
                        });

            GameController ctrl = new GameController(mock.Object);
            ctrl.pageSize = 3;

            //act
            int res1 = ((SportListViewModel)ctrl.List("Categ1").Model).PagingInfo.TotalItems;
            int res2 = ((SportListViewModel)ctrl.List("Categ2").Model).PagingInfo.TotalItems;
            int res3 = ((SportListViewModel)ctrl.List("Categ4").Model).PagingInfo.TotalItems;
            int resAll = ((SportListViewModel)ctrl.List(null).Model).PagingInfo.TotalItems;

            // Assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);

        }
    }
}
