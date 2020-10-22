using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Data;
using System.ComponentModel;


namespace Bismark.BOL
{
    public enum CmdType
    {
        Login,
        Logout,
        List,
        Message, 
        Null
    }

    public class IMData
    {
        public CmdType Command;   
        public string User;    
        public string Message;        

        public IMData()
        {
            this.Command = CmdType.Null;
            this.User = null;         
            this.Message = null; 
        }


        public IMData(byte[] data)
        {
            this.Command = (CmdType)BitConverter.ToInt32(data, 0);
            int userLength = BitConverter.ToInt32(data, 4);
            int messageLength = BitConverter.ToInt32(data, 8);

            if (userLength > 0)
                this.User = Encoding.UTF8.GetString(data, 12, userLength);
            else
                this.User = null;    

            if (messageLength > 0)
                this.Message = Encoding.UTF8.GetString(data, 12 + userLength, messageLength);
            else
                this.Message = null;
        }

        public byte[] ToByte()
        {
            List<byte> result = new List<byte>();
            result.AddRange(BitConverter.GetBytes((int)Command));


            if (User != null)
                result.AddRange(BitConverter.GetBytes(User.Length));
            else
                result.AddRange(BitConverter.GetBytes(0));

            if (Message != null)
                result.AddRange(BitConverter.GetBytes(Message.Length));
            else
                result.AddRange(BitConverter.GetBytes(0));



            if (User != null)
                result.AddRange(Encoding.UTF8.GetBytes(User));

            if (Message != null)
                result.AddRange(Encoding.UTF8.GetBytes(Message));

            return result.ToArray();
        }
    }
}
