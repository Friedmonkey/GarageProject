using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class Material : ICreationTableCompatible
{
    public int ID { get; set; }
    public string Name { get; set; }
    public float SingleCost { get; set; }
    public int Amount { get; set; }

    [NotMapped]
    public float TotalCost => SingleCost * Amount;

    [NotMapped]
    public float Cost { get => SingleCost; set => SingleCost = value; }

    public ICreationTableCompatible Copy()
    {
        return new Material() 
        {
            ID = ID,
            Name = Name,
            SingleCost = SingleCost,
            Amount = Amount,
        };
    }

    public void Fill(ICreationTableCompatible source)
    {
        if (source == null || source is not Material)
            throw new NotImplementedException();

        Material sourceMaterial = (source as Material);

        this.ID = sourceMaterial.ID;
        this.Name = sourceMaterial.Name;
        this.SingleCost = sourceMaterial.SingleCost;
        this.Amount = sourceMaterial.Amount;
    }

    public bool MatchesFilter(string filter)
    {
        if
        (
            Name.Contains(filter, StringComparison.OrdinalIgnoreCase) //||
            //SingleCost.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
            //Amount.Contains(filter, StringComparison.OrdinalIgnoreCase)
        )
        {
            return true;
        }
        return false;
    }
}
