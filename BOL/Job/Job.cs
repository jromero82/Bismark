using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Bismark.SQL;
using Bismark.Utilities;

namespace Bismark.BOL
{
    public class Job
    {
        //FIELDS
        private int jobId;
        private Department department;
        private string title;
        private decimal maxSalary;

        //PROPERTIES
        public int JobId
        {
            get
            {
                return jobId;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Job ID");
                jobId = value;
            }
        } 
      
        public Department Department 
        {
            get
            {
                return department;
            }
            set
            {
                department = value;
            }
        }
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                Validation.ValidateStringValue(value, CheckStringValue.MustNotBeNullOrEmpty, "Job Title");
                title = value;
            }
        }
        public decimal MaxSalary 
        {
            get
            {
                return maxSalary;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Max salary");
                maxSalary = value;
            }
        }


        //METHODS
        public static Job Create()
        {
            return new Job();
        }

        public static Job Create(int jobId)
        {
            return Repackage(JobSQL.GetJobDetails(jobId));
        }

        internal static Job Repackage(SqlDataReader dr)
        {
            var job = new Job();
            while (dr.Read())
            {
                job.jobId = Convert.ToInt32(dr["JobId"]);
                job.department = (Department)(dr["Department"]);
                job.title = Convert.ToString(dr["Title"]);
                job.maxSalary = Convert.ToDecimal(dr["MaxSalary"]);
            }
         

            return job;
        }


        public static string GetSupervisor(Department dept)
        {
            var dr = JobSQL.getJobSupervisor(dept);
            var supervisor = string.Empty;

            while (dr.Read())
            {
                supervisor = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["MiddleInitial"]) + " " + Convert.ToString(dr["LastName"]);
            }
            return supervisor;
        }
    }
}
