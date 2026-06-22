using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models;

public partial class SystemAccount
{
    [Range(1, short.MaxValue, ErrorMessage = "Account ID must be greater than 0.")]
    public short AccountId { get; set; }

    [Required(ErrorMessage = "Account name is required.")]
    [StringLength(100, ErrorMessage = "Account name cannot exceed 100 characters.")]
    public string? AccountName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid.")]
    [StringLength(70, ErrorMessage = "Email cannot exceed 70 characters.")]
    public string? AccountEmail { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    [Range(0, 2, ErrorMessage = "Role must be Admin, Staff, or Lecturer.")]
    public int? AccountRole { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(70, MinimumLength = 6, ErrorMessage = "Password must be 6-70 characters.")]
    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
