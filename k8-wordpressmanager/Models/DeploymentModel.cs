using System;
namespace k8_wordpressmanager.Models
{
    public class DeploymentModel :BaseModel
    {
        public DeploymentModel()
        {
        }
        public string Name { get; set;       }
        public string WordpressVersion { get; set; }
        public string PublicUrl { get; set; }
        public string RepositoryUrl { get; set; }
        public string CustomScripts { get; set; }
    }
}
