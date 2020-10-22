using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Bismark.Data
{
    public abstract class DALCnn
    {
        protected internal static SqlConnection GetConnection()
        {
            return new SqlConnection(Properties.Settings.Default.cnnString);
        }
    }
}
