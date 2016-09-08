using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VkPlayer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://oauth.vk.com/authorize?client_id=5621113&display=popup&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.53&state=123456");
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            toolStripStatusLabel1.Text = "Downloading";
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Downloaded";

            try
            {
                var url = webBrowser1.Url.ToString();

                var l = url.Split('#')[1];
                if (l[0] == 'a')
                {
                    Settings1.Default.token = l.Split('&')[0].Split('=')[1];
                    Settings1.Default.id = l.Split('=')[3].Split('&')[0];
                    Settings1.Default.auth = true;
                    //MessageBox.Show(Settings1.Default.token + "   " + Settings1.Default.id);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
