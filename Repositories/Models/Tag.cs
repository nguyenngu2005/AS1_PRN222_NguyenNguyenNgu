using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models;

public partial class Tag
{
    [Range(1, int.MaxValue, ErrorMessage = "Tag ID must be greater than 0.")]
    public int TagId { get; set; }

    [Required(ErrorMessage = "Tag name is required.")]
    [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters.")]
    public string? TagName { get; set; }

    [StringLength(400, ErrorMessage = "Note cannot exceed 400 characters.")]
    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
