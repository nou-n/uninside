using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.User;

namespace uninside.Gallery
{
    public class Gallery
    {
        public string Id { get; private set; }
        public GalleryType Type { get; private set; }

        public string Name { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public string Description { get; private set; }

        public User.User Master { get; private set; }
        public List<User.User> SubManagers { get; private set; }

        public string Date { get; private set; } // 2025.01.01 형식
        public bool IsNew { get; private set; }
        public string HotState { get; private set; } // e.g., 흥한갤 1위
        public int TotalCount { get; private set; }
        public string CategoryName { get; private set; } // e.g., 취미


        public Gallery(string galleryId, GalleryType galleryType, string koreanName, string imageUrl, string galleryDescription, string masterId, string masterName, List<object> subManagers, string createDt, bool isNew, string hotState, int totalCount, string categoryName)
        {
            Id = galleryId;
            Type = galleryType;

            Name = koreanName;
            ThumbnailUrl = imageUrl;
            Description = galleryDescription;

            Master = (masterId != null && masterName != null) ? new User.User(masterId, masterName) : null;

            if (subManagers != null)
            {
                SubManagers = new List<User.User>();
                foreach (Dictionary<string, object> subManager in subManagers)
                {
                    if (subManager.ContainsKey("id") && subManager.ContainsKey("name"))
                        SubManagers.Add(new User.User(subManager["id"].ToString(), subManager["name"].ToString()));
                }
            }
            else SubManagers = null;

            Date = createDt;
            IsNew = isNew;
            HotState = hotState;
            TotalCount = totalCount;
            CategoryName = categoryName;
        }
    }

    public enum GalleryType
    {
        Normal,
        Minor,
        Mini
    }
}