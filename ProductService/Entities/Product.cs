using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductService.Entities
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        public string name { get; set; }

        public string? manufacturer { get; set; }

        private decimal unitPriceValue;

        public int available { get; set; }

        public string? additionalInfo { get; set; }

        public Product()
        {
            PropertyChanged += sendMessage;
        }

        public Product(string name, decimal unitPrice, int available, string additionalInfo = "", string manufacturer = "")
        {
            PropertyChanged += sendMessage;
            this.name = name;
            this.unitPriceValue = unitPrice;
            this.available = available;
            this.additionalInfo = additionalInfo;
            this.manufacturer = manufacturer;
        }

        private void NotifyPropertyChanged(string propertyName, string name, string message,
            decimal oldPrice, decimal newPrice)
        {
            PropertyChangedEventArgs args = new
                CustomPropertyChangedEventArgs(propertyName, name, message,
                oldPrice, newPrice);
            if (oldPrice != 0)
            {
                PropertyChanged?.Invoke(this, args);
            }
        }

        private void sendMessage(object sender, PropertyChangedEventArgs args)
        {
            MessageSender.sendMessage(args);

        }

        public decimal unitPrice
        {
            get { return unitPriceValue; }

            set
            {
                if (value != unitPriceValue)
                {

                    NotifyPropertyChanged("unitPrice", "The price of "
                        + this.name + " changed from " + unitPriceValue + " to " + value, this.name, unitPriceValue, value);
                    unitPriceValue = value;
                }
            }
        }


    }

    public class CustomPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public string name { get; set; }
        public string message { get; set; }

        public decimal oldPrice { get; set; }

        public decimal newPrice { get; set; }
        public CustomPropertyChangedEventArgs(string? propertyName,
           string message, string name,
           decimal oldPrice, decimal newPrice) : base(propertyName)
        {
            this.message = message;
            this.name = name;
            this.oldPrice = oldPrice;
            this.newPrice = newPrice;
        }

    }


}
