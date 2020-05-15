using XSkills.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using XSkills.Models;

namespace XSkills.Models
{
    public class MessagesRepository
    {
        readonly string _connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<ScratchPadModel> GetAllMessages(int commentID, string Username)
        {
            var messages = new List<ScratchPadModel>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"dbo.sp_GetMessages", connection))
                {
                    command.Notification = null;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@CommentID", SqlDbType.Int).Value = commentID;
                    command.Parameters.Add("@Username", SqlDbType.VarChar).Value = Username;
                    
                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        messages.Add(item: new ScratchPadModel
                        {
                            CommentID = (int)reader["CommentID"],
                            Username = (string)reader["Username"],
                            MesgFrom = (string)reader["MesgFrom"],
                            MesgTo = (string)reader["MesgTo"],
                            CommentText = (string)reader["CommentText"],
                            CommentDate = Convert.ToDateTime(reader["CommentDate"]),
                            ParentId = (int)reader["ParentId"],
                            ImgUrl = (string)reader["ImgUrl"]
                        });
                    }
                }

            }
            return messages;


        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                MessagesHub.GetMessages();
            }
        }
    }
}