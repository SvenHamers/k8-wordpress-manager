using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8_wordpressmanager.Models;
using KubeClient;
using KubeClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace k8_wordpressmanager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeploymentController : ControllerBase
    {
        private readonly IKubeApiClient kubeApiClient;

        private string[] namespaces = { "default", "kube-node-lease", "kube-public", "kube-system" };

        public DeploymentController(IKubeApiClient kubeApiClient)
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


        //Generate a variable deployment based on 
        [HttpPost("project")]
        public async Task<IActionResult> Post([FromBody] DeploymentModel model, string project)
        {
            DeploymentV1 initialDeployment = await kubeApiClient.DeploymentsV1().Create(new DeploymentV1
            {
                Metadata = new ObjectMetaV1
                {
                    Name = model.Name,
                    Namespace = project
                },
                Spec = new DeploymentSpecV1
                {
                    Selector = new LabelSelectorV1
                    {
                        MatchLabels =
                        {
                            ["app"] = model.Name
                        }
                    },
                    Replicas = 2,
                    RevisionHistoryLimit = 3,
                    Template = new PodTemplateSpecV1
                    {

                        Spec = new PodSpecV1
                        {
                            Containers =
                            {
                                new ContainerV1
                                {
                                    Name = "wordpress",
                                    Image = $"wordpress:{model.WordpressVersion}-apache",
                              
                                }
                            }
                        }
                    }
                }
            });

            return Ok();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            await kubeApiClient.NamespacesV1().Delete(name);
            return Accepted();
        }


    }
}
