﻿using App.Data.Entities;

namespace SimoshStore;

public class OrderDTO
{
    public int UserId { get; set; }
    public string OrderCode { get; set; } = null!;

}
