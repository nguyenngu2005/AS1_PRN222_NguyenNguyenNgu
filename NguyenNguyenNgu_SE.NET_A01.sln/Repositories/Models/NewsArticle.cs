using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models;

public partial class NewsArticle
{
    [Required(ErrorMessage = "News article ID is required.")]
    [StringLength(20, ErrorMessage = "News article ID cannot exceed 20 characters.")]
    public string NewsArticleId { get; set; } = null!;

    [Required(ErrorMessage = "News title is required.")]
    [StringLength(400, ErrorMessage = "News title cannot exceed 400 characters.")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Headline is required.")]
    [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters.")]
    public string Headline { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    [StringLength(4000, ErrorMessage = "News content cannot exceed 4000 characters.")]
    public string? NewsContent { get; set; }

    [StringLength(400, ErrorMessage = "News source cannot exceed 400 characters.")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [Range(1, short.MaxValue, ErrorMessage = "Category is required.")]
    public short? CategoryId { get; set; }

    public bool? NewsStatus { get; set; }

    public short? CreatedById { get; set; }

    public short? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual SystemAccount? CreatedBy { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
