using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.AspNet.SignalR.Client.Http
{
    public class DefaultHttpClient
#if NET4
        : DefaultHttpClient40
#else
        : DefaultHttpClient45
#endif
    {
    }
}
