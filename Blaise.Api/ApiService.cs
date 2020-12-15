using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Blaise.Api.Providers;
using Microsoft.Owin.Hosting;

namespace Blaise.Api
{
    partial class ApiService : ServiceBase
    {
        public string BaseAddress = ConfigurationProvider.BaseUrl;
        private IDisposable _server = null;
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
