﻿using App.Data.Entities;

namespace SimoshStoreAPI;

public class BlogPostViewModel
{
    public IEnumerable<BlogEntity> blogs { get; set; } = null!;
    public BlogEntity blog { get; set; } = null!;
    public string quote { get; set; } = null!;
}
