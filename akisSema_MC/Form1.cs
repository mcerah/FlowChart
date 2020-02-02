using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace akisSema_MC
{  
    public partial class Form1 : Form
    {
        Sema sema;
       public bool baslangic = false,degiskengosteraktif=false;
       ArrayList degiskenler = new ArrayList();
       ArrayList programSemalari = new ArrayList();
       bool akGizle=false,ozelliklerGizle=false;
       public string []semaSatir ;
       string[] trdil = new string[77];


        public Form1()
        {
            InitializeComponent();
           // SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            CheckForIllegalCrossThreadCalls = false;
            sema = new Sema(this);
        
        }
       /*public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }*/
        private void dildegislemleri()
        {
            trdil[67] = "Tamam";
            trdil[68] = "Yeni Satır";
            trdil[69] = "Değiştir";
            trdil[70] = "Tanımla";
            trdil[71] = "Topla";
            trdil[72] = "Böl";
            trdil[73] = "Çarp";
            trdil[74] = "Kuvvet";
            trdil[76] = "Rastgele";
            trdil[75] = "Çıkar";
            comboBox2.Items.Clear();
            for (int i = 71; i < 77; i++)
                comboBox2.Items.Add(trdil[i]);
            comboBox2.Text = trdil[71];
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dildegislemleri();
                aracKonumlandir();
      
        }
        private void programKaydet()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "MCA Dosyası|*.mca";
            save.OverwritePrompt = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter SW = File.AppendText(save.FileName);

                foreach (Control c in this.Controls)
                {
                    if (c.Padding == new Padding(10, 1, 10, 1))
                    {
         
                        SW.WriteLine(Sifrele(c.Name));
                        SW.WriteLine(Sifrele(c.Top.ToString()));
                        SW.WriteLine(Sifrele(c.Left.ToString())); 
                        SW.WriteLine(Sifrele(c.Width.ToString()));
                        SW.WriteLine(Sifrele(c.Height.ToString()));
                        SW.WriteLine(Sifrele(c.Tag.ToString()));
                        SW.WriteLine(Sifrele(c.BackColor.Name.ToString()));
                        SW.WriteLine(Sifrele(c.Text));
                        SW.WriteLine(Sifrele("-*-SaTiRSoNu+-."));
                    }
                }

                SW.WriteLine(Sifrele(sema.semaNo.ToString()));
                SW.WriteLine(Sifrele(baslangic.ToString()));
                SW.WriteLine(Sifrele("-*-BagLaNtilaR+-."));
                SW.Close();
                sema.programKaydet(save.FileName);
                StreamWriter SW3 = File.AppendText(save.FileName);
                
                SW3.WriteLine(Sifrele("-*-DegiSKeNlER+-."));
                for (int i = 0; i<degiskenler.Count;i++ )
                {
                        SW3.WriteLine(Sifrele(degiskenler[i].ToString()));
                }
                SW3.WriteLine(Sifrele("*-*soN.*-"));
                SW3.Close();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
         
            sema.yenidenCiz();
        }
        public void aracGizle()
        {
            groupBox1.Visible = false;
            groupBox2.Visible = true;
        }
        public void metodTetikle(Button aktifSema,int satirSayisi)
        {
            if (aktifSema.Name.Length > 4 && aktifSema.Name.Substring(0, 5) != "basla" && aktifSema.Name.Substring(0, 3) != "son" && button13.BackColor == Color.Crimson && button22.BackColor == Color.Crimson && button8.BackColor == Color.Crimson)
            {
                semaSatir = aktifSema.Text.Split('\n');
                groupBox1.Tag=0;
                if (aktifSema.Name.Substring(0, 5) == "islem")
                {
                    tabControl1.SelectTab(0); degiskenleriDoldur(1, satirSayisi); comboBox20.Text = "";
                    comboBox1.Text = "";
                    comboBox3.Text = "";
                    comboBox4.Text = "";
                    textBox3.Text = "";
                    comboBox16.Text = trdil[68];
                }
                else if (aktifSema.Name.Substring(0, 5) == "giris")
                {
                    tabControl1.SelectTab(1); degiskenleriDoldur(2, satirSayisi);
                    comboBox5.Text = "";
                    textBox1.Text = "";
                    comboBox17.Text = trdil[68];
                }
                else if (aktifSema.Name.Substring(0, 5) == "kosul") { tabControl1.SelectTab(2); comboBox9.Text = ""; comboBox7.Text = ""; comboBox18.Text = trdil[68]; }
                else if (aktifSema.Name.Substring(0, 5) == "dongu")
                {
                    tabControl1.SelectTab(3); degiskenleriDoldur(3, satirSayisi);
                    comboBox10.Text = "";
                    comboBox12.Text = "";
                    comboBox13.Text = "";
                    comboBox14.Text = "";
                    if (aktifSema.Text != "")
                    {
                        int isaretKonum = aktifSema.Text.LastIndexOf("=");
                        if (isaretKonum == -1 || aktifSema.Text.IndexOf(";")>isaretKonum) isaretKonum = aktifSema.Text.IndexOf("<");
                        if (isaretKonum == -1 || aktifSema.Text.IndexOf(";") > isaretKonum) isaretKonum = aktifSema.Text.IndexOf(">");

                        comboBox13.Text = aktifSema.Text.Substring(0, aktifSema.Text.IndexOf("=") - 1);
                        comboBox12.Text = aktifSema.Text.Substring(aktifSema.Text.IndexOf("=") + 2, aktifSema.Text.IndexOf(";") - aktifSema.Text.IndexOf("=") - 3);
                        comboBox14.Text = aktifSema.Text.Substring(aktifSema.Text.LastIndexOf(";") + 2, aktifSema.Text.Length - 2 - aktifSema.Text.LastIndexOf(";"));
                        comboBox11.Text = aktifSema.Text.Substring(isaretKonum-1,2).Trim();
                        comboBox10.Text = aktifSema.Text.Substring(isaretKonum+2,aktifSema.Text.Length-isaretKonum -3-(aktifSema.Text.Length-aktifSema.Text.LastIndexOf(";")));
                    }
                }
                else if (aktifSema.Name.Substring(0, 5) == "cikti") { tabControl1.SelectTab(4); degiskenleriDoldur(5, satirSayisi);
                comboBox6.Text = "";
                textBox2.Text = "";
                comboBox19.Text = trdil[68];
                }
                groupBox1.Location = new Point(-5, (this.Height - groupBox1.Height) / 2);
                groupBox2.Visible = false;
                groupBox1.Visible = true;
                groupBox1.Tag = 1;
                groupBox1.BringToFront();
            }
            else
            {
                groupBox2.Visible = true;
                groupBox1.Visible = false;
            }
        }

        private void yeniSema(string ad,Image img)
        {
            sema.semaOlustur( ad, img);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (baslangic) yeniSema("son", button1.BackgroundImage);
            else { yeniSema("basla", button1.BackgroundImage); baslangic = true; }
        }

        public void calismaBitti()
        {
            button10.Enabled = false;
            button9.BackColor = Color.Crimson;
            button9.Enabled = true;
            button11.Enabled = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            yeniSema("islem",button3.BackgroundImage);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            yeniSema("giris", button4.BackgroundImage);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            yeniSema( "kosul", button5.BackgroundImage);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            yeniSema( "dongu", button6.BackgroundImage);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            yeniSema( "cikti", button7.BackgroundImage);
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
           if(Convert.ToInt16(groupBox1.Tag)==1) e.Cancel = true;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox2.Visible = true;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (comboBox16.Text == trdil[68]) sema.semaYaz("islemdeg",comboBox20.Text,"'"+textBox3.Text+"'",comboBox16);
            else if (sayiMi(Convert.ToChar(comboBox16.Text.Substring(0, 1))) && Sema.degiskenAdiUygunlugu(comboBox20.Text)) sema.semaYazDegistir(Convert.ToInt16(comboBox16.Text.Substring(0, 1)), comboBox20.Text + " = " + "'"+textBox3.Text+"'");
            comboBox16.Text = trdil[68];

            degiskenEkle(comboBox20.Text);
        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (semaSatir.Length > 0 && comboBox16.SelectedIndex > 0)
            {
                bool islemvar = true;
                string degisken, islem, seciliSatir = semaSatir[comboBox16.SelectedIndex - 1];

                degisken = seciliSatir.Substring(0, seciliSatir.IndexOf(" = "));
                islem = seciliSatir.Substring(seciliSatir.IndexOf(" = ") + 4, seciliSatir.Length - (seciliSatir.IndexOf(" = ") + 5));

                textBox3.Text = "";
                comboBox20.Text = "";
                comboBox1.Text = "";
                comboBox3.Text = "";
                comboBox4.Text = "";
                button17.Text = trdil[70];
                button18.Text = trdil[67];

                if (islem.Contains("' + '")) comboBox2.Text = trdil[71];
                else if (islem.Contains("' - '")) comboBox2.Text = trdil[75];
                else if (islem.Contains("' / '")) comboBox2.Text = trdil[72];
                else if (islem.Contains("' * '")) comboBox2.Text = trdil[73];
                else if (islem.Contains("' ^ '")) comboBox2.Text = trdil[74];
                else if (islem.Contains("' % '")) comboBox2.Text = "MOD";
                else if (islem.Contains("' ~ '")) comboBox2.Text = trdil[76];
                else islemvar = false;

                if (islemvar)
                {
                    button18.Text = trdil[69];
                    comboBox1.Text = islem.Substring(0, islem.IndexOf("'"));
                    comboBox3.Text = islem.Substring(comboBox1.Text.Length + 5, islem.Length - (comboBox1.Text.Length + 5));
                    comboBox4.Text = degisken;
                }
                else { button17.Text = trdil[69]; comboBox20.Text = degisken; textBox3.Text = islem; }
            }
            else
            {
                textBox3.Text = "";
                comboBox20.Text = "";
                comboBox1.Text = "";
                comboBox3.Text = "";
                comboBox4.Text = "";
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (sayiMi(Convert.ToChar(comboBox16.Text.Substring(0, 1)))) sema.semaSatirSil(Convert.ToInt16(comboBox16.Text.Substring(0, 1)), comboBox16);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (Sema.satirsay(textBox1.Text) == 1)
            {
                if (comboBox17.Text == trdil[68]) sema.semaYaz("giris", comboBox5.Text, textBox1.Text, comboBox17);
                else if (sayiMi(Convert.ToChar(comboBox17.Text.Substring(0, 1))) && Sema.degiskenAdiUygunlugu(comboBox5.Text)) sema.semaYazDegistir(Convert.ToInt16(comboBox17.Text.Substring(0, 1)), textBox1.Text + " | " + comboBox5.Text);
            }
            else {
                if (button32.Text == "TR") MessageBox.Show("Giriş mesajı tek satır olmalıdır!", "Giriş Mesajı Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else MessageBox.Show("Input message must be a single line!", "Input Message Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 

            degiskenEkle(comboBox5.Text);
            button14.Text = trdil[67];
        }
         

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 124) e.KeyChar = Convert.ToChar(Keys.None);
        }

        private bool sayiMi(char c)
        {
            if (Convert.ToInt16(c) > 47 && Convert.ToInt16(c) < 58) return true;
            else return false;
        }
        private void button27_Click(object sender, EventArgs e)
        {
            if (sayiMi(Convert.ToChar(comboBox17.Text.Substring(0, 1)))) sema.semaSatirSil(Convert.ToInt16(comboBox17.Text.Substring(0, 1)), comboBox17);
        }

        private void comboBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(Keys.None);
        }

        private void comboBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(Keys.None);
        }

        private void comboBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(Keys.None);
        }

        private void comboBox19_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(Keys.None);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (Sema.satirsay(textBox2.Text) == 1)
            {
                if (comboBox19.Text == trdil[68]) sema.semaYaz("cikti", comboBox6.Text, textBox2.Text, comboBox19);
                else if (sayiMi(Convert.ToChar(comboBox19.Text.Substring(0, 1))) && Sema.degiskenAdiUygunlugu(comboBox6.Text)) sema.semaYazDegistir(Convert.ToInt16(comboBox19.Text.Substring(0, 1)), "\"" + textBox2.Text + "\" | " + comboBox6.Text);

            }
            else
            {
                if (button32.Text== "TR") MessageBox.Show("Çıktı mesajı tek satır olmalıdır!", "Çıktı Mesajı Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else MessageBox.Show("Output message must be a single line!", "Output Message Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
            button21.Text = trdil[67];
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (sayiMi(Convert.ToChar(comboBox19.Text.Substring(0, 1)))) sema.semaSatirSil(Convert.ToInt16(comboBox19.Text.Substring(0, 1)), comboBox19);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if(Sema.degiskenAdiUygunlugu(comboBox13.Text))sema.semaYaz("dongu", comboBox13.Text, comboBox12.Text + " ; " + comboBox13.Text+ " "+ comboBox11.Text + " " + comboBox10.Text, comboBox14);
            degiskenEkle(comboBox13.Text);
        }

        private void comboBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(Keys.None);
        }
       
        private void button15_Click(object sender, EventArgs e)
        {
            string yanKosul="";
 
            if (comboBox15.Visible)
            {
                if (comboBox15.Text == "ve") yanKosul = "&& ";
                else if (comboBox15.Text == "veya") yanKosul = "|| ";
            }
            if ((sema.kosulkontrol() == 1 && yanKosul == "&& ") || (sema.kosulkontrol() == 0 && yanKosul == "|| ") || sema.kosulkontrol() == 2 || sema.kosulkontrol() == 3 || !comboBox15.Visible)
            {
                if (comboBox18.Text == trdil[68]) sema.semaYaz("kosul", comboBox15.Text + "______", comboBox9.Text + " " + comboBox8.Text + " " + comboBox7.Text, comboBox18);
                else if (sayiMi(Convert.ToChar(comboBox18.Text.Substring(0, 1)))) sema.semaYazDegistir(Convert.ToInt16(comboBox18.Text.Substring(0, 1)), yanKosul.Trim() + yanKosul + comboBox9.Text + " " + comboBox8.Text + " " + comboBox7.Text);
            }
            else
            {
                if (button32.Text== "TR") MessageBox.Show("Koşul oluştururken 've' , 'veya' yan koşulları aynı şema içinde olamaz !", "Koşul Sözdizimi Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else MessageBox.Show("Creating a condition 'and', 'or' adverse conditions can not be in the same chart!", "Condition Syntax Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              
            }
            yanKosulGoster();
            button15.Text = trdil[67];
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (sayiMi(Convert.ToChar(comboBox18.Text.Substring(0, 1)))) sema.semaSatirSil(Convert.ToInt16(comboBox18.Text.Substring(0, 1)), comboBox18);
        }

        private void comboBox18_SelectedIndexChanged(object sender, EventArgs e)
        {
            yanKosulGoster();
            if (semaSatir != null && semaSatir.Length > 0 && comboBox18.SelectedIndex > 0)
            {
                string seciliSatir = semaSatir[comboBox18.SelectedIndex - 1];
                int yankosul = 0;
                if (seciliSatir.Length > 1)
                {
                    if (seciliSatir.StartsWith("&& ")) yankosul = 1;
                    else if (seciliSatir.StartsWith("|| ")) yankosul = 2;
                }
                string kosul = "", deg1 = "", deg2 = "";

                if (seciliSatir.Contains(" = ")) kosul = "=";
                else if (seciliSatir.Contains(" < ")) kosul = "<";
                else if (seciliSatir.Contains(" > ")) kosul = ">";
                else if (seciliSatir.Contains(" <= ")) kosul = "<=";
                else if (seciliSatir.Contains(" >= ")) kosul = ">=";
                else if (seciliSatir.Contains(" != ")) kosul = "!=";

                if (comboBox18.SelectedIndex == 1) deg1 = seciliSatir.Substring(0, seciliSatir.IndexOf(kosul)-1);
                else deg1 = seciliSatir.Substring(5, seciliSatir.IndexOf(kosul) - 6);
                deg2 = seciliSatir.Substring(seciliSatir.IndexOf(kosul) + 2, seciliSatir.Length - (seciliSatir.IndexOf(kosul) + 2));

                comboBox9.Text = deg1;
                comboBox7.Text = deg2;
                comboBox8.Text = kosul;
                if (yankosul == 1) comboBox15.Text = "ve";
                else if (yankosul == 2) comboBox15.Text = "veya";
            }
            else
            {
                comboBox9.Text = "";
                comboBox7.Text = "";
            }
        }
        private void yanKosulGoster()
        {
            if (comboBox18.Text == "1. Satır") comboBox15.Visible = false;
            else if (comboBox18.Text == trdil[68] || comboBox18.Items.Count > 2) comboBox15.Visible = true;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string islem = "";
            if (comboBox2.Text == trdil[71]) islem = " + ";
            else if (comboBox2.Text == trdil[75]) islem = " - ";
            else if (comboBox2.Text == trdil[73]) islem = " * ";
            else if (comboBox2.Text == trdil[72]) islem = " / ";
            else if (comboBox2.Text == trdil[74]) islem = " ^ ";
            else if (comboBox2.Text == "MOD") islem = " % ";
            else if (comboBox2.Text == trdil[76]) islem = " ~ ";

            if (comboBox16.Text == trdil[68]) sema.semaYaz("islem", comboBox4.Text, "'"+comboBox1.Text+"'"+islem+"'"+comboBox3.Text+"'", comboBox16);
            else if (sayiMi(Convert.ToChar(comboBox16.Text.Substring(0, 1))) && Sema.degiskenAdiUygunlugu(comboBox4.Text)) sema.semaYazDegistir(Convert.ToInt16(comboBox16.Text.Substring(0, 1)),comboBox4.Text +" = '"+ comboBox1.Text+"'" + islem + "'"+comboBox3.Text+"'");
            degiskenEkle(comboBox4.Text);

            button18.Text = trdil[67];
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(Keys.None);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (sema.baglaAktif)
            {
                sema.semaBaglantiDurum(false);
                button13.BackColor = Color.Crimson;
                sema.sema1 = null;
                sema.sema2 = null;
            }
            else
            {
                if (button22.BackColor == Color.Green)
                {
                    sema.baglantiSilAktif = false;
                    sema.semaSil = false;
                    button22.BackColor = Color.Crimson;
                    sema.sema1 = null;
                    sema.sema2 = null;
                }
                sema.baglantiSilAktif = false;
                sema.semaBaglantiDurum(true);
                button13.BackColor = Color.Green;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            button9.Enabled = false;
            button9.BackColor = Color.Green;
            button10.Enabled = true;
            if(checkBox3.Checked)button11.Enabled = true;
            Application.DoEvents();
            sema.calistir();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (sema.baglantiSilAktif || sema.semaSil)
            {
                sema.baglantiSilAktif = false;
                sema.semaSil = false;
                button22.BackColor = Color.Crimson;
                sema.sema1 = null;
                sema.sema2 = null;
            }
            else
            {
                if (button13.BackColor == Color.Green)
                {
                    sema.semaBaglantiDurum(false);
                    button13.BackColor = Color.Crimson;
                    sema.sema1 = null;
                    sema.sema2 = null;
                }

                sema.baglaAktif = false;

                if (radioButton43.Checked) sema.baglantiSilAktif = true;
                else if (radioButton44.Checked) sema.semaSil = true;
                
                button22.BackColor = Color.Green;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            sema.durdur();
            calismaBitti();
            button10.Enabled = false;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==39) e.KeyChar = Convert.ToChar(Keys.None);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
       
        }
        private void aracKonumlandir()
        {

            groupBox3.Left = this.Width - 163;
            if (ozelliklerGizle) groupBox3.Left = groupBox3.Left + 135;
            groupBox3.Top = (this.Height - groupBox3.Height - 40) / 2;

            if (!akGizle)    groupBox2.Left = -5;

            groupBox2.Top = (this.Height - groupBox2.Height - 40) / 2;

            listView2.Left = -5;
            listView2.Top = (this.Height - listView2.Height - 40) / 2;
            groupBox1.Location = new Point(-5, (this.Height - groupBox1.Height) / 2);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            aracKonumlandir();
            sema.yenidenCiz();
        }

        public void degiskenGuncelle()
        {
            listView2.Items.Clear();
            degiskenler = sema.degiskenGoster();
            if (degiskenler != null)
            {
                ArrayList tempdeg;
                for (int i = 0; i < degiskenler.Count; i++)
                    listView2.Items.Add(degiskenler[i].ToString());

                tempdeg = sema.degiskenGoster();
                listView2.Items.Add("");

                for (int i = 0; i < tempdeg.Count; i++)
                    listView2.Items[i].SubItems.Add(tempdeg[i].ToString());
            }
            else listView2.Items.Add("Bulunamadı");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (button24.BackColor == Color.Crimson)
            {
                button24.BackColor = Color.Green;
                sema.akisDuraklat();
                degiskenGuncelle();
                degiskengosteraktif = true;
                listView2.Visible = true;
                listView2.Left = -5;
                listView2.Top = (this.Height - listView2.Height-40) / 2;
                sema.akisDevam();
            }
            else
            {
                button24.BackColor = Color.Crimson; 
                listView2.Visible = false ;
                degiskengosteraktif = false;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 39) e.KeyChar = Convert.ToChar(Keys.None);
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 39) e.KeyChar = Convert.ToChar(Keys.None);
        }
        private void arrayListDoldur(ComboBox cb)
        {
            cb.Items.Clear();
            if (degiskenler != null && degiskenler.Count>0)
            {
                for (int i = 0; i < degiskenler.Count; i++)
                    cb.Items.Add(degiskenler[i].ToString());
            }
        }
        private void degiskenleriDoldur(int sayfa,int satirSayisi)
        {
            if (sayfa == 1)
            {
                arrayListDoldur(comboBox20);
                arrayListDoldur(comboBox4);
                arrayListDoldur(comboBox1);
                arrayListDoldur(comboBox3);
                comboBox16.Items.Clear();
                comboBox16.Items.Add(trdil[68]);
                for (int i = 1; i <= satirSayisi; i++)
                    comboBox16.Items.Add(i+". Satır");
            }
            else if (sayfa == 2) 
            { 
                arrayListDoldur(comboBox5);
                comboBox17.Items.Clear();
                comboBox17.Items.Add(trdil[68]);
                for (int i = 1; i <= satirSayisi; i++)
                    comboBox17.Items.Add(i + ". Satır");
            }
            else if (sayfa == 3)
            {
                arrayListDoldur(comboBox13);
                arrayListDoldur(comboBox12);
                arrayListDoldur(comboBox10);
                arrayListDoldur(comboBox14);
                comboBox18.Items.Clear();
                comboBox18.Items.Add(trdil[68]);
                for (int i = 1; i <= satirSayisi; i++)
                    comboBox18.Items.Add(i + ". Satır");
            }
            else if (sayfa == 4) arrayListDoldur(comboBox6);
            else if (sayfa == 5)
            {
                arrayListDoldur(comboBox6);
                comboBox19.Items.Clear();
                comboBox19.Items.Add(trdil[68]);
                for (int i = 1; i <= satirSayisi; i++)
                    comboBox19.Items.Add(i + ". Satır");
            }
        }
        private void degiskenEkle(string degad)
        {
            if(Sema.degiskenAdiUygunlugu(degad) && degiskenler!=null && !degiskenler.Contains(degad))degiskenler.Add(degad);
        }
        public void ekranGoster()
        {
            richTextBox1.Visible = true;
            richTextBox1.BringToFront();
            button12.BackColor = Color.Green;
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (button12.BackColor == Color.Crimson)
            {
                ekranGoster();
            }
           else
            {
                richTextBox1.Visible = false;
                button12.BackColor = Color.Crimson;
            }
        }

        private void radioButton44_CheckedChanged(object sender, EventArgs e)
        {
            button22.PerformClick();
        }

        private void radioButton43_CheckedChanged(object sender, EventArgs e)
        {
            button22.PerformClick();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            sema.timerInterval(trackBar1.Value*10);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            sema.adimcalistir();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == trdil[76])
            {
                label13.Visible = false;
                label14.Visible = false;
                label7.Visible = true;
            }
            else
            {
                label13.Visible = true;
                label14.Visible = true;
                label7.Visible = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) { trackBar1.Enabled = false; sema.timerInterval(-1); }
            else trackBar1.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.BackColor == Color.Crimson)
            {
                button8.BackColor = Color.Green;
            }
            else
            {
                button8.BackColor = Color.Crimson ;
            }
        }

        protected  override void OnMouseWheel(MouseEventArgs e)
        {
            if (button8.BackColor == Color.Green)
            {
                int yon = 3;
                if (checkBox1.Checked && checkBox2.Checked) yon = 3;
                else if (checkBox1.Checked ) yon = 2;
                else if (checkBox2.Checked) yon = 1;

                if (e.Delta > 0) sema.boyutlandir(5,yon);
                else sema.boyutlandir(-5,yon);
            }
            base.OnMouseWheel(e);
        }

        private void comboBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Convert.ToChar(Keys.None);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (!akGizle) { akGizle = true; button23.Text = ">"; groupBox2.Left = -120; }
            else { akGizle = false; button23.Text = "<"; aracKonumlandir(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ozelliklerGizle)
            {
                ozelliklerGizle = true; groupBox3.Left = groupBox3.Left + 135;
                button2.Text = ">";
                groupBox3.Enabled = false;
            }
            else { ozelliklerGizle = false; button2.Text = "<"; aracKonumlandir(); }
          
        }

        private void button26_Click(object sender, EventArgs e)
        {
            programKaydet();
        }
        static public string Sifrele(string veri)
        {

            // gelen veri byte dizisine aktarılıyor

            byte[] veriByteDizisi = System.Text.ASCIIEncoding.UTF8.GetBytes(veri);

            // base64 şifreleme algoritmasına göre şifreleniyor.

            string sifrelenmisVeri = System.Convert.ToBase64String(veriByteDizisi);

            return sifrelenmisVeri;

        }



        static public string Coz(string cozVeri)
        {

            byte[] cozByteDizi = System.Convert.FromBase64String(cozVeri);

            string orjinalVeri = System.Text.ASCIIEncoding.UTF8.GetString(cozByteDizi);

            return orjinalVeri;

        }

        private void button16_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult;
            if (button32.Text == "EN") dialogResult = MessageBox.Show("Şu anki akışın silinmesini onaylıyor musunuz?", "Yeni Akış", MessageBoxButtons.YesNo);
            else dialogResult = MessageBox.Show("Do you approve the deletion of the current flow?", "Open Existing Flow", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                tumSemalariSil();
                sema.baglantilariSil();
                sema.semaNo = 0;
                baslangic = false;

                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "Akış Şema Dosyası |*.mca";
                file.FilterIndex = 2;
                file.RestoreDirectory = true;
                file.CheckFileExists = false;
                file.Title = "Akış Şema Seçiniz..";

                if (file.ShowDialog() == DialogResult.OK)
                {
                    string DosyaYolu = file.FileName;

                    StreamReader SR = new StreamReader(DosyaYolu);
                    string ad = "", tag = "", text = "", satir = "", oncekisatir = "", bag = "";
                    int sol = 0, ust = 0, genislik = 0, yukseklik = 0, say = 0;
                    Color renk = Color.White;
                    Image img = button1.BackgroundImage;
                    bool baglantiYukle = false, degiskenyukle = false;
                    string l1 = "", l2 = "", g1 = "", g2 = "", y1 = "", y2 = "", t1 = "", t2 = "";
                    while (SR != null)
                    {
                        oncekisatir = satir;
                        satir = Coz(SR.ReadLine());
                        if (satir == "True" || satir == "False") { baglantiYukle = true; say = -2; sema.semaNo = Convert.ToInt16(oncekisatir); baslangic = satir == "True"; }
                        if (!baglantiYukle)
                        {
                            if (say == 0) ad = satir;
                            else if (say == 1) { if (Convert.ToInt16(satir) > (this.Height - 50)) ust = this.Height - 100; else ust = Convert.ToInt16(satir); }
                            else if (say == 2) { if (Convert.ToInt16(satir) > (this.Width - 50)) sol = this.Width - 200; else sol = Convert.ToInt16(satir); }
                            else if (say == 3) genislik = Convert.ToInt16(satir);
                            else if (say == 4) yukseklik = Convert.ToInt16(satir);
                            else if (say == 5) tag = satir;
                            else if (say == 6) renk = Color.FromName(satir);
                            else if (say == 7) text = satir;
                            else if (satir != "-*-SaTiRSoNu+-." && say > 7) text += "\n" + satir;
                            else if (satir == "-*-SaTiRSoNu+-.")
                            {
                                say = -1;

                                if (ad.Substring(0, 3) == "son") img = button1.BackgroundImage;
                                else if (ad.Substring(0, 5) == "basla") img = button1.BackgroundImage;
                                else if (ad.Substring(0, 5) == "islem") img = button3.BackgroundImage;
                                else if (ad.Substring(0, 5) == "kosul") img = button5.BackgroundImage;
                                else if (ad.Substring(0, 5) == "giris") img = button4.BackgroundImage;
                                else if (ad.Substring(0, 5) == "dongu") img = button6.BackgroundImage;
                                else if (ad.Substring(0, 5) == "cikti") img = button7.BackgroundImage;

                                sema.semaOlusturAc(ad, img, ust, sol, genislik, yukseklik, tag, renk, text);
                            }
                        }
                        else if (!degiskenyukle)
                        {
                            if (satir == "-*-DegiSKeNlER+-.") degiskenyukle = true;
                            else if (satir == "*-*soN.*-") break;
                            if (say == 0) l1 = satir;
                            else if (say == 1) l2 = satir;
                            else if (say == 2) g1 = satir;
                            else if (say == 3) g2 = satir;
                            else if (say == 4) y1 = satir;
                            else if (say == 5) y2 = satir;
                            else if (say == 6) t1 = satir;
                            else if (say == 7) t2 = satir;
                            else if (say == 8)
                            {

                                bag = satir;
                                sema.semaBaglantiAc(l1, l2, g1, g2, y1, y2, t1, t2, bag);
                                say = -2;
                            }

                        }
                        else if (satir != null)
                        {
                            if (satir == "*-*soN.*-") break;
                            degiskenler.Add(satir);
                        }
                        else break;
                        say++;
                    }
                    SR.Close();
                }
                sema.acCiz();
            }
        }

     private void tumSemalariSil()
        {
            foreach (Control c in this.Controls)
            {
                if (c.Padding == new Padding(10, 1, 10, 1))
                {
                    c.Dispose();
                }
            }
            foreach (Control c in this.Controls)
            {
                if (c.Padding == new Padding(10, 1, 10, 1))
                {
                    c.Dispose();
                }
            }
            foreach (Control c in this.Controls)
            {
                if (c.Padding == new Padding(10, 1, 10, 1))
                {
                    c.Dispose();
                }
            }
            foreach (Control c in this.Controls)
            {
                if (c.Padding == new Padding(10, 1, 10, 1))
                {
                    c.Dispose();
                }
            }
        }


     private void button30_Click(object sender, EventArgs e)
     {
         Form frm=new Form2() ;
         frm.Show();
     }


     private void button1_MouseLeave(object sender, EventArgs e)
     {
         
         button1.FlatAppearance.BorderSize = 0;
     }

     private void button1_MouseMove(object sender, MouseEventArgs e)
     {
         button1.FlatAppearance.BorderSize = 1;
     }

     private void button3_MouseMove(object sender, MouseEventArgs e)
     {
         button3.FlatAppearance.BorderSize = 1;
     }

     private void button3_MouseLeave(object sender, EventArgs e)
     {
         button3.FlatAppearance.BorderSize = 0;
     }

     private void button4_MouseMove(object sender, MouseEventArgs e)
     {
         button4.FlatAppearance.BorderSize = 1;
     }

     private void button4_MouseLeave(object sender, EventArgs e)
     {
         button4.FlatAppearance.BorderSize = 0;
     }

     private void button5_MouseMove(object sender, MouseEventArgs e)
     {
         button5.FlatAppearance.BorderSize = 1;
     }

     private void button5_MouseLeave(object sender, EventArgs e)
     {
         button5.FlatAppearance.BorderSize = 0;
     }

     private void button6_MouseMove(object sender, MouseEventArgs e)
     {
         button6.FlatAppearance.BorderSize = 1;
     }

     private void button6_MouseLeave(object sender, EventArgs e)
     {
         button6.FlatAppearance.BorderSize = 0;
     }

     private void button7_MouseMove(object sender, MouseEventArgs e)
     {
         button7.FlatAppearance.BorderSize = 1;
     }

     private void button7_MouseLeave(object sender, EventArgs e)
     {
         button7.FlatAppearance.BorderSize = 0;
     }

     private void button31_Click(object sender, EventArgs e)
     {
         DialogResult dialogResult;
            if(button32.Text=="EN") dialogResult = MessageBox.Show("Var olan tüm akışı silmek istiyor musunuz? ", "Yeni Akış", MessageBoxButtons.YesNo);
            else  dialogResult = MessageBox.Show("Do you approve the deletion of the current flow?", "New Flow", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                sema.baglantilariSil();
                tumSemalariSil();
                baslangic = false;
            }
     }

     private void comboBox16_TextChanged(object sender, EventArgs e)
     {
     }

     private void comboBox17_TextChanged(object sender, EventArgs e)
     {
         if (comboBox17.Text != "" && comboBox17.Text != trdil[68]) button14.Text = trdil[69];
         else button14.Text = trdil[67];
     }

     private void comboBox18_TextChanged(object sender, EventArgs e)
     {
         if (comboBox18.Text != "" && comboBox18.Text != trdil[68]) button15.Text = trdil[69];
         else button15.Text = trdil[67];
     }

     private void comboBox19_TextChanged(object sender, EventArgs e)
     {
         if (comboBox19.Text != "" && comboBox19.Text != trdil[68]) button21.Text = trdil[69];
         else button21.Text = trdil[67];
     }

     private void comboBox17_SelectedIndexChanged(object sender, EventArgs e)
     {
         if (semaSatir.Length > 0 && comboBox17.SelectedIndex>0)
         {
             string satirtemp = semaSatir[comboBox17.SelectedIndex - 1];
             textBox1.Text = satirtemp.Substring(0, satirtemp.IndexOf("|")-1);
             comboBox5.Text = satirtemp.Substring(satirtemp.IndexOf("|")+2, satirtemp.Length - satirtemp.IndexOf("|")-2);
         }
         else
         {
             textBox1.Text = "";
             comboBox5.Text = "";
         }
     }

     private void comboBox19_SelectedIndexChanged(object sender, EventArgs e)
     {
         if (semaSatir.Length > 0 && comboBox19.SelectedIndex > 0)
         {
             string satirtemp = semaSatir[comboBox19.SelectedIndex - 1];
             textBox2.Text = satirtemp.Substring(1, satirtemp.IndexOf("|") - 3);
             comboBox6.Text = satirtemp.Substring(satirtemp.IndexOf("|") + 2, satirtemp.Length - satirtemp.IndexOf("|") - 2);
         }
         else
         {
             textBox2.Text = "";
             comboBox6.Text = "";
         }
     }

     private void tabPage4_Click(object sender, EventArgs e)
     {

     }

     private void Form1_ResizeEnd(object sender, EventArgs e)
     {
     }

     private void Form1_SizeChanged(object sender, EventArgs e)
     {
         sema.bagimsizCiz();
        
     }

     private void button32_Click(object sender, EventArgs e)
     {
         string[] ceviri = new string[67];
         if (button32.Tag.ToString() == "0") { button32.Tag = "1"; ceviri = dilDegistir(false); }
         else 
         {
             dildegislemleri();
             ceviri = trdil; 
             button32.Tag = "0";
         }
         
         int say = 0;

         bool trkaydet = false;
         if (trdil[0] == null) trkaydet = true;
         foreach (Control g in this.Controls)
         {
             if (g.Padding != new Padding(10, 1, 10, 1)) { 
             if (g.GetType() == typeof(GroupBox))
             {
                 foreach (Control c in g.Controls)
                     if (c.Text != "") { if(trkaydet)trdil[say] = c.Text; c.Text = ceviri[say]; say++; }
             }
             if (g.Text != "") { if (trkaydet)trdil[say] = g.Text; g.Text = ceviri[say]; say++; }}
         }

         foreach (TabPage g in tabControl1.TabPages)
         {     if (g.Padding != new Padding(10, 1, 10, 1)) {
             foreach (Control c in g.Controls)
             {
                 if (c.GetType() == typeof(GroupBox))
                 {
                     foreach (Control b in c.Controls)
                         if (b.Text != "") { if (trkaydet)trdil[say] = b.Text; b.Text = ceviri[say]; say++; }
                 }
                 if (c.Text != "") { if (trkaydet)trdil[say] = c.Text; c.Text = ceviri[say]; say++; }
             }
             }
         }
         

     }
     private string[] dilDegistir(bool dil)
     {
         if (dil) return trdil;

         string []ceviri=new string[67]; 

         if (button32.Tag.ToString() == "1")
         {
             ceviri[0] = "TR";
             ceviri[1] = "Step by Step";
             ceviri[2] = "Screen";
             ceviri[3] = "Variables";
             ceviri[4] = "<";
             ceviri[5] = "Delete";
             ceviri[6] = "Delete Connection";
             ceviri[7] = "Delete Chart";
             ceviri[8] = "Connection";
             ceviri[9] = "Resize";
             ceviri[10] = "Width";
             ceviri[11] = "Height";
             ceviri[12] = "Delay Time";
             ceviri[13] = "             Attributes";
             ceviri[14] = "<";
             ceviri[15] = "Output";
             ceviri[16] = "Start/End";
             ceviri[17] = "Cycle";
             ceviri[18] = "Process";
             ceviri[19] = "Condition";
             ceviri[20] = "Input";
             ceviri[21] = "  Flow Charts";
             ceviri[22] = "Delete Line";
             ceviri[23] = "Edit :";
             ceviri[24] = "New Line";
             ceviri[25] = "Interval :";
             ceviri[26] = "Gather";
             ceviri[27] = "OK";
             ceviri[28] = "Operand";
             ceviri[29] = "Result Variable :";
             ceviri[30] = "Process";
             ceviri[31] = "Operand";
             ceviri[32] = "Process";
             ceviri[33] = "0";
             ceviri[34] = "Define";
             ceviri[35] = "Name :";
             ceviri[36] = "Data :";
             ceviri[37] = "Veriable";
             ceviri[38] = "Delete Line";
             ceviri[39] = "Edit :";
             ceviri[40] = "New Line";
             ceviri[41] = "OK";
             ceviri[42] = "Variable :";
             ceviri[43] = "Message :";
             ceviri[44] = "Delete Line";
             ceviri[45] = "Edit :";
             ceviri[46] = "New Line";
             ceviri[47] = "as condition";
             ceviri[48] = "and";
             ceviri[49] = "OK";
             ceviri[50] = "Comparison Data :";
             ceviri[51] = "Condition";
             ceviri[52] = "Data :";
             ceviri[53] = "=";
             ceviri[54] = "İnitial Value :";
             ceviri[55] = "Accrual :";
             ceviri[56] = "Variable :";
             ceviri[57] = "Finale Condition :";
             ceviri[58] = "Cycle Condition :";
             ceviri[59] = "<";
             ceviri[60] = "OK";
             ceviri[61] = "Delete Line";
             ceviri[62] = "Edit :";
             ceviri[63] = "New Line";
             ceviri[64] = "OK";
             ceviri[65] = "Variable :";
             ceviri[66] = "Message :";
             trdil[67] = "OK";
             trdil[68] = "New Line";
             trdil[69] = "Edit";
             trdil[70] = "Define";
               trdil[71] = "Gather";
               trdil[72] = "Division";
               trdil[73] = "Multiply";
               trdil[74] = "Power";
               trdil[76] = "Random";
               trdil[75] = "Extraction";
               comboBox2.Items.Clear();
               for (int i = 71; i < 77; i++)
                   comboBox2.Items.Add(trdil[i]);
               comboBox2.Text = trdil[71];
         }

         return ceviri;

     }


    }
}
