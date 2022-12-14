namespace UticaBranch.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string client { get; set; }

        public List<OrderProduct> products { get; set; }

        public decimal price { get; set; }

        public DateTime placedAt { get; set; }

        public DateTime? deliveredAt { get; set; }

        public Order() { }

        public Order(string client)
        {

        }

        public Order(string client, List<OrderProduct> products)
        {
            this.client = client;
            this.products = products;
            this.price = calculatePrice(products);
            this.placedAt = DateTime.Now;
            this.deliveredAt = null;
        }


        private decimal calculatePrice(List<OrderProduct> products)
        {
            decimal price = 0;
            foreach (OrderProduct orderProduct in products)
            {
                price += orderProduct.product.unitPrice * orderProduct.amount;

            }
            return price;
        }




    }
}