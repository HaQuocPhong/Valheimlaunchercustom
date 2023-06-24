using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System.Threading;
using System.Diagnostics;

namespace ValheimLauncher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int totalFile = 0;
        int completed = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            if(button2.Visible == true && button3.Visible == true)
            {
                button2.Visible = false;
                button3.Visible = false;
            }
            else
            {
                button2.Visible = true;
                button3.Visible = true;
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            progressBar.Visible = true;
            //progressbar ftp download
            Progress<FtpProgress> progress = new Progress<FtpProgress>(x =>
            {
                if (x.Progress < 0)
                {
                    //
                }
                else
                {
                    //label1.Visible = true;
                    label1.Text = "Downloading....";
                    progressBar.Value = Convert.ToInt32(x.Progress);
                    label2.Text = Convert.ToInt32(x.Progress) + "%";
                }
            });

            //connect ftp
            var client = new AsyncFtpClient("171.246.94.204", "valheim-user", "123", 21);
            await client.Connect();

            //downloading
            await client.DownloadFile(@"C:\Downloads\1.rar", "/1.rar", FtpLocalExists.Overwrite, FtpVerify.None, progress);


            //extract rar
            completed = 0;
            //richTextBox1.Text = "";

            var progress1 = new Progress<DataResponsed>(data =>
            {
                //label1.Visible = true;
                label1.Text = "Installing.....";
                progressBar.Value = data.Percent;
                label2.Text = $"{data.Percent}%";
                //richTextBox1.AppendText(data.FileName + $" - Size: {data.Size}\n");
                //richTextBox1.ScrollToCaret();
                if (data.isFinish)
                {

                    label1.Text = "Installed.....";
                }
            });

            await Task.Run(() => DoExtractFile(progress1));
        }

        public void DoExtractFile(IProgress<DataResponsed> progress)
        {
            string path1 = @"D:\path.txt";
            StreamReader stream = new StreamReader(path1);
            string path = stream.ReadToEnd();

            using (var archive = RarArchive.Open(@"C:\Downloads\1.rar"))
            {
                totalFile = archive.Entries.Where(entry => !entry.IsDirectory).ToList().Count;
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {

                    entry.WriteToDirectory(path, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true,
                    });
                    Interlocked.Increment(ref completed);
                    var percentage = Convert.ToInt16(((double)completed / totalFile * 1.0) * 100d);
                    var data = new DataResponsed();
                    data.Percent = percentage;
                    data.FileName = entry.Key;
                    data.Size = entry.Size;
                    if (completed == totalFile)
                    {
                        data.isFinish = true;
                    }
                    progress.Report(data);
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string text = @"D:\path.txt";
            StreamReader stream = new StreamReader(text);
            string path = stream.ReadLine();

            string path1 = path + @"\BepInEx\";
            string path2 = path + @"\doorstop_libs\";
            string path3 = path + @"\unstripped_corlib\";
            if (Directory.Exists(path1) && Directory.Exists(path2) && Directory.Exists(path3))
            {
                var dir1 = new DirectoryInfo(path1);
                var dir2 = new DirectoryInfo(path2);
                var dir3 = new DirectoryInfo(path3);
                dir1.Attributes = dir1.Attributes & ~FileAttributes.ReadOnly;
                dir2.Attributes = dir2.Attributes & ~FileAttributes.ReadOnly;
                dir3.Attributes = dir3.Attributes & ~FileAttributes.ReadOnly;
                dir1.Delete(true);
                dir2.Delete(true);
                dir3.Delete(true);

                MessageBox.Show("Remove done");
            }
            else
            {
                MessageBox.Show("You don't have mods installed");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string text = @"D:\path.txt";
            StreamReader stream = new StreamReader(text);
            string path = stream.ReadLine();

            Process.Start(path + @"\valheim");
        }
    }

    public class DataResponsed
    {
        public string FileName { get; set; }
        public long Size { get; set; }
        public int Percent { get; set; }
        public bool isFinish { get; set; } = false;
    }
}
