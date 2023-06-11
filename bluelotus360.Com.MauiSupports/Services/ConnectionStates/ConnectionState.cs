using Microsoft.Maui.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net;

namespace bluelotus360.Com.MauiSupports.Services.ConnectionStates
{
    public class ConnectionState : IConnectionState
    {
        public bool IsConnected()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            Debug.WriteLine(accessType.ToString());
            if (accessType == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                return false;
            }
            //bool isConnectionAvailable = false;
            //string domain = "https://bluelotus360.co/CoreAPI/api/";
            //try
            //{
            //    Ping ping = new Ping();
            //    PingReply reply = ping.Send(new Uri(domain).Host);
            //    if(reply.Status == IPStatus.Success)
            //    {
            //        isConnectionAvailable = true;
            //    }
            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(domain);
            //httpWebRequest.Timeout= 5000;
            //using(HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            //{
            //    if(response.StatusCode == HttpStatusCode.OK)
            //    {
            //        isConnectionAvailable = true;
            //    }
            //}
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex);
            //}
            //return isConnectionAvailable;
        }
    }
}
