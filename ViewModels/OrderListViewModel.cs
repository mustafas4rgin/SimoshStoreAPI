﻿using App.Data.Entities;

namespace SimoshStoreAPI;

public class OrderListViewModel
{
    public List<OrderEntity> orders { get; set; } = null!;
    public int CurrentPage { get; set; } = 0;
    public int TotalPages { get; set; } = 0;
}
