using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Aow.Facebook.Attachments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AowGold.FaceBooks.Dto
{
    public class PostPageDto :  Entity<string>
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime Created_time { get; set; }
        public string Url { get; set; }
        public List<Attachments> Attachments { get; set; }
    }
}
