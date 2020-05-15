using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace XSkills.Hubs
{
    public class NotificationComponent
    {
        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            string sqlCommand = @"SELECT CommentID,Username,MesgFrom,MesgTo,CommentText,CommentDate,ParentId from [dbo].[ScratchPad] where [CommentDate] > @AddedOn";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                cmd.Parameters.AddWithValue("@AddedOn", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                //from here we will send notification message to client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.notify("added");
                notificationHub.Clients.All.updateMessages("added");

                //re-register notification
                RegisterNotification(DateTime.Now);

            }
        }

        public List<sp_GetMessages_Result> GetNotifications(DateTime afterDate,string username)
        {
            using (XSkillsEntities1 dc = new XSkillsEntities1())
            {
                return dc.sp_GetMessages(commentID: 0, username: username).Where(a => a.CommentDate > afterDate).OrderByDescending(a => a.CommentDate).ToList();
            }
        }
    }
}