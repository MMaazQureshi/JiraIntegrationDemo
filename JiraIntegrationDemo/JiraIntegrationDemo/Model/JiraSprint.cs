using System;
using System.Collections.Generic;
using System.Text;

namespace JiraIntegrationDemo.Model
{
    public class JiraSprint
    {
        public int id { get; set; }
        public string self { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int originBoardId { get; set; }
        public string goal { get; set; }
    }
}
