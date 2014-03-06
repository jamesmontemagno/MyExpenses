using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MyExpenses.Portable.Interfaces;

namespace MyExpenses.PlatformSpecific
{
  //This is required for WP (Profile 78) to ensure iOS Compat
  public class HttpClientHelper : IHttpClientHelper
  {
    public HttpMessageHandler HttpHandler
    {
      get { return null; }
    }

  }
}