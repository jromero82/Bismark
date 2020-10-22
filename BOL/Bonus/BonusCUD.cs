using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.Utilities;
using Bismark.SQL;

namespace Bismark.BOL
{
    public class BonusCUD
    {
        public static bool Create(List<int> employeeIdList, IBonus bonus)
        {
            return BonusSQL.Create(employeeIdList, bonus);
        }

        public static bool DeleteById(int bonusId)
        {
            return BonusSQL.DeleteById(bonusId);
        }

        public static bool DeleteSingleBonusByEmpId(int employeeId, int bonusId)
        {
            return BonusSQL.DeleteSingleBonusByEmpId(employeeId, bonusId);
        }
    }
}
