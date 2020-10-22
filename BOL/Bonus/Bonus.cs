using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.Utilities;
using System.Data.SqlClient;
using Bismark.SQL;

namespace Bismark.BOL
{
    public class Bonus : IBonus
    {
        //FIELDS
        private int bonusId;
        private DateTime effectiveDate;
        private decimal percentBonus;
        private decimal fixedBonus;

        //PROPERTIES
        public int BonusId
        {
            get
            {
                return bonusId;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Bonus ID");
                bonusId = value;
            }
        }
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDate;
            }
            set
            {                
                effectiveDate = value;
            }
        }
        public decimal PercentBonus
        {
            get
            {
                return percentBonus;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Bonus percentage");
                Validation.ValidateNumericValue(value, CheckNumericValue.MustBeBetween, "Bonus percentage", 0.01f, 1.0f);
                percentBonus = value;
            }
        }
        public decimal FixedBonus
        {
            get
            {
                return fixedBonus;
            }
            set
            {
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeNegative, "Fixed bonus");
                Validation.ValidateNumericValue(value, CheckNumericValue.MustNotBeZero, "Fixed bonus");
                fixedBonus = value;
            }
        }

        //METHODS
        public static Bonus Create()
        {
            return new Bonus();
        }

        public static Bonus Create(int bonusId)
        {
            return RePackage(BonusSQL.GetBonusById(bonusId))[0];
        }

        internal static List<Bonus> RePackage(SqlDataReader dr)
        {
            var bonusList = new List<Bonus>();

            var bonusBonusId = dr.GetOrdinal("BonusId");
            var bonusEffectiveDate = dr.GetOrdinal("PayPeriod");
            var bonusPercentBonus = dr.GetOrdinal("PercentBonus");
            var bonusFixedBonus = dr.GetOrdinal("FixedBonus");

            while (dr.Read())
            {
                var bonus = new Bonus();

                bonus.bonusId = Convert.ToInt32(dr[bonusBonusId]);
                bonus.effectiveDate = Convert.ToDateTime(dr[bonusEffectiveDate]);
                if(dr[bonusPercentBonus] != DBNull.Value) 
                { 
                    bonus.percentBonus = Convert.ToDecimal(dr[bonusPercentBonus]); 
                } else {
                    bonus.fixedBonus = Convert.ToDecimal(dr[bonusFixedBonus]);
                }

                bonusList.Add(bonus);
                
            }

            return bonusList;
        }

            
    }
}
