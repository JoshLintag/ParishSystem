﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParishSystem
{
    public partial class ApplicationModule : Form
    {
        DataHandler dh;
        private int profileID, groomID, brideID;
        private int applicationID;
        public ApplicationModule(DataHandler dh)
        {
            InitializeComponent();
            this.dh = dh;
            this.TopLevel = false;

            baptismApplication_birthDate_dtp.MaxDate = DateTime.Now.Date;
            confirmationApplication_birthDate_dtp.MaxDate = DateTime.Now.Date;
            dtpGBirthDate.MaxDate = DateTime.Now.Date;
            dtpBBirthDate.MaxDate = DateTime.Now.Date;
        }

        private void SacramentApplication_Load(object sender, EventArgs e)
        {
            
        }

        private DataGridView getDataGridView(SacramentType t)
        {
            if (t == SacramentType.Baptism)
                return baptismApplication_dgv;
            else if (t == SacramentType.Confirmation)
                return confirmationApplication_dgv;
            else
                return marriageApplication_dgv;
        }

        private TableLayoutPanel getRequirementTableLayoutPanel(SacramentType t)
        {
            if (t == SacramentType.Baptism)
                return baptismApplication_requirements_tlp;
            else if (t == SacramentType.Confirmation)
                return confirmationApplication_requirements_tlp;
            else
                return marriageApplication_requirements_tlp;
        }

        private ComboBox getFilter(SacramentType t)
        {
            if (t == SacramentType.Baptism)
                return baptismApplication_filter_cmb;
            else if (t == SacramentType.Confirmation)
                return confirmationApplication_filter_cmb;
            else
                return marriageApplication_filter_cmb;
        }

        private CheckBox getCheckAllCheckBox(SacramentType t)
        {
            if (t == SacramentType.Baptism)
                return baptismApplication_checkAll_checkBox;
            else if (t == SacramentType.Confirmation)
                return confirmationApplication_checkAll_checkBox;
            else
                return marriageApplication_checkAll_checkBox;
        }

        private Panel getApplicationDetailsPanel(SacramentType t)
        {
            if (t == SacramentType.Baptism)
                return baptismApplicationDetailsPanel;
            else if (t == SacramentType.Confirmation)
                return confirmationApplicationDetailsPanel;
            else
                return marriageApplicationDetailsPanel;
        }
        private Form getAddApplicationForm(SacramentType t)
        {
            if (t == SacramentType.Baptism)
                return new AddApplication(SacramentType.Baptism);
            else if (t == SacramentType.Confirmation)
                return new AddApplication(SacramentType.Confirmation);
            else
                return new AddMarriageApplication();
        }

        /// <summary>
        /// Loads all applications of given SacramentType
        /// </summary>
        /// <param name="t"></param>
        public void loadApplications(SacramentType t)
        {
            DataGridView dgv = getDataGridView(t);
            ComboBox filter = getFilter(t);
            dgv.AutoGenerateColumns = false;
            BindingSource bs = new BindingSource();
            bs.DataSource = dh.getApplications(t);
            dgv.DataSource = bs;
            dgv.ClearSelection();
            filter.SelectedIndex = 0;

        }

        private void showAddApplicationForm(SacramentType t)
        {
            Form f = getAddApplicationForm(t);
            f.ShowDialog();

            loadApplications(t);
        }

        private Form getApplicationForm(SacramentType t)
        {
            DataGridViewRow dgvr;
            if (t == SacramentType.Baptism)
            {
                dgvr = baptismApplication_dgv.SelectedRows[0];
                return new SacramentForm(SacramentType.Baptism, dgvr);
            }else if (t == SacramentType.Confirmation)
            {
                dgvr = confirmationApplication_dgv.SelectedRows[0];
                return new SacramentForm(SacramentType.Confirmation, dgvr);
            }else
            {
                dgvr = marriageApplication_dgv.SelectedRows[0];
                return new MarriageForm(dgvr);
            }
                
        }

        private void loadApplicationDetails(SacramentType t)
        {
            DataGridView dgv;
            LoadApplicationDetailsDelegate Load;
            if (t == SacramentType.Baptism)
            {
                dgv = baptismApplication_dgv;
                Load = loadBaptismApplicationDetails;

            }
            else if (t == SacramentType.Confirmation)
            {
                dgv = confirmationApplication_dgv;
                Load = loadConfirmationApplicationDetails;
            }
            else
            {
                dgv = marriageApplication_dgv;
                Load = loadMarriageApplicationDetails;
            }

            if (!dgv.Focused || dgv.SelectedRows.Count == 0)
                return;

            Load(dgv);
        }
        /*
                                         =============================================================
                                                ================ APPLICATION TAB =================
                                         =============================================================
        */

        #region Application

        

        /// <summary>
        /// Loads all baptism applications to Baptism DataGridView
        /// </summary>
        public void loadBaptismApplications()
        {
            baptismApplication_dgv.AutoGenerateColumns = false;
            BindingSource bs = new BindingSource();
            bs.DataSource = dh.getApplications(SacramentType.Baptism);
            baptismApplication_dgv.DataSource = bs;
            baptismApplication_filter_cmb.SelectedIndex = 0;
        }
        /// <summary>
        /// Loads all confirmation applications to Confirmation DataGridView
        /// </summary>
        public void loadConfirmationApplications()
        {
            confirmationApplication_dgv.AutoGenerateColumns = false;
            BindingSource bs = new BindingSource();
            bs.DataSource = dh.getApplications(SacramentType.Confirmation);
            confirmationApplication_dgv.DataSource = bs;
            confirmationApplication_filter_cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// Loads all marriage applications to Marriage DataGridView
        /// </summary>
        private void loadMarriageApplications()
        {
            marriageApplication_dgv.AutoGenerateColumns = false;
            BindingSource bs = new BindingSource();
            bs.DataSource = dh.getApplications(SacramentType.Marriage);
            marriageApplication_dgv.DataSource = bs;
            marriageApplication_filter_cmb.SelectedIndex = 0;
        }

        /// <summary>
        /// Loads details of the SelectedRow in DataGridView to the Baptism Details Panel
        /// </summary>
        /// <param name="dgv"></param>
        public void loadBaptismApplicationDetails(DataGridView dgv)
        {

            baptismApplicationDetailsPanel.Enabled = true;
            this.applicationID = int.Parse(dgv.SelectedRows[0].Cells[0].Value.ToString());
            this.profileID = int.Parse(dgv.SelectedRows[0].Cells[1].Value.ToString());
            string requirements = dgv.SelectedRows[0].Cells[2].Value.ToString();
            //MessageBox.Show(requirements);
            string fn = dgv.SelectedRows[0].Cells[3].Value.ToString();
            string mn = dgv.SelectedRows[0].Cells[4].Value.ToString();
            string ln = dgv.SelectedRows[0].Cells[5].Value.ToString();
            string suffix = dgv.SelectedRows[0].Cells[6].Value.ToString();
            string gender = dgv.SelectedRows[0].Cells[7].Value.ToString();
            DateTime birthdate = DateTime.ParseExact(dgv.SelectedRows[0].Cells[8].Value.ToString(), "yyyy-MM-dd", null);
            ApplicationStatus status = (ApplicationStatus)int.Parse(dgv.SelectedRows[0].Cells[9].Value.ToString());



            //Load into Edit Panel
            baptismApplication_firstName_textBox.Text = fn;
            baptismApplication_midName_textBox.Text = mn;
            baptismApplication_lastName_textBox.Text = ln;
            baptismApplication_suffix_textBox.Text = suffix;
            baptismApplication_birthDate_dtp.Value = birthdate;
            baptismApplication_male_radio.Checked = gender == "1";
            baptismApplication_female_radio.Checked = gender == "2";
            baptismApplication_status_label.Text = status.ToString();

            //Load into Display Panel
            baptismApplication_name_lbl.Text = string.Format("{0} {1} {2} {3}", fn, mn, ln, suffix);
            baptismApplication_gender_lbl.Text = gender == "1" ? "Male" : "Female";
            baptismApplication_birthdate_lbl.Text = birthdate.ToString("yyyy-MM-dd");

            tickRequirements(baptismApplication_requirements_tlp, requirements);
            DataTable dt = dh.getApplicationIncomeDetails(applicationID);

            double price = double.Parse(dt.Rows[0]["price"].ToString());

            double totalPayment = double.Parse(dt.Rows[0]["totalPayment"].ToString());
            baptismApplication_addPayment_btn.Enabled = (price - totalPayment) != 0;
            baptismApplication_payment_label.Text = (price - totalPayment).ToString("C");
            baptismApplication_payment_remarks.Text = dt.Rows[0]["remarks"].ToString();
        }

        /// <summary>
        /// Loads details of the SelectedRow in DataGridView to the Confirmation Details Panel
        /// </summary>
        /// <param name="dgv"></param>
        private void loadConfirmationApplicationDetails(DataGridView dgv)
        {
            confirmationApplicationDetailsPanel.Enabled = true;
            this.applicationID = int.Parse(dgv.SelectedRows[0].Cells[0].Value.ToString());
            this.profileID = int.Parse(dgv.SelectedRows[0].Cells[1].Value.ToString());
            int profileID = int.Parse(dgv.SelectedRows[0].Cells[1].Value.ToString());
            string requirements = dgv.SelectedRows[0].Cells[2].Value.ToString();
            string fn = dgv.SelectedRows[0].Cells[3].Value.ToString();
            string mn = dgv.SelectedRows[0].Cells[4].Value.ToString();
            string ln = dgv.SelectedRows[0].Cells[5].Value.ToString();
            string suffix = dgv.SelectedRows[0].Cells[6].Value.ToString();
            string gender = dgv.SelectedRows[0].Cells[7].Value.ToString();
            DateTime birthdate = DateTime.ParseExact(dgv.SelectedRows[0].Cells[8].Value.ToString(), "yyyy-MM-dd", null);
            ApplicationStatus status = (ApplicationStatus)int.Parse(dgv.SelectedRows[0].Cells[9].Value.ToString());

            confirmationApplication_name_lbl.Text = string.Format("{0} {1} {2} {3}", fn, mn, ln, suffix);
            confirmationApplication_gender_lbl.Text = gender == "1" ? "Male" : "Female";
            confirmationApplication_birthDate_lbl.Text = birthdate.ToString("yyyy-MM-dd");

            confirmationApplication_firstName_textBox.Text = fn;
            confirmationApplication_lastName_textBox.Text = mn;
            confirmationApplication_midName_textBox.Text = ln;
            confirmationApplication_suffix_textBox.Text = suffix;
            confirmationApplication_birthDate_dtp.Value = birthdate;
            confirmationApplication_male_radio.Checked = gender == "1";
            confirmationApplication_female_radio.Checked = gender == "2";
            confirmationApplication_status_label.Text = status.ToString();
            tickRequirements(confirmationApplication_requirements_tlp, requirements);


            DataTable dt = dh.getApplicationIncomeDetails(applicationID);

            double price = double.Parse(dt.Rows[0]["price"].ToString());

            double totalPayment = double.Parse(dt.Rows[0]["totalPayment"].ToString());
            confirmationApplication_addPayment_btn.Enabled = (price - totalPayment) != 0;
            confirmationApplication_payment_label.Text = (price - totalPayment).ToString("C");
            confirmationApplication_payment_remarks.Text = dt.Rows[0]["remarks"].ToString();
        }


        /// <summary>
        /// Loads details of the SelectedRow in DataGridView to the Marriage Details Panel
        /// </summary>
        /// <param name="dgv"></param>
        public void loadMarriageApplicationDetails(DataGridView dgv)
        {
            marriageApplicationDetailsPanel.Enabled = true;
            this.applicationID = int.Parse(dgv.SelectedRows[0].Cells[0].Value.ToString());

            this.groomID = int.Parse(dgv.SelectedRows[0].Cells[1].Value.ToString());
            this.brideID = this.profileID = int.Parse(dgv.SelectedRows[0].Cells[2].Value.ToString());

            string requirements = dgv.SelectedRows[0].Cells[3].Value.ToString();

            string[] GName = dgv.SelectedRows[0].Cells[4].Value.ToString().Split(new char[] { ' ' });
            txtGFN.Text = GName[0];
            txtGMI.Text = GName[1];
            txtGLN.Text = GName[2];
            txtGSuffix.Text = GName[3];

            dtpGBirthDate.Value = DateTime.ParseExact(dgv.SelectedRows[0].Cells[5].Value.ToString(), "yyyy-MM-dd", null);

            string[] BName = dgv.SelectedRows[0].Cells[6].Value.ToString().Split(new char[] { ' ' });
            txtBFN.Text = BName[0];
            txtBMI.Text = BName[1];
            txtBLN.Text = BName[2];
            txtBSuffix.Text = BName[3];

            dtpBBirthDate.Value = DateTime.ParseExact(dgv.SelectedRows[0].Cells[7].Value.ToString(), "yyyy-MM-dd", null);

            ApplicationStatus status = (ApplicationStatus)int.Parse(dgv.SelectedRows[0].Cells[8].Value.ToString());

            marriageApplication_groomName_lbl.Text = string.Format("{0} {1} {2} {3}", GName[0], GName[1], GName[2], GName[3]);
            marriageApplication_groomBirthdate_lbl.Text = dtpGBirthDate.Value.ToString("yyyy-MM-dd");
            marriageApplication_brideName_lbl.Text = string.Format("{0} {1} {2} {3}", BName[0], BName[1], BName[2], BName[3]);
            marriageApplication_brideBirthDate_lbl.Text = dtpBBirthDate.Value.ToString("yyyy-MM-dd");



            tickRequirements(marriageApplication_requirements_tlp, requirements);

            DataTable dt = dh.getApplicationIncomeDetails(applicationID);

            double price = double.Parse(dt.Rows[0]["price"].ToString());

            double totalPayment = double.Parse(dt.Rows[0]["totalPayment"].ToString());
            marriageApplication_addPayment_btn.Enabled = (price - totalPayment) != 0;
            marriageApplication_price_label.Text = (price - totalPayment).ToString("C");
            marriageApplication_payment_remarks.Text = dt.Rows[0]["remarks"].ToString();
        }

        /// <summary>
        /// Approves and opens a certain SacramentForm based on SacramentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dgv"></param>
        /// <param name="tlp"></param>
        private void applicationApprove(SacramentType type)
        {

            DataGridViewRow dgvr = getDataGridView(type).SelectedRows[0];
            TableLayoutPanel tlp = getRequirementTableLayoutPanel(type);
            DialogResult d = DialogResult.None;


            Form f = getApplicationForm(type);

            if (allRequirementsFulfilled(type))
            {
                d = f.ShowDialog();
            }
            else
            {
                DialogResult dr = MessageDialog.Show("Not all requirements were fulfilled. Proceed anyway?", "Warning", MessageDialogButtons.YesNo, MessageDialogIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    d = f.ShowDialog();
                }
            }

            if (d == DialogResult.OK)
            {
                SystemNotification.Notify(State.ApplicationApproveSuccess);
                loadApplications(type);
            }
        }

        /// <summary>
        /// Sets the status of an Application to Revoked
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dgv"></param>
        private void applicationRevoke(SacramentType type)
        {
            DataGridView dgv = getDataGridView(type);
            
            bool success = dh.editApplication(this.applicationID, ApplicationStatus.Revoked);

            if (success)
                SystemNotification.Notify(State.RevokeSucess);
            else
                SystemNotification.Notify(State.RevokeFail);

            loadApplications(type);
            
        }

        /// <summary>
        /// Applies Check property of CheckAll to the CheckBoxes in the TableLayout
        /// </summary>
        /// <param name="checkAll"></param>
        /// <param name="tlp"></param>
        private void checkAllCheckChanged(CheckBox checkAll, TableLayoutPanel tlp)
        {
            if (!checkAll.Focused)
                return;

            foreach (CheckBox c in tlp.Controls)
            {
                c.Checked = checkAll.Checked;
            }
        }

        /// <summary>
        /// Filters DataGridView on based on ComboBox SelectedIndex
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cb"></param>
        private void applicationFilter(SacramentType t)
        {
            DataGridView dgv = getDataGridView(t);
            ComboBox filter = getFilter(t);
            
            BindingSource bs = dgv.DataSource as BindingSource;
            DataTable dt = bs.DataSource as DataTable;

            if (filter.SelectedIndex == 0)
                dt.DefaultView.RowFilter = "";
            else
                dt.DefaultView.RowFilter = "status = " + filter.SelectedIndex;

            
            Control parent = dgv.Parent;

            Panel applicationDetailsPanel = getApplicationDetailsPanel(t);

            RecursiveClearControl(applicationDetailsPanel);

            dgv.ClearSelection();
            applicationDetailsPanel.Enabled = false;
        }

        /// <summary>
        /// Returns the requirement Check Box objects in the Panel as a series of 1 and 0's
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string getRequirements(SacramentType t)
        {

            TableLayoutPanel tlpReq = getRequirementTableLayoutPanel(t);

            string r = "";
            foreach (CheckBox c in tlpReq.Controls)
                r += c.Checked ? "1" : "0";

            return r;

        }
        /// <summary>
        /// Checks if all requirement CheckBoxes are Checked
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool allRequirementsFulfilled(SacramentType t)
        {
            TableLayoutPanel tlp = getRequirementTableLayoutPanel(t);
            bool fulfilled = true;
            foreach (CheckBox c in tlp.Controls)
            {
                fulfilled = fulfilled && c.Checked;
            }

            return fulfilled;
        }

        /// <summary>
        /// Changes the Check property of checkAll according to the Check state of TableLayoutPanel CheckBoxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="checkAll"></param>
        /// <param name="tlp"></param>
        private void requirementCheckChanged(CheckBox sender, CheckBox checkAll, TableLayoutPanel tlp)
        {
            if (!sender.Focused)
                return;

            bool allChecked = true;
            foreach (CheckBox c in tlp.Controls)
            {
                allChecked = allChecked && c.Checked;
            }

            checkAll.Checked = allChecked;
        }

        /// <summary>
        /// Loads the 1 and 0's of requirements into their respective CheckBox objects.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="requirements"></param>
        private void tickRequirements(Panel p, string requirements)
        {
            bool allChecked = true;
            foreach (CheckBox c in p.Controls)
            {
                int i = p.Controls.GetChildIndex(c);
                c.Checked = requirements[i] == '1';
                allChecked &= c.Checked;
            }

            baptismApplication_checkAll_checkBox.Checked = allChecked;
        }



        /// <summary>
        /// Saves changes of requirements to the database
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="tb"></param>
        /// <returns></returns>
        private bool editRequirements(SacramentType t)
        {
            DataGridView dgv = getDataGridView(t);
            string r = getRequirements(t);
            bool a = dh.editApplication(this.applicationID, r);

            return a;
        }

        /// <summary>
        /// Opens an ApplicationPayment Form based on SacramentType
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dgv"></param>
        private void payApplication(SacramentType type)
        {
            DataGridView dgv = getDataGridView(type);
            DataGridViewRow dgvr = dgv.SelectedRows[0];
            ApplicationPayment ap = new ApplicationPayment(type, dgvr, dh);
            DialogResult dr = ap.ShowDialog();
            Control parent = dgv.Parent;

            foreach (Control c in parent.Controls)
            {
                if (c.Tag != null && c.Tag.ToString() == "Details")
                {
                    RecursiveClearControl(c);
                    break;
                }
            }

            loadApplicationDetails(type);

        }

        /// <summary>
        /// Indicates whether the TextBox or CueTextBox controls in a Control has empty text or filler text
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool hasEmptyTextBoxes(Control c)
        {
            foreach (Control con in c.Controls)
            {
                if (con is CueTextBox)
                {
                    CueTextBox ctxt = con as CueTextBox;
                    if (ctxt.isEmpty())
                        return true;
                }
                else if (con is TextBox)
                {
                    TextBox t = con as TextBox;
                    if (string.IsNullOrWhiteSpace(t.Text))
                        return true;
                }
            }

            return false;
        }

        
        public bool editApplicationProfile(SacramentType type)
        {

            bool success = true;
            
            if (type == SacramentType.Baptism)
            {
                if (hasEmptyTextBoxes(baptismApplication_profile_tlp))
                {
                    SystemNotification.Notify(State.MissingFields);
                    return false;
                }

                string fn = baptismApplication_firstName_textBox.Text;
                string mi = baptismApplication_midName_textBox.Text;
                string ln = baptismApplication_lastName_textBox.Text;
                string suffix = baptismApplication_suffix_textBox.Text;
                Gender g = baptismApplication_male_radio.Checked ? Gender.Male : Gender.Female;
                DateTime birthDate = baptismApplication_birthDate_dtp.Value;
                

                bool profileExists = dh.generalProfileExists(profileID, fn, mi, ln, suffix, g, birthDate);
                //MessageBox.Show("Profile Exists: " + profileExists);
                if (profileExists)
                {
                    SystemNotification.Notify(State.ProfileExists);
                    return false;
                }

                success &= dh.editApplication(this.applicationID, getRequirements(type));
                success &= dh.editGeneralProfile(this.profileID, fn, mi, ln, suffix, g, birthDate);
                
            }
            else if (type == SacramentType.Confirmation)
            {
                if (hasEmptyTextBoxes(confirmationApplication_profile_tlp))
                {
                    SystemNotification.Notify(State.MissingFields);
                    return false;
                }

                string fn = confirmationApplication_firstName_textBox.Text;
                string mi = confirmationApplication_lastName_textBox.Text;
                string ln = confirmationApplication_midName_textBox.Text;
                string suffix = confirmationApplication_suffix_textBox.Text;
                Gender g = confirmationApplication_male_radio.Checked ? Gender.Male : Gender.Female;
                DateTime birthDate = confirmationApplication_birthDate_dtp.Value;

                bool profileExists = dh.generalProfileExists(this.profileID, fn, mi, ln, suffix, g, birthDate);
                if (profileExists)
                {
                    SystemNotification.Notify(State.ProfileExists);
                    return false;
                }

                success &= dh.editApplication(this.applicationID, getRequirements(type));
                success &= dh.editGeneralProfile(this.profileID, fn, mi, ln, suffix, g, birthDate);
                
            }
            else //Marriage
            {

                if (hasEmptyTextBoxes(marriageApplication_profile_tlp))
                {
                    SystemNotification.Notify(State.MissingFields);
                    return false;
                }


                string gfn = txtGFN.Text;
                string gmi = txtGMI.Text;
                string gln = txtGLN.Text;
                string gsuffix = txtGSuffix.Text;
                DateTime gbd = dtpGBirthDate.Value;
                bool groomExists = dh.generalProfileExists(this.groomID, gfn, gmi, gln, gsuffix, Gender.Male, gbd);

                if (groomExists)
                {
                    SystemNotification.Notify(State.GroomExists);
                    return false;
                }

                string bfn = txtBFN.Text;
                string bmi = txtBMI.Text;
                string bln = txtBLN.Text;
                string bsuffix = txtBSuffix.Text;
                DateTime bbd = dtpBBirthDate.Value;

                bool brideExists = dh.generalProfileExists(this.brideID, bfn, bmi, bln, bsuffix, Gender.Female, bbd);
                if (brideExists)
                {
                    SystemNotification.Notify(State.BrideExists);
                    return false;
                }

                success &= dh.editApplication(this.applicationID, getRequirements(type));
                success &= dh.editGeneralProfile(this.groomID, gfn, gmi, gln, gsuffix, Gender.Male, gbd);
                success &= dh.editGeneralProfile(this.brideID, bfn, bmi, bln, bsuffix, Gender.Female, bbd);
            }

            if (success)
            {
                Notification.Show("Successfully applied changes", NotificationType.success);
                loadApplications(type);
                Panel p = getApplicationDetailsPanel(type);
                RecursiveClearControl(p);

            }
            else
            {
                Notification.Show("Something went wrong", NotificationType.warning);
            }

            return success;
        }


        // DGV CELL FORMATTING ==================================================================================================
        private void baptismApplication_dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {//Might be really slow!
            if (e.ColumnIndex == 7)//Gender
                e.Value = e.Value.ToString() == "1" ? "M" : "F";
            else if (e.ColumnIndex == 9)
            {
                switch (e.Value.ToString())
                {
                    case "1":
                        e.Value = "P";
                        break;
                    case "2":
                        e.Value = "A";
                        break;
                    case "3":
                        e.Value = "F";
                        break;
                    case "4":
                        e.Value = "R";
                        break;
                }
            }
        }

        private void confirmationApplication_dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            baptismApplication_dgv_CellFormatting(sender, e);
        }

        private void marriageApplication_dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 8)
            {
                switch (e.Value.ToString())
                {
                    case "1":
                        e.Value = "P";
                        break;
                    case "2":
                        e.Value = "A";
                        break;
                    case "3":
                        e.Value = "F";
                        break;
                    case "4":
                        e.Value = "R";
                        break;
                }
            }
        }
        //=====================================================================================================================





        // APPROVE BUTTON CLICK ===============================================================================================

        private void confirmationApplication_approve_btn_Click(object sender, EventArgs e)
        {
            applicationApprove(SacramentType.Confirmation);
        }

        private void marriageApplication_approve_btn_Click(object sender, EventArgs e)
        {
            applicationApprove(SacramentType.Marriage);
        }

        private void baptismApplication_approve_button_Click(object sender, EventArgs e)
        {
            applicationApprove(SacramentType.Baptism);
        }
        //=====================================================================================================================




        // APPLICATION CHECKALL CHECK CHANGED =================================================================================

        private void baptismApplication_checkAll_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            checkAllCheckChanged(baptismApplication_checkAll_checkBox, baptismApplication_requirements_tlp);
        }

        private void confirmationApplication_checkAll_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            checkAllCheckChanged(confirmationApplication_checkAll_checkBox, confirmationApplication_requirements_tlp);
        }

        private void marriageApplication_checkAll_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            checkAllCheckChanged(marriageApplication_checkAll_checkBox, marriageApplication_requirements_tlp);
        }
        //=====================================================================================================================





        // APPLICATION REQUIREMENT CHECK CHANGED =================================================================================
        private void baptismApplication_requirement_checkBox_CheckedChanged(object sender, EventArgs e)
        {

            requirementCheckChanged((CheckBox)sender, baptismApplication_checkAll_checkBox, baptismApplication_requirements_tlp);

        }

        private void confirmationApplication_requirement_checkBox_CheckChanged(object sender, EventArgs e)
        {
            requirementCheckChanged((CheckBox)sender, confirmationApplication_checkAll_checkBox, confirmationApplication_requirements_tlp);
        }

        private void marriage_requirement_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            requirementCheckChanged((CheckBox)sender, marriageApplication_checkAll_checkBox, marriageApplication_requirements_tlp);
        }

        //=====================================================================================================================





        // APPLICATION REVOKE BUTTON CLICK=====================================================================================
        private void baptismApplication_revoke_button_Click(object sender, EventArgs e)
        {
            applicationRevoke(SacramentType.Baptism);
        }

        private void confirmationApplication_revoke_btn_Click(object sender, EventArgs e)
        {
            applicationRevoke(SacramentType.Confirmation);
        }


        private void marriageApplication_revoke_btn_Click(object sender, EventArgs e)
        {
            applicationRevoke(SacramentType.Marriage);
        }

        //======================================================================================================================







        #endregion

        // APPLICATION ADD BUTTON CLICK=====================================================================================

        

        private void baptismApplication_add_button_Click(object sender, EventArgs e)
        {
            Form f = getAddApplicationForm(SacramentType.Baptism);
            f.ShowDialog();

            loadApplications(SacramentType.Baptism);
            
        }

        private void confirmationApplication_add_btn_Click(object sender, EventArgs e)
        {
            Form f = getAddApplicationForm(SacramentType.Confirmation);
            f.ShowDialog();

            loadApplications(SacramentType.Confirmation);

        }

        private void marriageApplication_add_button_Click(object sender, EventArgs e)
        {
            Form f = getAddApplicationForm(SacramentType.Marriage);
            f.ShowDialog();

            loadApplications(SacramentType.Marriage);

        }
        //======================================================================================================================





        // APPLICATION ADD PAYMENT BUTTON CLICK=================================================================================
        private void baptismApplication_addPayment_button_Click(object sender, EventArgs e)
        {
            payApplication(SacramentType.Baptism);
        }

        private void confirmationApplication_addPayment_btn_Click(object sender, EventArgs e)
        {
            payApplication(SacramentType.Confirmation);
        }


        private void marriageApplication_addPayment_btn_Click(object sender, EventArgs e)
        {
            payApplication(SacramentType.Marriage);
        }
        //======================================================================================================================






        //APPLICATION DGV VISIBLE CHANGED========================================================================================
        private void baptismApplication_dgv_VisibleChanged(object sender, EventArgs e)
        {
            loadApplications(SacramentType.Baptism);
            
        }

        private void confirmationApplication_dgv_VisibleChanged(object sender, EventArgs e)
        {
            loadApplications(SacramentType.Confirmation);
        }

        private void marriageApplication_dgv_VisibleChanged(object sender, EventArgs e)
        {
            loadApplications(SacramentType.Marriage);
        }
        //========================================================================================================================


        //APPLICATION FILTER COMBOBOX SELECTED INDEX CHANGED======================================================================
        private void baptismApplication_filter_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if(c.Focused)
                applicationFilter(SacramentType.Baptism);
        }

        private void confirmationApplication_filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c.Focused)
                applicationFilter(SacramentType.Confirmation);
        }

        private void marriageApplication_filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c.Focused)
                applicationFilter(SacramentType.Marriage);
        }
        //========================================================================================================================



        // APPLICATION EDIT CHECK CHANGED=======================================================================================
        private void baptismApplication_edit_check_CheckedChanged(object sender, EventArgs e)
        {
            applicationEditCheckChanged(SacramentType.Baptism);
        }
        private void confirmationApplication_edit_check_CheckedChanged(object sender, EventArgs e)
        {
            applicationEditCheckChanged(SacramentType.Confirmation);
        }

        private void marriageApplication_edit_check_CheckedChanged(object sender, EventArgs e)
        {
            applicationEditCheckChanged(SacramentType.Marriage);
        }

        //=======================================================================================================================

        

        

        //APPLICATION DGV CELL ENTER==============================================================================================
        private void baptismApplication_dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            loadApplicationDetails(SacramentType.Baptism);
        }

        private void confirmationApplication_dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            loadApplicationDetails(SacramentType.Confirmation);
        }

        private void marriageApplication_dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            loadApplicationDetails(SacramentType.Marriage);
        }

        //=========================================================================================================================

        /// <summary>
        /// Recursively clears the Controls inside a Control
        /// </summary>
        /// <param name="c"></param>
        private void RecursiveClearControl(Control c)
        {
            Console.WriteLine("ENTERED RECURSIVE CLEAR");
            if (c.Controls.Count == 0)
                return;
            else
            {
                foreach (Control con in c.Controls)
                {

                    if (con is TextBox)
                    {
                        TextBox a = con as TextBox;
                        a.Text = "";
                    }
                    else if (con is ComboBox)
                    {
                        ComboBox a = con as ComboBox;
                        a.SelectedIndex = 0;
                    }
                    else if (con is DateTimePicker)
                    {
                        DateTimePicker a = con as DateTimePicker;
                        a.Value = DateTime.Now.Date;
                    }
                    else if (con is Button)
                    {
                        Button a = con as Button;

                    }
                    else if (con is RadioButton)
                    {
                        RadioButton a = con as RadioButton;
                        a.Checked = false;
                    }
                    else if (con is CheckBox)
                    {
                        CheckBox a = con as CheckBox;
                        a.Checked = false;
                    }
                    else if (con.Tag != null && con.Tag.ToString() == "detail" && con is Label)
                    {
                        Console.WriteLine("ENTERED LABEL");
                        Label a = con as Label;
                        a.Text = "";
                    }
                    else if (con.Controls.Count > 0)
                    {
                        RecursiveClearControl(con);
                    }
                }
            }
        }

        

        private void applicationTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (applicationTabControl.SelectedIndex == 0)
            {
                RecursiveClearControl(baptismApplicationDetailsPanel);
                baptismApplicationDetailsPanel.Enabled = false;
            }
            else if (applicationTabControl.SelectedIndex == 0)
            {
                RecursiveClearControl(confirmationApplicationDetailsPanel);
                confirmationApplicationDetailsPanel.Enabled = false;
            }
            else
            {
                RecursiveClearControl(marriageApplicationDetailsPanel);
                marriageApplicationDetailsPanel.Enabled = false;
            }
        }

        private void applicationEditCheckChanged(SacramentType type)
        {


            TableLayoutPanel tlpProfile;
            GroupBox gbReq;
            CheckBox checkEdit;
            if (type == SacramentType.Baptism)
            {
                tlpProfile = baptismApplication_profile_tlp;
                gbReq = baptismApplication_requirements_groupbox;
                checkEdit = baptismApplication_edit_check;

            }
            else if (type == SacramentType.Confirmation)
            {
                tlpProfile = confirmationApplication_profile_tlp;
                gbReq = confirmationApplication_requirements_groupbox;
                checkEdit = confirmationApplication_edit_check;
            }
            else
            {
                tlpProfile = marriageApplication_profile_tlp;
                gbReq = marriageApplication_requirements_groupbox;
                checkEdit = marriageApplication_edit_check;
            }


            tlpProfile.Visible = checkEdit.Checked;
            gbReq.Enabled = checkEdit.Checked;

            if (!checkEdit.Checked)
            {
                checkEdit.Text = "Edit";
                editApplicationProfile(type);
            }
            else
            {
                checkEdit.Text = "Save";
            }


        }
    }
}