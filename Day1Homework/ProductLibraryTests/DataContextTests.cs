using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ProductLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpectedObjects;
using System.Linq.Expressions;
using FluentAssertions;

namespace ProductLibrary.Tests
{
    [TestClass()]
    public class DataContextTests
    {
        IDataSource source = null;

        [TestInitialize()]
        public void TestInitialize_測試資料初始化()
        {
            List<Product> data = Enumerable.Range(1, 11)
                .Select(i =>
                    new Product
                    {
                        Id = i,
                        Cost = i,
                        Revenue = 10 + i,
                        SellPrice = 20 + i,
                    })
                .ToList();

            source = Substitute.For<IDataSource>();
            source.GetProducts().Returns(data);
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

            DataContext dataContext = new DataContext(source);
            var actual = dataContext.ClcSeqGroupSum(groupCount, sumProperty);
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


            DataContext dataContext = new DataContext(source);
            var actual = dataContext.ClcSeqGroupSum(groupCount, sumProperty);
            Console.WriteLine($"Actual => {string.Join(",", actual.Select(n => $"{n}"))}");

            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試加總數目小於等於0()
        {
            var groupCount = 0;
            Expression<Func<Product, int>> sumProperty = p => p.SellPrice;

            DataContext dataContext = new DataContext(source);
            Action action = () => dataContext.ClcSeqGroupSum(groupCount, sumProperty);

            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試加總屬性為null()
        {
            var groupCount = 3;
            Expression<Func<Product, int>> sumProperty = null;

            DataContext dataContext = new DataContext(source);
            Action action = () => dataContext.ClcSeqGroupSum(groupCount, sumProperty);

            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試加總屬性並非為Product屬性()
        {
            var groupCount = 3;
            Expression<Func<Product, int>> sumProperty = p => 2;

            DataContext dataContext = new DataContext(source);
            Action action = () => dataContext.ClcSeqGroupSum(groupCount, sumProperty);

            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ClcSeqGroupSumTest_測試不支援加總類型的屬性DateTime()
        {
            var groupCount = 3;
            Expression<Func<Product, DateTime>> sumProperty = p => p.CreateTime;

            DataContext dataContext = new DataContext(source);
            Action clc = () => dataContext.ClcSeqGroupSum(groupCount, sumProperty);

            clc.ShouldThrow<ArgumentException>();
        }
    }
}