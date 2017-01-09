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
    public partial class fDesignation : Form
    {

        private Designaiton _Designation = null;
        public Action ItemChanged;
        bool _IsNew = false;


        public void ShowDlg(Designaiton oDesignation, bool IsNew)
        {
            _IsNew = IsNew;
            _Designation = oDesignation;
            RefreshValue();
            this.ShowDialog();
        }

        private void RefreshValue()
        {
            txtCode.Text = _Designation.Code;
            txtName.Text = _Designation.Name;

        }
        private void RefreshObject()
        {
            _Designation.Code = txtCode.Text;
            _Designation.Name = txtName.Text;

        }
        public fDesignation()
        {
            InitializeComponent();
        }
        private void fDesignation_Load(object sender, EventArgs e)
        {
            if (_IsNew)
                txtCode.Text = GenerateCategoryCode();
        }
        private bool IsValid()
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("Please Enter Student Name.", "Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
                return false;
            }
            return true;
        }


        private string GenerateCategoryCode()
        {
            int i = 0;
            string sCode = "";

            using (DLMSEntities db = new DLMSEntities())
            {
                i = db.Designaitons.Count();

                if (i.ToString().Length == 1)
                {
                    sCode = "0000" + Convert.ToString(db.Designaitons.Count() + 1);
                }
                else if (i.ToString().Length == 2)
                {
                    sCode = "000" + Convert.ToString(db.Designaitons.Count() + 1);
                }
                else if (i.ToString().Length == 3)
                {
                    sCode = "00" + Convert.ToString(db.Designaitons.Count() + 1);
                }
                else if (i.ToString().Length == 4)
                {
                    sCode = "0" + Convert.ToString(db.Designaitons.Count() + 1);
                }
                else
                {
                    sCode = "0" + Convert.ToString(db.Designaitons.Count() + 1);
                }
            }
            return sCode;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (!IsValid()) return;
                bool isNew = false;
                using (DLMSEntities db = new DLMSEntities())
                {
                    if (_Designation.DesignationID <= 0)
                    {
                        RefreshObject();
                        _Designation.DesignationID = db.Designaitons.Count() > 0 ? db.Designaitons.Max(obj => obj.DesignationID) + 1 : 1;
                        db.Designaitons.Add(_Designation);
                        isNew = true;
                    }
                    else
                    {
                        _Designation = db.Designaitons.FirstOrDefault(obj => obj.DesignationID == _Designation.DesignationID);
                        RefreshObject();
                    }

                    db.SaveChanges();

                    MessageBox.Show("Data saved successfully.", "Save Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!isNew)
                    {
                        if (ItemChanged != null)
                        {
                            ItemChanged();
                        }
                        this.Close();
                    }
                    else
                    {
                        if (ItemChanged != null)
                        {
                            ItemChanged();
                        }
                        _Designation = new Designaiton();
                        RefreshValue();
                        txtCode.Text = GenerateCategoryCode();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    MessageBox.Show(ex.Message, "Failed to save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(ex.InnerException.Message, "Failed to save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
