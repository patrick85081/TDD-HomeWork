using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace ProductLibrary
{
    /// <summary>
    /// 產品資料模型
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int Revenue { get; set; }
        public int SellPrice { get; set; }

        public long Long { get; set; }
        public double Double { get; set; }
        public System.DateTime CreateTime { get; set;}
    }
}
