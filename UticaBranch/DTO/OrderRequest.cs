using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UticaBranch.DTO
{
    public class OrderRequest
    {
        public string client { get; set; }

        public List<Dictionary<string, int>> orderedProducts { get; set; }

        public OrderRequest() { }

        public OrderRequest(string client, List<Dictionary<string, int>> orderedProducts)
        {
            this.client = client;
            this.orderedProducts = orderedProducts;
        }

    }
}
