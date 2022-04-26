using System;
using System.Collections.Generic;
using System.Text;

namespace JiraIntegrationDemo.Model
{

    public class JiraResponse<T>
    {
        public int maxResults { get; set; }
        public int startAt { get; set; }
        public int total { get; set; }
        public bool isLast { get; set; }
        public List<T> values { get; set; }
    }

    



}
