using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using MetroFramework.Controls;
using System.Runtime.Remoting.Messaging;
using MetroFramework;
using System.Drawing.Printing;
using System.Collections;

namespace HMS_test1
{
    public partial class HMS : Form
    {
        public HMS()
        {
            InitializeComponent();
        }
        // Global Variables

        DataLayer data = new DataLayer(@"ABDALLAHS\SQLEXPRESS", "Hospital_System");
        string user_phone, recov_mail;
        bool log_in = true;
        string user;
        RoundedButton print = new RoundedButton();
        string ex_fname, ex_lname;
        bool exit_1st_click = true;

        // Functions

        public void Send_mail(string address, string subject, string body)
        {
            try
            {
                var fromAddress = new MailAddress("hospitalmanagement.team01@gmail.com", "Hospital Management");
                var toAddress = new MailAddress(address);
                const string fromPassword = "A1727R1723";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body

                })
                {
                    smtp.Send(message);
                }
                MetroFramework.MetroMessageBox.Show(this, "Mail sent!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Check(string table,string column,string value,bool lower)
        {
            string cmnd = "select " + column + " from " + table;
            DataTable dt = data.GetData(cmnd, table);
            foreach (DataRow dr in dt.Rows)
            {
                if (lower)
                {
                    if (dr[0].ToString().ToLower() == value) 
                        return true;
                }
                else
                {
                    if (dr[0].ToString() == value)
                        return true;
                }
            }
            return false;
        }

        static public Font ChangeFontSize(Font font, float fontSize)
        {
            if (font != null)
            {
                float currentSize = font.Size;
                if (currentSize != fontSize)
                {
                    font = new Font(font.Name, fontSize,
                        font.Style, font.Unit,
                        font.GdiCharSet, font.GdiVerticalFont);
                }
            }
            return font;
        }

        // Events
        private void Form1_Load(object sender, EventArgs e)
        {



            // Clear incomplete appointments

            string cmnd = "delete from appointments where A_date < '" + DateTime.Now + "' and Is_Confirmed = 0";
            data.ExecuteActionCommand(cmnd);
            cmnd = "select A_date , A_ID from appointments where is_confirmed = -1";
            DataTable dt = data.GetData(cmnd, "table");
            foreach(DataRow row in dt.Rows)
                if((DateTime.Now - DateTime.Parse(row[0].ToString())).TotalDays > 5)
                {
                    cmnd = "delete from Appointments where A_ID = " + row[1].ToString();
                    data.ExecuteActionCommand(cmnd);
                }



            // TODO: This line of code loads data into the 'hospital_SystemDataSet20.Adm_inv' table. You can move, or remove it, as needed.
            this.adm_invTableAdapter.Fill(this.hospital_SystemDataSet20.Adm_inv);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet19.Doc_Appoints' table. You can move, or remove it, as needed.
            this.doc_AppointsTableAdapter1.Fill(this.hospital_SystemDataSet19.Doc_Appoints);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet18.Adm_appoints' table. You can move, or remove it, as needed.
            this.adm_appointsTableAdapter.Fill(this.hospital_SystemDataSet18.Adm_appoints);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet17.Invoices' table. You can move, or remove it, as needed.
            this.invoicesTableAdapter.Fill(this.hospital_SystemDataSet17.Invoices);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet16.Users' table. You can move, or remove it, as needed.
            this.usersTableAdapter.Fill(this.hospital_SystemDataSet16.Users);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet15.Appointments' table. You can move, or remove it, as needed.
            this.appointmentsTableAdapter.Fill(this.hospital_SystemDataSet15.Appointments);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet14.Adm_room' table. You can move, or remove it, as needed.
            this.adm_roomTableAdapter1.Fill(this.hospital_SystemDataSet14.Adm_room);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet12.Adm_med' table. You can move, or remove it, as needed.
            this.adm_medTableAdapter2.Fill(this.hospital_SystemDataSet12.Adm_med);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet11.Adm_med' table. You can move, or remove it, as needed.
            this.adm_medTableAdapter1.Fill(this.hospital_SystemDataSet11.Adm_med);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet10.Adm_med' table. You can move, or remove it, as needed.
            this.adm_medTableAdapter.Fill(this.hospital_SystemDataSet10.Adm_med);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet9.Adm_doc' table. You can move, or remove it, as needed.
            this.adm_docTableAdapter.Fill(this.hospital_SystemDataSet9.Adm_doc);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet8.Patients' table. You can move, or remove it, as needed.
            this.patientsTableAdapter.Fill(this.hospital_SystemDataSet8.Patients);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet7.Treatments_Patient' table. You can move, or remove it, as needed.
            this.treatments_PatientTableAdapter1.Fill(this.hospital_SystemDataSet7.Treatments_Patient);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet6.Treatments_Patient' table. You can move, or remove it, as needed.
            this.treatments_PatientTableAdapter.Fill(this.hospital_SystemDataSet6.Treatments_Patient);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet5.Doc_Appoints' table. You can move, or remove it, as needed.
            this.doc_AppointsTableAdapter.Fill(this.hospital_SystemDataSet5.Doc_Appoints);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet4.Docs' table. You can move, or remove it, as needed.
            this.docsTableAdapter1.Fill(this.hospital_SystemDataSet4.Docs);
            // TODO: This line of code loads data into the 'hospital_SystemDataSet3.Appoints' table. You can move, or remove it, as needed.
            this.appointsTableAdapter2.Fill(this.hospital_SystemDataSet3.Appoints);
            recovery_msg.Text = "Please enter the number associated with your account in order to confirm your recovery";
            Submit.Location = new Point(recovery_num.Location.X , recovery_num.Location.Y + recovery_num.Height + 4);
            Tab.Location = new Point(0, 0);
            Tab.Width = this.Width + 25;
            Tab.Height = this.Height;
            Tab.Dock = DockStyle.None;
            Login_Panel.Width = (int)(this.Width / 2.5);
            Login_Panel.Height = this.Height;
            Login_Panel.Location = new Point(0, 0);
            Login_pic.Width = this.Width - Login_Panel.Width;
            Login_pic.Height = this.Height;
            Login_pic.Location = new Point(Login_Panel.Width, 0);
            panel1.Location = new Point((Login_Panel.Width - panel1.Width) / 2, (Login_Panel.Height - panel1.Height) / 2);
            HMS_label.Location = new Point((Login_Panel.Width - HMS_label.Width) / 2, panel1.Location.Y - 100);
            Signup_panel.Width = Login_Panel.Width;
            Signup_panel.Height = this.Height;
            Signup_panel.Location = Login_Panel.Location;
            panel2.Location = new Point((Signup_panel.Width - panel2.Width) / 2, (Signup_panel.Height - panel2.Height) / 2);
            Welcome_label.Location = new Point((Signup_panel.Width - Welcome_label.Width) / 2, panel2.Location.Y - 100);
            missing_informations.Location = new Point((panel2.Width - missing_informations.Width) / 2, Signup.Location.Y + Signup.Height + 5);
            panel4.Location = new Point((this.Width - panel4.Width) / 2, 30);
            pictureBox4.Width = this.Width - 100;
            pictureBox4.Height = this.Height - panel4.Height - 100;
            pictureBox4.Location = new Point((this.Width - 10 - pictureBox4.Width) / 2, panel4.Location.Y + panel4.Height);
            panel5.Location = new Point((this.Width - panel5.Width) / 2, 30);
            Srg_info.Location = new Point(panel5.Location.X, panel5.Location.Y + panel5.Height);
            label3.Location = new Point(Srg_info.Location.X + (Srg_info.Width - label3.Width) / 2, Srg_info.Location.Y + Srg_info.Height + 2);
            pictureBox7.Location = new Point(0, 10);
            panel6.Location = panel4.Location;
            docs_grid.Width = this.Width - 100;
            docs_grid.Height = this.Height / 2;
            docs_grid.RowTemplate.Height = 40;
            docs_grid.Location = new Point((this.Width - docs_grid.Width) / 2 + 25 , (this.Height - docs_grid.Height) / 2 + 100);
            pictureBox11.Location = pictureBox7.Location;
            metroLabel1.Location = new Point(docs_grid.Location.X, docs_grid.Location.Y - 100);
            doc_cat_search.Location = new Point(metroLabel1.Location.X + metroLabel1.Width + 5 , metroLabel1.Location.Y);
            panel7.Location = panel4.Location;
            pictureBox15.Width = 850;
            pictureBox15.Height = 450;
            pictureBox15.Location = new Point((this.Width - pictureBox15.Width) / 2, panel7.Location.Y +panel7.Height + 50);
            txt_special_ward.Location = new Point(pictureBox15.Location.X + (pictureBox15.Width - txt_special_ward.Width) / 2, pictureBox15.Location.Y + pictureBox15.Height + 10);
            pictureBox16.Width = 850;
            pictureBox16.Height = 450;
            pictureBox16.Location = new Point((this.Width - pictureBox16.Width) / 2, panel7.Location.Y + panel7.Height + 50);
            txt_private_room.Location = new Point(pictureBox16.Location.X + (pictureBox16.Width - txt_private_room.Width) / 2, pictureBox16.Location.Y + pictureBox16.Height + 10);
            pictureBox17.Width = 850;
            pictureBox17.Height = 450;
            pictureBox17.Location = new Point((this.Width - pictureBox17.Width) / 2, panel7.Location.Y + panel7.Height + 50);
            txt_deluxe.Location = new Point(pictureBox17.Location.X + (pictureBox17.Width - txt_deluxe.Width) / 2, pictureBox17.Location.Y + pictureBox17.Height + 10);
            pictureBox18.Width = 850;
            pictureBox18.Height = 450;
            pictureBox18.Location = new Point((this.Width - pictureBox18.Width) / 2, panel7.Location.Y + panel7.Height + 50);
            txt_suite.Location = new Point(pictureBox18.Location.X + (pictureBox18.Width - txt_suite.Width) / 2, pictureBox18.Location.Y + pictureBox18.Height + 10);
            panel9.Location = panel6.Location;
            appoints_grid.Location = docs_grid.Location;
            label8.Location = new Point(appoints_grid.Location.X, appoints_grid.Location.Y - label8.Height - 5);
            panel10.Location = panel6.Location;
            groupBox1.Location = new Point((this.Width - groupBox1.Width) / 2, panel10.Location.Y + panel10.Height);
            pictureBox26.Location = pictureBox11.Location;
            panel8.Location = panel6.Location;
            metroGrid1.Location = new Point((this.Width - metroGrid1.Width)/2 , panel8.Location.Y + panel8.Height + 100);
            label12.Location = new Point(metroGrid1.Location.X,metroGrid1.Location.Y - label12.Height - 5);
            pictureBox28.Location = pictureBox7.Location;
            pictureBox27.Location = pictureBox7.Location;
            panel11.Location = panel6.Location;
            metroGrid2.Location = metroGrid1.Location;
            check_pay.Location = new Point(metroGrid2.Location.X + (metroGrid2.Width - check_pay.Width) / 2, metroGrid2.Location.Y + metroGrid2.Height + 5);
            label14.Location = new Point(metroGrid2.Location.X, metroGrid2.Location.Y - label14.Height - 5);
            pictureBox30.Location = pictureBox14.Location;
            panel12.Location = panel6.Location;
            add_Ptreatment.Location = new Point((this.Width - add_Ptreatment.Width) / 2, (this.Height - add_Ptreatment.Height) / 2 + 50);
            appoints_grid.Location = new Point((this.Width - appoints_grid.Width) / 2, (this.Height - appoints_grid.Height) / 2);
            label8.Location = new Point(appoints_grid.Location.X, appoints_grid.Location.Y - label8.Height - 5);
            RoundedButton exit = new RoundedButton();
            exit.Text = "Exit";
            exit.UseCustomBackColor = true;
            exit.UseCustomForeColor = true;
            exit.BackColor = Color.Maroon;
            exit.ForeColor = Color.White;
            exit.Width = 100;
            patient_exit.Controls.Add(exit);
            exit.Location = new Point(P_lname_exit.Location.X, P_lname_exit.Location.Y + 50);
            exit.Click += Exit_Click;
            print.Text = "Print";
            print.UseCustomBackColor = true;
            print.UseCustomForeColor = true;
            print.BackColor = Color.ForestGreen;
            print.ForeColor = Color.White;
            print.Width = 100;
            print.Enabled = false;
            patient_exit.Controls.Add(print);
            print.Location = new Point(exit.Location.X + exit.Width + (P_lname_exit.Width - 200), exit.Location.Y);
            print.Click += Print_Click;
            patient_exit.Location = new Point((this.Width - patient_exit.Width) / 2, (this.Height - patient_exit.Height) / 2 + 50);
            treat_exit.Location = new Point(add_Ptreatment.Location.X, add_Ptreatment.Location.Y + add_Ptreatment.Height + 10);
            panel13.Location = panel12.Location;
            panel14.Location = new Point((this.Width - panel14.Width) / 2 , (this.Height - panel14.Height) / 2 + 50);
            pictureBox35.Location = pictureBox28.Location;
            pictureBox36.Location = pictureBox28.Location;
            panel15.Location = panel12.Location;
            add_doc.Location = Srg_info.Location;
            add_emp.Location = add_doc.Location;
            pictureBox39.Location = pictureBox28.Location;
            panel16.Location = panel15.Location;
            metroLabel16.Location = new Point(panel16.Location.X - 150, panel16.Location.Y + panel16.Height + 40);
            src_by_pa.Location = new Point(metroLabel16.Location.X + metroLabel16.Width + 10, metroLabel16.Location.Y);
            adm_pa_grid.Width += 350;
            adm_pa_grid.Height += 100;
            adm_pa_grid.Location = new Point((this.Width - adm_pa_grid.Width) / 2, metroLabel16.Location.Y + metroLabel16.Height + adm_pa_src_but.Height + 60);
            pictureBox42.Location = pictureBox39.Location;
            panel19.Location = panel16.Location;
            adm_dr_grid.Width += 350;
            adm_dr_grid.Height += 100;
            adm_dr_grid.Location = new Point((this.Width - adm_dr_grid.Width) / 2, metroLabel17.Location.Y + metroLabel17.Height + adm_dr_src_but.Height + 90);
            metroLabel17.Location = metroLabel16.Location;
            src_by_dr.Location = src_by_pa.Location;
            panel21.Location = panel19.Location;
            adm_med_grid.Width += 150;
            adm_med_grid.Height += 100;
            metroLabel18.Location = metroLabel16.Location;
            src_by_med.Location = src_by_dr.Location;
            adm_med_grid.Location = new Point((this.Width - adm_med_grid.Width) / 2, metroLabel18.Location.Y + metroLabel18.Height + 20);
            panel22.Location = panel21.Location;
            metroLabel19.Location = metroLabel18.Location;
            src_by_room.Location = src_by_dr.Location;
            panel23.Location = panel22.Location;
            adm_room_search_cat.Location = new Point (src_by_room.Location.X + src_by_room.Width + 50 , src_by_room.Location.Y);
            adm_room_search_type.Location = adm_room_search_cat.Location;
            adm_room_grid.Width += 100;
            adm_room_grid.Height += 150;
            adm_room_grid.Location = new Point((this.Width - adm_room_grid.Width) / 2, metroLabel19.Location.Y + metroLabel19.Height + 20);
            metroLabel20.Location = metroLabel19.Location;
            panel24.Location = new Point(metroLabel20.Location.X + metroLabel20.Width + 50,metroLabel20.Location.Y);
            adm_appoints_grid.Width += 350;
            adm_appoints_grid.Height += 50;
            adm_appoints_grid.Location = new Point((this.Width - adm_appoints_grid.Width) / 2, panel24.Location.Y + panel24.Height + 20);
            pictureBox53.Location = pictureBox48.Location;
            pictureBox54.Location = pictureBox48.Location;
            panel25.Location = panel23.Location;
            metroLabel21.Location = metroLabel20.Location;
            panel26.Location = new Point(metroLabel21.Location.X + metroLabel21.Width + 40, metroLabel21.Location.Y);
            adm_users_grid.Width += 150;
            adm_users_grid.Height += 150;
            adm_users_grid.Location = new Point((this.Width - adm_users_grid.Width) / 2, panel26.Location.Y + panel26.Height + 20);
            pictureBox57.Location = pictureBox54.Location;
            metroLabel22.Location = metroLabel20.Location;
            src_by_inv.Location = src_by_dr.Location;
            adm_inv_year_select.Location = new Point(src_by_inv.Location.X + src_by_inv.Width + 40, src_by_inv.Location.Y);
            adm_inv_grid.Width += 70;
            adm_inv_grid.Height += 150;
            adm_inv_grid.Location = new Point((this.Width - adm_inv_grid.Width) / 2, adm_inv_year_select.Location.Y + adm_inv_year_select.Height + 20);
            pictureBox60.Location = pictureBox57.Location;
            panel27.Location = panel25.Location;
            label36.Location = new Point((Srg_info.Width - label36.Width) / 2, (Srg_info.Height - label36.Height) / 2);
            srg_new_app.Location = new Point(label36.Location.X + (label36.Width - srg_new_app.Width) / 2, label36.Location.Y + label36.Height + 5);
            appoints_req.Location = new Point(this.Width - appoints_req.Width - 25, pictureBox27.Location.Y);
        }

        private void Print_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                pd.PrinterSettings = printDialog1.PrinterSettings;
                pd.PrintPage += Pd_PrintPage;
                pd.Print();

            }
        }
        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            string cmnd = "Select P_ID, P_email ,R_ID from Patients where P_fName = '" + P_fname_exit.Text + "' and P_lName = '" + P_lname_exit.Text + "'";
            DataTable dt = data.GetData(cmnd, "ID");

