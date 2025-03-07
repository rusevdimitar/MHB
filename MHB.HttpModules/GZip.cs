using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.UI;

namespace MHB.HttpModules
{
    internal class GZip : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PostResolveRequestCache += new EventHandler(PostResolveRequestCache);
        }

        private void PostResolveRequestCache(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            Stream prevUncompressedStream = app.Response.Filter;

            if (app.Request["HTTP_X_MICROSOFTAJAX"] != null)
                return;
            if (app.Context.CurrentHandler != null && app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler")
                return;

            if (!(app.Context.CurrentHandler is Page))
                return;

            if (acceptEncoding == null || acceptEncoding.Length == 0)
                return;

            acceptEncoding = acceptEncoding.ToLower();

            if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
            {
                // defalte
                app.Response.Filter = new DeflateStream(prevUncompressedStream,
                    CompressionMode.Compress);
                app.Response.AppendHeader("Content-Encoding", "deflate");
            }
            else if (acceptEncoding.Contains("gzip"))
            {
                // gzip
                app.Response.Filter = new GZipStream(prevUncompressedStream,
                    CompressionMode.Compress);
                app.Response.AppendHeader("Content-Encoding", "gzip");
            }
        }
    }
}