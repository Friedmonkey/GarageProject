using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models;

public class ServiceAction
{
    public int ID { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public string Description { get; set; }
}
