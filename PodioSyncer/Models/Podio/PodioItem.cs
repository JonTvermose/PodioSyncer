using PodioAPI.Models;
using PodioAPI.Utils.ItemFields;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PodioSyncer.Models.Podio
{
    public class PodioItem
    {
        public string CaseId { get; set; }
        public string Summary { get; set; }
        public UserMember ReportedBy { get; set; }
        public UserMember AssignedTo { get; set; }
        public string Description { get; set; }
        public List<FileContent> Screenshots { get; set; }
        public string FoundInVersion { get; set; }
        public int Priority { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Tags { get; set; }
        public List<FileContent> Files { get; set; }
        public DateTime? EndDate { get; set; }
        public int PodioRevision { get; set; }

        public PodioItem(Item item, PodioAPI.Podio podio)
        {
            CaseId = item.ExternalId;
            Summary = item.Field<TextItemField>("Summary")?.Value;
            Description = item.Field<TextItemField>("description")?.Value;
            FoundInVersion = item.Field<TextItemField>("Found in version")?.Value;
            EndDate = item.Field<DateItemField>("End Date").Start.HasValue ? item.Field<DateItemField>("End Date").Start : item.Field<DateItemField>("End Date").End;
            PodioRevision = item.CurrentRevision.Revision;
            ReportedBy = new UserMember {
                Name = item.CreatedBy.Name,
                UserName = item.CreatedBy.Name
            };
            // TODO get assigned to?
            // TODO get the category items
            var statusCat = item.Field<CategoryItemField>("Status");
            var issueTypeCat = item.Field<CategoryItemField>("Type of issue");
            var priorityCat = item.Field<CategoryItemField>("Priority");
            // TODO get tags?
            
            var images = item.Field<ImageItemField>("Screenshot(s)")?.Images.ToList();
            Screenshots = new List<FileContent>();
            foreach(var file in images)
            {
                FileResponse fileResponse = podio.FileService.DownloadFile(file).GetAwaiter().GetResult();
                Screenshots.Add(new FileContent
                {
                    FileName = file.Name,
                    FileContents = fileResponse.FileContents
                });
            }

            Files = new List<FileContent>();
            foreach (var file in item.Files)
            {
                FileResponse fileResponse = podio.FileService.DownloadFile(file).GetAwaiter().GetResult();
                Files.Add(new FileContent
                {
                    FileName = file.Name,
                    FileContents = fileResponse.FileContents
                });
            }
        }
    }
}
