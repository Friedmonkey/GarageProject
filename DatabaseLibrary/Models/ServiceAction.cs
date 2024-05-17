using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class ServiceAction : ICreationTableCompatible
{
    public int ID { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public string Description { get; set; }


    [NotMapped]
    public float Cost { get => Price; set => Price = value; }

    public ICreationTableCompatible Copy()
    {
        return new ServiceAction() 
        {
            ID = ID,
            Name = Name,
            Price = Price,
            Description = Description
        };
    }
    public void Fill(ICreationTableCompatible source)
    {
        if (source == null || source is not ServiceAction)
            throw new NotImplementedException();

        ServiceAction sourceServiceAction = (source as ServiceAction);

        this.ID = sourceServiceAction.ID;
        this.Name = sourceServiceAction.Name;
        this.Price = sourceServiceAction.Price;
        this.Description = sourceServiceAction.Description;
    }

    public bool MatchesFilter(string filter)
    {
        if
        (
            Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
            Description.Contains(filter, StringComparison.OrdinalIgnoreCase) //||
            //Amount.Contains(filter, StringComparison.OrdinalIgnoreCase)
        )
        {
            return true;
        }
        return false;
    }
}
