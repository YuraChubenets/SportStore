using System;
using System.Diagnostics;

using System.Web;

namespace SportStore.WebUI.Modules
{
    public class TimerModule : IHttpModule
    {
        private Stopwatch timer;
        public void Init(HttpApplication app)
        {
            app.BeginRequest += HandleBeginRequest;
            app.EndRequest += HandleEndRequest;
        }
        private void HandleBeginRequest(object src, EventArgs args)
        {
            timer = Stopwatch.StartNew();
        }
        private void HandleEndRequest(object src, EventArgs args)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Write(string.Format(
                "<div style='color:red;'>Время обработки запроса: {0:F5} секунд</div>",
                ((float)timer.ElapsedTicks) / Stopwatch.Frequency));
        }
        public void Dispose()
        { }
    }
}