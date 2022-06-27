using System.ComponentModel.DataAnnotations;

namespace InventoryModels;

public class Category : FullAuditModel
{
    [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
    [Required]
    public string Name { get; set; }

    public virtual CategoryDetail Detail { get; set; }


}
