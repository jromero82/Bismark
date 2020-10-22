using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Bismark.SQL;

namespace Bismark.BOL
{
    public class PayStub
    {
       
        //Properties
        public int PayStubId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PayPeriod { get; set; }
        public decimal GrossPay { get; set; }
        public decimal YTDGrossPay { get; set; }
        public decimal BonusPay { get; set; }
        public decimal YTDBonusPay { get; set; }
        public decimal IncomeTaxDeduction { get; set; }
        public decimal YTDIncomeTaxDeduction { get; set; }
        public decimal CPPDeduction { get; set; }
        public decimal YTDCPPDeduction { get; set; }
        public decimal EIDeduction { get; set; }
        public decimal YTDEIDeduction { get; set; }
        public decimal PensionDeduction { get; set; }
        public decimal YTDPensionDeduction { get; set; }
        public decimal NetPay { get; set; }
        public decimal YTDNetPay { get; set; }

        public static PayStub Create()
        {
            return new PayStub();
        }

        public static PayStub Create(int payStubId)
        {
            return RePackage(PayStubSQL.GetPayStubById(payStubId))[0];
        }

        internal static List<PayStub> RePackage(SqlDataReader dr)
        {
            var payStubList = new List<PayStub>();

            var payStubPayStubId = dr.GetOrdinal("PayStubId");
            var payStubEmployeeId = dr.GetOrdinal("EmployeeId");
            var payStubPayPeriod = dr.GetOrdinal("PayPeriod");
            var payStubGrossPay = dr.GetOrdinal("GrossPay");
            var payStubYTDGrossPay = dr.GetOrdinal("YTDGrossPay");
            var payStubBonus = dr.GetOrdinal("BonusPay");
            var payStubYTDBonus = dr.GetOrdinal("YTDBonusPay");
            var payStubIncomeTaxDeduction = dr.GetOrdinal("IncomeTaxDeduction");
            var payStubYTDIncomeTaxDeduction = dr.GetOrdinal("YTDIncomeTaxDeduction");
            var payStubCPPDeduction = dr.GetOrdinal("CPPDeduction");
            var payStubYTDCPPDeduction = dr.GetOrdinal("YTDCPPDeduction");
            var payStubEIDeduction = dr.GetOrdinal("EIDeduction");
            var payStubYTDEIDeduction = dr.GetOrdinal("YTDEIDeduction");
            var payStubPensionDeduction = dr.GetOrdinal("PensionDeduction");
            var payStubYTDPensionDeduction = dr.GetOrdinal("YTDPensionDeduction");
            var payStubNetPay = dr.GetOrdinal("NetPay");
            var payStubYTDNetPay = dr.GetOrdinal("YTDNetPay");

            while(dr.Read())
            {
                var payStub = new PayStub();
                
                payStub.PayStubId = Convert.ToInt32(dr[payStubPayStubId]);
                payStub.EmployeeId = Convert.ToInt32(dr[payStubEmployeeId]);
                payStub.PayPeriod = Convert.ToDateTime(dr[payStubPayPeriod]);
                payStub.GrossPay = Convert.ToDecimal(dr[payStubGrossPay]);
                payStub.YTDGrossPay = Convert.ToDecimal(dr[payStubYTDGrossPay]);
                payStub.BonusPay = Convert.ToDecimal(dr[payStubBonus]);
                payStub.YTDBonusPay = Convert.ToDecimal(dr[payStubYTDBonus]);
                payStub.IncomeTaxDeduction = Convert.ToDecimal(dr[payStubIncomeTaxDeduction]);
                payStub.YTDIncomeTaxDeduction = Convert.ToDecimal(dr[payStubYTDIncomeTaxDeduction]);
                payStub.CPPDeduction = Convert.ToDecimal(dr[payStubCPPDeduction]);
                payStub.YTDCPPDeduction = Convert.ToDecimal(dr[payStubYTDCPPDeduction]);
                payStub.EIDeduction = Convert.ToDecimal(dr[payStubEIDeduction]);
                payStub.YTDEIDeduction = Convert.ToDecimal(dr[payStubYTDEIDeduction]);
                payStub.PensionDeduction = Convert.ToDecimal(dr[payStubPensionDeduction]);
                payStub.YTDPensionDeduction = Convert.ToDecimal(dr[payStubYTDPensionDeduction]);
                payStub.NetPay = Convert.ToDecimal(dr[payStubNetPay]);
                payStub.YTDNetPay = Convert.ToDecimal(dr[payStubYTDNetPay]);

                payStubList.Add(payStub);
            }
            return payStubList;
        }      
    }
}
