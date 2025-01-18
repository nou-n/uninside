using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tiny;
using uninside.Http;
using uninside.Util;

namespace uninside.Gallery
{
    public class GalleryManager
    {
        public Uninside Client { get; private set; }

        public GalleryManager(Uninside uninside)
        {
            this.Client = uninside;
        }

        public async Task<Gallery> GetGallery(string galleryId, GalleryType galleryType)
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();
            formData.Add(new StringContent(Utils.GetGalleryId(galleryId, galleryType)), "id");
            formData.Add(new StringContent(await Client.GetAppId()), "app_id");
            HttpResponse galleryInfoResponse = await HttpRequest.PostAsync(ApiUrls.APP_API + "minor_info", payload: formData, headers: Utils.defaultHeaders);

            Dictionary<string, object> data = Json.Decode<List<Dictionary<string, object>>>(await galleryInfoResponse.Message.Content.ReadAsStringAsync())[0];
            return new Gallery(
                galleryId,
                galleryType,
                (string)data.GetValue("ko_name"),
                (string)data.GetValue("img"),
                (string)data.GetValue("mgallery_desc"),
                (string)data.GetValue("master_id"),
                (string)data.GetValue("master_name"),
                (List<object>)data.GetValue("submanager"),
                (string)data.GetValue("create_dt"),
                (bool)data.GetValue("new"),
                (string)data.GetValue("hot_state"),
                int.Parse(((string)data.GetValue("total_count")).Replace(",", "")),
                (string)data.GetValue("cate_name")
            );
        }
    }
}