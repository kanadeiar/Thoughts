using System.ComponentModel.DataAnnotations;

using Thoughts.Domain.Base;

namespace Thoughts.Domain;

/// <summary>
/// Категория (раздел)
/// </summary>

public class CategoryModel: NamedEntityModel
{
    /// <summary>Статус категории</summary>
    [Required]
    public StatusModel Status { get; set; } = null!;    
    
    /// <summary>Список постов входящих в категорию</summary>
    public ICollection<PostModel> Posts { get; set; }=new HashSet<PostModel>();

    public CategoryModel() { }

    public CategoryModel(string Name)=>this.Name = Name;

    public override string ToString() => Name;
}
