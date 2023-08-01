namespace APICatalog.DTOs;

public class ProductOutputDTO : OutputDTOBase
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public int? Stock { get; set; }

    public Guid CategoryId { get; set; }
}
