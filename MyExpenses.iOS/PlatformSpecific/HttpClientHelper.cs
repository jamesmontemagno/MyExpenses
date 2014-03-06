using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ModernHttpClient;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MyExpenses.Portable.Interfaces;

namespace MyExpenses.PlatformSpecific
{
  //This is required for WP (Profile 78) to ensure iOS Compat
  public class HttpClientHelper : IHttpClientHelper
  {
    private AFNetworkHandler handler;
    /// <summary>
    /// Use AFNetworkHandler to replace the NuGet as it is not compatible with iOS
    /// </summary>
    public HttpMessageHandler HttpHandler
    {
      get { return handler ?? (handler =new AFNetworkHandler()); }
    }
  }
  
}