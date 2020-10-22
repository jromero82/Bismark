using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bismark.GUI
{
    public partial class CalcPayPeriods : Form
    {
        public CalcPayPeriods()
        {
            InitializeComponent();
        }

        private void CalcPayPeriods_Load(object sender, EventArgs e)
        {

            var payRollDay = new DateTime(2012, 04, 27);

            while (payRollDay.Year > 2005)
            {
                lstPayPeriods.Items.Add(payRollDay.ToLongDateString());
                payRollDay = payRollDay.AddDays(-14);
            }

        }
    }
}
