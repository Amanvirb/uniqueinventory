using System.ComponentModel.DataAnnotations;

namespace Domain;
public class Order
{
    [Key]
    public string OrderId { get; set; } //Id has to change to GUID instead of string and change propertyname from orderId to Id
    public bool Confirmed { get; set; }
    public bool Packed { get; set; }

    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
