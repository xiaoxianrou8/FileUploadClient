using Plugin.FilePicker;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FileUploadClient
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebFileUploadPage : ContentPage
    {
        private readonly string url;

        public string filePath { get; set; }
        public WebFileUploadPage(string url)
        {
            InitializeComponent();
            this.url = url;
            //this.WebView.Source = url;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var pickFile = await CrossFilePicker.Current.PickFile();
            if (pickFile is null)
            {
                // 用户拒绝选择文件
            }
            else
            {
                this.FileText.Text = $@"选取文件路径 :{pickFile.FilePath}";
                filePath = pickFile.FilePath;
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                if (filePath == null)
                {
                    DisplayAlert("提示", "请先选择文件", "Giao!!!", "雷霆嘎巴");
                    return;
                }
                if (!File.Exists(filePath))
                {
                    DisplayAlert("提示", "文件不存在", "Giao!!!", "雷霆嘎巴");
                    return;
                }
                //var fileStream = File.OpenRead(filePath);
                var buffer = File.ReadAllBytes(filePath);
                //var bytes=File.ReadAllBytes()
                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (msg, crt, chain, sslp) => { return true; };
                using (HttpClient client = new HttpClient(handler))
                {
                    using (var postContent = new MultipartFormDataContent(string.Format("--{0}", DateTime.Now.Ticks.ToString("x"))))
                    {
                        var fileName = Path.GetFileName(filePath);
                        var stream = new MemoryStream(buffer);
                        HttpContent content = new StreamContent(stream);
                        content.Headers.Clear();
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { FileNameStar = fileName, FileName = fileName };
                        postContent.Add(content);
                        using (var massage = client.PostAsync(url, postContent))
                        {
                            massage.Wait();
                            if (massage.Result.IsSuccessStatusCode)
                            {
                                DisplayAlert("成功", "盖了帽了，上传成功了", "Giao!!!", "雷霆嘎巴");
                            }
                            else
                            {
                                DisplayAlert("失败", "返回首页检查IP及服务状态", "Giao!!!", "雷霆嘎巴");
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                DisplayAlert("异常", $"{ex.Message}", "Giao!!!", "雷霆嘎巴");
                return;
            }
            
        }
    }
}