﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpectedObjects;
using System.Linq.Expressions;
using FluentAssertions;
using ProductLibraryTests;

namespace ProductLibrary.Tests
{
    [TestClass()]
    public class EnumerableExTests
    {
        List<Product> products = null;

        [TestInitialize()]
        public void TestInitialize_測試資料初始化()
        {
            products = new List<Product>()
            {
                new Product() { Id =  1, Cost =  1, Revenue = 11, SellPrice = 21 },
                new Product() { Id =  2, Cost =  2, Revenue = 12, SellPrice = 22 },
                new Product() { Id =  3, Cost =  3, Revenue = 13, SellPrice = 23 },
                new Product() { Id =  4, Cost =  4, Revenue = 14, SellPrice = 24 },
                new Product() { Id =  5, Cost =  5, Revenue = 15, SellPrice = 25 },
                new Product() { Id =  6, Cost =  6, Revenue = 16, SellPrice = 26 },
                new Product() { Id =  7, Cost =  7, Revenue = 17, SellPrice = 27 },
                new Product() { Id =  8, Cost =  8, Revenue = 18, SellPrice = 28 },
                new Product() { Id =  9, Cost =  9, Revenue = 19, SellPrice = 29 },
                new Product() { Id = 10, Cost = 10, Revenue = 20, SellPrice = 30 },
                new Product() { Id = 11, Cost = 11, Revenue = 21, SellPrice = 31 }
            };
        }

        [TestMethod()]
        public void GetSeqGroupSumTest_測試每3筆資料_加總Cost()
        {
            //Arrange
            var expected = new int[] { 6, 15, 24, 21 };

            var groupCount = 3;
            Expression<Func<Product, int>> sumProperty = p => p.Cost;

            //act
            var actual = products.GetGroupSum(groupCount, sumProperty);

            //assert
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod()]
        public void GetSeqGroupSumTest_測試每4筆資料_加總Revenue()
        {
            //Arrange
            var expected = new int[] { 50, 66, 60 };

            var groupCount = 4;
            Expression<Func<Product, int>> sumProperty = p => p.Revenue;

            //act
            var actual = products.GetGroupSum(groupCount, sumProperty);

            //assert
            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void GetGroupSumTest_測試加總數目小於等於0_發生ArgumentException()
        {
            //arrange
            var groupCount = 0;
            Expression<Func<Product, int>> sumProperty = p => p.SellPrice;

            //act
            Action actual = () => products.GetGroupSum(groupCount, sumProperty);

            //assert
            actual.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void GetGroupSumTest_測試加總數目小於0_引發ArgumentException()
        {
            //Arrange
            var groupCount = -1;
            Expression<Func<Product, int>> sumProperty = p => p.SellPrice;

            //act
            Action action = () => products.GetGroupSum(groupCount, sumProperty);

            //assert
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void GetGroupSumTest_測試加總屬性為null_引發ArgumentNullException()
        {
            //arrange
            var groupCount = 3;
            Expression<Func<Product, int>> sumProperty = null;

            //act
            Action action = () => products.GetGroupSum(groupCount, sumProperty);

            //assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void GetGroupSumTest_測試加總屬性並非為Product屬性_引發ArgumentException()
        {
            //arrange
            var groupCount = 3;
            Expression<Func<Product, int>> sumProperty = p => 2;

            //act
            Action action = () => products.GetGroupSum(groupCount, sumProperty);

            //assert
            action.ShouldThrow<ArgumentException>();
        }
    }
}