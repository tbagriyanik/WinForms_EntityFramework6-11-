using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ef3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //https://www.yazilimkodlama.com/programlama/c-windows-form-entity-framework-veritabani-baglantisi/

        vtEntities _db = new vtEntities();

        private void Form1_Load(object sender, EventArgs e)
        {
            doldur();
        }

        private void doldur()
        {
            _db.SaveChanges();
            dataGridView1.DataSource = _db.tabloes.ToList();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                label3.Text = "Kayıt seçildi " + DateTime.Now.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ekle
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                tablo _tablo = new tablo();
                _tablo.Ad = textBox1.Text;
                _tablo.Ucret = Convert.ToDouble(textBox2.Text);
                _db.tabloes.Add(_tablo);
                doldur();
                textBox1.Text = "";
                textBox2.Text = "0";
                label3.Text = "Kayıt eklendi. " + DateTime.Now.ToString();
            }
            else
            {
                MessageBox.Show("Metin kutusu boş...");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //sil            
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Kayıt silme içi emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        DataGridViewRow row = dataGridView1.SelectedRows[i];
                        int silinecekID = (int)row.Cells[0].Value;
                        var silinecek = _db.tabloes.Where(w => w.id == silinecekID).FirstOrDefault();
                        _db.tabloes.Remove(silinecek);
                        label3.Text = "Kayıt silindi. " + DateTime.Now.ToString();
                    }
                    doldur();
                }
            }
            else
            {
                MessageBox.Show("Seçili bir kayıt yok...");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //güncelle
            int nerede = dataGridView1.CurrentCell.RowIndex;
            if (dataGridView1.CurrentCell.RowIndex > -1 && !String.IsNullOrEmpty(textBox1.Text))
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                int guncellenecekID = (int)row.Cells[0].Value;
                var guncellenecek = _db.tabloes.Where(w => w.id == guncellenecekID).FirstOrDefault();
                guncellenecek.Ad = textBox1.Text;
                guncellenecek.Ucret = Convert.ToDouble(textBox2.Text);
                textBox1.Text = "";
                textBox2.Text = "0";
                doldur();

                //imleci eski yerine alalım
                dataGridView1.CurrentCell = dataGridView1.Rows[nerede].Cells[0];

                label3.Text = "Kayıt güncellendi. " + DateTime.Now.ToString();
            }
            else
            {
                MessageBox.Show("Seçili bir kayıt yok veya boş giriş ...");
            }
        }
    }
}
