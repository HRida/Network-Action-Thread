using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;   
using System.IO;            
using System.Threading;
using System.Data.SqlClient;
using System.Configuration; 

namespace Network
{
    public partial class Server : Form
    {
        private NetworkStream socketStream;
        private BinaryWriter writer;
        private BinaryReader reader;
        private Socket connection;

        DbUtilcs dbUtil = new DbUtilcs();


        public Server()
        {
            InitializeComponent();
        }

        public void RunServer()
        {
            TcpListener listener;
            try
            {

                listener = new TcpListener(5001);
                listener.Start();

                    connection = listener.AcceptSocket();
                    socketStream = new NetworkStream(connection);
                    writer = new BinaryWriter(socketStream);
                    reader = new BinaryReader(socketStream);

                    Action updateText = () => viewTextBox.Text = "Connected";
                    viewTextBox.Invoke(updateText);
                
                while (true)
                {
                    string theReply = "";
                    try
                    {
                        theReply = reader.ReadString();
                        updateText = () => viewTextBox.Text = returnCode(theReply);
                        viewTextBox.Invoke(updateText);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception e )
            {
                MessageBox.Show(e.Message);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {

            Client c = new Client();
            c.Show();
            Thread run = new Thread(RunServer);
            run.Start();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            string sendCountry = "";
            using (SqlConnection c = dbUtil.GetSqlConnection(dbUtil.GetConnectionString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Select * from CountryCodes", c))
                {
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        sendCountry += sqlReader.GetValue(0) + " ";
                    }
                }
            }

            writer.Write(sendCountry);
            
        }

        private string returnCode(string name)
        {
            string sendCountry = "";

            using (SqlConnection c = dbUtil.GetSqlConnection(dbUtil.GetConnectionString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand("Select * from CountryCodes where Country='"+name+"'", c))
                {
                    SqlDataReader sqlReader = sqlCommand.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        sendCountry = sqlReader.GetValue(1).ToString();
                    }
                }
            }
            return sendCountry;
        }
    }
}
