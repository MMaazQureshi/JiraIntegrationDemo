using Atlassian.Jira;
using JiraIntegrationDemo.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JiraIntegrationDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Beep();
            //Console.WriteLine("Hello World!");
            var jira = Jira.CreateRestClient("Server url", "Email", "API key");

            var projects = await jira.Projects.GetProjectsAsync();
            var restClient = jira.RestClient;
            var boardsResponse = await restClient.ExecuteRequestAsync<JiraResponse<JiraBoard>>(Method.GET,
                   @"/rest/agile/1.0/board");
            var boards = boardsResponse.values;
            foreach (var item in boards)
            {
                var sprintResponse = await restClient.ExecuteRequestAsync<JiraResponse<JiraSprint>>(Method.GET,
                   $"/rest/agile/1.0/board/{item.id}/sprint");
                var currentSprint = sprintResponse.values.FirstOrDefault();

                var sprintIssues = await restClient.ExecuteRequestAsync<JiraSprintIssueResponse>(Method.GET,
                   $"/rest/agile/1.0/board/{item.id}/sprint/{currentSprint.id}/issue ");
                var issuesInMoreThanOneSprintCount = sprintIssues.issues.Where(x => x.fields.customfield_10020.Count() > 1);
                var totalIssues = sprintIssues.issues;

            }
            var issue = jira.CreateIssue("AH");
            issue.Type = "Task";
            //issue.Priority = "Major";
            issue.Summary = "Issue Summary";

            issue.SaveChanges();
            //var issues = await jira.Issues.GetIssueAsync("AH-18");
            var issues = await jira.Issues.GetIssuesAsync();
            foreach (var item in issues)
            {
                Console.WriteLine("---------------------------start issue-------------------------------------------------------");
                var issueItem = item.Value;
                //var jirassue = JsonConvert.SerializeObject(item, Formatting.Indented, new JsonSerializerSettings()
                //{ ContractResolver = new IgnorePropertiesResolver(new[] { nameof(Issue.Jira)}) });
                var comments = issueItem.GetCommentsAsync().Result.Select(c => new { c.AuthorUser.DisplayName, c.Body, c.CreatedDate });
                var lastUpdatedBy = issueItem.GetChangeLogsAsync().Result.FirstOrDefault()?.Author.DisplayName;
                var jirassue = JsonConvert.SerializeObject(new
                {
                    issueItem.Summary,
                    issueItem.Description,
                    issueItem.Status,
                    Type = issueItem.Type.Name,
                    priority = issueItem.Priority.Name,
                    Creator = issueItem.ReporterUser.DisplayName,
                    issueItem.Updated,
                    issueItem.Created,
                    issueItem.DueDate,
                    issueItem.Project,
                    comments,
                    StoryPoints = issueItem.CustomFields.FirstOrDefault(x => x.Name == "Story point estimate").Values,
                    lastUpdatedBy
                }, Formatting.Indented);
                Console.WriteLine(jirassue);
                Console.WriteLine("-------------------------------End issue---------------------------------------------------");
            }
        }

    }
    //short helper class to ignore some properties from serialization
    public class IgnorePropertiesResolver : DefaultContractResolver
    {
        private readonly HashSet<string> ignoreProps;
        public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
        {
            this.ignoreProps = new HashSet<string>(propNamesToIgnore);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (this.ignoreProps.Contains(property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
            }
            return property;
        }
    }
}
