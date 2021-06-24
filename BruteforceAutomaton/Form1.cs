using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BruteforceAutomaton
{
    public partial class Form1 : Form
    {
        OpenFileDialog fileDialog;
        bool clf=false;
        public Form1()
        {
            InitializeComponent();
            fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = @"C:\Users\CH2\Documents\大三下\软工实验\mywork\text";
            fileDialog.RestoreDirectory = true;
            fileDialog.Title = "选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "C:\\";

            openFileDialog1.Filter = "文本文件 (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {

                //label1.Text = openFileDialog1.FileName;

                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);

                textBox1.Text = sr.ReadToEnd();

                sr.Close();

            }*/

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                
                string file = fileDialog.FileName; 
                System.IO.StreamReader sr = new System.IO.StreamReader(fileDialog.FileName);
                textBox1.Text = sr.ReadToEnd();
                sr.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;
                System.IO.StreamReader sr = new System.IO.StreamReader(fileDialog.FileName);
                textBox2.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
            if (clf && e.CurrentValue == CheckState.Indeterminate)
            {
                e.NewValue = CheckState.Indeterminate;
            }
            
            if (e.Index == 0)
            {
                if (e.CurrentValue==CheckState.Unchecked)
                {
                    /*checkedListBox1.SetItemChecked(1, false);
                    checkedListBox1.SetItemChecked(2, false);
                    checkedListBox1.SetItemChecked(3, false);*/
                    clf = true;
                    checkedListBox1.SetItemCheckState(1, CheckState.Indeterminate);
                    checkedListBox1.SetItemCheckState(2, CheckState.Indeterminate);
                    checkedListBox1.SetItemCheckState(3, CheckState.Indeterminate);
                    //checkedListBox1.
                }
                else if(e.CurrentValue==CheckState.Checked)
                {
                    clf = false;
                    checkedListBox1.SetItemCheckState(1, CheckState.Unchecked);
                    checkedListBox1.SetItemCheckState(2, CheckState.Unchecked);
                    checkedListBox1.SetItemCheckState(3, CheckState.Unchecked);
                }
            }
            //else if (!checkedListBox1.GetItemChecked(0) && e.CurrentValue == CheckState.Indeterminate)
            //{
            //    e.NewValue = CheckState.Indeterminate;
            //}
        }

        public static int readInt(Byte[] buffer, ref int offset)
        {
            Int32 value = BitConverter.ToInt32(buffer, offset);
            offset += 4;
            return value;
        }
        public static void write(Byte[] buffer, int value, ref int offset)
        {
            byte[] bTemp = BitConverter.GetBytes(value);
            for (Int32 i = 0; i < bTemp.Length; i++)
            {
                buffer[offset++] = bTemp[i];
            }
        }
        public static void write(Byte[] buffer, bool value, ref int offset)
        {
            byte[] bTemp = BitConverter.GetBytes(value);
            for (Int32 i = 0; i < bTemp.Length; i++)
            {
                buffer[offset++] = bTemp[i];
            }
        }
        public static void write(Byte[] buffer, String value, ref Int32 offset)
        {
            if (!String.IsNullOrEmpty(value))
            {
                byte[] bTemp = Encoding.ASCII.GetBytes(value);

                write(buffer, bTemp.Length, ref offset);
                for (Int32 i = 0; i < bTemp.Length; i++)
                {
                    buffer[offset++] = bTemp[i];
                }
            }
            else
            {
                write(buffer, 0, ref offset);
            }
        }
        private byte[] getBytes()
        {
            string charBuf1 = textBox1.Text;
            string charBuf2 = textBox2.Text;
            
            byte[] buffer=new byte[4+4+charBuf1.Length+4+charBuf2.Length];
            int offset = 0;
            write(buffer, checkedListBox1.GetItemChecked(0), ref offset);
            write(buffer, checkedListBox1.GetItemChecked(1), ref offset);
            write(buffer, checkedListBox1.GetItemChecked(2), ref offset);
            write(buffer, checkedListBox1.GetItemChecked(3), ref offset);

            //write(buffer, charBuf1.Length, ref offset);
            write(buffer, charBuf1, ref offset);
            //write(buffer, charBuf2.Length, ref offset);
            write(buffer, charBuf2, ref offset);
            return buffer;
        }
        private byte[] getByteResult()
        {
            String IP = "127.0.0.1";
            int port = 8885;
            IPAddress ip = IPAddress.Parse(IP);
            Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(ip, port);
            ClientSocket.Connect(endPoint);
            Console.WriteLine("开始发送消息");
            byte[] message = getBytes();
            ClientSocket.Send(message);
            Console.WriteLine("发送消息为:" + Encoding.ASCII.GetString(message));
            byte[] receive = new byte[1024];
            Console.WriteLine("接收消息为：" + Encoding.ASCII.GetString(receive));
            ClientSocket.Close();
            return receive;
        }
        private byte[] _getByteResult()
        {
            BinaryReader bw = new BinaryReader(new FileStream(@"C:\Users\CH2\Documents\大三下\软工实验\mywork\text\in.txt",
FileMode.Open));
            byte[] bt=bw.ReadBytes(1024);
            bw.Close();
            Console.WriteLine("接收消息为：" +Encoding.ASCII.GetString(bt));
            return bt;
        }
        private Tuple<int,int>[] _getReveiveArray()
        {
            Tuple<int, int>[] c= new Tuple<int, int>[2];

            c[0] = new Tuple<int, int>(6, 7);
            c[1] = new Tuple<int, int>(8,9);
           
            return c;
        }
        private Tuple<int, int>[] getReveiveArray()
        {
            int offset = 0;
            byte[] bs=_getByteResult();
            int len=readInt(bs,ref offset);
            Tuple<int, int>[] res=new Tuple<int, int>[len];
            for(int i = 0; i < len; ++i) {
                int x = readInt(bs, ref offset);
                int y = readInt(bs, ref offset);
                res[i] = new Tuple<int, int>(x,y);
            }
            return res;
        }
        private Tuple<int,int,String>[] _getFindArray(Tuple<int, int>[] offArray)
        {
            return new Tuple<int, int, string>[2] {
                new Tuple<int, int, string>(1,2,"12"),
                new Tuple<int, int, string>(3,4,"34")
            };
        }
        private Tuple<int, int, String>[] getFindArray(Tuple<int, int>[] offArray)
        {
            int showContextLen = 3;
            Tuple<int, int, String>[] res=new Tuple<int, int, String>[offArray.Length];
            int offset = 0,nowline=0;
            //foreach(Tuple<int, int> t in offArray)
            for(int i=0;i< offArray.Length;++i)
            {
                Tuple<int, int> t = offArray[i];
                while (offset+ textBox1.Lines[nowline].Length + 2 <= t.Item1)
                {
                    offset += textBox1.Lines[nowline].Length+2;
                    nowline++;
                }
                int r = nowline + 1, c = t.Item1 - offset + 1;
                string s= textBox1.Text.Substring(t.Item1,t.Item2-t.Item1);
                int spreLen = Math.Min(c-1,showContextLen);
                string spre= textBox1.Text.Substring(t.Item1-spreLen,spreLen);
                if (c > showContextLen+1) spre = "..." + spre;

                Tuple<int, int, String> nt = new Tuple<int, int, String>(r,c,spre+s+"...");
                
                res[i]=nt;
            }
            /*for (int i = 0; i < offArray.Length; ++i)
            {
                Tuple<int, int> t = offArray[i];
                while (offset + textBox1.Lines[nowline].Length + 2 <= t.Item2-1)
                {
                    offset += textBox1.Lines[nowline].Length + 2;
                    nowline++;
                }
                int r = nowline + 1, c = t.Item2 - offset + 1;

                int stailLen = Math.Min(textBox1.Lines[nowline].Length - c, showContextLen);
                string stail = textBox1.Text.Substring(t.Item2 + stailLen, stailLen);
                if (c+ showContextLen < textBox1.Lines[nowline].Length) stail += "...";

                //Tuple<int, int, String> nt = new Tuple<int, int, String>(r, c, spre + s);

                res[i].Item3+=stail;
            }*/


            return res;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream(@"C:\Users\CH2\Documents\大三下\软工实验\mywork\text\out.txt",
            FileMode.Create));
            bw.Write(getBytes());
            bw.Close();

            foreach(string s in textBox1.Lines){
                Console.WriteLine(s.Length);
            }

            Tuple<int, int>[] reveiveArray= getReveiveArray();
            Tuple<int, int, String>[] findArray=getFindArray(reveiveArray);
            listView1.Items.Clear();
            foreach (Tuple<int, int, String> fd in findArray)
            {
                ListViewItem lvi = new ListViewItem();

                lvi.Text = fd.Item1.ToString();
                lvi.SubItems.Add(fd.Item2.ToString());
                lvi.SubItems.Add(fd.Item3);
                listView1.Items.Add(lvi);
            }

        }
    }
}
