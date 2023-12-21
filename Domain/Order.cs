using System.ComponentModel.DataAnnotations;

namespace Domain;
public class Order
{
    [Key]
    public string OrderId { get; set; }
    public bool Confirmed { get; set; }
    public bool Packed { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
