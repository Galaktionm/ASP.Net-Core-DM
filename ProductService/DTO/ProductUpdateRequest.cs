
namespace ProductService.DTO
{
    public class ProductUpdateRequest
    {
        public string? manufacturer { get; set; }

        public decimal unitPrice { get; set; }

        public int available { get; set; }

        public ProductUpdateRequest() { }

        public ProductUpdateRequest(decimal unitPrice, int available)
        {
            this.unitPrice = unitPrice;
            this.available = available;
        }

        public ProductUpdateRequest(string? manufacturer, decimal unitPrice, int available)
        {
            this.manufacturer = manufacturer;
            this.unitPrice = unitPrice;
            this.available = available;
        }
    }
}
