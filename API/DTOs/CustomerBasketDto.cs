using Domain;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CustomerBasketDto
    {
        public CustomerBasketDto()
        {
            
        }
        public CustomerBasketDto(string id)
        {
            Id = id;
        }

        [Required]
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
       
    }
}
