﻿namespace Application.Dto;
public class ProductNameDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public string Tags { get; set; }
    public int Quantity { get; set; }
    public ICollection<ProductDetailDto> Products { get; set; } = new List<ProductDetailDto>();
}
