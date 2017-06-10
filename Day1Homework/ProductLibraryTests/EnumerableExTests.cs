using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        List<Product> dataSource = null;

        [TestInitialize()]
        public void TestInitialize_測試資料初始化()
        {
            //dataSource = Enumerable.Range(1, 11)
            //    .Select(i =>
            //        new Product
            //        {
            //            Id = i,
            //            Cost = i,
            //            Revenue = 10 + i,
            //            SellPrice = 20 + i,
            //        })
            //    .ToList();
            dataSource = new List<Product>()
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
        public void ClcSeqGroupSumTest_測試每3筆資料_加總Cost()
        {
            var expected = new int[] 
            {
                1 + 2 + 3,
                4 + 5 + 6,
                7 + 8 + 9,
                10+ 11,
            };
            Console.WriteLine($"Expected => {string.Join(",", expected.Select(n => $"{n}"))}");

            var groupCount = 3;
            Expression<Func<Product, int>> sumProperty = p => p.Cost;

            var actual = dataSource.ClcSeqGroupSum(groupCount, sumProperty);
            Console.WriteLine($"Actual => {string.Join(",", actual.Select(n => $"{n}"))}");

            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod()]
        public void ClcSeqGroupSumTest_測試每4筆資料_加總Revenue()
        {
            var expected = new int[]
            {
                11 + 12 + 13 + 14,
                15 + 16 + 17 + 18,
                19 + 20 + 21
            };
            Console.WriteLine($"Expected => {string.Join(",", expected.Select(n => $"{n}"))}");

            var groupCount = 4;
            Expression<Func<Product, int>> sumProperty = p => p.Revenue;

            var actual = dataSource.ClcSeqGroupSum(groupCount, sumProperty);
            Console.WriteLine($"Actual => {string.Join(",", actual.Select(n => $"{n}"))}");

            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試加總數目等於0()
        {
            var expectec = new int[] { 0 };

            var groupCount = 0;
            Expression<Func<Product, int>> sumProperty = p => p.SellPrice;

            var actual = dataSource.ClcSeqGroupSum(groupCount, sumProperty);

            expectec.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試加總數目小於0()
        {
            var groupCount = -1;
            Expression<Func<Product, int>> sumProperty = p => p.SellPrice;

            Action action = () => dataSource.ClcSeqGroupSum(groupCount, sumProperty);

            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試加總屬性為null()
        {
            var groupCount = 3;
            Expression<Func<Product, int>> sumProperty = null;

            Action action = () => dataSource.ClcSeqGroupSum(groupCount, sumProperty);

            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試加總屬性並非為Product屬性()
        {
            var groupCount = 3;
            Expression<Func<Product, int>> sumProperty = p => 2;

            Action action = () => dataSource.ClcSeqGroupSum(groupCount, sumProperty);

            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試不支援加總類型的屬性DateTime()
        {
            var groupCount = 3;
            Expression<Func<Product, DateTime>> sumProperty = p => p.CreateTime;

            Action clc = () => dataSource.ClcSeqGroupSum(groupCount, sumProperty);

            clc.ShouldThrow<ArgumentException>();
        }
    }
}