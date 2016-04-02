using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

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

    }
}
