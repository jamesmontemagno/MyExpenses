using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExpenses.Portable.DataLayer.SQLite;
using Newtonsoft.Json;

namespace MyExpenses.Portable.Models
{
  public class Alert
  {
    [JsonProperty("details")]
    public string Details { get; set; }

    [JsonProperty("alertdate")]
    public DateTime AlertDate { get; set; }

    [Ignore]
    public string AlertDateDisplay
    {
      get { return AlertDate.ToString("D"); }
    }
  }
}
