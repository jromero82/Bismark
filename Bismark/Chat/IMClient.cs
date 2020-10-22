using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Security.Principal;
using System.Media;
using Bismark.BOL;

namespace Bismark.GUI
{
    public partial class IMClient : Form
    {
        private Socket clientSocket;
        private string userName;
        private Employee employee;
        private bool online;
        //SoundPlayer fxSendMessage = new SoundPlayer(Properties.Resources.send_message);
        //SoundPlayer fxMessageReceived = new SoundPlayer(Properties.Resources.message_received);

        private byte[] byteData = new byte[1024];

        public IMClient()
        {
            InitializeComponent();
        }

        private void ChatBoxClient_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#d6d6d6");
            toolStrip1.BackColor = ColorTranslator.FromHtml("#d6d6d6");

            employee = Bismark.employee;
            userName = employee.FirstName + ", " + employee.LastName + " " + employee.MiddleInitial;
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!online)
                {
                    var ip = IPAddress.Parse("127.0.0.1");
                    var port = Convert.ToInt32(8080);
                    UpdateLog("[" + DateTime.Now.ToShortTimeString() + "] [CONNECTING...]");
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // Was used to remove computer name from Windows Username.
                    //if (userName.Contains("\\"))
                    //    userName = userName.Split('\\')[1];
                    var serverAddress = new IPEndPoint(ip, port);
                    clientSocket.BeginConnect(serverAddress, new AsyncCallback(OnConnect), null);
                }
                else
                    Online(false);
            }
            catch (Exception ex)
            {
                UpdateLog("[" + DateTime.Now.ToShortTimeString() + "] [ERROR] " + ex.Message);
            }
        }

        private void Online(bool value)
        {
            try
            {
                if (value)
                {
                    online = true;
                    toolStripButton1.Text = "Disconnect";
                    toolStripButton1.Image = Properties.Resources.disconnect;
                    IMData msgToSend = new IMData();
                    msgToSend.Command = CmdType.List;
                    msgToSend.User = userName;
                    msgToSend.Message = null;

                    byteData = msgToSend.ToByte();

                    clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

                    byteData = new byte[1024];
                    clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                    UpdateLog("[" + DateTime.Now.ToShortTimeString() + "] [CONNECTED]");
                }
                else
                {
                    online = false;
                    ClearClientList();
                    toolStripButton1.Text = "Connect";
                    toolStripButton1.Image = Properties.Resources.connect;

                    IMData msgToSend = new IMData();
                    msgToSend.Command = CmdType.Logout;
                    msgToSend.User = userName;
                    msgToSend.Message = null;

                    byteData = msgToSend.ToByte();
                    clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    UpdateLog("[" + DateTime.Now.ToShortTimeString() + "] [DISCONNECTED]");
                }
            }
            catch (Exception ex) 
            {
                UpdateLog("[" + DateTime.Now.ToShortTimeString() + "] [ERROR] " + ex.Message);
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);

                // We are connected so we login into the server
                IMData msgToSend = new IMData();
                msgToSend.Command = CmdType.Login;
                msgToSend.User = userName;
                msgToSend.Message = null;

                byte[] b = msgToSend.ToByte();

                // Send the message to the server
                clientSocket.BeginSend(b, 0, b.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                Online(true);
            }
            catch (SocketException ex)
            {
                UpdateLog("[" + DateTime.Now.ToShortTimeString() + "] [ERROR] " + "Unable to connect to the server. The server has either refused the connection, " +
                                "or it is not online. Please check with your administrator.");
            }
            catch (Exception ex)
            {
                UpdateLog("[" + DateTime.Now.ToShortTimeString() + "] [ERROR] " + ex.Message);
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (txtMessage.Focused && txtMessage.Text.Trim() != string.Empty)
                {
                    try
                    {
                        if (txtMessage.Text.Trim().StartsWith("/clear"))
                        {
                            txtLog.Clear();
                            txtMessage.Clear();
                            e.Handled = true;
                            return;
                        }

                        // fxSendMessage.Play();
                        IMData msgToSend = new IMData();
                        msgToSend.User = userName;
                        msgToSend.Message = txtMessage.Text.Trim();
                        msgToSend.Command = CmdType.Message;

                        byte[] byteData = msgToSend.ToByte();

                        clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                        txtMessage.Clear();
                        e.Handled = true;

                    }
                    catch (Exception ex)
                    {
                        UpdateLog("[" + DateTime.Now.ToShortTimeString() + "] [ERROR] " + ex.Message);
                    }
                }
            }
        }        

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
            }
            catch (SocketException)
            {
                Online(false);
            }
            catch (Exception ex)
            {
                
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndReceive(ar);
                IMData msgReceived = new IMData(byteData);

                switch (msgReceived.Command)
                {
                    case CmdType.Login:
                        AddUser(msgReceived.User);
                        break;
                    case CmdType.Logout:
                        RemoveUser(msgReceived.User);
                        break;
                    case CmdType.Message:
                        break;
                    case CmdType.List:
                        ListUsers(msgReceived.Message);
                        break;
                }

                if (msgReceived.Message != null && msgReceived.Command != CmdType.List)
                {
                    UpdateLog(msgReceived.Message);                 
                    if (!msgReceived.Message.Contains("SERVER:") && msgReceived.User != userName && !this.Focused)
                    {
                        //fxMessageReceived.Play();
                    }
                }
                byteData = new byte[1024];
                clientSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                
            }
            catch (SocketException)
            {
                Online(false);
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ChannelWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (online)
            {
                if (MessageBox.Show("Are you sure you want to disconnect?", "Bismark Chat", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                try
                {
                    IMData msgToSend = new IMData();
                    msgToSend.Command = CmdType.Logout;
                    msgToSend.User = userName;
                    msgToSend.Message = null;

                    byte[] b = msgToSend.ToByte();
                    clientSocket.Send(b, 0, b.Length, SocketFlags.None);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            
                catch (SocketException)
                {

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtLog_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void lstClients_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (lstClients.SelectedItem != null && lstClients.SelectedItem.ToString() != userName)
            //{
            //    QueryWin queryWin = new QueryWin(clientSocket, lstClients.SelectedItem.ToString(), userName);
            //    queryWin.TopLevel = false;
            //    queryWin.Parent = this.Parent;
            //    queryWin.FormBorderStyle = FormBorderStyle.None;
            //    queryWin.Dock = DockStyle.Fill;
            //    queryWin.Show();
            //    queryWin.BringToFront();

                
            //}
        }

        #region UIThreadSafety

        private delegate void AddUserDelegate(string user);
        private void AddUser(string user)
        {
            if (lstClients.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                lstClients.Invoke(new AddUserDelegate(AddUser), user);
            }
            else
            {
                // This is the UI thread so perform the task.
                lstClients.Items.Add(user);
            }
        }

        private delegate void RemoveUserDelegate(string user);
        private void RemoveUser(string user)
        {
            if (lstClients.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                lstClients.Invoke(new RemoveUserDelegate(RemoveUser), user);
            }
            else
            {
                // This is the UI thread so perform the task.
                lstClients.Items.Remove(user);
            }
        }

        private delegate void ListUsersDelegate(string userList);
        private void ListUsers(string userList)
        {
            if (lstClients.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                lstClients.Invoke(new ListUsersDelegate(ListUsers), userList);
            }
            else
            {
                // This is the UI thread so perform the task.
                lstClients.Items.AddRange(userList.Split('|'));
                lstClients.Items.RemoveAt(lstClients.Items.Count - 1);
            }
        }

        private delegate void ClearClientsDelegate();
        private void ClearClientList()
        {
            if (lstClients.InvokeRequired)
                lstClients.Invoke(new ClearClientsDelegate(ClearClientList), null);
            else
                lstClients.Items.Clear();
        }

        private delegate void UpdateLogDelegate(string message);
        private void UpdateLog(string message)
        {
            if (txtLog.InvokeRequired)
            {                
                // This is a worker thread so delegate the task.
                txtLog.Invoke(new UpdateLogDelegate(UpdateLog), message);
            }
            else
            {
                // This is the UI thread so perform the task.
                txtLog.Text += message + "\r\n";
            }
        }
        #endregion

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.ScrollToBottom();
        }

        private void txtIP_DoubleClick(object sender, EventArgs e)
        {
            var button = (ToolStripTextBox)sender;
            button.ReadOnly = false;
        }

        private void txtIP_Leave(object sender, EventArgs e)
        {
            var button = (ToolStripTextBox)sender;
            button.ReadOnly = true;
        }        
    }
}
