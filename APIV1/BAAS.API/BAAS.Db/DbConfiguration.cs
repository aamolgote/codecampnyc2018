using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Db
{
    public class DbConfiguration
    {
        public static string ConnectionString
        {
            get
            {
                string connectionString = ConfigurationManager.ConnectionStrings["baas"]?.ToString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    return connectionString;
                }
                else
                {
                    throw new Exception("Connection string not defined.");
                }
            }
        }
    }
}
