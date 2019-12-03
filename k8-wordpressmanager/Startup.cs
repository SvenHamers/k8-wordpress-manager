using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KubeClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace k8_wordpressmanager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddKubeClient(new KubeClientOptions
            {
                ApiEndPoint = new Uri("https://192.168.64.42:8443"),
                AuthStrategy = KubeAuthStrategy.BearerToken,
                AccessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IiJ9.eyJpc3MiOiJrdWJlcm5ldGVzL3NlcnZpY2VhY2NvdW50Iiwia3ViZXJuZXRlcy5pby9zZXJ2aWNlYWNjb3VudC9uYW1lc3BhY2UiOiJkZWZhdWx0Iiwia3ViZXJuZXRlcy5pby9zZXJ2aWNlYWNjb3VudC9zZWNyZXQubmFtZSI6ImRlZmF1bHQtdG9rZW4tbDJiZ2oiLCJrdWJlcm5ldGVzLmlvL3NlcnZpY2VhY2NvdW50L3NlcnZpY2UtYWNjb3VudC5uYW1lIjoiZGVmYXVsdCIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VydmljZS1hY2NvdW50LnVpZCI6IjlhMmJkMTYzLTA4OWUtNGU5OS05MmMyLWQ1ZDgyMjc0NWQ1YSIsInN1YiI6InN5c3RlbTpzZXJ2aWNlYWNjb3VudDpkZWZhdWx0OmRlZmF1bHQifQ.HCIsQHmDtV9pijQb3GG826r-PwkACxZEeZNh1LF113MBoFxA28K3AKIBttAGnZHoyOijgrLDSDIQF2Y5HwaXgFQZqAN0NHdIFl8-8AqACrIs4LKFzm2NpQbNAYtRuANSbGbj-mK9oJ5sC_asNbwsmJ9EI7jS1en73_uMx6vq8O-QWmK7gAOAD-e3VnAaDvdRQzPobP7k1rpvMUpiR-OVZIebRwGSDmgEst2VJN0fdRYP2hPg5jtvjAemk2H9t7l81vQ3yCMdCfnaaHc74cvxlpiiHMBi6tck7bl86IYUy0YVp8z2rfuRR-kfHOMXfW9CdruIT5YBCXgAViRVWfMM0g",
                AllowInsecure = true // Don't validate server certificate
            });

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Damnesia API";
                    document.Info.Description = "";
                    document.Info.TermsOfService = "None";

                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
