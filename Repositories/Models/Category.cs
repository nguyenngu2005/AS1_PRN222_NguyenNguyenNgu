using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models;

public partial class Category
{
    [Range(1, short.MaxValue, ErrorMessage = "Category ID must be greater than 0.")]
    public short CategoryId { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string CategoryName { get; set; } = null!;

    [Required(ErrorMessage = "Category description is required.")]
    [StringLength(250, ErrorMessage = "Category description cannot exceed 250 characters.")]
    public string CategoryDesciption { get; set; } = null!;

    [Range(1, short.MaxValue, ErrorMessage = "Parent category ID must be greater than 0.")]
    public short? ParentCategoryId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

    public virtual Category? ParentCategory { get; set; }
}