            cmnd = "Select M_Price from Medicine inner join Treatments on Medicine.M_ID = Treatments.M_ID where P_ID = " + dt.Rows[0][0].ToString();
            DataTable dt1 = data.GetData(cmnd, "medicine");
            float amount = 0;
            foreach (DataRow dr in dt1.Rows)
                amount += float.Parse(dr[0].ToString());
            cmnd = "Select R_price from patients inner join rooms on patients.R_ID = rooms.R_ID where P_ID = " + dt.Rows[0][0].ToString();
            float r_price = float.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select Entrance_date from patients where P_email = '" + dt.Rows[0][1].ToString() + "'";
            DateTime t1 = DateTime.Parse(data.GetValue(cmnd).ToString()), t2 = DateTime.Now;
            int day_diff = (int)(t2 - t1).TotalDays;
            amount += day_diff * r_price;


            Graphics g = e.Graphics;

            Font titlefont = new Font("Segoe UI", 25, FontStyle.Bold);
            Font normalfont = new Font("Segoe UI", 15, FontStyle.Regular);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            StringFormat sfr = new StringFormat();
            sfr.Alignment = StringAlignment.Near;

            Brush b = Brushes.Black;
            Brush signature = Brushes.Blue;
            Pen p = new Pen(Color.Blue, 1.8f);

            int TopToUse = 20;

