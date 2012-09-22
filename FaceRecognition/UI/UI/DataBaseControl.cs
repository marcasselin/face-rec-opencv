﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class DataBaseControl : Form
    {
        List<string> imagePaths;
        bool imageAdded = false;
        string prevText;
        int countP;
        int globalIndex;

        public DataBaseControl(string name,List<string> imagePath)
        {
            InitializeComponent();
            lbl_name.Text = name;
            imagePaths = new List<string>(imagePath);
            //imagePaths = imagePath;
            GeniratePicBox(imagePath);
        }

        private void GeniratePicBox(List<string> imagePath)
        {
            int count = imagePath.Count;
            string picName = "PictureBox";
            int num = 1, x = 14, y = 14, width = 168, height = 159;
            

            for (int i = 0; i < count; i++)
            {
                PictureBox varBox = new PictureBox();
                varBox.Width = width;
                varBox.Height = height;
                Point pos = new Point(x, y);
                varBox.Location = pos;
                varBox.Image = new Bitmap(imagePath[i]);
                string name = picName + num;
                varBox.SizeMode = PictureBoxSizeMode.StretchImage;

                if (num == 1)
                {
                    varBox.BorderStyle = BorderStyle.Fixed3D;
                    panel1.Controls.Add(varBox);
                    varBox.Name = name;
                    num++;
                }

                else if ((num-1) % 5 != 0)
                {
                    varBox.Width = 80;
                    varBox.Height = 77;
                    varBox.Location = pos;
                    varBox.MouseMove += lbl_pic_Click;
                    panel_maiPanel.Controls.Add(varBox);
                    varBox.Name = name;
                    num++;
                    x += 103;
                }

                else
                {
                    varBox.Width = 80;
                    varBox.Height = 77;
                    varBox.Location = pos; 
                    varBox.MouseMove += lbl_pic_Click;
                    panel_maiPanel.Controls.Add(varBox);
                    varBox.Name = name;
                    num++;
                    y += 99;
                    x = 14;
                }
            } 
        }

        private void lbl_pic_Click(object sender,  MouseEventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            picBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Point pos = picBox.Location;
            globalIndex = int.Parse(picBox.Name.Substring(10));
            if (pos.X >= 426)
            {
                pos.X -= 90;
            }

            while ((pos.Y + picBox_Large.Height+10) > panel1.Height)
            {
                pos.Y -= 10;
            }
            picBox_Large.Location = pos;
            picBox_Large.Image = picBox.Image;
            picBox_Large.BorderStyle = BorderStyle.Fixed3D;
            Point lblPos = new Point(pos.X, pos.Y);
            lblPos.X += 46;
            lblPos.Y += 156;
            lbl_profilePic.Location = lblPos;
            lbl_profilePic.Show();
            picBox_Large.Show();
        }

        private void lbl_Remove_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            int index = int.Parse(lbl.Name.Substring(5));
            if (imagePaths.Count > 1)
            {
                Form1.db.deleltedImages.Add(imagePaths[index - 1]);
                imagePaths.RemoveAt(index - 1);
                imageAdded = true;
                RemoveAllPictureBox();
                RemoveAllLabels();
                GeniratePicBox(imagePaths);
                SetLabels();
            }
            else
                MessageBox.Show("Sorry, you can't remove all pictures");
        }

        private void RemoveAllPictureBox()
        {
            foreach (Panel panel in Controls)
            {
                for (int i = 0; i < panel.Controls.Count; i++)
                {
                    if (panel.Controls[i].GetType() == typeof(PictureBox) && panel.Controls[i].Name != this.picBox_close.Name && panel.Controls[i].Name != this.picBox_Large.Name)
                    {
                        panel.Controls.Remove(panel.Controls[i]);
                        i--;
                    }
                }
            }
        }

        private void picBox_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_AddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> images = openFileDialog1.FileNames.ToList();

                for (int i = 0; i < images.Count; i++)
                {
                    imagePaths.Add(images[i]);
                }

                imageAdded = true;
            }
        }

        private void btn_done_Click(object sender, EventArgs e)
        {
            bool nameEdit = true, imageEdit = true;
            if (imageAdded)
            {
                imageEdit = Form1.db.EditDictionary(prevText, imagePaths);
            }

            if (prevText != txt_name.Text)
            {
                nameEdit = Form1.db.EditDictionary(prevText, txt_name.Text);
                lbl_name.Text = txt_name.Text;
                ((Item)Form1.itc.items[Form1.itc.selectedItem]).LblName.Text = txt_name.Text;
            }

            if (nameEdit && imageEdit)
            {
                lbl_name.Show();
                txt_name.Hide();
                lbl_edit.Show();
                btn_done.Hide();
                btn_AddImage.Hide();
                RemoveAllLabels();
                MessageBox.Show("Edited Successfuly");
            }
            else
                MessageBox.Show("sorry the name you have entered already founded");

        }

        private void RemoveAllLabels()
        {
            string name = "label";
            int num;

            for (int i = 0; i < countP; i++)
            {
                if (i == 0)
                {
                    num = i + 1;
                    panel1.Controls.Remove(panel1.Controls[name + num]);
                }

                else
                {
                    num = i + 1;
                    panel_maiPanel.Controls.Remove(panel_maiPanel.Controls[name + num]);
                }
            }
        }

        private void lbl_edit_Click(object sender, EventArgs e)
        {
            countP = imagePaths.Count;
            prevText = lbl_name.Text;
            txt_name.Text = lbl_name.Text;
            lbl_name.Hide();
            txt_name.Show();
            lbl_edit.Hide();
            btn_done.Show();
            btn_AddImage.Show();
            SetLabels();
        }

        private void SetLabels()
        {
            int num = 1;
            int count = imagePaths.Count;
            string name = "label";
            string picName = "PictureBox";
            Point pos;

            for (int i = 0; i < count; i++)
            {
                if (num == 1)
                {
                    pos = panel1.Controls[picName + num].Location;
                    Label lbl = new Label();
                    lbl.Text = "Remove";
                    lbl.ForeColor = Color.Red;
                    pos.X += 59;
                    pos.Y += 163;
                    lbl.Location = pos;
                    lbl.Click += lbl_Remove_Click;
                    panel1.Controls.Add(lbl);
                    lbl.Name = name + num;
                }
                else
                {
                    pos = panel_maiPanel.Controls[picName + num].Location;
                    Label lbl = new Label();
                    lbl.Text = "Remove";
                    lbl.ForeColor = Color.Red;
                    pos.X += 45;
                    pos.Y += 80;
                    lbl.Location = pos;
                    lbl.Click += lbl_Remove_Click;
                    panel_maiPanel.Controls.Add(lbl);
                    lbl.Name = name + num;
                }
                num++;
            }
        }

        private void lbl_profilePic_Click(object sender, EventArgs e)
        {
            string path = imagePaths[globalIndex - 1];
            imagePaths[globalIndex - 1] = imagePaths[0];
            imagePaths[0] = path;
            PictureBox pb = (PictureBox)panel1.Controls["PictureBox1"];
            PictureBox pb2 = (PictureBox)panel_maiPanel.Controls["PictureBox" + globalIndex];
            pb.Image = new Bitmap(path);
            pb2.Image = new Bitmap(imagePaths[globalIndex - 1]);
            //Form1.db.EditDictionary(lbl_name.Text, imagePaths);
            ((Item)Form1.itc.items[Form1.itc.selectedItem]).Pic = pb.Image;
        }

        private void picBox_close_MouseEnter(object sender, EventArgs e)
        {
            picBox_close.Image = UI.Properties.Resources.button_cancel;

        }

        private void picBox_close_MouseLeave(object sender, EventArgs e)
        {
            picBox_close.Image = UI.Properties.Resources.button_cancelOff;

        }

        private void picBox_Large_MouseLeave(object sender, EventArgs e)
        {
            picBox_Large.Hide();
            lbl_profilePic.Hide();
        }

        private void lbl_edit_MouseEnter(object sender, EventArgs e)
        {
            lbl_edit.ForeColor = Color.Red;
        }

        private void lbl_edit_MouseLeave(object sender, EventArgs e)
        {
            lbl_edit.ForeColor = Color.Brown;
        }
    }
}
