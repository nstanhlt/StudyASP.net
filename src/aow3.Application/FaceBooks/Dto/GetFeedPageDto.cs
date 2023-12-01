using System;
using System.Collections.Generic;

namespace AowGold.Users.Dto
{
    public class FacebookApiCommentsPageResponseDTO
    {
        public class FacebookApiGetPostPage
        {
            public List<DataClass> data { get; set; }
            public PagingClass paging { get; set; }
        }
        public class DataClass
        {
            public string id { get; set; }
            public DateTime created_time { get; set; }
            public AttachmentsClass attachments { get; set; }
            public string? permalink_url { get; set; }
            public string? link { get; set; }
            public string message { get; set; }
        }
        public class AttachmentsClass
        {
            public List<DataAchmentsClass> data { get; set; }
        }

        public class DataAchmentsClass
        {
            public SubattachmentsClass? subattachments { get; set; }
            public string? description { get; set; }
            public string? title { get; set; }
            public MediaClass media { get; set; }
            public TargetClass target { get; set; }
            public string type { get; set; }
            public string url { get; set; }
        }

 
        public class MediaClass
        {
            public ImageClass image { get; set; }
            public string? source { get; set; }  
        }
        public class SubattachmentsClass
        {
            public List<DataAchmentsClass> data { get; set; }
        }

        public class ImageClass
        {
            public int height { get; set; }
            public int width { get; set; }
            public string src { get; set; }
        }
        public class TargetClass
        {
            public string id { get; set; }
            public string url { get; set; }
        }

        public class FacebookApiPageCommentsResponseDTO
        {
            public List<CommentDataResponseDTO> data { get; set; }
            public PagingClass paging { get; set; }
            public DateTime id { get; set; }
        }
        public class CommentDataResponseDTO
        {
            public DateTime created_time { get; set; }
            public List<CommentFromResponseDTO> from { get; set; }
            public string message { get; set; }
            public string id { get; set; }

        }
        public class CommentFromResponseDTO
        {
            public string name { get; set; }
            public string id { get; set; }

        }


        public class PagingClass
        {
            public CursorsClass? cursors { get; set; }
            public string? next { get; set; }

            public string? previous { get; set; }
            
        }
        public class CursorsClass
        {
            public string before { get; set; }
            public string after { get; set; }
        }
    }
}
