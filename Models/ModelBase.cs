using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalog.Models;

public class ModelBase
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "timestamp(6)")]
    public DateTime CreatedAt { get; set; }

    [Required]
    public Guid CreatedBy { get; set; }

    [Column(TypeName = "timestamp(6)")]
    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public void Create(Guid creationUserId)
    {
        Id = Guid.NewGuid();

        CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        CreatedBy = creationUserId;
    }

    public void Update(Guid updateUserId)
    {
        UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        UpdatedBy = updateUserId;
    }
}

