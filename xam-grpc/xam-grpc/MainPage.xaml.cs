using Grpc.Core;
using grpc_lib;
using Helloworld;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace xam_grpc
{
    public partial class MainPage : ContentPage
    {
        private int count;
        //private ClientCore client;
        private ClientNet client;

        public MainPage()
        {
            InitializeComponent();

            count = 0;
            //client = new ClientCore();
            client = new ClientNet();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                //count++;
                // for Android emulator, use loopback on host machine: https://developer.android.com/studio/run/emulator-networking
                // 10.0.2.2:50001
                //client.SendTestRequest("10.0.2.2", 50001, "Xamarin", count);

                var data = client.GetTestReplyAsArray();
                var reply = client.GetTestReplyFromArray(data);
                Console.WriteLine(">" + reply);
            }
            catch (Exception ex)
            {
                Console.WriteLine("XXX | error:" + ex);
            }
        }
    }
}