            g.DrawImage(new Bitmap(HMS_test1.Resource1.HMS), new Point(25, 20));
            TopToUse += 40;
            g.DrawString("Hospital Management System", titlefont, b, new Rectangle(new Point(0, TopToUse), new Size(850, 50)), sf);
            TopToUse += 100;
            g.DrawString("Patient Name:", normalfont, b, new Rectangle(new Point(50, TopToUse), new Size(200, 40)), sfr);
            g.DrawString(P_fname_exit.Text + " " + P_lname_exit.Text, normalfont, b, new Rectangle(new Point(220, TopToUse), new Size(200, 40)), sfr);
            TopToUse += 50;
            g.DrawString("Money :", normalfont, b, new Rectangle(new Point(50, TopToUse), new Size(200, 40)), sfr);
            g.DrawString(amount.ToString() + " $", normalfont, b, new Rectangle(new Point(220, TopToUse), new Size(200, 40)), sfr);
            TopToUse += 50;
            g.DrawString("Date :", normalfont, b, new Rectangle(new Point(50, TopToUse), new Size(200, 40)), sfr);
            g.DrawString(DateTime.Now.ToString(), normalfont, b, new Rectangle(new Point(220, TopToUse), new Size(300, 40)), sfr);
            TopToUse += 200;
            g.DrawString("Signature :", normalfont, b, new Rectangle(new Point(500, TopToUse), new Size(200, 40)), sfr);
            g.DrawString("Natalie DAHER", normalfont, signature, new Rectangle(new Point(620, TopToUse), new Size(300, 40)), sfr);

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            string cmnd = "Select P_ID, P_email ,R_ID from Patients where P_fName = '" + P_fname_exit.Text + "' and P_lName = '" + P_lname_exit.Text + "'";
            DataTable dt = data.GetData(cmnd, "ID");
            if (dt.Rows.Count == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Patient Not Found!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if(exit_1st_click)
                {
                    cmnd = "Select M_Price from Medicine inner join Treatments on Medicine.M_ID = Treatments.M_ID where P_ID = " + dt.Rows[0][0].ToString();
                    DataTable dt1 = data.GetData(cmnd, "medicine");
                    float amount = 0;
                    foreach (DataRow dr in dt1.Rows)
                        amount += float.Parse(dr[0].ToString());
                    cmnd = "Select R_price from patients inner join rooms on patients.R_ID = rooms.R_ID where P_ID = " + dt.Rows[0][0].ToString();
                    float r_price = float.Parse(data.GetValue(cmnd).ToString());
                    cmnd = "Select Entrance_date from patients where P_email = '" + dt.Rows[0][1].ToString() + "'";
                    DateTime t1 = DateTime.Parse(data.GetValue(cmnd).ToString()), t2 = DateTime.Now;
                    int day_diff = (int)(t2 - t1).TotalDays;
                    amount += day_diff * r_price;
                    cmnd = "Insert into Invoices values (" + dt.Rows[0][0].ToString() + ",'" + t2 + "'," + amount.ToString() + ")";
                    int rep1 = data.ExecuteActionCommand(cmnd);
                    cmnd = "Update Rooms set R_Availability = 'Available' where R_ID = " + dt.Rows[0][2].ToString();
                    int rep2 = data.ExecuteActionCommand(cmnd);
                    if (rep1 != 0 && rep2 != 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Exit transactions completed successfully. Thank you!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ex_fname = P_fname_exit.Text;
                        ex_lname = P_lname_exit.Text;
                        exit_1st_click = false;
                        print.Enabled = true;
                    }
                }
                else
                {
                    if(ex_fname == P_fname_exit.Text && ex_lname == P_lname_exit.Text)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Exit transactions for this patient are already completed!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        cmnd = "Select M_Price from Medicine inner join Treatments on Medicine.M_ID = Treatments.M_ID where P_ID = " + dt.Rows[0][0].ToString();
                        DataTable dt1 = data.GetData(cmnd, "medicine");
                        float amount = 0;
                        foreach (DataRow dr in dt1.Rows)
                            amount += float.Parse(dr[0].ToString());
                        cmnd = "Select R_price from patients inner join rooms on patients.R_ID = rooms.R_ID where P_ID = " + dt.Rows[0][0].ToString();
                        float r_price = float.Parse(data.GetValue(cmnd).ToString());
                        cmnd = "Select Entrance_date from patients where P_email = '" + dt.Rows[0][1].ToString() + "'";
                        DateTime t1 = DateTime.Parse(data.GetValue(cmnd).ToString()), t2 = DateTime.Now;
                        int day_diff = (int)(t2 - t1).TotalDays;
                        amount += day_diff * r_price;
                        cmnd = "Insert into Invoices values (" + dt.Rows[0][0].ToString() + ",'" + t2 + "'," + amount.ToString() + ")";
                        int rep1 = data.ExecuteActionCommand(cmnd);
                        cmnd = "Update Rooms set R_Availability = 'Available' where R_ID = " + dt.Rows[0][2].ToString();
                        int rep2 = data.ExecuteActionCommand(cmnd);
                        if (rep1 != 0 && rep2 != 0)
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Exit transactions completed successfully. Thank you!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ex_fname = P_fname_exit.Text;
                            ex_lname = P_lname_exit.Text;
                            exit_1st_click = false;
                            print.Enabled = true;
                        }
                    }
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Tab.Width = this.Width + 10;
            Tab.Height = this.Height;
            Login_Panel.Width = (int)(this.Width / 2.5);
            Login_Panel.Height = this.Height;
            Login_Panel.Location = new Point(0,0);
            Login_pic.Width = this.Width - Login_Panel.Width;
            Login_pic.Height = this.Height;
            Login_pic.Location = new Point(Login_Panel.Width, 0);
            panel1.Location = new Point((Login_Panel.Width - panel1.Width) / 2, (Login_Panel.Height - panel1.Height) / 2);
            HMS_label.Location = new Point((Login_Panel.Width - HMS_label.Width) / 2, panel1.Location.Y - 100);
            Signup_panel.Width = Login_Panel.Width;
            Signup_panel.Height = this.Height;
            Signup_panel.Location = Login_Panel.Location;
            panel2.Location = new Point((Signup_panel.Width - panel2.Width) / 2, (Signup_panel.Height - panel2.Height) / 2);
            Welcome_label.Location = new Point((Signup_panel.Width - Welcome_label.Width) / 2, panel2.Location.Y - 100);
            missing_informations.Location = new Point((panel2.Width - missing_informations.Width) / 2, Signup.Location.Y + Signup.Height + 5);
        }

        private void Signup_button_Click(object sender, EventArgs e)
        {
            First_name.Text = "";
            Last_name.Text = "";
            Day.Text = "";
            Month.Text = "";
            Year.Text = "";
            Signup_email.Text = "";
            Phone_num.Text = "";
            Address.Text = "";
            Signup_pass.Text = "";
            Confirm_pass.Text = "";
            Recovery_email.Text = "";
            Login_Panel.Visible = false;
            Signup_panel.Visible = true;
            missing_informations.Visible = false;
        }

        private void Back_login_Click(object sender, EventArgs e)
        {
            Login_Panel.Visible = true;
            Signup_panel.Visible = false;
            Login_email.Text = "";
            Login_pass.Text = "";
        }

        private void Signup_Click(object sender, EventArgs e)
        {
            //Local variables

            bool missing_info = false;
            bool format = false;
            int rep1, rep2;
            string cmnd = "";
            string pass = "";
            int mo = 0;
            string DOB = "";
            string[] t = { First_name.Text, Last_name.Text, Day.Text, Month.Text, Year.Text, Signup_email.Text, Phone_num.Text, Address.Text, Signup_pass.Text, Confirm_pass.Text, BloodType.Text, Gender.Text };

            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == "")
                {
                    missing_informations.Visible = true;
                    missing_informations.Text = "Please enter all informations";
                    missing_informations.Location = new Point((panel2.Width - missing_informations.Width) / 2, Signup.Location.Y + Signup.Height + 5);
                    missing_info = true;
                }
            }
            if (!missing_info)
            {
                string email = Signup_email.Text.ToLower();
                if (Check("Users", "U_Email", email, true))
                {
                    missing_informations.Text = "Already a user";
                    missing_informations.Visible = true;
                    missing_informations.Location = new Point((panel2.Width - missing_informations.Width) / 2, Signup.Location.Y + Signup.Height + 5);
                }
                else
                {
                    if (email.Contains("@gmail.com") || email.Contains("@hotmail.com") || email.Contains("@outlook.com") || email.Contains("@yahoo.com") || email.Contains("@icloud.com") || email.Contains("@windowes.com") || email.Contains("@msn.com"))
                    {
                        if (Signup_pass.Text == Confirm_pass.Text)
                        {
                            switch(Month.Text)
                            {
                                case "January":
                                    mo = 1;
                                    break;
                                case "February":
                                    mo = 2;
                                    break;
                                case "March":
                                    mo = 3;
                                    break;
                                case "April":
                                    mo = 4;
                                    break;
                                case "May":
                                    mo = 5;
                                    break;
                                case "June":
                                    mo = 6;
                                    break;
                                case "July":
                                    mo = 7;
                                    break;
                                case "August":
                                    mo = 8;
                                    break;
                                case "September":
                                    mo = 9;
                                    break;
                                case "October":
                                    mo = 10;
                                    break;
                                case "November":
                                    mo = 11;
                                    break;
                                case "December":
                                    mo = 12;
                                    break;
                            }
                                try
                                {
                                    int.Parse(Phone_num.Text);
                                    format = true;
                                }
                                catch
                                {
                                    missing_informations.Text = "The phone number is incorrect";
                                    missing_informations.Visible = true;
                                    missing_informations.Location = new Point((panel2.Width - missing_informations.Width) / 2, Signup.Location.Y + Signup.Height + 5);
                                    format = false;
                                }
                                if (format)
                                {
                                    DOB = mo.ToString() + "-" + Day.Text + "-" + Year.Text;
                                    pass = CryptoEngine.Encrypt(Signup_pass.Text);
                                if (Recovery_email.Text != "")
                                {
                                    email = Recovery_email.Text.ToLower();
                                    if (email.Contains("@gmail.com") || email.Contains("@hotmail.com") || email.Contains("@outlook.com") || email.Contains("@yahoo.com") || email.Contains("@icloud.com") || email.Contains("@windowes.com") || email.Contains("@msn.com"))
                                    {
                                        cmnd = "insert into Users values (" + "'" + Signup_email.Text.ToLower() + "'" + "," + "'" + pass + "'" + "," + "'" + "Patient" + "'" + "," + "'" + Recovery_email.Text.ToLower() + "'" + ")";
                                        rep1 = data.ExecuteActionCommand(cmnd);
                                        cmnd = "insert into patients values (NULL," + "'" + First_name.Text + "'" + "," + "'" + Last_name.Text + "'" + "," + "'" + Gender.Text + "'" + "," + "'" + BloodType.Text + "'" + "," + "'" + DOB + "'" + "," + "'" + Signup_email.Text.ToLower() + "'" + "," + Phone_num.Text + "," + "'" + Address.Text + "'" + "," + "NULL" + ")";
                                        rep2 = data.ExecuteActionCommand(cmnd);
                                        missing_informations.Visible = false;
                                        if (rep1 != 0 && rep2 != 0)
                                        {
                                            Tab.SelectTab(Patient);
                                            user = Signup_email.Text;
                                            log_in = false;
                                            Surgery.Text = "Surgery";
                                        }


                                    }
                                    else
                                    {
                                        missing_informations.Visible = true;
                                        missing_informations.Text = "The recovery email you entered is incorrect";
                                        missing_informations.Location = new Point((panel2.Width - missing_informations.Width) / 2, Signup.Location.Y + Signup.Height + 5);
                                    }
                                }
                                else
                                {
                                    cmnd = "insert into Users values (" + "'" + Signup_email.Text.ToLower() + "'" + "," + "'" + pass + "'" + "," + "'" + "Patient" + "'" + "," + "NULL"+ ")";
                                    rep1 = data.ExecuteActionCommand(cmnd);
                                    cmnd = "insert into patients values (NULL," + "'" + First_name.Text + "'" + "," + "'" + Last_name.Text + "'" + "," + "'" + Gender.Text + "'" + "," + "'" + BloodType.Text + "'" + "," + "'" + DOB + "'" + "," + "'" + Signup_email.Text.ToLower() + "'" + "," + Phone_num.Text + "," + "'" + Address.Text + "'" + "," + "NULL" + ")";
                                    rep2 = data.ExecuteActionCommand(cmnd);
                                    missing_informations.Visible = false;
                                        if (rep1 != 0 && rep2 != 0)
                                        {
                                            Tab.SelectTab(Patient);
                                            user = Signup_email.Text;
                                            log_in = false;
                                            Surgery.Text = "Surgery";
                                        }
                                    }
                                }
                        }
                        else
                        {
                            missing_informations.Text = "The password doesn't match with its confirmation";
                            missing_informations.Visible = true;
                            missing_informations.Location = new Point((panel2.Width - missing_informations.Width) / 2, Signup.Location.Y + Signup.Height + 5);
                        }
                    }
                    else
                    {
                        missing_informations.Visible = true;
                        missing_informations.Text = "The email you entered is incorrect";
                        missing_informations.Location = new Point((panel2.Width - missing_informations.Width) / 2, Signup.Location.Y + Signup.Height + 5);
                    }
                }
            }
        }
        private void Forgot_pass_Click(object sender, EventArgs e)
        {
            recovery_num.Text = "";
            MissingPass.Text = "";
            string cmnd;
            if(Login_email.Text != "")
            {
                if(Check("Users", "U_Email", Login_email.Text.ToLower(), true))
                {
                    if(CheckForInternetConnection())
                    {
                        Login_email.Visible = false;
                        Login_pass.Visible = false;
                        Login_tile.Visible = false;
                        Forgot_pass.Visible = false;
                        New_patient.Visible = false;
                        To_signup.Visible = false;
                        panel3.Location = Login_email.Location;
                        Back_forgot_log.Enabled = true;
                        Back_forgot_log.Visible = true;
                        cmnd = "Select U_RecoveryEmail from Users where U_Email = " + "'" + Login_email.Text + "'";
                        recov_mail = data.GetValue(cmnd).ToString();
                        cmnd = "Select U_Type from Users where U_Email = " + "'" + Login_email.Text + "'";
                        string user_type = data.GetValue(cmnd).ToString();
                        cmnd = "Select " + user_type[0] + "_phone " + "from " + user_type + "s " + "where " + user_type[0] + "_email = " + "'" + Login_email.Text + "'";
                        user_phone = data.GetValue(cmnd).ToString();
                        recovery_num.WaterMark = "******";
                        recovery_num.WaterMark += user_phone[6].ToString() + user_phone[7].ToString();
                        if (recov_mail != "NULL")
                        {
                            another_method.Enabled = true;
                            another_method.Visible = true;
                        }

                        panel3.Visible = true;
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Please check your internet connection", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                }
                else
                {
                    MissingPass.Text = "Not a user";
                    MissingPass.Location = new Point((panel1.Width - MissingPass.Width) / 2, MissingPass.Location.Y);
                    MissingPass.Visible = true;
                }
            }
        }

        private void Back_forgot_log_Click(object sender, EventArgs e)
        {
            Login_email.Visible = true;
            Login_pass.Visible = true;
            Login_tile.Visible = true;
            Forgot_pass.Visible = true;
            New_patient.Visible = true;
            To_signup.Visible = true;
            Login_email.Text = "";
            Login_pass.Text = "";
            Back_forgot_log.Enabled = false;
            Back_forgot_log.Visible = false;
            panel3.Visible = false;
            another_method.Visible = false;
            another_method.Enabled = false;
        }

        private void Submit_Click(object sender, EventArgs e)
        {
          
                string cmnd = "select U_Password from Users where U_Email = " + "'" + Login_email.Text + "'";
                string pass = data.GetValue(cmnd).ToString();
                pass = CryptoEngine.Decrypt(pass);
                string mail_body = "Dear user, your password is " + "'" + pass + "'";
                string mail_subject = "Reset Password";
                if (another_method.Text != "Back")
                {

                    if (recovery_num.Text == user_phone)
                    {
                        Send_mail(Login_email.Text, mail_subject, mail_body);
                    }
                    else
                    {
                         MetroFramework.MetroMessageBox.Show(this, "The number you entered is incorrect!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    if (recov_email.Text == recov_mail)
                    {
                        Send_mail(Login_email.Text, mail_subject, mail_body);
                    }
                    else
                    {
                         MetroFramework.MetroMessageBox.Show(this, "The email you entered is incorrect!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
        }

        private void Login_tile_Click(object sender, EventArgs e)
        {
            //Local variables
            string cmnd;

            if ((Login_email.Text == "") && (Login_pass.Text == ""))
            {
                MissingPass.Text = "Please enter your login info";
                MissingPass.Location = new Point((panel1.Width - MissingPass.Width) / 2, MissingPass.Location.Y);
                MissingPass.Visible = true;
            }
            if (Login_email.Text != "")
            {
                if (Check("Users", "U_Email", Login_email.Text.ToLower(), true))
                {
                    if(Login_pass.Text == "")
                    {
                        MissingPass.Text = "Please enter your password";
                        MissingPass.Location = new Point((panel1.Width - MissingPass.Width) / 2, MissingPass.Location.Y);
                        MissingPass.Visible = true;
                    }
                    else
                    {
                        cmnd = "select U_Password,U_type from Users where U_email = " + "'" + Login_email.Text.ToLower() + "'";
                        DataTable dtt = data.GetData(cmnd,"Users");
                        string pass = CryptoEngine.Decrypt(dtt.Rows[0][0].ToString());
                        string type = dtt.Rows[0][1].ToString();
                        if (pass == Login_pass.Text)
                        {
                            try
                            {
                                Tab.SelectTab(type);
                            }
                            catch
                            {

                            }
                            user = Login_email.Text;
                            log_in = true;
                            if(type == "Doctor")
                            {
                                cmnd = "Select D_ID from Doctors where D_email = " + "'" + Login_email.Text + "'";
                                int id = int.Parse(data.GetValue(cmnd).ToString());
                                cmnd = "Select P_fName, P_lName, A_Date, R_ID from Doc_Appoints where D_ID = " + id + "and Is_Confirmed = 1 and A_date > '" + DateTime.Now +"'";
                                DataTable dt = data.GetData(cmnd, "Appoints");
                                metroGrid1.DataSource = dt;
                            }
                            if(type == "Patient")
                            {
                                // textBox1.Text = CryptoEngine.Encrypt("0000");
                                cmnd = "Select P_ID from Patients where P_email = '" + Login_email.Text + "'";
                                string id = data.GetValue(cmnd).ToString();
                                cmnd = "Select Entrance_date from patients where P_email = '" + Login_email.Text + "'";
                                DateTime t1 = DateTime.Now;
                                bool srg = false;
                                try
                                {
                                    t1 = DateTime.Parse(data.GetValue(cmnd).ToString());
                                    DateTime t2 = DateTime.Now;
                                    if (DateTime.Compare(t1, t2) < 0)
                                    {
                                        cmnd = "Select Inv_date from Invoices where P_ID = " + id;
                                        DataTable dt1 = data.GetData(cmnd, "table");
                                        foreach(DataRow dr in dt1.Rows)
                                        {
                                            if(DateTime.Parse(dr[0].ToString()) > t1)
                                            {
                                                Surgery.Text = "Surgery";
                                                srg = true;
                                            }  
                                        }
                                        if(!srg)
                                            Surgery.Text = "Treatment";
                                    }
                                    else
                                    {
                                       /* 
                                        if(Check("Appointments","P_ID",id,false))
                                        {
                                            cmnd = "select A_date, is_confirmed from appointments where P_ID = " + id;
                                            DataTable dt = data.GetData(cmnd, "Table");
                                            foreach(DataRow row in dt.Rows)
                                                if(row[1].ToString() == "1")
                                                {
                                                    cmnd = "Select Entrance_date from Patients where P_ID = " + id;
                                                    if(DateTime.Parse(row[0].ToString()) == DateTime.Parse(data.GetValue(cmnd).ToString()))
                                                        if(DateTime.Now > DateTime.Parse(row[0].ToString()))
                                                        {
                                                            cmnd = "Select Inv_date from Invoices where P_ID = " + id;
                                                            DataTable dt1 = data.GetData(cmnd, "table");
                                                            foreach(DataRow dr in dt1.Rows)
                                                                if(DateTime.Parse(dr[0].ToString()) > )
                                                        }
                                                }
                                        }*/
                                        Surgery.Text = "Surgery";
                                    }
                                }
                                catch
                                {
                                    Surgery.Text = "Surgery";
                                }
                            }
                        }
                        else
                        {
                            MissingPass.Text = "Password is incorrect";
                            MissingPass.Location = new Point((panel1.Width - MissingPass.Width) / 2, MissingPass.Location.Y);
                            MissingPass.Visible = true;
                        }
                    }
                }
                else
                {
                    MissingPass.Text = "Not a user";
                    MissingPass.Location = new Point((panel1.Width - MissingPass.Width) / 2, MissingPass.Location.Y);
                    MissingPass.Visible = true;
                }
            }
        }

        private void Login_email_Click(object sender, EventArgs e)
        {
            MissingPass.Visible = false;
        }

        private void Login_pass_Click(object sender, EventArgs e)
        {
            MissingPass.Visible = false;
        }

        private void Surgery_Click(object sender, EventArgs e)
        {
            string cmnd;
            string mail;
            if (log_in)
            {
                cmnd = "Select P_ID from Patients where P_Email = '" + Login_email.Text + "'";
                mail = Login_email.Text;
            }
            else
            {
                cmnd = "Select P_ID from Patients where P_Email = '" + Signup_email.Text + "'";
                mail = Signup_email.Text;
            }

            string id = data.GetValue(cmnd).ToString();

            if (Surgery.Text == "Surgery")
            {
                foreach (Control ctrl in Srg_info.Controls)
                {
                    ctrl.Visible = true;
                    ctrl.Enabled = true;
                }
                label27.Visible = false;
                label27.Enabled = true;
                label3.Visible = true;
                textBox1.Visible = false;
                textBox1.Enabled = true;
                srg_docID1.Text = "";
                label36.Visible = false;
                srg_new_app.Visible = false;

                if (Check("Appointments","P_ID",id,false))
                {
                    cmnd = "Select is_confirmed , A_date from Appointments where P_ID = " + id;
                    DataTable dt = data.GetData(cmnd, "Appo");
                    foreach(DataRow dr in dt.Rows)
                        if(dr[0].ToString() == "0")
                        {
                            foreach (Control ctrl in Srg_info.Controls)
                                ctrl.Visible = false;
                            label27.Visible = true;
                            label27.Location = new Point((Srg_info.Width - label27.Width) / 2, (Srg_info.Height - label27.Height) / 2);
                            label3.Visible = false;
                        }
                        else
                        {
                            if(DateTime.Parse(dr[1].ToString()) > DateTime.Now)
                            {
                                if(dr[0].ToString() == "1")
                                {
                                    foreach (Control ctrl in Srg_info.Controls)
                                        ctrl.Visible = false;
                                    textBox1.Visible = true;
                                    //label27.Font = ChangeFontSize(label27.Font, 14);
                                    textBox1.Text = "The doctor confirmed your appointment! We are waiting for you on " + dr[1].ToString();
                                    textBox1.Location = new Point((Srg_info.Width - textBox1.Width) / 2, (Srg_info.Height - textBox1.Height) / 2);
                                    label3.Visible = false;
                                }
                                else
                                {
                                    foreach (Control ctrl in Srg_info.Controls)
                                        ctrl.Visible = false;
                                    label3.Visible = false;
                                    label36.Visible = true;
                                    srg_new_app.Visible = true;
                                }
                            }

                        }
                    
                }

                cmnd = "select P_fname,P_lname,P_DOB,P_phone,P_address from Patients where P_email = " + "'" + mail + "'";
                DataTable dtt = data.GetData(cmnd, "Patients");
                srg_fname1.Text = dtt.Rows[0]["P_fname"].ToString();
                srg_lname1.Text = dtt.Rows[0]["P_lname"].ToString();
                DOB_srg1.Text = dtt.Rows[0]["P_DOB"].ToString();
                srg_nmbr1.Text = dtt.Rows[0]["P_phone"].ToString();
                srg_address1.Text = dtt.Rows[0]["P_address"].ToString();
                Tab.SelectTab(Srg_tab);
            }
            else
            {
                cmnd = "Select M_Name, M_Price, T_Period from Treatments_Patient where P_ID = " + id;
                metroGrid2.DataSource = data.GetData(cmnd, "Treatments");
                Tab.SelectTab(Treatments);
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Patient);
        }

        private void save_srg_Click(object sender, EventArgs e)
        {
            // Local variables

            bool ok = true;
            string date;
            int mo = 0;

            foreach (Control ctrl in Srg_info.Controls)
            {
                if (ctrl.Text == "")
                {
                    ok = false;
                    break;
                }
            }
            if(!ok)
            {
                label3.Text = "Please enter all informations";
                label3.Location = new Point(Srg_info.Location.X + (Srg_info.Width - label3.Width) / 2, Srg_info.Location.Y + Srg_info.Height + 2);
            }
            else
            {
                switch (srg_month.Text)
                {
                    case "January":
                        mo = 1;
                        break;
                    case "February":
                        mo = 2;
                        break;
                    case "March":
                        mo = 3;
                        break;
                    case "April":
                        mo = 4;
                        break;
                    case "May":
                        mo = 5;
                        break;
                    case "June":
                        mo = 6;
                        break;
                    case "July":
                        mo = 7;
                        break;
                    case "August":
                        mo = 8;
                        break;
                    case "September":
                        mo = 9;
                        break;
                    case "October":
                        mo = 10;
                        break;
                    case "November":
                        mo = 11;
                        break;
                    case "December":
                        mo = 12;
                        break;
                }
                if (!Check("Doctors", "D_ID", srg_docID1.Text, false))
                {
                    label3.Text = "Doctor ID is incorrect";
                    label3.Location = new Point(Srg_info.Location.X + (Srg_info.Width - label3.Width) / 2, Srg_info.Location.Y + Srg_info.Height + 2);
                }
                else
                {
                    string cmd = "select D_Specialization from Doctors where D_ID =" + srg_docID1.Text;
                    int id = int.Parse(data.GetValue(cmd).ToString());
                    cmd = "select R_ID from Rooms where (R_Availability = 'Available') and (R_cat =" + id + ") and (R_Type = '" + srg_room_type1.Text + "')";
                    DataTable dt = data.GetData(cmd, "Rooms");
                    if(dt.Rows.Count == 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "There is no available rooms with this type! Try another type or contact the directory.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        date = mo + "-" + srg_day.Text + "-2020";
                        cmd = "Update Patients set R_ID = " + dt.Rows[0][0].ToString() + "," + "P_fname = " + "'" + srg_fname1.Text + "'" + "," + "P_lname = " + "'" + srg_lname1.Text + "'" + "," + "P_DOB = " + "'" + DOB_srg1.Text + "'" + "," + "P_phone = " + srg_nmbr1.Text + "," + "P_address = " + "'" + srg_address1.Text + "'" + "," + "Entrance_date = " + "'" + date + "'" + "WHERE P_email = " + "'" + user + "'";
                        data.ExecuteActionCommand(cmd);
                        cmd = "Update Rooms set R_Availability = 'Unavailable' WHERE R_ID = " + dt.Rows[0][0].ToString();
                        data.ExecuteActionCommand(cmd);
                        cmd = "Select P_ID from Patients where P_email = '" + user + "'";
                        int p_id = int.Parse(data.GetValue(cmd).ToString());
                        cmd = "Insert into Appointments values (" + p_id.ToString() + ",'" + srg_docID1.Text + "','" + date + "'," + "0)";
                        data.ExecuteActionCommand(cmd);
                        foreach (Control ctrl in Srg_info.Controls)
                        {
                            ctrl.Enabled = false;
                        }
                        MetroFramework.MetroMessageBox.Show(this, "Your room ID is " + dt.Rows[0][0].ToString() + ". Thank you for choosing our hospital! We will contact you soon.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        private void Drs_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Doc);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Patient);
        }

        private void doc_cat_search_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmnd;
            if(doc_cat_search.Text == "All")
                cmnd = "Select * from Docs";
            else
                cmnd = "Select * from Docs where C_Name = " + "'" + doc_cat_search.Text + "'";
            DataTable dt = data.GetData(cmnd, "Docs");
            docs_grid.DataSource = dt;
        }

        private void rooms_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(rooms_tab);
        }

        private void room_type_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox15.Visible = false;
            pictureBox16.Visible = false;
            pictureBox17.Visible = false;
            pictureBox18.Visible = false;
            txt_special_ward.Visible = false;
            txt_private_room.Visible = false;
            txt_deluxe.Visible = false;
            txt_suite.Visible = false;
            switch (room_type_select.Text)
            {
                case "Special ward":
                    pictureBox15.Visible = true;
                    txt_special_ward.Visible = true;
                    break;
                case "Private room":
                    pictureBox16.Visible = true;
                    txt_private_room.Visible = true;
                    break;
                case "Deluxe room":
                    pictureBox17.Visible = true;
                    txt_deluxe.Visible = true;
                    break;
                case "Suite":
                    pictureBox18.Visible = true;
                    txt_suite.Visible = true;
                    break;
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Patient);
        }

        private void appoints_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(appoints_tab);
            string mail;
            if (log_in)
            {
                mail = Login_email.Text;
            }
            else
            {
                mail = Signup_email.Text;
            }
            string cmnd = "select Patient_fname, Patient_lname, Doctor_fname, Doctor_lname, C_name, A_date from Appoints where P_email = " + "'" + mail + "'" + " and is_confirmed = 1";
            DataTable dt = data.GetData(cmnd, "Appoints");
            appoints_grid.DataSource = dt;
        }
        private void pictureBox21_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Patient);
        }

        private void appoints_req_Click(object sender, EventArgs e)
        {
            string cmnd = "Select D_ID from Doctors where D_email = " + "'" + Login_email.Text + "'";
            int d_id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select P_fname , P_lname , A_date from Patients inner join Appointments on Patients.P_ID = Appointments.P_ID where D_ID = " + d_id + "and is_confirmed = 0";
            DataTable dt = data.GetData(cmnd, "Appoints");
            switch (dt.Rows.Count)
            {
                case 1:
                    metroLabel3.Visible = true;
                    metroLabel3.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1];
                    metroLabel3.Location = new Point(metroLabel10.Location.X - (metroLabel3.Width - metroLabel10.Width) / 2, metroLabel3.Location.Y);
                    metroLabel8.Visible = true;
                    metroLabel8.Text = dt.Rows[0][2].ToString();
                    metroLabel8.Location = new Point(metroLabel9.Location.X - (metroLabel8.Width - metroLabel10.Width) / 2, metroLabel8.Location.Y);

                    metroButton3.Visible = true;
                    metroButton4.Visible = true;
                    break;
                case 2:
                    metroLabel3.Visible = true;
                    metroLabel3.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1];
                    metroLabel3.Location = new Point(metroLabel10.Location.X - (metroLabel3.Width - metroLabel10.Width) / 2, metroLabel3.Location.Y);
                    metroLabel4.Visible = true;
                    metroLabel4.Text = dt.Rows[1][0].ToString() + " " + dt.Rows[1][1];
                    metroLabel4.Location = new Point(metroLabel10.Location.X - (metroLabel4.Width - metroLabel10.Width) / 2, metroLabel4.Location.Y);
                    metroLabel8.Visible = true;
                    metroLabel8.Text = dt.Rows[0][2].ToString();
                    metroLabel8.Location = new Point(metroLabel9.Location.X - (metroLabel8.Width - metroLabel9.Width) / 2, metroLabel8.Location.Y);
                    metroLabel11.Visible = true;
                    metroLabel11.Text = dt.Rows[1][2].ToString();
                    metroLabel11.Location = new Point(metroLabel9.Location.X - (metroLabel11.Width - metroLabel9.Width) / 2, metroLabel11.Location.Y);

                    metroButton3.Visible = true;
                    metroButton4.Visible = true;
                    metroButton6.Visible = true;
                    metroButton5.Visible = true;
                    break;
                case 3:
                    metroLabel3.Visible = true;
                    metroLabel3.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1];
                    metroLabel3.Location = new Point(metroLabel10.Location.X - (metroLabel3.Width - metroLabel10.Width) / 2, metroLabel3.Location.Y);
                    metroLabel4.Visible = true;
                    metroLabel4.Text = dt.Rows[1][0].ToString() + " " + dt.Rows[1][1];
                    metroLabel4.Location = new Point(metroLabel10.Location.X - (metroLabel4.Width - metroLabel10.Width) / 2, metroLabel4.Location.Y);
                    metroLabel5.Visible = true;
                    metroLabel5.Text = dt.Rows[2][0].ToString() + " " + dt.Rows[2][1];
                    metroLabel5.Location = new Point(metroLabel10.Location.X - (metroLabel5.Width - metroLabel10.Width) / 2, metroLabel5.Location.Y);
                    metroLabel8.Visible = true;
                    metroLabel8.Text = dt.Rows[0][2].ToString();
                    metroLabel8.Location = new Point(metroLabel9.Location.X - (metroLabel8.Width - metroLabel9.Width) / 2, metroLabel8.Location.Y);
                    metroLabel11.Visible = true;
                    metroLabel11.Text = dt.Rows[1][2].ToString();
                    metroLabel11.Location = new Point(metroLabel9.Location.X - (metroLabel11.Width - metroLabel9.Width) / 2, metroLabel11.Location.Y);
                    metroLabel12.Visible = true;
                    metroLabel12.Text = dt.Rows[2][2].ToString();
                    metroLabel12.Location = new Point(metroLabel9.Location.X - (metroLabel12.Width - metroLabel9.Width) / 2, metroLabel12.Location.Y);
                    
                    metroButton3.Visible = true;
                    metroButton4.Visible = true;
                    metroButton6.Visible = true;
                    metroButton5.Visible = true;
                    metroButton8.Visible = true;
                    metroButton7.Visible = true;
                    break;
                case 4:
                    metroLabel3.Visible = true;
                    metroLabel3.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1];
                    metroLabel3.Location = new Point(metroLabel10.Location.X - (metroLabel3.Width - metroLabel10.Width) / 2, metroLabel3.Location.Y);
                    metroLabel4.Visible = true;
                    metroLabel4.Text = dt.Rows[1][0].ToString() + " " + dt.Rows[1][1];
                    metroLabel4.Location = new Point(metroLabel10.Location.X - (metroLabel4.Width - metroLabel10.Width) / 2, metroLabel4.Location.Y);
                    metroLabel5.Visible = true;
                    metroLabel5.Text = dt.Rows[2][0].ToString() + " " + dt.Rows[2][1];
                    metroLabel5.Location = new Point(metroLabel10.Location.X - (metroLabel5.Width - metroLabel10.Width) / 2, metroLabel5.Location.Y);
                    metroLabel6.Visible = true;
                    metroLabel6.Text = dt.Rows[3][0].ToString() + " " + dt.Rows[3][1];
                    metroLabel6.Location = new Point(metroLabel10.Location.X - (metroLabel6.Width - metroLabel10.Width) / 2, metroLabel6.Location.Y);
                    metroLabel8.Visible = true;
                    metroLabel8.Text = dt.Rows[0][2].ToString();
                    metroLabel8.Location = new Point(metroLabel9.Location.X - (metroLabel8.Width - metroLabel9.Width) / 2, metroLabel8.Location.Y);
                    metroLabel11.Visible = true;
                    metroLabel11.Text = dt.Rows[1][2].ToString();
                    metroLabel11.Location = new Point(metroLabel9.Location.X - (metroLabel11.Width - metroLabel9.Width) / 2, metroLabel11.Location.Y);
                    metroLabel12.Visible = true;
                    metroLabel12.Text = dt.Rows[2][2].ToString();
                    metroLabel12.Location = new Point(metroLabel9.Location.X - (metroLabel12.Width - metroLabel9.Width) / 2, metroLabel12.Location.Y);
                    metroLabel13.Visible = true;
                    metroLabel13.Text = dt.Rows[3][2].ToString();
                    metroLabel13.Location = new Point(metroLabel9.Location.X - (metroLabel13.Width - metroLabel9.Width) / 2, metroLabel13.Location.Y);

                    metroButton3.Visible = true;
                    metroButton4.Visible = true;
                    metroButton6.Visible = true;
                    metroButton5.Visible = true;
                    metroButton8.Visible = true;
                    metroButton7.Visible = true;
                    metroButton10.Visible = true;
                    metroButton9.Visible = true;
                    break;
            }
            Tab.SelectTab(doc_appoints);
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Doctor);
            foreach(Control ctrl in groupBox1.Controls)
            {
                    ctrl.Visible = false;
            }
            string cmnd = "Select D_ID from Doctors where D_email = " + "'" + Login_email.Text + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select P_fName, P_lName, A_Date, R_ID from Doc_Appoints where D_ID = " + id + "and Is_Confirmed = 1 and A_date > '" + DateTime.Now + "'";
            DataTable dt = data.GetData(cmnd, "Appoints");
            metroGrid1.DataSource = dt;
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            string[] t = metroLabel3.Text.Split(' ');
            string cmnd = "Select P_ID from patients where P_fname = " + "'" + t[0] + "'" + "and P_lname = " + "'" + t[1] + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "update Appointments set Is_Confirmed = 1 where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            metroLabel3.Visible = false;
            metroLabel8.Visible = false;
            metroButton3.Visible = false;
            metroButton4.Visible = false;
        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            string[] t = metroLabel4.Text.Split(' ');
            string cmnd = "Select P_ID from patients where P_fname = " + "'" + t[0] + "'" + "and P_lname = " + "'" + t[1] + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "update Appointments set Is_Confirmed = 1 where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            metroLabel4.Visible = false;
            metroLabel11.Visible = false;
            metroButton5.Visible = false;
            metroButton6.Visible = false;
        }

        private void metroButton8_Click(object sender, EventArgs e)
        {
            string[] t = metroLabel5.Text.Split(' ');
            string cmnd = "Select P_ID from patients where P_fname = " + "'" + t[0] + "'" + "and P_lname = " + "'" + t[1] + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "update Appointments set Is_Confirmed = 1 where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            metroLabel5.Visible = false;
            metroLabel12.Visible = false;
            metroButton7.Visible = false;
            metroButton8.Visible = false;
        }

        private void metroButton10_Click(object sender, EventArgs e)
        {
            string[] t = metroLabel6.Text.Split(' ');
            string cmnd = "Select P_ID from patients where P_fname = " + "'" + t[0] + "'" + "and P_lname = " + "'" + t[1] + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "update Appointments set Is_Confirmed = 1 where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            metroLabel6.Visible = false;
            metroLabel13.Visible = false;
            metroButton10.Visible = false;
            metroButton9.Visible = false;
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            string[] t = metroLabel3.Text.Split(' ');
            string cmnd = "Select P_ID from patients where P_fname = " + "'" + t[0] + "'" + "and P_lname = " + "'" + t[1] + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select R_ID from patients where P_ID = " + id;
            int r_id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Update Rooms set R_Availability = 'Available' where R_ID = " + r_id;
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Appointments set is_confirmed = -1 where P_ID = " + id + "and is_confirmed = 0";
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Patients set R_ID = NULL where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Patients set Entrance_date = NULL where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            metroLabel3.Visible = false;
            metroLabel8.Visible = false;
            metroButton3.Visible = false;
            metroButton4.Visible = false;
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            string[] t = metroLabel4.Text.Split(' ');
            string cmnd = "Select P_ID from patients where P_fname = " + "'" + t[0] + "'" + "and P_lname = " + "'" + t[1] + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select R_ID from patients where P_ID = " + id;
            int r_id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Update Rooms set R_Availability = 'Available' where R_ID = " + r_id;
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Appointments set is_confirmed = -1 where P_ID = " + id + "and is_confirmed = 0";
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Patients set R_ID = NULL where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Patients set Entrance_date = NULL where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            metroLabel4.Visible = false;
            metroLabel11.Visible = false;
            metroButton5.Visible = false;
            metroButton6.Visible = false;
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            string[] t = metroLabel5.Text.Split(' ');
            string cmnd = "Select P_ID from patients where P_fname = " + "'" + t[0] + "'" + "and P_lname = " + "'" + t[1] + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select R_ID from patients where P_ID = " + id;
            int r_id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Update Rooms set R_Availability = 'Available' where R_ID = " + r_id;
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Appointments set is_confirmed = -1 where P_ID = " + id + "and is_confirmed = 0";
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Patients set R_ID = NULL where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Patients set Entrance_date = NULL where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            metroLabel5.Visible = false;
            metroLabel12.Visible = false;
            metroButton7.Visible = false;
            metroButton8.Visible = false;
        }

        private void metroButton9_Click(object sender, EventArgs e)
        {
            string[] t = metroLabel6.Text.Split(' ');
            string cmnd = "Select P_ID from patients where P_fname = " + "'" + t[0] + "'" + "and P_lname = " + "'" + t[1] + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select R_ID from patients where P_ID = " + id;
            int r_id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Update Rooms set R_Availability = 'Available' where R_ID = " + r_id;
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Appointments set is_confirmed = -1 where P_ID = " + id + "and is_confirmed = 0";
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Patients set R_ID = NULL where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            cmnd = "Update Patients set Entrance_date = NULL where P_ID = " + id;
            data.ExecuteActionCommand(cmnd);
            metroLabel6.Visible = false;
            metroLabel13.Visible = false;
            metroButton10.Visible = false;
            metroButton9.Visible = false;
        }

        private void metroLabel7_Click(object sender, EventArgs e)
        {
            string cmnd = "Select D_ID from Doctors where D_email = " + "'" + Login_email.Text + "'";
            int d_id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select P_fname , P_lname , A_date from Patients inner join Appointments on Patients.P_ID = Appointments.P_ID where D_ID = " + d_id + "and is_confirmed = 0";
            DataTable dt = data.GetData(cmnd, "Appoints");
            if(dt.Rows.Count > 0)
            {
                metroLabel10.Visible = true;
                metroLabel9.Visible = true;
            }
            else
            {
                label15.Location = new Point((groupBox1.Width - label15.Width) / 2, (groupBox1.Height - label15.Height) / 2);
                label15.Visible = true;
            }
            switch (dt.Rows.Count)
            {
                case 1:
                    metroLabel3.Visible = true;
                    metroLabel3.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1];
                    metroLabel3.Location = new Point(metroLabel10.Location.X - (metroLabel3.Width - metroLabel10.Width) / 2, metroLabel3.Location.Y);
                    metroLabel8.Visible = true;
                    metroLabel8.Text = dt.Rows[0][2].ToString();
                    metroLabel8.Location = new Point(metroLabel9.Location.X - (metroLabel8.Width - metroLabel10.Width) / 2, metroLabel8.Location.Y);

                    metroButton3.Visible = true;
                    metroButton4.Visible = true;
                    break;
                case 2:
                    metroLabel3.Visible = true;
                    metroLabel3.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1];
                    metroLabel3.Location = new Point(metroLabel10.Location.X - (metroLabel3.Width - metroLabel10.Width) / 2, metroLabel3.Location.Y);
                    metroLabel4.Visible = true;
                    metroLabel4.Text = dt.Rows[1][0].ToString() + " " + dt.Rows[1][1];
                    metroLabel4.Location = new Point(metroLabel10.Location.X - (metroLabel4.Width - metroLabel10.Width) / 2, metroLabel4.Location.Y);
                    metroLabel8.Visible = true;
                    metroLabel8.Text = dt.Rows[0][2].ToString();
                    metroLabel8.Location = new Point(metroLabel9.Location.X - (metroLabel8.Width - metroLabel9.Width) / 2, metroLabel8.Location.Y);
                    metroLabel11.Visible = true;
                    metroLabel11.Text = dt.Rows[1][2].ToString();
                    metroLabel11.Location = new Point(metroLabel9.Location.X - (metroLabel11.Width - metroLabel9.Width) / 2, metroLabel11.Location.Y);

                    metroButton3.Visible = true;
                    metroButton4.Visible = true;
                    metroButton6.Visible = true;
                    metroButton5.Visible = true;
                    break;
                case 3:
                    metroLabel3.Visible = true;
                    metroLabel3.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1];
                    metroLabel3.Location = new Point(metroLabel10.Location.X - (metroLabel3.Width - metroLabel10.Width) / 2, metroLabel3.Location.Y);
                    metroLabel4.Visible = true;
                    metroLabel4.Text = dt.Rows[1][0].ToString() + " " + dt.Rows[1][1];
                    metroLabel4.Location = new Point(metroLabel10.Location.X - (metroLabel4.Width - metroLabel10.Width) / 2, metroLabel4.Location.Y);
                    metroLabel5.Visible = true;
                    metroLabel5.Text = dt.Rows[2][0].ToString() + " " + dt.Rows[2][1];
                    metroLabel5.Location = new Point(metroLabel10.Location.X - (metroLabel5.Width - metroLabel10.Width) / 2, metroLabel5.Location.Y);
                    metroLabel8.Visible = true;
                    metroLabel8.Text = dt.Rows[0][2].ToString();
                    metroLabel8.Location = new Point(metroLabel9.Location.X - (metroLabel8.Width - metroLabel9.Width) / 2, metroLabel8.Location.Y);
                    metroLabel11.Visible = true;
                    metroLabel11.Text = dt.Rows[1][2].ToString();
                    metroLabel11.Location = new Point(metroLabel9.Location.X - (metroLabel11.Width - metroLabel9.Width) / 2, metroLabel11.Location.Y);
                    metroLabel12.Visible = true;
                    metroLabel12.Text = dt.Rows[2][2].ToString();
                    metroLabel12.Location = new Point(metroLabel9.Location.X - (metroLabel12.Width - metroLabel9.Width) / 2, metroLabel12.Location.Y);

                    metroButton3.Visible = true;
                    metroButton4.Visible = true;
                    metroButton6.Visible = true;
                    metroButton5.Visible = true;
                    metroButton8.Visible = true;
                    metroButton7.Visible = true;
                    break;
                case 4:
                    metroLabel3.Visible = true;
                    metroLabel3.Text = dt.Rows[0][0].ToString() + " " + dt.Rows[0][1];
                    metroLabel3.Location = new Point(metroLabel10.Location.X - (metroLabel3.Width - metroLabel10.Width) / 2, metroLabel3.Location.Y);
                    metroLabel4.Visible = true;
                    metroLabel4.Text = dt.Rows[1][0].ToString() + " " + dt.Rows[1][1];
                    metroLabel4.Location = new Point(metroLabel10.Location.X - (metroLabel4.Width - metroLabel10.Width) / 2, metroLabel4.Location.Y);
                    metroLabel5.Visible = true;
                    metroLabel5.Text = dt.Rows[2][0].ToString() + " " + dt.Rows[2][1];
                    metroLabel5.Location = new Point(metroLabel10.Location.X - (metroLabel5.Width - metroLabel10.Width) / 2, metroLabel5.Location.Y);
                    metroLabel6.Visible = true;
                    metroLabel6.Text = dt.Rows[3][0].ToString() + " " + dt.Rows[3][1];
                    metroLabel6.Location = new Point(metroLabel10.Location.X - (metroLabel6.Width - metroLabel10.Width) / 2, metroLabel6.Location.Y);
                    metroLabel8.Visible = true;
                    metroLabel8.Text = dt.Rows[0][2].ToString();
                    metroLabel8.Location = new Point(metroLabel9.Location.X - (metroLabel8.Width - metroLabel9.Width) / 2, metroLabel8.Location.Y);
                    metroLabel11.Visible = true;
                    metroLabel11.Text = dt.Rows[1][2].ToString();
                    metroLabel11.Location = new Point(metroLabel9.Location.X - (metroLabel11.Width - metroLabel9.Width) / 2, metroLabel11.Location.Y);
                    metroLabel12.Visible = true;
                    metroLabel12.Text = dt.Rows[2][2].ToString();
                    metroLabel12.Location = new Point(metroLabel9.Location.X - (metroLabel12.Width - metroLabel9.Width) / 2, metroLabel12.Location.Y);
                    metroLabel13.Visible = true;
                    metroLabel13.Text = dt.Rows[3][2].ToString();
                    metroLabel13.Location = new Point(metroLabel9.Location.X - (metroLabel13.Width - metroLabel9.Width) / 2, metroLabel13.Location.Y);

                    metroButton3.Visible = true;
                    metroButton4.Visible = true;
                    metroButton6.Visible = true;
                    metroButton5.Visible = true;
                    metroButton8.Visible = true;
                    metroButton7.Visible = true;
                    metroButton10.Visible = true;
                    metroButton9.Visible = true;
                    break;
                default:
                    
                    break;
            }
            Tab.SelectTab(doc_appoints);
        }

        private void pictureBox27_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Login);
            Login_email.Text = "";
            Login_pass.Text = "";
        }

        private void pictureBox28_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Login);
            Login_email.Text = "";
            Login_pass.Text = "";
            Signup_panel.Visible = false;
            Login_Panel.Visible = true;
        }

        private void check_pay_Click(object sender, EventArgs e)
        {
            string cmnd = "Select P_ID from Patients where P_email = " + "'" + Login_email.Text + "'";
            int id = int.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select M_Price from Medicine inner join Treatments on Medicine.M_ID = Treatments.M_ID where P_ID = " + id;
            DataTable dt = data.GetData(cmnd, "medicine");
            float amount = 0;
            foreach (DataRow dr in dt.Rows)
                amount += float.Parse(dr[0].ToString());
            cmnd = "Select R_price from patients inner join rooms on patients.R_ID = rooms.R_ID where P_ID = " + id;
            float r_price = float.Parse(data.GetValue(cmnd).ToString());
            cmnd = "Select Entrance_date from patients where P_email = '" + Login_email.Text + "'";
            DateTime t1 = DateTime.Parse(data.GetValue(cmnd).ToString()), t2 = DateTime.Now;
            int day_diff = (int)(t2 - t1).TotalDays;
            amount += day_diff * r_price;
            MetroFramework.MetroMessageBox.Show(this, "Your payment amount is " + amount.ToString() + " $", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox30_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Patient);
        }

        private void add_treat_Psearch_Click(object sender, EventArgs e)
        {
            string cmnd = "Select P_ID from Patients where P_fname = '" + add_treat_Pfname.Text + "' and P_lname = '" + add_treat_Plname.Text + "'";
            DataTable dt = data.GetData(cmnd, "ID");
            if(dt.Rows.Count == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Patient Not Found!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                label19.Text += dt.Rows[0][0].ToString();
            }
        }

        private void add_treat_Dsearch_Click(object sender, EventArgs e)
        {
            string cmnd = "Select D_ID from Doctors where D_fname = '" + add_treat_Dfname.Text + "' and D_lname = '" + add_treat_Dlname.Text + "'";
            DataTable dt = data.GetData(cmnd, "ID");
            if (dt.Rows.Count == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Doctor Not Found!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                label22.Text += dt.Rows[0][0].ToString();
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            string cmnd = "Select M_ID from Medicine where M_name = '" + add_treat_Mname.Text + "'";
            DataTable dt = data.GetData(cmnd, "ID");
            if (dt.Rows.Count == 0)
            {
                MetroFramework.MetroMessageBox.Show(this, "Medicament Not Found!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                label24.Text += dt.Rows[0][0].ToString();
            }
        }

        private void treat_add_Click(object sender, EventArgs e)
        {
            if(Check("Patients","P_ID",add_treat_P_ID.Text,false) && Check("Doctors", "D_ID", add_treat_D_ID.Text, false) && Check("Medicine", "M_ID", add_treat_M_ID.Text, false))
            {
                string cmnd = "Insert into Treatments values (" + add_treat_P_ID.Text + "," + add_treat_D_ID.Text + "," + add_treat_M_ID.Text + ",'" + treat_duration.Text + "')";
                int rep = data.ExecuteActionCommand(cmnd);
                if(rep != 0)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Informations are saved! Thank you.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    add_treat_Pfname.Text = "";
                    add_treat_Plname.Text = "";
                    add_treat_Dfname.Text = "";
                    add_treat_Dlname.Text = "";
                    add_treat_Mname.Text = "";
                    add_treat_P_ID.Text = "";
                    add_treat_D_ID.Text = "";
                    add_treat_M_ID.Text = "";
                    treat_duration.Text = "";
                    label19.Text = "ID : ";
                    label22.Text = "ID : ";
                    label24.Text = "ID : ";
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Can not insert same medicament to the same patient!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Please check informations", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Login);
            Login_email.Text = "";
            Login_pass.Text = "";
        }

        private void pictureBox36_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Login);
            Login_email.Text = "";
            Login_pass.Text = "";
        }

        private void new_doc_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(adm_add_doc_emp);
            add_emp.Visible = false;
            add_doc.Visible = true;
            doc_fname1.Text = "";
            doc_lname1.Text = "";
            doc_email1.Text = "";
            doc_pass1.Text = "";
            doc_numb1.Text = "";
            doc_address1.Text = "";
            doc_spec1.Text = "";
            doc_day.Text = "";
            doc_month.Text = "";
            doc_year.Text = "";
        }

        private void add_doc_save_Click(object sender, EventArgs e)
        {
            bool ok = true;
            string cmnd, date;
            int mo = 0;


            foreach (Control ctrl in add_doc.Controls)
            {
                if (ctrl.Text == "")
                {
                    ok = false;
                    break;
                }
            }
            if (!ok)
            {
                MetroFramework.MetroMessageBox.Show(this, "Please enter all informations", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string email = doc_email1.Text;
                if (Check("Users", "U_email", email, true))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Email is already taken! Please choose another username.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    switch (doc_month.Text)
                    {
                        case "January":
                            mo = 1;
                            break;
                        case "February":
                            mo = 2;
                            break;
                        case "March":
                            mo = 3;
                            break;
                        case "April":
                            mo = 4;
                            break;
                        case "May":
                            mo = 5;
                            break;
                        case "June":
                            mo = 6;
                            break;
                        case "July":
                            mo = 7;
                            break;
                        case "August":
                            mo = 8;
                            break;
                        case "September":
                            mo = 9;
                            break;
                        case "October":
                            mo = 10;
                            break;
                        case "November":
                            mo = 11;
                            break;
                        case "December":
                            mo = 12;
                            break;
                    }
                    date = mo.ToString() + "-" + doc_day.Text + "-" + doc_year.Text;
                    cmnd = "Insert into Doctors values ('" + doc_fname1.Text + "','" + doc_lname1.Text + "','" + date + "'," + doc_spec1.Text + ",'" + doc_email1.Text + "'," + doc_numb1.Text + ",'" + doc_address1.Text + "')";
                    int rep1 = data.ExecuteActionCommand(cmnd);
                    string pass = CryptoEngine.Encrypt(doc_pass1.Text);
                    cmnd = "Insert into Users values ('" + doc_email1.Text + "','" + pass + "','Doctor', NULL)";
                    int rep2 = data.ExecuteActionCommand(cmnd);
                    if(rep1 !=0 && rep2 != 0)
                    {
                        doc_fname1.Text = "";
                        doc_lname1.Text = "";
                        doc_email1.Text = "";
                        doc_pass1.Text = "";
                        doc_numb1.Text = "";
                        doc_address1.Text = "";
                        doc_spec1.Text = "";
                        doc_day.Text = "";
                        doc_month.Text = "";
                        doc_year.Text = "";
                        MetroFramework.MetroMessageBox.Show(this, "Doctor added successfully.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void emp_save_Click(object sender, EventArgs e)
        {
            bool ok = true;
            string cmnd, date;
            int mo = 0;


            foreach (Control ctrl in add_emp.Controls)
            {
                if (ctrl.Text == "")
                {
                    ok = false;
                    break;
                }
            }
            if (!ok)
            {
                MetroFramework.MetroMessageBox.Show(this, "Please enter all informations", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (Check("Employees", "E_email", emp_email1.Text, true))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Email is already taken! Please choose another username.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    switch (emp_month.Text)
                    {
                        case "January":
                            mo = 1;
                            break;
                        case "February":
                            mo = 2;
                            break;
                        case "March":
                            mo = 3;
                            break;
                        case "April":
                            mo = 4;
                            break;
                        case "May":
                            mo = 5;
                            break;
                        case "June":
                            mo = 6;
                            break;
                        case "July":
                            mo = 7;
                            break;
                        case "August":
                            mo = 8;
                            break;
                        case "September":
                            mo = 9;
                            break;
                        case "October":
                            mo = 10;
                            break;
                        case "November":
                            mo = 11;
                            break;
                        case "December":
                            mo = 12;
                            break;
                    }
                    date = mo.ToString() + "-" + emp_day.Text + "-" + emp_year.Text;
                    cmnd = "Insert into Employees values ('" + emp_fname1.Text + "','" + emp_lname1.Text + "','" + date + "','" + emp_email1.Text + "'," + emp_nmbr1.Text + ",'" + emp_addr1.Text + "'," + emp_salary1.Text + ")";
                    int rep1 = data.ExecuteActionCommand(cmnd);
                    string pass = CryptoEngine.Encrypt(emp_pass1.Text);
                    cmnd = "Insert into Users values ('" + emp_email1.Text + "','" + pass + "','Employee', NULL)";
                    int rep2 = data.ExecuteActionCommand(cmnd);
                    if (rep1 != 0 && rep2 != 0)
                    {
                        emp_fname1.Text = "";
                        emp_lname1.Text = "";
                        emp_email1.Text = "";
                        emp_pass1.Text = "";
                        emp_nmbr1.Text = "";
                        emp_addr1.Text = "";
                        emp_salary1.Text = "";
                        emp_day.Text = "";
                        emp_month.Text = "";
                        emp_year.Text = "";
                        MetroFramework.MetroMessageBox.Show(this, "Employee added successfully.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void pictureBox39_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Admin);
        }

        private void new_emp_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(adm_add_doc_emp);
            add_emp.Visible = true;
            add_doc.Visible = false;
            emp_fname1.Text = "";
            emp_lname1.Text = "";
            emp_email1.Text = "";
            emp_pass1.Text = "";
            emp_nmbr1.Text = "";
            emp_addr1.Text = "";
            emp_salary1.Text = "";
            emp_day.Text = "";
            emp_month.Text = "";
            emp_year.Text = "";
        }

        private void ad_patient_Click(object sender, EventArgs e)
        {
            string cmnd = "Select * from Patients";
            DataTable dt = data.GetData(cmnd, "Patients");
            adm_pa_grid.DataSource = dt;
            Tab.SelectTab(adm_patient);
        }

        private void src_by_pa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmnd = "Select * from patients";
            adm_pa_grid.DataSource = data.GetData(cmnd, "Patients");
            foreach (Control ctrl in panel18.Controls)
                ctrl.Text = "";
            foreach (Control ctrl in panel17.Controls)
                ctrl.Text = "";
            adm_pa_src_ID.Text = "";

            switch (src_by_pa.Text)
            {
                case "ID":
                    adm_pa_src_ID.Visible = true;
                    panel17.Visible = false;
                    panel18.Visible = false;
                    adm_pa_src_ID.Location = new Point(src_by_pa.Location.X + src_by_pa.Width + 100, src_by_pa.Location.Y);
                    adm_pa_src_but.Location = new Point(adm_pa_src_ID.Location.X + (adm_pa_src_ID.Width - adm_pa_src_but.Width) / 2, adm_pa_src_ID.Location.Y + adm_pa_src_ID.Height + 3);
                    break;
                case "Name":
                    panel18.Visible = true;
                    adm_pa_src_ID.Visible = false;
                    panel17.Visible = false;
                    panel18.Location = new Point(src_by_pa.Location.X + src_by_pa.Width + 100, src_by_pa.Location.Y - 6);
                    adm_pa_src_but.Location = new Point(panel18.Location.X + (panel18.Width - adm_pa_src_but.Width) / 2, panel18.Location.Y + panel18.Height + 3);
                    break;
                case "Entrance date":
                    panel17.Visible = true;
                    adm_pa_src_ID.Visible = false;
                    panel18.Visible = false;
                    panel17.Location = new Point(src_by_pa.Location.X + src_by_pa.Width + 100, src_by_pa.Location.Y);
                    adm_pa_src_but.Location = new Point(panel17.Location.X + (panel17.Width - adm_pa_src_but.Width) / 2, panel17.Location.Y + panel17.Height + 3);
                    break;
            }
            adm_pa_src_but.Visible = true;
        }

        private void adm_pa_src_but_Click(object sender, EventArgs e)
        {
            string cmnd = "", date = "";
            int mo = 0;
            switch (adm_p_search_month.Text)
            {
                case "January":
                    mo = 1;
                    break;
                case "February":
                    mo = 2;
                    break;
                case "March":
                    mo = 3;
                    break;
                case "April":
                    mo = 4;
                    break;
                case "May":
                    mo = 5;
                    break;
                case "June":
                    mo = 6;
                    break;
                case "July":
                    mo = 7;
                    break;
                case "August":
                    mo = 8;
                    break;
                case "September":
                    mo = 9;
                    break;
                case "October":
                    mo = 10;
                    break;
                case "November":
                    mo = 11;
                    break;
                case "December":
                    mo = 12;
                    break;
            }
            if (panel17.Visible == true)
            {
                foreach(Control ctrl in panel17.Controls)
                    if(ctrl.Text == "")
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Please enter all informations.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                date = "'" + mo.ToString() + "-" + adm_p_search_day.Text + "-" + adm_p_search_year.Text + "'";
                cmnd = "Select * from patients where entrance_date = " + date;
                adm_pa_grid.DataSource = data.GetData(cmnd, "table");

            }
            if(panel18.Visible == true)
            {
                foreach (Control ctrl in panel18.Controls)
                    if (ctrl.Text == "")
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Please enter all informations.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                cmnd = "Select * from Patients where P_fname = '" + adm_pa_src_fname.Text + "' and P_lname = '" + adm_pa_src_lname.Text + "'";
                adm_pa_grid.DataSource = data.GetData(cmnd, "table");
            }
            if(adm_pa_src_ID.Visible == true)
            {
                if(adm_pa_src_ID.Text == "")
                {
                    MetroFramework.MetroMessageBox.Show(this, "Please enter all informations.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cmnd = "Select * from Patients where P_ID = " + adm_pa_src_ID.Text;
                adm_pa_grid.DataSource = data.GetData(cmnd, "table");
            }
        }

        private void pictureBox42_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Admin);
            panel17.Visible = false;
            panel18.Visible = false;
            adm_pa_src_ID.Visible = false;
            adm_pa_src_but.Visible = false;
        }

        private void ad_doc_Click(object sender, EventArgs e)
        {
            string cmnd = "Select * from Adm_doc";
            DataTable dt = data.GetData(cmnd, "Doctors");
            adm_dr_grid.DataSource = dt;
            Tab.SelectTab(adm_doctor);
            cmnd = "Select C_name from categories";
            DataTable dtt = data.GetData(cmnd, "table");
            foreach (DataRow dr in dtt.Rows)
                adm_dr_search_cat.Items.Add(dr[0]);
        }

        private void src_by_dr_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmnd = "Select * from adm_doc";
            adm_dr_grid.DataSource = data.GetData(cmnd, "Doctors");
            foreach (Control ctrl in panel20.Controls)
                ctrl.Text = "";
            adm_dr_search_cat.Text = "";
            adm_dr_src_ID.Text = "";

            switch (src_by_dr.Text)
            {
                case "ID":
                    adm_dr_src_ID.Visible = true;
                    panel20.Visible = false;
                    adm_dr_search_cat.Visible = false;
                    adm_dr_src_ID.Location = new Point(src_by_dr.Location.X + src_by_dr.Width + 100, src_by_dr.Location.Y);
                    adm_dr_src_but.Location = new Point(adm_dr_src_ID.Location.X + (adm_dr_src_ID.Width - adm_dr_src_but.Width) / 2, adm_dr_src_ID.Location.Y + adm_dr_src_ID.Height + 3);
                    break;
                case "Name":
                    panel20.Visible = true;
                    adm_dr_src_ID.Visible = false;
                    adm_dr_search_cat.Visible = false;
                    panel20.Location = new Point(src_by_dr.Location.X + src_by_dr.Width + 100, src_by_dr.Location.Y - 6);
                    adm_dr_src_but.Location = new Point(panel20.Location.X + (panel20.Width - adm_dr_src_but.Width) / 2, panel20.Location.Y + panel20.Height + 3);
                    break;
                case "Category":
                    adm_dr_search_cat.Visible = true;
                    adm_dr_src_ID.Visible = false;
                    panel20.Visible = false;
                    adm_dr_search_cat.Location = new Point(src_by_dr.Location.X + src_by_dr.Width + 100, src_by_dr.Location.Y);
                    adm_dr_src_but.Location = new Point(adm_dr_search_cat.Location.X + (adm_dr_search_cat.Width - adm_dr_src_but.Width) / 2, adm_dr_search_cat.Location.Y + adm_dr_search_cat.Height + 3);
                    break;
            }
            adm_dr_src_but.Visible = true;
        }

        private void adm_dr_src_but_Click(object sender, EventArgs e)
        {
            string cmnd = "";

            if (panel20.Visible == true)
            {
                foreach (Control ctrl in panel20.Controls)
                    if (ctrl.Text == "")
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Please enter all informations.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                cmnd = "Select * from adm_doc where D_fname = '" + adm_dr_src_fname.Text + "' and D_lname = '" + adm_dr_src_lname.Text + "'";
                adm_dr_grid.DataSource = data.GetData(cmnd, "table");
            }
            if (adm_dr_search_cat.Visible == true)
            {
               if(adm_dr_search_cat.Text == "")
               {
                    MetroFramework.MetroMessageBox.Show(this, "Please enter all informations.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
               }
                cmnd = "Select * from adm_doc where C_name = '" + adm_dr_search_cat.Text + "'";
                adm_dr_grid.DataSource = data.GetData(cmnd, "table");
            }
            if (adm_dr_src_ID.Visible == true)
            {
                if (adm_dr_src_ID.Text == "")
                {
                    MetroFramework.MetroMessageBox.Show(this, "Please enter all informations.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cmnd = "Select * from adm_doc where D_ID = " + adm_dr_src_ID.Text;
                adm_dr_grid.DataSource = data.GetData(cmnd, "table");
            }
        }

        private void pictureBox45_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Admin);
            panel20.Visible = false;
            adm_dr_search_cat.Visible = false;
            adm_dr_src_ID.Visible = false;
            adm_dr_src_but.Visible = false;
        }

        private void ad_med_Click(object sender, EventArgs e)
        {
            string cmnd = "Select * from adm_med";
            DataTable dt = data.GetData(cmnd, "Patients");
            adm_med_grid.DataSource = dt;
            Tab.SelectTab(adm_med);
        }

        private void src_by_med_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmnd = "Select * from Adm_med";
            adm_dr_grid.DataSource = data.GetData(cmnd, "Medicine");
            double amount = 0;

            switch (src_by_med.Text)
            {
                case "Cheapest":
                    string cmd = "Select * from Adm_med order by M_Price";
                    adm_med_grid.DataSource = data.GetData(cmd, "Medicine");
                    break;
                case "Most Expensive":
                    string com = "Select * from Adm_med order by M_Price desc";
                    adm_med_grid.DataSource = data.GetData(com, "Medicine");
                    break;
                case "Total Price":
                    string comm = "Select M_Quantity, M_Price from Adm_med";
                    DataTable dt = data.GetData(comm, "Tot_Price");
                    foreach(DataRow dr in dt.Rows)
                    {
                        amount += (double.Parse(dr[0].ToString())) * (double.Parse(dr[1].ToString()));
                    }
                    MetroFramework.MetroMessageBox.Show(this, "The Total Price of Drugs is: " + amount.ToString(), "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void pictureBox48_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Admin);
        }

        private void ad_rooms_Click(object sender, EventArgs e)
        {
            string cmnd = "Select * from Adm_room";
            adm_dr_grid.DataSource = data.GetData(cmnd, "Rooms");
            Tab.SelectTab(adm_room);
            cmnd = "Select C_name from categories";
            DataTable dtt = data.GetData(cmnd, "table");
            foreach (DataRow dr in dtt.Rows)
            adm_room_search_cat.Items.Add(dr[0]);

            string com = "Select * from adm_room";
            adm_room_grid.DataSource = data.GetData(com, "Rooms"); ;
            cmnd = "Select distinct R_type from Rooms";
            DataTable dt = data.GetData(cmnd, "table");
            foreach (DataRow dr in dt.Rows)
                adm_room_search_type.Items.Add(dr[0]);
            Tab.SelectTab(adm_room);
            adm_rooms_av_num.Visible = false;
        }

        private void src_by_room_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmd;

            adm_room_search_cat.Visible = false;
            adm_room_search_type.Visible = false;

            switch (src_by_room.Text)
            {
                case "Lowest Price":
                    cmd = "Select * from Adm_room order by R_Price";
                    adm_room_grid.DataSource = data.GetData(cmd, "Rooms");
                    adm_rooms_av_num.Visible = false;
                    break;
                case "Available Rooms":
                    cmd = "Select * from Adm_room where R_Availability='Available'";
                    adm_room_grid.DataSource = data.GetData(cmd, "Rooms");
                    adm_rooms_av_num.Visible = false;
                    break;
                case "Category":
                    adm_room_search_type.Visible = false; ;
                    adm_room_search_cat.Visible = true;
                    adm_rooms_av_num.Visible = false;
                    break;
                case "Type":
                    adm_room_search_type.Visible = true;
                    adm_room_search_cat.Visible = false;
                    adm_rooms_av_num.Visible = true;
                    adm_rooms_av_num.Location = new Point(adm_room_search_type.Location.X + adm_room_search_type.Width + 20, adm_room_search_type.Location.Y);
                    break;
            }
        }

        private void adm_room_search_cat_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmnd = "Select * from adm_room where C_name  = '" + adm_room_search_cat.Text + "'";
            adm_room_grid.DataSource = data.GetData(cmnd, "table");
        }

        private void adm_room_search_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmnd = "Select * from adm_room where R_type = '" + adm_room_search_type.Text + "'";
            adm_room_grid.DataSource = data.GetData(cmnd, "type");
        }

        private void metroButton11_Click(object sender, EventArgs e)
        {
            string cmnd = "", date;
            foreach(Control ctrl in panel24.Controls)
                if (ctrl.Text == "")
                {
                    MetroFramework.MetroMessageBox.Show(this, "Please enter all informations ", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            int mo = 0;
            switch (adm_ap_search_month.Text)
            {
                case "January":
                    mo = 1;
                    break;
                case "February":
                    mo = 2;
                    break;
                case "March":
                    mo = 3;
                    break;
                case "April":
                    mo = 4;
                    break;
                case "May":
                    mo = 5;
                    break;
                case "June":
                    mo = 6;
                    break;
                case "July":
                    mo = 7;
                    break;
                case "August":
                    mo = 8;
                    break;
                case "September":
                    mo = 9;
                    break;
                case "October":
                    mo = 10;
                    break;
                case "November":
                    mo = 11;
                    break;
                case "December":
                    mo = 12;
                    break;
            }
            date = mo.ToString() + "-" + adm_ap_search_day.Text + "-" + adm_ap_search_year.Text;
            cmnd = "Select * from adm_appoints where A_date = '" + date + "'";
            adm_appoints_grid.DataSource = data.GetData(cmnd, "table");
        }

        private void ad_appoi_Click(object sender, EventArgs e)
        {
            string cmnd = "Select * from adm_appoints";
            adm_appoints_grid.DataSource = data.GetData(cmnd, "Rooms");
            Tab.SelectTab(adm_appoints);
        }

        private void pictureBox53_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Admin);
            adm_room_search_cat.Visible = false;
            adm_room_search_type.Visible = false;
        }

        private void pictureBox54_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Admin);
        }

        private void ad_users_Click(object sender, EventArgs e)
        {
            string cmnd = "Select * from users";
            adm_users_grid.DataSource = data.GetData(cmnd, "Rooms");
            Tab.SelectTab(adm_users);
        }

        private void metroButton12_Click(object sender, EventArgs e)
        {
            string cmnd;
            foreach(Control ctrl in panel26.Controls)
                if(ctrl.Text == "")
                {
                    MetroFramework.MetroMessageBox.Show(this, "Please enter all informations.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            if(!Check("Users","U_email",adm_user_email.Text,true))
            {
                MetroFramework.MetroMessageBox.Show(this, "User not found!", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                cmnd = "Delete from Users where U_email = '" + adm_user_email.Text + "'";
                data.ExecuteActionCommand(cmnd);
                MetroFramework.MetroMessageBox.Show(this, "User is deleted successfully.", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmnd = "Select * from users";
                adm_users_grid.DataSource = data.GetData(cmnd, "Rooms");
            }
        }

        private void pictureBox57_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Admin);
            adm_user_email.Text = "";
        }

        private void ad_inv_Click(object sender, EventArgs e)
        {
            string cmnd = "Select * from adm_inv";
            adm_appoints_grid.DataSource = data.GetData(cmnd, "invoices");
            Tab.SelectTab(adm_inv);
        }

        private void src_by_inv_SelectedIndexChanged(object sender, EventArgs e)
        {
            adm_inv_year_select.Visible = true;
            string cmnd = "Select * from adm_inv";
            adm_inv_grid.DataSource = data.GetData(cmnd, "inv");
        }

        private void adm_inv_year_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmnd;
            double amount = 0;
            switch (src_by_inv.Text)
            {
                case "After this year":
                    cmnd = "Select * from adm_inv where year(inv_date) > " + adm_inv_year_select.Text + "order by year(inv_date)";
                    adm_inv_grid.DataSource = data.GetData(cmnd, "inv");
                    break;
                case "Year":
                    cmnd = "Select * from adm_inv where year(inv_date) = " + adm_inv_year_select.Text;
                    adm_inv_grid.DataSource = data.GetData(cmnd, "inv");
                    break;
                case "Total invoices amount in a year":
                    cmnd = "Select inv_amount from invoices where year(inv_date) = " + adm_inv_year_select.Text;
                    DataTable dt = data.GetData(cmnd, "inv");
                    foreach (DataRow dr in dt.Rows)
                        amount += double.Parse(dr[0].ToString());
                    MetroFramework.MetroMessageBox.Show(this, "Total amount in year " + adm_inv_year_select.Text + " is " + amount.ToString() + "$", "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void pictureBox60_Click(object sender, EventArgs e)
        {
            Tab.SelectTab(Admin);
            adm_inv_year_select.Visible = false;
        }

        private void srg_new_app_Click(object sender, EventArgs e)
        {
            label3.Visible = true;
            foreach (Control ctrl in Srg_info.Controls)
            {
                ctrl.Visible = true;
                ctrl.Enabled = true;
            }
            label27.Visible = false;
            label27.Enabled = true;
            label3.Visible = true;
            textBox1.Visible = false;
            textBox1.Enabled = true;
            srg_docID1.Text = "";
            label36.Visible = false;
            srg_new_app.Visible = false;
            string cmnd = "Select P_ID from Patients where P_email = '" + Login_email.Text + "'";
            string id = data.GetValue(cmnd).ToString();
            cmnd = "Delete from Appointments where P_ID = " + id + "and is_confirmed = -1";
            data.ExecuteActionCommand(cmnd);
        }

        private void adm_rooms_av_num_Click(object sender, EventArgs e)
        {
            if(adm_room_search_type.Text != "")
            {
                object[,] P = new object[2, 1];
                P[0, 0] = "@p";
                P[1, 0] = adm_room_search_type.Text;
                int num = int.Parse(data.newGetValue("Nb_Rooms", P).ToString());
                MetroFramework.MetroMessageBox.Show(this, "The number of available rooms is " + num.ToString() , "HMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void another_method_Click(object sender, EventArgs e)
        {
            if (another_method.Text == "Another recovery method")
            {
                recovery_msg.Text = "Please insert the recovery email associated with your account in order to confirm your recovery";
                recovery_num.Visible = false;
                recov_email.Location = recovery_num.Location;
                recov_email.Visible = true;
                recov_email.Text = "";
                another_method.Text = "Back";
                Back_forgot_log.Enabled = false;
                Back_forgot_log.Visible = false;
            }
            else
            {
                recovery_msg.Text = "Please insert the number associated with your account in order to confirm your recovery";
                recov_email.Visible = false;
                recovery_num.Visible = true;
                recovery_num.Text = "";
                another_method.Text = "Another recovery method";
                Back_forgot_log.Enabled = true;
                Back_forgot_log.Visible = true;
            }
        }

        private void treat_exit_Click(object sender, EventArgs e)
        {
            if (treat_exit.Text == "Patient Exit")
            {
                add_Ptreatment.Visible = false;
                patient_exit.Visible = true;
                treat_exit.Text = "Back";
                treat_exit.Location = new Point(patient_exit.Location.X, patient_exit.Location.Y + patient_exit.Height + 10);
            }
            else
            {
                add_Ptreatment.Visible = true;
                patient_exit.Visible = false;
                treat_exit.Text = "Patient Exit";
                treat_exit.Location = new Point(add_Ptreatment.Location.X, add_Ptreatment.Location.Y + add_Ptreatment.Height + 10);
            }
        }
    }
}
