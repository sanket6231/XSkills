using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace XSkills.Hubs
{
    [HubName("messagesHub")]
    public class MessagesHub : Hub
    {
        //private static string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        //public void Hello()
        //{
        //    Clients.All.hello();
        //}

        
        public static void GetMessages()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
            context.Clients.All.updateMessages();
        }
    }
}