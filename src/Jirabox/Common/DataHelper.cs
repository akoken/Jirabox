using Jirabox.Model;
using System.Collections.Generic;

namespace Jirabox.Common
{
    public class DataHelper
    {
        public static List<Issue> GetIssues()
        {
            var issues = new List<Issue>
            {
                new Issue
                {
                    ProxyKey = "AOP-101",
                    Fields = new Fields
                    {
                        IssueType = new IssueType
                        {
                             IconUrl ="https://dev.obss.com.tr/jira/images/icons/issuetypes/bug.png",
                             Id = 3,
                             Name= "Bug"
                        },
                        Summary = "Connection Problem"
                    }   
                },
                new Issue
                {
                     ProxyKey = "AOP-102",
                     Fields = new Fields
                    {
                        IssueType = new IssueType
                        {
                             IconUrl ="https://dev.obss.com.tr/jira/images/icons/issuetypes/task.png",
                             Id = 3,
                             Name= "Task"
                        },
                        Summary = "Exception handling"
                    }   
                }
                ,
                new Issue
                {
                    ProxyKey = "AOP-103",
                    Fields = new Fields
                    {
                        IssueType = new IssueType
                        {
                             IconUrl ="https://dev.obss.com.tr/jira/images/icons/issuetypes/improvement.png",
                             Id = 3,
                             Name= "Improvement"
                        },
                        Summary = "Performance improvements"
                    }   
                },
                new Issue
                {
                    ProxyKey = "AOP-104",
                    Fields = new Fields
                    {
                        IssueType = new IssueType
                        {
                             IconUrl ="https://dev.obss.com.tr/jira/images/icons/issuetypes/improvement.png",
                             Id = 3,
                             Name= "Task"
                        },
                        Summary = "Sprint Review Meeting"
                    }   
                },
                new Issue
                {
                    ProxyKey = "AOP-105",
                    Fields = new Fields
                    {
                        IssueType = new IssueType
                        {
                             IconUrl ="https://dev.obss.com.tr/jira/images/icons/issuetypes/improvement.png",
                             Id = 3,
                             Name= "Task"
                        },
                        Summary = "Sprint Retrospective Meeting"
                    }   
                },
                new Issue
                {
                    ProxyKey = "AOP-106",
                    Fields = new Fields
                    {
                        IssueType = new IssueType
                        {
                             IconUrl ="https://dev.obss.com.tr/jira/images/icons/issuetypes/improvement.png",
                             Id = 3,
                             Name= "Task"
                        },
                        Summary = "Sprint Grooming Meeting"
                    }   
                },
                new Issue
                {
                    ProxyKey = "AOP-107",
                    Fields = new Fields
                    {
                        IssueType = new IssueType
                        {
                             IconUrl ="https://dev.obss.com.tr/jira/images/icons/issuetypes/improvement.png",
                             Id = 3,
                             Name= "Task"
                        },
                        Summary = "Code Review"
                    }   
                }
            };
            return issues;
        }
    }
}
