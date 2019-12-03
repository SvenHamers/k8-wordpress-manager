using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KubeClient;
using KubeClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace k8_wordpressmanager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IKubeApiClient kubeApiClient;

        private string[] namespaces = { "default", "kube-node-lease", "kube-public", "kube-system" };

        public ProjectsController(IKubeApiClient kubeApiClient)
        {
            this.kubeApiClient = kubeApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var projects = new List<string>();

            foreach (var result in await kubeApiClient.NamespacesV1().List())
            {
                if (!namespaces.Contains(result.Metadata.Name))
                    projects.Add(result.Metadata.Name);
            }

            return Ok(projects);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name)
        {
            var result = await kubeApiClient.NamespacesV1().List();

            if (!result.Any(x => x.Metadata.Name.ToLower() == name.ToLower()))
                await kubeApiClient.NamespacesV1().Create(new KubeClient.Models.NamespaceV1 { Metadata = new KubeClient.Models.ObjectMetaV1 { Name = name } });


            await kubeApiClient.ConfigMapsV1().Create(new ConfigMapV1
            {
                Metadata = new ObjectMetaV1
                {
                    Name = "wordpress-manager-settings",
                    Namespace = name
                },
                Data =
                    {
                        ["environments"] = "",
                    }
            });

            return Accepted(name);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            await kubeApiClient.NamespacesV1().Delete(name);
            return Accepted();
        }


    }
}
