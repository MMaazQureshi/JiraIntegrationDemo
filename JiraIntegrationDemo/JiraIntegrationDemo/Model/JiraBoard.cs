using System;
using System.Collections.Generic;
using System.Text;

namespace JiraIntegrationDemo.Model
{
    public class JiraBoard
    {
        public int id { get; set; }
        public string self { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public Location location { get; set; }
    }

    public class Location
    {
        public int projectId { get; set; }
        public string displayName { get; set; }
        public string projectName { get; set; }
        public string projectKey { get; set; }
        public string projectTypeKey { get; set; }
        public string avatarURI { get; set; }
        public string name { get; set; }
    }
}
