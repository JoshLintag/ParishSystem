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
    public partial class ItemTypePopUp : Form
    {
        int IncomeTypeID;
        int cashreceipt_cashdisbursment;
        DataHandler dh;
        public ItemTypePopUp(int cashreceipt_cashdisbursment, DataHandler dh)
        {
            InitializeComponent();
            this.dh = dh;
            this.cashreceipt_cashdisbursment = cashreceipt_cashdisbursment;
            suggestedPrice_nud.Maximum = decimal.MaxValue;
        }
        public ItemTypePopUp(int IncomeTypeID, int cashreceipt_cashdisbursment, DataHandler dh)
        {
            InitializeComponent();
            this.dh = dh;
            this.IncomeTypeID = IncomeTypeID;
            this.cashreceipt_cashdisbursment = cashreceipt_cashdisbursment;
            suggestedPrice_nud.Maximum = decimal.MaxValue;
        }

        private void save_button_Click(object sender, EventArgs e)
        {      
                if (name_textbox.Text.Trim() == "" || book_combobox.Text == "")
                {
                Notification.Show(State.MissingFields);
                }
                else
                {
                    if (IncomeTypeID == 0)
                    {
                        dh.addItemType(name_textbox.Text, book_combobox.SelectedIndex, suggestedPrice_nud.Value, (active_button.Checked ? 1 : 2), cashreceipt_cashdisbursment, details_textbox.Text);
                        IncomeTypeID = dh.getMaxIncomeType();
                    }
                    else
                    {
                        dh.editIncomeType(IncomeTypeID, name_textbox.Text, book_combobox.SelectedIndex, suggestedPrice_nud.Value, (active_button.Checked ? 1 : 2), cashreceipt_cashdisbursment, details_textbox.Text);
                    }
                this.Close();
                }
       }
           
        public void refreshIncomeType()
        {
            if (IncomeTypeID != 0)
            {
                DataTable dt = dh.getItem(IncomeTypeID, cashreceipt_cashdisbursment);
                name_textbox.Text = dt.Rows[0]["itemType"].ToString();
                book_combobox.SelectedIndex = int.Parse(dt.Rows[0]["bookType"].ToString());
                suggestedPrice_nud.Value = decimal.Parse(dt.Rows[0]["suggestedPrice"].ToString());
                details_textbox.Text = dt.Rows[0]["details"].ToString();
                active_button.Checked = dt.Rows[0]["status"].ToString()=="1" ?  true : false;
            }
            else
            {
                name_textbox.Text="";
                book_combobox.SelectedIndex=0;
                suggestedPrice_nud.Value = 0;
                details_textbox.Clear();
                active_button.Checked = true;
            }
        }
        

        private void IncomeType_Load(object sender, EventArgs e)
        {
            refreshIncomeType();
            Draggable a = new Draggable(this);
            a.makeDraggable(controlBar_panel);

        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            refreshIncomeType();
        }

        private void close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ItemType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void controlBar_panel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
