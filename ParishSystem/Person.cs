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
    public partial class Person : Form
    {
        public int ProfileID;
        public DataHandler dh;
        public Person(int ProfileID, DataHandler dh)
        {
            InitializeComponent();
            this.ProfileID = ProfileID;
            this.dh = dh;

        }
        #region GUI basic


        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Saves
        private void saveGeneralProfile(object sender, EventArgs e)
        {
            char gender = 'z';
            if (gender_radiobutton1_baptist.Checked == true) { gender = 'm'; }
            else if (gender_radiobutton2_baptist.Checked == true) { gender = 'f'; }
            dh.editGeneralProfile(
                ProfileID,
                firstname_textbox.Text,
                middlename_textbox.Text,
                lastname_textbox.Text,
                suffix_textbox.Text,
                gender,
                birthdate_datetimepicker_baptist.Value,
                contactNumber_textbox_baptist.Text,
                address_baptist_textarea.Text,
                birthplace_textbox_baptist.Text,
                bloodtype_combobox_baptist.Text
                );


            if (father_checkbox.Checked == true)
            {
                try
                {

                    int fatherID = int.Parse(dh.getFatherOf(ProfileID).Rows[0]["parentID"].ToString());
                    dh.editParent(fatherID, firstname_textbox_father.Text, middlename_textbox_father.Text, lastname_textbox_father.Text, suffix_textbox_father.Text, 'm', birthplace_textbox_father.Text);
                }
                catch
                {
                    dh.conn.Close();
                    dh.addParent(ProfileID, firstname_textbox_father.Text, middlename_textbox_father.Text, lastname_textbox_father.Text, suffix_textbox_father.Text, 'm', birthplace_textbox_father.Text);
                }
            }

            if (mother_checkbox.Checked == true)
            {
                try
                {
                    int motherID = int.Parse(dh.getMotherOf(ProfileID).Rows[0]["parentID"].ToString());
                    dh.editParent(motherID, firstname_textbox_father.Text, middlename_textbox_father.Text, lastname_textbox_father.Text, suffix_textbox_father.Text, 'f', birthplace_textbox_father.Text);
                }
                catch
                {
                    dh.conn.Close();
                    dh.addParent(ProfileID, firstname_textbox_father.Text, middlename_textbox_father.Text, lastname_textbox_father.Text, suffix_textbox_father.Text, 'f', birthplace_textbox_father.Text);
                }
            }
        }

        #endregion

        #region Loads
        private void load_Biodata()
        {

            DataTable TempDT = dh.getGeneralProfile(ProfileID);
            //LOAD PERSON
            if (!(TempDT.Rows[0]["gender"] == null))
            {
                if (TempDT.Rows[0]["gender"].ToString() == "m")
                    gender_radiobutton1_baptist.Checked = true;
                else if (TempDT.Rows[0]["gender"].ToString() == "f")
                    gender_radiobutton2_baptist.Checked = true;
            }
            firstname_textbox.Text = TempDT.Rows[0]["firstname"].ToString();
            middlename_textbox.Text = TempDT.Rows[0]["midname"].ToString();
            lastname_textbox.Text = TempDT.Rows[0]["lastname"].ToString();
            suffix_textbox.Text = TempDT.Rows[0]["suffix"].ToString();
            birthplace_textbox_baptist.Text = TempDT.Rows[0]["birthplace"].ToString();
            address_baptist_textarea.Text = TempDT.Rows[0]["address"].ToString();
            contactNumber_textbox_baptist.Text = TempDT.Rows[0]["contactNumber"].ToString();
            bloodtype_combobox_baptist.SelectedItem = TempDT.Rows[0]["bloodType"].ToString();
            try
            {
                birthdate_datetimepicker_baptist.Format = DateTimePickerFormat.Short;
                birthdate_datetimepicker_baptist.Value = dh.toDateTime(TempDT.Rows[0]["birthdate"].ToString(), false);
            }
            catch
            {
                birthdate_datetimepicker_baptist.Format = DateTimePickerFormat.Custom;
            }
            //Console.WriteLine(TempDT.Rows[0]["birthdate"].ToString());


            //LOAD FATHER
            if (dh.getFatherOf(ProfileID).Rows.Count != 0)
            {
                DataTable TempFatherDT = dh.getFatherOf(ProfileID);
                firstname_textbox_father.Text = TempFatherDT.Rows[0]["firstname"].ToString();
                middlename_textbox_father.Text = TempFatherDT.Rows[0]["midname"].ToString();
                lastname_textbox_father.Text = TempFatherDT.Rows[0]["lastname"].ToString();
                suffix_textbox_father.Text = TempFatherDT.Rows[0]["suffix"].ToString();
                birthplace_textbox_father.Text = TempFatherDT.Rows[0]["birthplace"].ToString();
                residence_textbox_father.Text = TempFatherDT.Rows[0]["residence"].ToString();
                father_checkbox.Checked = true;
                father_checkbox.Enabled = false;

            }

            //LOAD MOTHER
            if (dh.getFatherOf(ProfileID).Rows.Count != 0)
            {
                DataTable TempMotherDT = dh.getMotherOf(ProfileID);
                firstname_textbox_mother.Text = TempMotherDT.Rows[0]["firstname"].ToString();
                middlename_textbox_mother.Text = TempMotherDT.Rows[0]["midname"].ToString();
                lastname_textbox_mother.Text = TempMotherDT.Rows[0]["lastname"].ToString();
                suffix_textbox_mother.Text = TempMotherDT.Rows[0]["suffix"].ToString();
                birthplace_textbox_mother.Text = TempMotherDT.Rows[0]["birthplace"].ToString();
                residence_textbox_mother.Text = TempMotherDT.Rows[0]["residence"].ToString();
                mother_checkbox.Checked = true;
                mother_checkbox.Enabled = false;

            }
        }




        public void load_Baptism()
        {
            DataTable dt = dh.getConfirmationOf(ProfileID);

            int ministerID = int.Parse(dt.Rows[0]["ministerID"].ToString());
            int applicationID = int.Parse(dt.Rows[0]["applicationID"].ToString());
            int confirmationID = int.Parse(dt.Rows[0]["confirmationID"].ToString());

            registrynumber_confirmation_textbox.Text = dt.Rows[0]["registryNumber"].ToString();
            pagenumber_confirmation_textbox.Text = dt.Rows[0]["pageNumber"].ToString();
            recordnumber_confirmation_textbox.Text = dt.Rows[0]["recordNumber"].ToString();
            Console.WriteLine("Date: " + dt.Rows[0]["confirmationDate"].ToString());
            confirmationdate_confirmation_dtp.Value = dh.toDateTime(dt.Rows[0]["confirmationDate"].ToString(), false);
            remarks_sponsor_confirmation_textbox.Text = dt.Rows[0]["remarks"].ToString();

            MessageBox.Show("loading");
            dt = dh.getMinister(ministerID);

            minister_confirmation_textbox.Text = dt.Rows[0]["lastName"].ToString() + ", "
                + dt.Rows[0]["firstName"].ToString() + " " + dt.Rows[0]["midName"].ToString()
                + " " + dt.Rows[0]["suffix"].ToString();

            dt = dh.getSponsors(confirmationID, "C");

            firstname_sponsor_confirmation_textbox.Text = dt.Rows[0]["firstName"].ToString();
            middlename_sponsor_confirmation_textbox.Text = dt.Rows[0]["midName"].ToString();
            lastname_sponsor_confirmation_textbox.Text = dt.Rows[0]["lastName"].ToString();
            suffix_sponsor_confirmation_textbox.Text = dt.Rows[0]["suffix"].ToString();
            residence_sponsor_confirmation_textbox.Text = dt.Rows[0]["residence"].ToString();


            string gender = dt.Rows[0]["gender"].ToString();

            male_sponsor_confirmation_radio.Checked = gender == "M";
            female_sponsor_confirmation_radio.Checked = gender == "F";


        }

      

        public void load_Confirmation()
        {
            DataTable dt = dh.getConfirmationOf(ProfileID);

            int ministerID = int.Parse(dt.Rows[0]["ministerID"].ToString());
            int applicationID = int.Parse(dt.Rows[0]["applicationID"].ToString());
            int confirmationID = int.Parse(dt.Rows[0]["confirmationID"].ToString());

            registrynumber_confirmation_textbox.Text = dt.Rows[0]["registryNumber"].ToString();
            pagenumber_confirmation_textbox.Text = dt.Rows[0]["pageNumber"].ToString();
            recordnumber_confirmation_textbox.Text = dt.Rows[0]["recordNumber"].ToString();
            Console.WriteLine("Date: " + dt.Rows[0]["confirmationDate"].ToString());
            confirmationdate_confirmation_dtp.Value = dh.toDateTime(dt.Rows[0]["confirmationDate"].ToString(), false);
            remarks_sponsor_confirmation_textbox.Text = dt.Rows[0]["remarks"].ToString();

            MessageBox.Show("loading");
            dt = dh.getMinister(ministerID);

            minister_confirmation_textbox.Text = dt.Rows[0]["lastName"].ToString() + ", "
                + dt.Rows[0]["firstName"].ToString() + " " + dt.Rows[0]["midName"].ToString()
                + " " + dt.Rows[0]["suffix"].ToString();

            dt = dh.getSponsors(confirmationID, "C");

            firstname_sponsor_confirmation_textbox.Text = dt.Rows[0]["firstName"].ToString();
            middlename_sponsor_confirmation_textbox.Text = dt.Rows[0]["midName"].ToString();
            lastname_sponsor_confirmation_textbox.Text = dt.Rows[0]["lastName"].ToString();
            suffix_sponsor_confirmation_textbox.Text = dt.Rows[0]["suffix"].ToString();
            residence_sponsor_confirmation_textbox.Text = dt.Rows[0]["residence"].ToString();
            

            string gender = dt.Rows[0]["gender"].ToString();

            male_sponsor_confirmation_radio.Checked = gender == "M";
            female_sponsor_confirmation_radio.Checked = gender == "F";
        }

        public void load_Marriage()
        {

        }


        #endregion

        #region DGV clicks
        int sponsorBaptismLastClick;

        private void baptism_sponsor_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sponsorBaptismLastClick = int.Parse(baptism_sponsor_dgv.CurrentRow.Cells["sponsorID"].Value.ToString());
            firstname_textbox_sponsor_baptism.Text = baptism_sponsor_dgv.CurrentRow.Cells["firstName"].Value.ToString();
            middlename_textbox_sponsor_baptism.Text = baptism_sponsor_dgv.CurrentRow.Cells["midName"].Value.ToString();
            lastname_textbox_sponsor_baptism.Text = baptism_sponsor_dgv.CurrentRow.Cells["lastName"].Value.ToString();
            suffix_textbox_sponsor_baptism.Text = baptism_sponsor_dgv.CurrentRow.Cells["suffix"].Value.ToString();
            residence_textbox_sponsor_baptism.Text = baptism_sponsor_dgv.CurrentRow.Cells["residence"].Value.ToString();

            if (baptism_sponsor_dgv.CurrentRow.Cells["gender"].Value != null)
            {
                if (baptism_sponsor_dgv.CurrentRow.Cells["gender"].Value.ToString().Equals("m"))
                {
                    genderM_radiobutton_sponsor_baptism.Checked = true;
                }
                else if (baptism_sponsor_dgv.CurrentRow.Cells["gender"].Value.ToString().Equals("f"))
                {
                    genderF_radiobutton_sponsor_baptism.Checked = false;
                }
            }
        }
        #endregion

        #region Menu_Clicks
       
       
        private void biodata_button_Click(object sender, EventArgs e)
		{
            load_Biodata();
           
            profile_panel.Visible = true;
            baptism_panel.Visible = false;
            confirmation_panel.Visible = false;
            marriage_panel.Visible = false;
            balance_panel.Visible = false;
            bloodletting_panel.Visible = false;
            
		}

		private void baptism_button_Click(object sender, EventArgs e)
		{
            if (dh.hasBaptismApplication(ProfileID))
            {
                load_Baptism();
                profile_panel.Visible = false;
                baptism_panel.Visible = true;
                confirmation_panel.Visible = false;
                marriage_panel.Visible = false;
                balance_panel.Visible = false;
                bloodletting_panel.Visible = false;
            }
            else
            {
                dh.addApplication(ProfileID, "B");      
            }

        }


        private void confirmation_button_Click(object sender, EventArgs e)
        {

            
            if (dh.hasConfirmationApplication(ProfileID))
            {
                load_Confirmation();
                profile_panel.Visible = false;
                baptism_panel.Visible = false;
                confirmation_panel.Visible = true;
                marriage_panel.Visible = false;
                balance_panel.Visible = false;
                bloodletting_panel.Visible = false;
            }
            else
            {
                //dh.addApplication(ProfileID, "C");
            }
        }

        private void marriage_button_Click(object sender, EventArgs e)
        {
            if (dh.hasMarriageApplication(ProfileID))
            {
                load_Marriage();
                profile_panel.Visible = false;
                baptism_panel.Visible = false;
                confirmation_panel.Visible = false;
                marriage_panel.Visible = true;
                balance_panel.Visible = false;
                bloodletting_panel.Visible = false;
            }
            else
            {
                dh.addApplication(ProfileID, "M");
            }
        }


        #endregion


        private void Person_Load(object sender, EventArgs e)
        {
           // if (dh.hasBaptismApplication(ProfileID)) { baptism_button.BackColor = Color.Green; } else { baptism_button.BackColor = Color.Red; }
            //if (dh.hasMarriageApplication(ProfileID)) { marriage_button.BackColor = Color.Green; } else { marriage_button.BackColor = Color.Red; }
            //if (dh.hasConfirmationApplication(ProfileID)) { confirmation_button.BackColor = Color.Green; } else { confirmation_button.BackColor = Color.Red; }

        }

        private void birthdate_datetimepicker_baptist_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            try {
                birthdate_datetimepicker_baptist.Format = DateTimePickerFormat.Short;     
            }
            catch
            {
                birthdate_datetimepicker_baptist.Format = DateTimePickerFormat.Custom;
            }
        }

        private void add_button_sponsor_baptism_Click(object sender, EventArgs e)
        {
            if (add_button_sponsor_baptism.Text.Equals("Add"))
            {
                int baptismID = dh.getBaptismID(ProfileID);
                char gender = '0';
                if (genderM_radiobutton_sponsor_baptism.Checked)
                {
                    gender = 'm';
                }
                else if (genderF_radiobutton_sponsor_baptism.Checked)
                {
                    gender = 'f';
                }
                dh.addSponsor(firstname_textbox_sponsor_baptism.Text, middlename_textbox_sponsor_baptism.Text, lastname_textbox_sponsor_baptism.Text, suffix_textbox_sponsor_baptism.Text, "B", residence_textbox_sponsor_baptism.Text, gender);
            }
            else
            {
                int baptismID = dh.getBaptismID(ProfileID);
                char gender = '0';
                if (genderM_radiobutton_sponsor_baptism.Checked)
                {
                    gender = 'm';
                }
                else if (genderF_radiobutton_sponsor_baptism.Checked)
                {
                    gender = 'f';
                }
                dh.editSponsor(sponsorBaptismLastClick,firstname_textbox_sponsor_baptism.Text, middlename_textbox_sponsor_baptism.Text, lastname_textbox_sponsor_baptism.Text, suffix_textbox_sponsor_baptism.Text, "bap", residence_textbox_sponsor_baptism.Text, gender);
            }
           
        }

        private void registry_baptism_textbox_TextChanged(object sender, EventArgs e)
        {
           // dh.addBaptism(int.Parse(dh.getApplications(ProfileID, "B").Rows[0]["applicationID"].ToString()), minister_baptism_combobox.SelectedIndex,dh.toDateTime(baptism_date_dtp.Value.ToString(),false));
        }

        private void delete_button_sponsor_baptism_Click(object sender, EventArgs e)
        {

        }

        Color BackColorOnClick = Color.FromArgb(255, 255, 255);
        Color ForeColorOnClick = Color.FromArgb(21, 40, 54);

        private void approve_baptism_button_Click(object sender, EventArgs e)
        {
            baptism_information_panel.Enabled = true;
        }

        
        private void menu_button_Enter(object sender, EventArgs e)
        {
            Button a = sender as Button;
            a.Font = new Font(biodata_button.Font, FontStyle.Bold);
            a.ForeColor = ForeColorOnClick;
            a.BackColor = BackColorOnClick;
        }

        private void menu_button_Leave(object sender, EventArgs e)
        {
            Button a = sender as Button;
            a.Font = new Font(biodata_button.Font, FontStyle.Regular);
            a.ForeColor = BackColorOnClick;
            a.BackColor = ForeColorOnClick;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void confirmation_viewCertificate_button_Click(object sender, EventArgs e)
        {

        }
    }
}
