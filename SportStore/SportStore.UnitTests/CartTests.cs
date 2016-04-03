using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using SportStore.WebUI.Controllers;
using Moq;
using SportStore.Domain.Abstract;
using System.Web.Mvc;
using SportStore.WebUI.Models;

namespace SportStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // arrange
            Game game1 = new Game { GameId = 1, Name = "Sport1" };
            Game game2 = new Game { GameId = 2, Name = "Sport2" };

           
            Cart cart = new Cart();

            // Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Assert
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // arrange
            Game game1 = new Game { GameId = 1, Name = "Sport1" };
            Game game2 = new Game { GameId = 2, Name = "Sport2" };


            Cart cart = new Cart();
            //Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            // Assetr
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // arrange
            Game game1 = new Game { GameId = 1, Name = "Sport1" };
            Game game2 = new Game { GameId = 2, Name = "Sport2" };
            Game game3 = new Game { GameId = 3, Name = "Sport3" };


            Cart cart = new Cart();
            //Act

            cart.AddItem(game1, 1);
            cart.AddItem(game2, 4);
            cart.AddItem(game3, 2);
            cart.AddItem(game2, 1);

            //Act
            cart.RemoveLine(game2);

            //Assert
            Assert.AreEqual(cart.Lines.Where(c => c.Game == game2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);

        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // arrange
            Game game1 = new Game { GameId = 1, Name = "Sport1" };
            Game game2 = new Game { GameId = 2, Name = "Sport2" };
            Game game3 = new Game { GameId = 3, Name = "Sport3" };


            Cart cart = new Cart();
            //Act
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            cart.Clear();

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 0);

        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
             //arrange
            Mock < IGameRepository > mock =
                new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
                       {
                            new Game { GameId=1, Name="Game1", Category="Categ1" },
                            new Game {GameId=2,  Name="Game2", Category="Categ2"  },
                            new Game {GameId=3,  Name="Game3", Category="Categ1" },
                            new Game {GameId=4,  Name="Game4", Category="Categ4" },
                            new Game {GameId=5,  Name="Game5", Category="Categ2" }
                        });

            Cart cart = new Cart();

             CartController ctrl = new CartController(mock.Object, null);
            //Act

            ctrl.AddToCart(cart, 1, null);

            //assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Game.GameId, 1);
        }

        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
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
                            new Game {GameId=5,  Name="Game5", Category="Categ2" }
                        });

            Cart cart = new Cart();

            CartController ctrl = new CartController(mock.Object, null);
            //Act                    
            RedirectToRouteResult result = ctrl.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            //arrange
           
            Cart cart = new Cart();

            CartController ctrl = new CartController(null,null);
            //Act                
            CartIndexViewModel result
                  = (CartIndexViewModel)ctrl.Index(cart, "myUrl").ViewData.Model;

            //assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");

        }


        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);
           
            CartController controller = new CartController(null, mock.Object);
           
            controller.ModelState.AddModelError("error", "error");

            // Act
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());
            
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            
            Cart cart = new Cart();
            cart.AddItem(new Game(), 1);

            CartController controller = new CartController(null, mock.Object);

            // Act
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());            
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
