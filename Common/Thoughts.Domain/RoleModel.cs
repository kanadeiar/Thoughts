using System.ComponentModel.DataAnnotations;


using Thoughts.Domain.Base;

namespace Thoughts.Domain;


public class RoleModel : NamedEntityModel
{
    public ICollection<UserModel> Users { get; set; } = new HashSet<UserModel>();
    public RoleModel() { }

    public RoleModel(string name) => Name = name;

    public override string ToString() => Name;

}
