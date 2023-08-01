using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalog.Models;

[Table("Category")]
public class CategoryModel : ModelBase
{
    public CategoryModel()
    {
        Products = new Collection<ProductModel>();
    }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }

    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public ICollection<ProductModel>? Products { get; set; }
}

