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
        public Person(int ProfileID,DataHandler dh)
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
				bloodtype_combobox_baptist.SelectedValue.ToString());
		}
		#endregion

		#region Loads
		private void load_Biodata() {

			DataTable TempDT = dh.getGeneralProfile(ProfileID);
			//LOAD PERSON
			if (!(TempDT.Rows[0]["gender"].Equals(null)))
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
			contactNumber_textbox_baptist.Text= TempDT.Rows[0]["contactNumber"].ToString();
			bloodtype_combobox_baptist.SelectedIndex = int.Parse(TempDT.Rows[0]["bloodType"].ToString());
			Console.WriteLine(TempDT.Rows[0]["birthdate"].ToString());
			
			//==================================================================================================================insert birthdate
			//LOAD FATHER
			DataTable TempFatherDT = dh.getParentsOf(ProfileID);
			//==================================================================================================================change get parents of function
			firstname_textbox_father.Text = TempFatherDT.Rows[0]["firstname"].ToString();
			middlename_textbox_father.Text = TempFatherDT.Rows[0]["midname"].ToString();
			lastname_textbox_father.Text = TempFatherDT.Rows[0]["lastname"].ToString();
			suffix_textbox_father.Text = TempFatherDT.Rows[0]["suffix"].ToString();
			birthplace_textbox_father.Text = TempFatherDT.Rows[0]["birthplace"].ToString();
			residence_textbox_father.Text= TempFatherDT.Rows[0]["residence"].ToString();

			//LOAD MOTHER
			DataTable TempMotherDT = dh.getParentsOf(ProfileID);
			//==================================================================================================================change get parents of function
			firstname_textbox_mother.Text = TempMotherDT.Rows[0]["firstname"].ToString();
			middlename_textbox_mother.Text = TempMotherDT.Rows[0]["midname"].ToString();
			lastname_textbox_mother.Text = TempMotherDT.Rows[0]["lastname"].ToString();
			suffix_textbox_mother.Text = TempMotherDT.Rows[0]["suffix"].ToString();
			birthplace_textbox_mother.Text = TempMotherDT.Rows[0]["birthplace"].ToString();
			residence_textbox_mother.Text = TempMotherDT.Rows[0]["residence"].ToString();
			
		}


		#endregion

		private void biodata_button_Click(object sender, EventArgs e)
		{
            load_Biodata();
            
		}

		private void baptism_button_Click(object sender, EventArgs e)
		{
		
		}

        private void basic_panel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
