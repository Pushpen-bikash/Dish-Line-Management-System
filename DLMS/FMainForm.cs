using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLMS
{
    public partial class FMainForm : Form
    {
        public FMainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DesignationsbasicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fDesignations frm = new fDesignations();
            frm.ShowDialog();
        }

        private void feeCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fFeeCollDetails frm = new fFeeCollDetails();
            frm.ShowDialog();
        }

        private void employeeToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void employeeToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void designationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fCustomers frm = new fCustomers();
            frm.ShowDialog();
        }

        private void outletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fOutlets frm = new fOutlets();
            frm.ShowDialog();
        }

        private void employeeToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            fEmployees frm = new fEmployees();
            frm.ShowDialog();
        }

        private void designationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fDesignations frm = new fDesignations();
            frm.ShowDialog();
        }

        private void feeCollectionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fFeeCollDetails frm = new fFeeCollDetails();
            frm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void systemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fSystemInformation frm = new fSystemInformation();
            frm.ShowDialog();
        }

        private void expenseItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fExpenseIteams frm = new fExpenseIteams();
            frm.ShowDialog();
        }

        private void expenditureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fExpenditures frm = new fExpenditures();
            frm.ShowDialog();
        }
    }
}
