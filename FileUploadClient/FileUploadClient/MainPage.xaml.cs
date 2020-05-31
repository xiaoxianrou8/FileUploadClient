using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FileUploadClient
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public string NetUrl { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new WebFileUploadPage("https://www.baidu.com"), true);
            var ip = this.inputIp.Text;
            var hostUrl = string.Concat("http://", ip,"/Home", "/UploadPhysical");
            
             NetUrl = hostUrl;
            this.status.Text = "服务正常";
            await Navigation.PushAsync(new WebFileUploadPage(hostUrl),true);
        }

        public bool ConnectHostTest(IPAddress iPAddress)
        {
            var ping = new Ping();
            this.status.Text = "正在检测网络";
            var result=ping.Send(iPAddress);
            if (result.Status==IPStatus.Success)
            {
                return true;
            }
            return false;
        }
    }
}
