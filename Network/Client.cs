using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;


namespace Network
{
    public partial class Client : Form
    {
        private NetworkStream output;
        private BinaryWriter writer;
        private BinaryReader reader;
        private string message = "";

        public Client()
        {
            InitializeComponent();
            Thread run = new Thread(RunClient);
            run.Start();
        }

        public void RunClient()
        {
            TcpClient client;

            try
            {
                    client = new TcpClient();
                    client.Connect("localhost", 5001);
                    output = client.GetStream();
                    writer = new BinaryWriter(output);
                    reader = new BinaryReader(output);

                    while (true)
                    {
                        try
                        {
                            // read message from server
                            message = reader.ReadString();
                            String[] words = message.Split(' ');

                            foreach (string word in words)
                            {
                                Action updateLabel = () => countryComboBox.Items.Add(word);
                                countryComboBox.Invoke(updateLabel);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            
            catch (Exception  e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void listcodeButton_Click(object sender, EventArgs e)
        {
            writer.Write(countryComboBox.Items[countryComboBox.SelectedIndex].ToString());
        }


    }
}
