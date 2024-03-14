using Domain.OrderAggregate;

namespace API.DTOs
{
    public class OrderDto
    {

        public string BasketId { get; set; }
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public AddressDto ShippingAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public string Status { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
