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
using System.IO;

namespace DLMS
{
    public partial class fEmployee : Form
    {


        private Employee _Employee = null;
        public Action ItemChanged;
        private DLMS.BO.SystemInformation _SystemInformation = null;
        bool _IsNew = false;

        public fEmployee()
        {
            InitializeComponent();
        }

        private bool IsValid()
        {
            if (txtEmpCode.Text.Length == 0)
            {
                MessageBox.Show("Please Enter Employee Name.", "Employee", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmpCode.Focus();
                return false;
            }
            return true;
        }

        private void RefreshValue()
        {
            txtEmpCode.Text = _Employee.EmpCode;
            txtEmpName.Text = _Employee.EmpName;
            txtFName.Text = _Employee.FName;
            txtMName.Text = _Employee.MName;
            txtContactNo.Text = _Employee.ContactNo;
           
         
            if ((Designaiton)cboDesignation.SelectedItem != null)
                cboDesignation.SelectedValue = ((Designaiton)cboDesignation.SelectedItem).DesignationID;

            txtPreAddress.Text = _Employee.PresentAdd;
            txtPerAddress.Text = _Employee.PermanentAdd;

            if (_Employee.Photopath != null)
            {
                string path = _SystemInformation.EmpPhotoPath + "\\" + _Employee.Photopath;
                if (File.Exists(path))
                {

                    pbxEmpPic.ImageLocation = _SystemInformation.EmpPhotoPath + "//" + _Employee.Photopath;
                }
                else
                {
                    MessageBox.Show("Error Loading Employee Picture" + Environment.NewLine + "Picture Not Found");
                }
            }
        }

        public string GenerateEmpCode()
        {
            int i = 0;
            string sCode = "";

            using (DLMSEntities db = new DLMSEntities())
            {
                i = db.Employees.Count();

                if (i.ToString().Length == 1)
                {
                    sCode = "0000" + Convert.ToString(db.Employees.Count() + 1);
                }
                else if (i.ToString().Length == 2)
                {
                    sCode = "000" + Convert.ToString(db.Employees.Count() + 1);
                }
                else if (i.ToString().Length == 3)
                {
                    sCode = "00" + Convert.ToString(db.Employees.Count() + 1);
                }
                else if (i.ToString().Length == 4)
                {
                    sCode = "0" + Convert.ToString(db.Employees.Count() + 1);
                }
                else
                {
                    sCode = "0" + Convert.ToString(db.Employees.Count() + 1);
                }
            }
            return sCode;
        }
        private void RefreshObject()
        {
            _Employee.EmpCode = txtEmpCode.Text;
            _Employee.EmpName = txtEmpName.Text;
            _Employee.FName = txtFName.Text;
            _Employee.MName = txtMName.Text;
            _Employee.ContactNo = txtContactNo.Text;
          
            _Employee.DesignationID = ((Designaiton)cboDesignation.SelectedItem).DesignationID;

            _Employee.PresentAdd = txtPreAddress.Text;
            _Employee.PermanentAdd = txtPerAddress.Text;

            #region Employee Picture

            if ((pbxEmpPic.ImageLocation != string.Empty) && (pbxEmpPic.ImageLocation != null))
            {
                if (_Employee.Photopath == null)
                {
                    _Employee.Photopath = _SystemInformation.EmpPhotoPath;
                }

                if ((_Employee.Photopath.Trim()).ToUpper() != new FileInfo(pbxEmpPic.ImageLocation).Name.Trim().ToUpper())
                {
                    FileInfo fInfo = new FileInfo(pbxEmpPic.ImageLocation.Trim());
                    string photoName = txtEmpCode.Text + fInfo.Extension;
                    _Employee.Photopath = _SystemInformation.EmpPhotoPath;

                    if (fInfo.Exists == false)
                    {
                        MessageBox.Show("Picture does not exist", "Employee Picture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (_Employee.Photopath== "")
                    {
                        MessageBox.Show("Employee's Picture will not be saved. because it's path is not defined.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (!Directory.Exists(_SystemInformation.EmpPhotoPath))
                            Directory.CreateDirectory(_SystemInformation.EmpPhotoPath);

                        if (new FileInfo(_SystemInformation.EmpPhotoPath + photoName).Exists)
                        {
                            if (MessageBox.Show("A picture named " + photoName + " already exists. Do you want to replace it?", "Employee Picture", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                FileInfo info = new FileInfo(_SystemInformation.EmpPhotoPath + photoName);
                                if (info.IsReadOnly)
                                    info.IsReadOnly = false;
                                File.Copy(pbxEmpPic.ImageLocation.Trim(), _SystemInformation.EmpPhotoPath + "\\" + photoName, true);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            File.Copy(pbxEmpPic.ImageLocation.Trim(), _SystemInformation.EmpPhotoPath + photoName, true);
                        }

                        new FileInfo(_SystemInformation.EmpPhotoPath + photoName).IsReadOnly = true;
                        _Employee.Photopath = photoName;
                    }
                }
            }
            else
            {
                _Employee.Photopath = string.Empty;
            }
            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValid()) return;
                if (MessageBox.Show("Do you want to save the information?", "Save Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (DLMSEntities db = new DLMSEntities())
                    {
                        if (_Employee.EmployeeID <= 0)
                        {
                            RefreshObject();
                            _Employee.EmployeeID = db.Employees.Count() > 0 ? db.Employees.Max(obj => obj.EmployeeID) + 1 : 1;
                            db.Employees.Add(_Employee);
                        }
                        else
                        {
                            _Employee = db.Employees.FirstOrDefault(obj => obj.EmployeeID == _Employee.EmployeeID);
                            RefreshObject();
                        }

                        db.SaveChanges();

                        MessageBox.Show("Data saved successfully.", "Save Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (MessageBox.Show("Do you want to create another Employee?", "Create Employee", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
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
                            _Employee = new Employee();
                            RefreshValue();
                            txtEmpCode.Text = GenerateEmpCode();
                        }
                    }
                }

            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }

        }
        public void ShowDlg(Employee oEmployee, bool IsNew)
        {
           

            _IsNew = IsNew;
            DLMSEntities db = new DLMSEntities();
            var _SysInfos = db.SystemInformations;

            _SystemInformation = (from c in db.SystemInformations
                                  where c.SystemInfoID== 1
                                  select c).FirstOrDefault();

            _Employee = oEmployee;
            PopulateDesignationCbo();
            RefreshValue();
            this.ShowDialog();


        }

        private void PopulateDesignationCbo()
        {
            using (DLMSEntities db = new DLMSEntities())
            {
                cboDesignation.DisplayMember = "Name";
                cboDesignation.ValueMember = "DesignationID";
                cboDesignation.DataSource = db.Designaitons.OrderBy(x => x.Name).ToList();
            }
        }
        private void fEmployee_Load(object sender, EventArgs e)
        {
            PopulateDesignationCbo();
            if (_IsNew)
                txtEmpCode.Text = GenerateEmpCode();
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                odPicture.FileName = "";
                odPicture.ShowDialog();
                if (odPicture.FileName != "")
                {
                    pbxEmpPic.ImageLocation = odPicture.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
