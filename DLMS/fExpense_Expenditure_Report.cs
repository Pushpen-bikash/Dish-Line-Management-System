using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLMS.BO;

namespace DLMS
{
    public partial class fExpense_Expenditure_Report : Form
    {
        public fExpense_Expenditure_Report()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpMonth.Value;

            using(DLMSEntities db = new DLMSEntities())
            {
                pcdExIEpen_ResultBindingSource.DataSource = db.pcdExIEpen(dt).ToList();
                reportViewer1.RefreshReport();
            }
        }

        private void fExpense_Expenditure_Report_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
