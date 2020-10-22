using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.Utilities;
using Bismark.SQL;
using System.Data.SqlClient;

namespace Bismark.BOL
{
    public class JobLists
    {
        public static List<Job> getJobsByDept(Department dept)
        {
            return JobLists.RePackage(JobSQL.getJobsByDept(dept));
        }

        internal static List<Job> RePackage(SqlDataReader dr)
        {
            var jobList = new List<Job>();

            while (dr.Read())
            {
                var job = new Job();

                job.JobId = Convert.ToInt32(dr["JobId"]);                
                job.Title = Convert.ToString(dr["Title"]);                
                jobList.Add(job);
            }
            return jobList;
        }
    }
}
