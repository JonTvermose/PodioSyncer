﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PodioSyncer.Data.Models
{
    public class FieldMapping
    {
        public int Id { get; set; }
        public string AzureFieldName { get; set; }
        public string PodioFieldName { get; set; }
        public FieldType FieldType { get; set; }
        public int PodioAppId { get; set; }
        public PodioApp PodioApp { get; set; }
        public ICollection<CategoryMapping> CategoryMappings { get; set; }
    }

    public enum FieldType
    {
        Boolean,
        Int,
        String,
        Image,
        File,
        User,
        Category,
        Date
    }
}