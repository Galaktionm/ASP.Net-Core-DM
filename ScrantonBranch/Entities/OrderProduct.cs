using System.ComponentModel.DataAnnotations;

namespace ScrantonBranch.Entities
{
    public class OrderProduct
    {

        [Key]
        public long id { get; set; }

        public Product product;

        public int amount { get; set; }

        public OrderProduct() { }

        public OrderProduct(Product product, int amount)
        {
            this.product = product;
            this.amount = amount;

        }
    }
}

