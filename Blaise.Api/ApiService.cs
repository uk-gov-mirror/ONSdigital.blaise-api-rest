using System;
using System.ServiceProcess;
using Blaise.Api.Providers;
using Microsoft.Owin.Hosting;

namespace Blaise.Api
{
    internal partial class ApiService : ServiceBase
    {
        public string BaseAddress = ConfigurationProvider.BaseUrl;
        private IDisposable _server;

        public ApiService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _server = WebApp.Start<Startup>(url: BaseAddress);
        }

        protected override void OnStop()
        {
            _server?.Dispose();
            base.OnStop();
        }
    }
}
