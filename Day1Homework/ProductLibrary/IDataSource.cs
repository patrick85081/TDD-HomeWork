using System.Collections.Generic;

namespace ProductLibrary
{
    public interface IDataSource
    {
        IEnumerable<Product> GetProducts();
    }
}