﻿namespace Application.Dto;
public class LocationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TotalCapacity { get; set; }
    public int TotalProducts { get; set; }
    public int CountOfEnteredProductNameProducts { get; set; }
    public int EmptySpace { get; set; }
    public string Message { get; set; }

    public List<ProductDetailDto> Products { get; set; } = new List<ProductDetailDto>();
}
