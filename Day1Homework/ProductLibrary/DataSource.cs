using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductLibrary
{
    public class DataSource : IDataSource
    {
        public IEnumerable<Product> GetProducts()
        {
            return Enumerable.Empty<Product>();
        }
    }
}