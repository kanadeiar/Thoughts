
using System.ComponentModel.DataAnnotations;

using Thoughts.Domain.Base;

namespace Thoughts.Domain;

public class StatusModel : NamedEntityModel
{
    public StatusModel() { }

    public StatusModel(string name) => Name = name;
    public override string ToString() => Name;
}
