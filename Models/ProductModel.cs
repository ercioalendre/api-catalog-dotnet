using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalog.Models;

[Table("Product")]
public class ProductModel : ModelBase
{
    [Required]
    [StringLength(80)]
    public string? Name { get; set; }

    [Required]
    [StringLength(1024)]
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    [Required]
    public int? Stock { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [JsonIgnore]
    public CategoryModel? Category { get; set; }
}

