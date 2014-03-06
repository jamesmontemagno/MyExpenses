using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyExpenses.Portable.Interfaces
{
  //This is required for WP (Profile 78) to ensure iOS Compat
  public interface IHttpClientHelper
  {
    HttpMessageHandler HttpHandler { get; }
  }
}
