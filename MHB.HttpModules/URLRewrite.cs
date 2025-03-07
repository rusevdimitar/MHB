using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MHB.BL;
using System.Web.Caching;

namespace MHB.HttpModules
{
    public class URLRewrite : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            RewriteAddress[] addresses = HttpRuntime.Cache.Get("UrlRewriteAddressesList") as RewriteAddress[];

            if (addresses != null)
            {
                HttpApplication context = sender as HttpApplication;

                string url = context.Request.Url.AbsolutePath;

                RewriteAddress address = addresses.FirstOrDefault(a => a.RequestedAddress == url);

                if (address != null)
                {
                    string responseURL = address.ActualLocation;

                    if (!string.IsNullOrEmpty(responseURL))
                        context.Context.RewritePath(responseURL);
                }
            }
        }
    }
}