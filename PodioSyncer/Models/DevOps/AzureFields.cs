using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PodioSyncer.Models.DevOps
{
    public class AzureFields
    {
        [JsonPropertyName("System.WorkItemType")]
        public string WorkItemType { get; set; }

        [JsonPropertyName("System.IterationPath")]
        public string IterationPath { get; set; }

        [JsonPropertyName("System.State")]
        public string State { get; set; }

        [JsonPropertyName("System.Title")]
        public string Title { get; set; }

        [JsonPropertyName("Microsoft.VSTS.Common.Priority")]
        public int Priority { get; set; }

        [JsonPropertyName("System.Description")]
        public string Description { get; set; }

        [JsonPropertyName("System.Tags")]
        public string Tags { get; set; }

        [JsonPropertyName("Microsoft.VSTS.Scheduling.StoryPoints")]
        public double StoryPoints { get; set; }

        [JsonPropertyName("System.AreaPath")]
        public string AreaPath { get; set; }

        [JsonPropertyName("System.TeamProject")]
        public string TeamProject { get; set; }

        [JsonPropertyName("System.AssignedTo")]
        public string AssignedTo { get; set; }

        [JsonPropertyName("System.ChangedBy")]
        public string ChangedBy { get; set; }

        [JsonPropertyName("System.CommentCount")]
        public int CommentCount { get; set; }
    }
}
