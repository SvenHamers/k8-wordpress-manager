using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KubeClient;
using KubeClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace k8_wordpressmanager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnvironmentsController : ControllerBase
    {
        private readonly IKubeApiClient kubeApiClient;

        private string[] namespaces = { "default", "kube-node-lease", "kube-public", "kube-system" };

        public EnvironmentsController(IKubeApiClient kubeApiClient)
        {
            this.kubeApiClient = kubeApiClient;
        }

        [HttpGet("{project}")]
        public async Task<IActionResult> Get(string project)
        {
            var result = await kubeApiClient.ConfigMapsV1().Get("wordpress-manager-settings", kubeNamespace: project);
            var data = result.Data["environments"];
            return Ok(data);
        }

        [HttpPut("{project}/{name}")]
        public async Task<IActionResult> Put(string project,string name)
        {
            var result = await kubeApiClient.ConfigMapsV1().Get("wordpress-manager-settings", kubeNamespace: project);

            var data = JsonConvert.DeserializeObject<List<string>>(result.Data["environments"]);

            if (data == null)
                data = new List<string>();

            data.Add(name);

            result.Data["environments"] = JsonConvert.SerializeObject(data);

            await kubeApiClient.ConfigMapsV1().Update("wordpress-manager-settings",patch =>
            {
                patch.Replace(patchConfigMap => patchConfigMap.Data,
                    value: result.Data
                ); 
            },kubeNamespace: project);
     

            return Accepted(name);
        }

        [HttpDelete("{project}/{name}")]
        public async Task<IActionResult> Delete(string project, string name)
        {
            var result = await kubeApiClient.ConfigMapsV1().Get("wordpress-manager-settings", kubeNamespace: project);

            var data = JsonConvert.DeserializeObject<List<string>>(result.Data["environments"]);

            if (data == null)
                data = new List<string>();

            data.Remove(name);

            result.Data["environments"] = JsonConvert.SerializeObject(data);

            await kubeApiClient.ConfigMapsV1().Update("wordpress-manager-settings", patch =>
            {
                patch.Replace(patchConfigMap => patchConfigMap.Data,
                    value: result.Data
                );
            }, kubeNamespace: project);


            return Accepted(name);
        }


    }
}
