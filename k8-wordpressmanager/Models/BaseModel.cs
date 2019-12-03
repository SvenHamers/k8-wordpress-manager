using System;
namespace k8_wordpressmanager.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
        }
        public string Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
