using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uninside.Util;

namespace uninside.Gallery
{
    /// <summary>
    /// GetPostList를 호출했을 때 갤러리에 대한 정보를 담는 모델
    /// </summary>
    public class GalleryInfo
    {
        public string Title { get; private set; }
        public string Category { get; private set; }

        public int FileCount { get; private set; } // file_cnt -> cnt가 Count를 의미하는 거겠지
        public int FileSize { get; private set; } 

        public bool IsMinor { get; private set; }
        public bool AIEnabled { get; private set; } // use_ai_write

        public List<HeadText> HeadTexts { get; private set; }
        public List<Placeholder> Placeholders { get; private set; }

        public string RecentNotifyDate { get; private set; } // notify_recent

        public string MustReadTitle { get; private set; } // must_read -> subject
        public string MustReadId { get; private set; } // must_read -> no (int 형식)

        public string DefaultNickname { get; private set; } // gall_nickname

        public GalleryInfo(string gallTitle, string category, string fileCnt, string fileSize, bool isMinor, bool useAiWrite, List<object> headTexts, List<object> placeholders, string notifyRecent, Dictionary<string, object> mustRead, string gallNickname)
        {
            Title = gallTitle;
            Category = category;

            FileCount = int.Parse(fileCnt);
            FileSize = int.Parse(fileSize);

            IsMinor = isMinor;
            AIEnabled = useAiWrite;

            if (headTexts != null)
            {
                HeadTexts = new List<HeadText>();
                foreach(Dictionary<string, object> dict in headTexts)
                {
                    HeadTexts.Add(new HeadText(
                        (string)dict.GetValue("no"),
                        (string)dict.GetValue("name"),
                        (string)dict.GetValue("level"),
                        (bool)(dict.GetValue("selected") ?? false),
                        (bool)(dict.GetValue("recomm_unused") ?? false)
                    ));
                }
            }else HeadTexts = null;

            if(placeholders != null)
            {
                Placeholders = new List<Placeholder>();
                foreach (Dictionary<string, object> dict in placeholders)
                {
                    //Console.WriteLine(dict.GetValue("no"));
                    //Console.WriteLine(dict.GetValue("no").GetType().Name);

                    Placeholders.Add(new Placeholder(
                        (Int64) (dict.GetValue("no") ?? -1),
                        (string)dict.GetValue("msg")
                    ));
                }
            }
            else Placeholders = null;

            if(mustRead != null)
            {
                MustReadTitle = (string)mustRead.GetValue("subject");
                MustReadId = ((Int64) (mustRead.GetValue("no") ?? -1)).ToString();
            }

            DefaultNickname = gallNickname;
        }
    }

    public class HeadText
    {
        public string Id { get; private set; } // no
        public string Name { get; private set; }
        public string Level { get; private set; }
        public bool Selected { get; private set; }
        public bool RecommendEnabled { get; private set; } // !recomm_unused

        public HeadText(string no, string name, string level, bool selected, bool recommUnused)
        {
            Id = no;
            Name = name;
            Level = level;
            Selected = selected;
            RecommendEnabled = !recommUnused;
        }
    }

    public class Placeholder
    {
        public string Id { get; private set; } // no (response json에서 int 형식)
        public string ContentHTML { get; private set; } // msg
        public Placeholder(Int64 no, string msg)
        {
            Id = no.ToString();
            ContentHTML = msg;
        }
    }
}
