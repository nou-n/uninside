using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.API.Post;
using uninside.Util;

namespace uninside.API.Comment
{
    public class CommentList
    {
        public string GalleryId { get; private set; }
        public GalleryType GalleryType { get; private set; }

        public PostInfo CurrentPost { get; private set; }

        public int TotalPages { get; private set; }
        public int TotalComments { get; private set; } // 전체 페이지의 댓글 수
        public int CurrentPage { get; private set; } // 현재 페이지

        public List<Comment> Items { get; private set; }

        public CommentList(PostInfo currentPost, Dictionary<string, object> jsonObject)
        {
            GalleryId = currentPost.GalleryId;
            GalleryType = currentPost.GalleryType;

            CurrentPost = currentPost;

            TotalPages = int.Parse((string)jsonObject.GetValue("total_page"));
            TotalComments = int.Parse((string)jsonObject.GetValue("total_comment"));
            CurrentPage = int.Parse((string)jsonObject.GetValue("re_page"));

            Items = new List<Comment>();
            if (jsonObject.ContainsKey("comment_list"))
            {
                List<object> commentList = (List<object>)jsonObject["comment_list"];
                foreach (Dictionary<string, object> comment in commentList)
                {
                    Items.Add(new Comment(currentPost, comment));
                }
            }

            CurrentPost = currentPost;
        }
    }
}
