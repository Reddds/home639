using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace HomeServer.Models.Base
{
    class BaseUtils
    {
        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
        public static void WriteParamToBase(string id, bool? boolValue = null, long? intValue = null, double? doubleValue = null,
    string stringValue = null, byte[] blobValue = null)
        {
            //var conStrConfig = ConfigurationManager.ConnectionStrings["homeBase"];
            
            //var connectionString = conStrConfig.ConnectionString;

/*
            if (IsRunningOnMono())
            {
//                var builder = new SqlConnectionStringBuilder(connectionString);
//                var user = builder.UserID;
//                var pass = builder.Password;
                var proc = new Process
                {
                    StartInfo =
                    {
                        FileName = "mysql",
                        Arguments = $@"--host=localhost --user=hsdatawriter --password=test homeserver << EOF
                                    insert into DataLog(ParameterId, Time, IntValue, DoubleValue, BoolValue, StringValue, BlobValue)
                                    values('{id}', '{DateTime.Now:yyyy-MM-dd hh:mm:ss}', {intValue?.ToString() ?? "NULL"}, {doubleValue?.ToString() ?? "NULL"}, {boolValue?.ToString() ?? "NULL"}, '{stringValue ?? "NULL"}', {blobValue?.ToString() ?? "NULL"});
EOF"
                                    ,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true
                    }
                };
                proc.Start();
                var output = proc.StandardOutput.ReadToEnd();
                Console.WriteLine($"MYSQL: {output}");
                return;
            }
*/



            try
            {
                const string connectionString = "Server=localhost;" +
                                                "Database=homeserver;" +
                                                "User ID=hsdatawriter;" +
                                                "Password=test;" +
                                                "Pooling=false";


                using (var conn = new MySqlConnection {ConnectionString = connectionString})
                {
                    conn.Open();

                    var cmd = new MySqlCommand
                    {
                        CommandText = $@"insert into DataLog(ParameterId, Time, IntValue, DoubleValue, BoolValue, StringValue, BlobValue) 
                                        values(@ParameterId, @Time, @IntValue, @DoubleValue, @BoolValue, @StringValue, @BlobValue)",
                        Connection = conn,
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@ParameterId", id);
                    cmd.Parameters.AddWithValue("@Time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IntValue", intValue);
                    cmd.Parameters.AddWithValue("@DoubleValue", doubleValue);
                    cmd.Parameters.AddWithValue("@BoolValue", boolValue);
                    cmd.Parameters.AddWithValue("@StringValue", stringValue);
                    cmd.Parameters.AddWithValue("@BlobValue", blobValue);
                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
/*
            using (var hb = new homeserverEntities())
            {
                hb.DataLog.Add(new DataLog
                {
                    ParameterId = id,
                    Time = DateTime.Now,
                    BoolValue = boolValue,
                    IntValue = intValue,
                    DoubleValue = doubleValue,
                    StringValue = stringValue,
                    BlobValue = blobValue
                });
                hb.SaveChanges();
            }
*/

        }

    }
}
