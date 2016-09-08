using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace VkPlayer
{
    public partial class Form1 : Form
    {
        public List<Audio> audioList;
        WMPLib.IWMPPlaylist PlayList;
        WMPLib.IWMPMedia Media;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new Form2().Show();
            backgroundWorker1.RunWorkerAsync();
        }

        public class Audio
        {
            public int id { get; set; }
            public int owner_id { get; set; }
            public string artist { get; set; }
            public string title { get; set; }
            public int duration { get; set; }
            public string url { get; set; }
            public string lyrics_id { get; set; }
            public int genre_id { get; set; }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!Settings1.Default.auth) { Thread.Sleep(500); }

            var request = WebRequest.Create("https://api.vk.com/method/audio.get?owner_id=" + Settings1.Default.id + "&need_user=0&access_token=" + Settings1.Default.token);
            var response = request.GetResponse();
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            responseFromServer = HttpUtility.HtmlDecode(responseFromServer);

            var token = JToken.Parse(responseFromServer);
            audioList = token["response"].Children().Skip(1).Select(c => c.ToObject<Audio>()).ToList();

            Invoke((MethodInvoker)delegate
            {
                PlayList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("vkPlayList");

                for (int i = 0; i < audioList.Count(); i++)
                {
                    Media = axWindowsMediaPlayer1.newMedia(audioList[i].url);
                    PlayList.appendItem(Media);
                    listBox1.Items.Add(audioList[i].artist + " - " + audioList[i].title);
                }

                axWindowsMediaPlayer1.currentPlaylist = PlayList;
                axWindowsMediaPlayer1.Ctlcontrols.stop();
            });

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.currentItem = axWindowsMediaPlayer1.currentPlaylist.get_Item(listBox1.SelectedIndex);
            }
        }
    }
}
