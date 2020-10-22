using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bismark.SQL;
using Bismark.Utilities;

namespace Bismark.BOL
{
    public class BonusLists
    {

        public static List<Bonus> GetBonusByEmpId(int employeeId, BonusGetType type)
        {
            return Bonus.RePackage(BonusSQL.GetBonusByEmpId(employeeId, type));
        }       

    }
}
