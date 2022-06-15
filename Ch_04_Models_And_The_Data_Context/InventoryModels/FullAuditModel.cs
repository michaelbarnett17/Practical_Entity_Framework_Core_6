using InventoryModels.Interfaces;

namespace InventoryModels;

public abstract class FullAuditModel : IIdenityModel, IAuditedModel, IActivatableModel
{
    public int Id { get; set; }
    public string? CreatedByUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedUserId { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public bool IsActive { get; set; }
}