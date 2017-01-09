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
    public partial class fOutlet : Form
    {
        private Outlet _Outlet = null;
        public Action ItemChanged;
        bool _IsNew = false;
        public fOutlet()
        {
            InitializeComponent();
        }

        public void ShowDlg(Outlet oOutlet,bool IsNew)
        {
            _IsNew = IsNew;
            _Outlet = oOutlet;
            RefreshValue();
            this.ShowDialog();
        }

        private void RefreshValue()
        {
            txtOutletCode.Text = _Outlet.OutletCode;
            txtOutletName.Text = _Outlet.OutletName;
            txtOutletName.Focus();

        }
        private string GenerateOutletCode()
        {
            int i = 0;
            string sCode = "";

            using (DLMSEntities db = new DLMSEntities())
            {
                i = db.Outlets.Count();

                if (i.ToString().Length == 1)
                {
                    sCode = "0000" + Convert.ToString(i + 1);
                }
                else if (i.ToString().Length == 2)
                {
                    sCode = "000" + Convert.ToString(i + 1);
                }
                else if (i.ToString().Length == 3)
                {
                    sCode = "00" + Convert.ToString(i + 1);
                }
                else if (i.ToString().Length == 4)
                {
                    sCode = "0" + Convert.ToString(i + 1);
                }
                else
                {
                    sCode = "0" + Convert.ToString(i + 1);
                }
            }
            return sCode;
        }
        private void fOutlet_Load(object sender, EventArgs e)
        {
            if (_IsNew)
                txtOutletCode.Text = GenerateOutletCode();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!IsValid()) return;
            if (MessageBox.Show("Do you want to save the information?", "Save Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (DLMSEntities db = new DLMSEntities())
                    {
                        if (_Outlet.OutletID <= 0)
                        {
                            RefreshObject();
                            _Outlet.OutletID = db.Outlets.Count() > 0 ? db.Outlets.Max(obj => obj.OutletID) + 1 : 1;
                            db.Outlets.Add(_Outlet);
                        }
                        else
                        {
                            _Outlet = db.Outlets.FirstOrDefault(obj => obj.OutletID == _Outlet.OutletID);
                            RefreshObject();
                        }

                        db.SaveChanges();
                        MessageBox.Show("Data saved successfully.", "Save Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("Do you want to insert another Outlet?", "Insert Outlet", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
                            _Outlet = new Outlet();
                            RefreshValue();
                            txtOutletCode.Text = GenerateOutletCode();
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
        }

        private void RefreshObject()
        {
            _Outlet.OutletCode = txtOutletCode.Text;
            _Outlet.OutletName = txtOutletName.Text;
        }

        private bool IsValid()
        {
            if (txtOutletName.Text.Length == 0)
            {
                MessageBox.Show("Please Enter Outlet Name.", "Outlet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOutletName.Focus();
                return false;
            }
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
