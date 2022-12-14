using System.ComponentModel.DataAnnotations;

namespace ScrantonBranch.Entities
{
    public class Product
    {
        [Key]
        public string name { get; set; }

        public string? provider { get; set; }

        public decimal unitPrice { get; set; }

        public int available { get; set; }

        public Product() { }

        public Product(string name, decimal unitPrice, int amount)
        {
            this.name = name;
            this.unitPrice = unitPrice;
            this.available = amount;
        }


    }
}
