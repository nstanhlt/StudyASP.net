using System;
using System.Collections.Generic;
using System.Text;

namespace AowGold.Users.Dto
{
    public class PostAndCommentsPageFBDto
    {
        public class PostAndCommentsFanPageFBDto
        {
            public List<DataClassPostPage> data { get; set; }
            public PagingClass paging { get; set; }
        }
        public class DataClassPostPage
        {
            public string full_picture { get; set; }
            public string id { get; set; }
            public DateTime created_time { get; set; }
            public AttachmentsPageClass attachments { get; set; }
            public DataCommentsClass comments { get; set; }
            public string? permalink_url { get; set; }
            public string? link { get; set; }
            public string message { get; set; }
            public TypeNameClass __type__ { get; set; }

        }
        public class TypeNameClass
        {
            public string name { get; set; }
        }
        public class AttachmentsPageClass
        {
            public List<DataAchmentsPostPage> data { get; set; }
        }

        public class DataAchmentsPostPage
        {
            public SubattachmentsPageClass? subattachments { get; set; }
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
        public class SubattachmentsPageClass
        {
            public List<DataAchmentsPostPage> data { get; set; }
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

        public class DataCommentsClass
        {
            public List<DataCommentClass> data { get; set; }
        }

        public class DataCommentClass
        {
            public DateTime created_time { get; set; }
            public string message { get; set; }

            public FromCommentClass from { get; set; }

            public string id { get; set; }

        }

        public class FromCommentClass
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
        public class TypeClass
        {
            public string name { get; set; }
        }
    }
}
