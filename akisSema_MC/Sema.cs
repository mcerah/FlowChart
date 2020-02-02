using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace akisSema_MC
{
    class Sema //************  Bir sınıfı olabildiğince bağımsız yap
    {
        Button control, aktifSema, konumButonu=new Button();
        public Button sema1, sema2;
        bool surukle = false;
        bool derlendi=false;
        ArrayList sematext = new ArrayList();
        public bool baglaAktif=false,baglantiSilAktif=false,semaSil=false;
        Point ilkkonum,olusmaKonumu=new Point(-289,-124);
        static Form1 frm1;
        Derleyici dr;
        Baglayici baglayici;
         string kslyaz = "-";

        ArrayList siraliBaglantilar = new ArrayList();
        ArrayList kosulDongusu = new ArrayList();
         int  ciz = 0,aracGoster=0;
         public int semaNo = 0;

        public void ClickEtkinlestir(Control prnt)
        {
            foreach (Control c in prnt.Controls)
              ClickEtkinlestir(c);
            prnt.Click += prnt_Click;
        }
        private void prnt_Click(object sender, EventArgs e)
        {
            control = (sender as Button);
        }
        public Sema(Form1 f)
        {
            frm1 = f;
            baglayici = new Baglayici(f);
            konumButonu.Location = new Point((frm1.Width) / 2, 10); konumButonu.Tag = "knmbtn"; konumButonu.Size = new Size(100, 55);
            ClickEtkinlestir(f);
        }

        public void  semaBaglantiDurum(bool bAktif){
            baglaAktif = bAktif;
            if (aktifSema != null && kslyaz != "-") { aktifSema.Text = kslyaz; kslyaz = "-"; }
        }
        void btn_TextChanged(object sender, MouseEventArgs e)
        { 
        
        
        }
        void btn_MouseMove(object sender, MouseEventArgs e)
        {
            Button semabtn = sender as Button;
            if (surukle && !baglaAktif && !baglantiSilAktif)
            {
                ciz++;
                aracGoster++;
                semabtn.Left = e.X + semabtn.Left - (ilkkonum.X);
                semabtn.Top = e.Y + semabtn.Top - (ilkkonum.Y);
                konumButonu.Location = semabtn.Location;
                if (ciz > 20) { yenidenCiz(); ciz = 1; }
                if (ciz % 2 == 0) frm1.aracGizle();
            }
        }
       
        public void acCiz()
        {
            baglayici.ciz();
        }

        public void yeniCizim()
        {
            baglayici.Yenidenciz();
        }
        private bool baglantiKural(Button sema1, Button sema2)
        {
            if (baglayici.baglantiKontrol(sema1.Name, sema2.Name))
            {
                int sema1AdUzunluk = sema1.Name.Length, sema2AdUzunluk = sema2.Name.Length;

                if (sema2AdUzunluk > 4 && sema2.Name.Substring(0, 5) == "basla") return false;
                if (sema1AdUzunluk > 2 && sema1.Name.Substring(0, 3) == "son") return false;
                if (sema1.Name == sema2.Name) return false;

                int cikis = 0;
                string[] sayi = sema1.Tag.ToString().Split('%');

                if (sayi[0] != "")
                {

                    for (int i = 0; i < sayi.Length; i++)
                        if (Convert.ToInt16(sayi[i]) > 0) cikis++;
                }

                if (sema1AdUzunluk > 4)
                {
                    string semaAd = sema1.Name.Substring(0, 5);
                    if (semaAd == "islem" && cikis > 0) return false;
                    else if (semaAd == "cikti" && cikis > 0) return false;
                    else if (semaAd == "kosul" && cikis > 1) return false;
                    else if (semaAd == "dongu" && cikis > 1) return false;
                    else if (semaAd == "basla" && cikis > 0) return false;
                }
                else return false;

                return true;
            }
            return false;
        }
        public void yenidenCiz()
        {
            int result;
            if (aktifSema == null) return;
            string[] sayi = aktifSema.Tag.ToString().Split('%');
           
            if(sayi[0]!="")
            for (int i = 0; sayi.Length > i; i++)
                if (int.TryParse(sayi[i], out result)) baglayici.konumGuncelle(Convert.ToInt16(sayi[i]), aktifSema.Left, aktifSema.Top);
        }
        public void yuklemekonumuguncelle(int inds,int x,int y)
        {
            baglayici.konumGuncelle(inds,x, y);
        }
        public void bagimsizCiz()
        {
            if (baglayici != null) { baglayici.ciz(); }
        }
        void btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button semabtn = sender as Button;
            aktifSema = sender as Button;
            surukle = true;
            semabtn.BringToFront();
           if(!baglantiSilAktif && !baglaAktif) semabtn.Cursor = Cursors.SizeAll;
            semabtn.FlatAppearance.BorderSize = 1;
            semabtn.FlatAppearance.BorderColor = Color.Crimson;
            ilkkonum = e.Location;
        }
        void btn_MouseUp(object sender, MouseEventArgs e)
        {
            Button semabtn = sender as Button;
            surukle = false;
            semabtn.Cursor = Cursors.Default;
            semabtn.FlatAppearance.BorderSize = 0;
            yenidenCiz();// Bu gerekli mi
            if (aracGoster > 20) frm1.aracGizle();
            else if(aktifSema!=null)frm1.metodTetikle(aktifSema, satirsayTam(aktifSema.Text));
            aracGoster = 0;
        }
        void btn_Click(object sender, EventArgs e)
        { 
            Button semabtn = sender as Button;
           
            if (baglaAktif || baglantiSilAktif)
            {
                if (sema1 == null)
                {
                    sema1 = aktifSema;
                    if (kosulOrdongu(sema1.Name) == true && !baglantiSilAktif)
                    {
                        kslyaz = sema1.Text;
                        if (sema1.BackColor == Color.Green) sema1.Text = "YANLIŞ";
                        else sema1.Text = "DOĞRU";
                    }
                }
                else if (sema2 == null)
                {
                    sema2 = aktifSema;
                    if (baglaAktif)
                    {
                        if (kosulOrdongu(sema1.Name) == true) { sema1.Text = kslyaz; kslyaz = "-"; }
                        bagla(sema1, sema2);
                    }
                    else if (baglantiSilAktif) baglantiKopar(sema1, sema2);
                }
            }
            else if (semaSil)
            {
                if (semabtn.Tag.ToString() == "")
                {
                    if (semabtn.Text == "Başla") frm1.baslangic = false;
                    if (konumButonu == semabtn) konumButonu.Location=new Point(semabtn.Left,semabtn.Top-semabtn.Height-15);
                    semabtn.Dispose();
                }
                else
                {
                    if (frm1.button32.Text == "EN") MessageBox.Show("Şema silebilmek için önce şemanın tüm bağlantıları silinmelidir!", "Şema Silme Bildirimi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("All connections to the chart should be deleted before you can delete Chart!", "Chart Deletion Notification", MessageBoxButtons.OK, MessageBoxIcon.Information); 

                } 
            }
            else  { sema1 = null; sema2 = null; }

        }
        private void semaTagGuncelle(Button sema1, Button sema2)//Geliştirilmeli
        {
            int result;
            string[] sema1Tag = sema1.Tag.ToString().Split('%');
            string[] sema2Tag = sema2.Tag.ToString().Split('%');

            for (int i = 0; sema1Tag.Length > i; i++)
            {
                for (int j = 0; sema2Tag.Length > j; j++)
                {
                    if (int.TryParse(sema1Tag[i], out result) && int.TryParse(sema2Tag[j], out result))
                    {
                        int sema1TagInt = Convert.ToInt16(sema1Tag[i]), sema2TagInt = Convert.ToInt16(sema2Tag[j]) * -1;
                        if (sema1TagInt == sema2TagInt) { sema1Tag[i] = ""; sema2Tag[j] = ""; break; }
                    }
                    if (sema1Tag[i] == "") break;
                }
            }
            sema1.Tag = ""; sema2.Tag = "";
            for (int i = 0; sema1Tag.Length > i; i++)
                if (sema1Tag[i] != "") sema1.Tag += sema1Tag[i] + '%';

            if (sema1.Tag.ToString().Length > 0) sema1.Tag = sema1.Tag.ToString().Remove(sema1.Tag.ToString().Length - 1, 1);

            for (int i = 0; sema2Tag.Length > i; i++)
                if (sema2Tag[i] != "") sema2.Tag += sema2Tag[i] + '%';

            if (sema2.Tag.ToString().Length > 0) sema2.Tag = sema2.Tag.ToString().Remove(sema2.Tag.ToString().Length - 1, 1);
        }
        private void semaTagGuncelle(Button sema1, Button sema2,int bagNo)//Geliştirilmeli
        {
            int result;
            string[] sema1Tag = sema1.Tag.ToString().Split('%');
            string[] sema2Tag = sema2.Tag.ToString().Split('%');

            for (int i = 0; sema1Tag.Length > i; i++)
            {
                for (int j = 0; sema2Tag.Length > j; j++)
                {
                    if (int.TryParse(sema1Tag[i], out result) && int.TryParse(sema2Tag[j], out result))
                    {
                        int sema1TagInt = Convert.ToInt16(sema1Tag[i]), sema2TagInt = Convert.ToInt16(sema2Tag[j]) * -1;

                        if (sema1TagInt == sema2TagInt && sema1TagInt == bagNo) { sema1Tag[i] = ""; sema2Tag[j] = ""; break; }
                    }
                    if (sema1Tag[i] == "") break;
                }
            }
            sema1.Tag = ""; sema2.Tag = "";
            for (int i = 0; sema1Tag.Length > i; i++)
                if (sema1Tag[i] != "") sema1.Tag += sema1Tag[i] + '%';

            if (sema1.Tag.ToString().Length > 0) sema1.Tag = sema1.Tag.ToString().Remove(sema1.Tag.ToString().Length - 1, 1);

            for (int i = 0; sema2Tag.Length > i; i++)
                if (sema2Tag[i] != "") sema2.Tag += sema2Tag[i] + '%';

            if (sema2.Tag.ToString().Length > 0) sema2.Tag = sema2.Tag.ToString().Remove(sema2.Tag.ToString().Length - 1, 1);

        }
        private void baglantiKopar(Button sema1, Button sema2)//kosul şemasının evet ve hayır bağlantılarını aynı şema ile yapmasını engelle
        {
            if (kosulOrdongu(sema1.Name))
            {
                int bulunan = baglayici.kosulDonguBaglantiSil(sema1.Name + "-" + sema2.Name);
                    if (bulunan > 0)
                    {
                        if (sema1.BackColor == Color.Green) sema1.BackColor = Color.White;
                        else if (sema1.BackColor == Color.Red) sema1.BackColor = Color.White;
                        else if (sema1.BackColor == Color.Yellow) sema1.BackColor = Color.Red;
                        semaTagGuncelle(sema1, sema2, bulunan+1);
                    }
                    else if (bulunan < 0)
                    {
                        if (sema1.BackColor == Color.Green) sema1.BackColor = Color.White;
                        else if (sema1.BackColor == Color.Red) sema1.BackColor = Color.White;
                        else if (sema1.BackColor == Color.Yellow) sema1.BackColor = Color.Green;
                        semaTagGuncelle(sema1, sema2, (bulunan * -1)+1);
                    }
            }
            else if (kosulOrdongu(sema2.Name))
            {
                int bulunan = baglayici.baglantiSil(sema1.Name + "-" + sema2.Name);
                if (bulunan > -1)  semaTagGuncelle(sema1, sema2, bulunan + 1);
            }
            else if (baglayici.baglantiSil(sema1.Name + "-" + sema2.Name)>-1) semaTagGuncelle(sema1, sema2);

            this.sema1 = null;
            this.sema2 = null;
        }
        private bool kosulOrdongu(string isim)
        {
            if (isim.Length > 4 && (isim.Substring(0, 5) == "kosul" || isim.Substring(0, 5) == "dongu")) return true;
            else return false;
        }
        private char kosul_dongu_BaglantiKural(Button sema1)
        {
            if (sema1.BackColor == Color.White) { sema1.BackColor = Color.Green;  return 'E'; }
            else if (sema1.BackColor == Color.Green) {sema1.BackColor = Color.Yellow; return 'H'; }
            else if (sema1.BackColor == Color.Red){ sema1.BackColor = Color.Yellow; return 'E'; }
            return 'N';
        }
        private void bagla(Button sema1,Button sema2)
        {
            if (baglantiKural(sema1, sema2))
            {
                char yon = 'N';
                if (kosulOrdongu(sema1.Name)) yon = kosul_dongu_BaglantiKural(sema1);

                baglayici.bagla(sema1.Location, sema2.Location, sema1.Size, sema2.Size);

                int baglantiNo;

                if (yon == 'E' || yon == 'H') baglantiNo = baglayici.baglantiKaydet(sema1.Location, sema2.Location, sema1.Size, sema2.Size, sema1.Name.ToString(), sema2.Name.ToString() + yon);
                else baglantiNo = baglayici.baglantiKaydet(sema1.Location, sema2.Location, sema1.Size, sema2.Size, sema1.Name.ToString(), sema2.Name.ToString());

                if (baglantiNo > 0)
                {
                    if (sema1.Tag.ToString().Length > 0) sema1.Tag += "%" + baglantiNo.ToString();
                    else sema1.Tag = baglantiNo;

                    if (sema2.Tag.ToString().Length > 0) sema2.Tag += "%" + (baglantiNo * -1).ToString();
                    else sema2.Tag = (baglantiNo * -1).ToString();
                }
            }
            else
            {
                if (frm1.button32.Text == "EN") MessageBox.Show("Bu bağlantı, bağlantı kurallarına uygun değil!", "Bağlantı Uyarısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("This connection is not appropriate to link the rules!", "Connection Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          this. sema1 = null;
          this. sema2 = null;
        }
        public void semaYaz(string semaTuru, string degAd, string veri, ComboBox cb)
        {
            veri = veri.Replace("&", "&&");
           
            if (degiskenAdiUygunlugu(degAd) && aktifSema.Text.Length == 0)//Şema tek satırlı ise
            {
                bool semayazdegisti = true;
                if (semaTuru == "islemdeg") aktifSema.Text = degAd + " = " + veri;
                else if (semaTuru == "islem") aktifSema.Text = degAd + " = " + veri;
                else if (semaTuru == "giris") aktifSema.Text = veri + " | " + degAd;
                else if (semaTuru == "cikti") aktifSema.Text = "\"" + veri + "\" | " + degAd;
                else if (semaTuru == "dongu") aktifSema.Text = degAd + " = " + veri + " ; " + cb.Text;
                else if (semaTuru == "kosul") aktifSema.Text = veri;
                else semayazdegisti = false;
                frm1.semaSatir = aktifSema.Text.Split('\n');
                if (semayazdegisti)
                {
                    if (!sematext.Contains(aktifSema.Name))
                    {
                        sematext.Add(aktifSema.Name);
                        sematext.Add(aktifSema.Text);
                    }
                    else    sematext.Insert(sematext.IndexOf(aktifSema.Name) + 1, aktifSema.Text);
                }
               if(semaTuru!="dongu") cb.Items.Add(satirsay(aktifSema.Text).ToString() + ". Satır");
            }
            else if (degiskenAdiUygunlugu(degAd))// Şema çok satırlı ise
            {
                bool semayazdegisti = true;
                if (semaTuru == "islemdeg") aktifSema.Text += "\n" + degAd + " = " + veri;
                else if (semaTuru == "islem") aktifSema.Text += "\n" + degAd + " = " + veri;
                else if (semaTuru == "giris") aktifSema.Text += "\n" + veri + " | " + degAd;
                else if (semaTuru == "cikti") aktifSema.Text += "\n\"" + veri + "\" | " + degAd;
                else if (semaTuru == "dongu") aktifSema.Text = degAd + " = " + veri + " ; " + cb.Text;
                else if (semaTuru == "kosul" && degAd.Substring(0, 4) == "ve__") aktifSema.Text += "\n&&&& " + veri;
                else if (semaTuru == "kosul" && degAd.Substring(0, 4) == "veya") aktifSema.Text += "\n|| " + veri;
                else semayazdegisti = false;
                frm1.semaSatir = aktifSema.Text.Split('\n');

                if (semayazdegisti)
                { sematext.RemoveAt(sematext.IndexOf(aktifSema.Name) + 1); sematext.Insert(sematext.IndexOf(aktifSema.Name) + 1, aktifSema.Text); }

                cb.Items.Add(satirsay(aktifSema.Text).ToString() + ". Satır");// Satır sayısını göstermek için

            }
            else
            {
                if (frm1.button32.Text == "EN") MessageBox.Show("Bu ad veri saklayıcı için uygun değil!", "Veri Saklayıcı Adı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else MessageBox.Show("This name is not suitable for variable name!", "Wrong Variable Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public int kosulkontrol()
        {
            if (aktifSema.Name.Length > 4 && aktifSema.Name.Substring(0, 5) == "kosul" && aktifSema.Text.Length > 0)
            {
                if (aktifSema.Text.Contains("&& ")) return 1;
                else if (aktifSema.Text.Contains("|| ")) return 0;
                else return 2;
            }
            else return 3;
                
        }
        public void semaSatirSil(int satirno,ComboBox cb)
        {
            string[] satir = aktifSema.Text.Split('\n');
            satir[satirno - 1] = "";

            aktifSema.Text = "";
            cb.Items.Clear();
            cb.Items.Add("Yeni Satır");

            for (int i = 0; i < satir.Length; i++) 
            {
                if (satir[i] == "")
                {
                    if (i == 0 && satir.Length > 1 && aktifSema.Name.Substring(0, 5) == "kosul") satir[1] = satir[1].Remove(0,3);
                    continue;
                }

                if (aktifSema.Text.Length == 0) aktifSema.Text = satir[i];
                else if (satir[i]!="") aktifSema.Text += "\n" + satir[i];

                cb.Items.Add(satirsay(aktifSema.Text).ToString() + ". Satır");
            }

            if (satir.Length > 1) { sematext.RemoveAt(sematext.IndexOf(aktifSema.Name) + 1); sematext.Insert(sematext.IndexOf(aktifSema.Name) + 1, aktifSema.Text); }
            else { sematext.RemoveAt(sematext.IndexOf(aktifSema.Name) + 1); sematext.RemoveAt(sematext.IndexOf(aktifSema.Name)); }
                cb.Text = "Yeni Satır";
        }
        public static int satirsay(string s)
        {
            int ssay = 1;
            for (int i = 0; i < s.Length; i++)
            {
                if (s.Substring(i, 1) == "\n") ssay++;
            }
            return ssay;
        }
        public static int satirsayTam(string s)
        {
            int ssay = 1;
            if (s == "") return 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s.Substring(i, 1) == "\n") ssay++;
            }
            return ssay;
        }
        public void semaYazDegistir(int satirno,string yeniSatir)
        {  
                string[] satir = aktifSema.Text.Split('\n'); // Satırlar bir string dizi değişkene aktarılıyor 
                satir[satirno - 1] = "";
                aktifSema.Text = "";
              
                for (int i = 0; i < satir.Length; i++)
                {
                    if (satir[i] == "") satir[i] = yeniSatir;

                    if (aktifSema.Text.Length == 0) aktifSema.Text = satir[i];
                    else if (satir[i] != "") aktifSema.Text += "\n" + satir[i];
                }
                 if(sematext.Count>0)sematext.RemoveAt(sematext.IndexOf(aktifSema.Name)+1); sematext.Insert(sematext.IndexOf(aktifSema.Name) + 1, aktifSema.Text); 
        }
        public static bool degiskenAdiUygunlugu(string degAd)
        {
            int ascii;

            if (degAd.Length > 0)
            {
                ascii = Convert.ToInt16(Convert.ToChar(degAd.Substring(0, 1)));

                if (((ascii > 64 && ascii < 91) || (ascii > 96 && ascii < 123)))
                {
                    for (int i = 1; i < degAd.Length; i++)
                    {
                        ascii = Convert.ToInt16(Convert.ToChar(degAd.Substring(i, 1)));
                        if (ascii == 45 || (ascii > 47 && ascii < 58) || (ascii > 64 && ascii < 91) || ascii == 95 || (ascii > 96 && ascii < 123)) continue;
                        else return false;
                    }
                    return true;
                }
            }
            return false;
        }
       /* private bool yolAyrimi(string[] baglantilar, string sonraki,string donguson)
        {
            int i = 0, bulunduguyer = -1 ;
            string kosul = sonraki,sonrakibag=sonraki;
            bool k1=false, k2=false;
            while (true)
            {


                if (bulunduguyer == i) return false;

                if (baglantilar[i].Length > kosul.Length && baglantilar[i].Substring(0, kosul.Length) == kosul)
                {
                    siraliBaglantilar.Add(baglantilar[i]);
                    sonrakibag = baglantilar[i].Substring(baglantilar[i].IndexOf('-') + 1, baglantilar[i].Length - baglantilar[i].IndexOf('-') - 1);
                    if (sonrakibag.Substring(sonrakibag.Length - 1, 1) == "E" || sonrakibag.Substring(sonrakibag.Length - 1, 1) == "H")
                    {
                        sonrakibag = sonrakibag.Substring(0, sonrakibag.Length - 1);
                        if (sonrakibag.Length > 4 && sonrakibag.Substring(0, 5) == "kosul")
                        {
                            if (yolAyrimi(baglantilar, sonrakibag,donguson))
                            {
                                if (!k1) k1 = true;
                                else if (!k2) k2 = true;
                            }
                            else return false;

                            if (k1 && k2) return true;
                        }
                        else if (sonrakibag.Length > 4 && sonrakibag.Substring(0, 5) == "dongu") return dongu(baglantilar, sonrakibag);
                        else
                        {
        
                            if (sonrakibag.Substring(0, 3) == donguson || sonrakibag == donguson || BaglıSema(baglantilar, sonrakibag) == kosul || baglantiKontrol(baglantilar, sonrakibag)) 
                            {
                                if (!k1) k1 = true;
                                else if (!k2) k2 = true;
                            }
                            else return false;

                            if (k1 && k2) return true;
                        }


                    }
                  
                    bulunduguyer = i;
                }

                if (i < baglantilar.Length - 1) i++;
                else if (bulunduguyer != -1) i = 0;
                else return false;
            }
        }*/
      /*  private bool dongu(string[] baglantilar, string sonraki)
        {
            int i = 0, bulunduguyer = -1;
            string donguad = sonraki, sonrakibag = sonraki,son=sonraki;
            bool k1 = false, k2 = false;
            while (true)
            {


                if (bulunduguyer == i) return false;

                if (baglantilar[i].Length > donguad.Length && baglantilar[i].Substring(0, donguad.Length) == donguad)
                {
                    siraliBaglantilar.Add(baglantilar[i]);
                   // frm1.listBox1.Items.Add(baglantilar[i]);
                    sonrakibag = baglantilar[i].Substring(baglantilar[i].IndexOf('-') + 1, baglantilar[i].Length - baglantilar[i].IndexOf('-') - 1);
                    if (sonrakibag.Substring(sonrakibag.Length - 1, 1) == "E" || sonrakibag.Substring(sonrakibag.Length - 1, 1) == "H")
                    {
                        bool yol = false;
                        if (sonrakibag.Substring(sonrakibag.Length - 1, 1) == "E") yol = true;
                        if (!yol) son = "son";

                        sonrakibag = sonrakibag.Substring(0, sonrakibag.Length - 1);
                        if (sonrakibag.Length > 4 && sonrakibag.Substring(0, 5) == "kosul")
                        {
                            if (yolAyrimi(baglantilar, sonrakibag, son))
                            {
                                if (!k1) k1 = true;
                                else if (!k2) k2 = true;
                            }
                            else return false;

                            if (k1 && k2) return true;
                        }
                        else if (sonrakibag.Length > 4 && sonrakibag.Substring(0, 5) == "dongu") return dongu(baglantilar, sonrakibag);
                        else
                        {
                            if (sonrakibag.Substring(0, 3) == son || sonrakibag == son  || baglantiKontrol(baglantilar, sonrakibag))
                            {
                                if (!k1) k1 = true;
                                else if (!k2) k2 = true;
                            }
                            else return false;

                            if (k1 && k2) return true;
                        }


                    }

                    bulunduguyer = i;
                }

                if (i < baglantilar.Length - 1) i++;
                else if (bulunduguyer != -1) i = 0;
                else return false;
            }
        }*/
        private bool baglantiKontrol(string[] baglantilar, string sonraki,string sonKsl)//İç içe döngüler ve iç içe koşullar çalışıyor, bu ikilinin kombinasyonlarını da test et
        {
           
          
            bool kosE = false, kosH = false;
            string  kos1 = "bos", kos2 = "bos",dngEvt="bos";
            for (int i = 0; i<baglantilar.Length;i++ )
            {
                if (baglantilar[i]!=null &&baglantilar[i].Length>=sonraki.Length &&baglantilar[i].Substring(0, sonraki.Length) == sonraki)
                {
                   // frm1.listBox1.Items.Add(baglantilar[i]);
                    if(!siraliBaglantilar.Contains(baglantilar[i]))siraliBaglantilar.Add(baglantilar[i]);
                    string bag1,bag2 = baglantilar[i].Substring(baglantilar[i].IndexOf('-') + 1, baglantilar[i].Length - baglantilar[i].IndexOf('-') - 1);
                    bag1 = sonraki;
                    sonraki = bag2;
                    if (bag1.Length > 2)
                    {
                        if (bag1.Substring(0, 3) == "kos")
                        {
                            if (bag1.Substring(bag1.Length - 1, 1) == "E" || bag1.Substring(bag1.Length - 1, 1) == "H") bag1 = bag1.Substring(0,bag1.Length-1);

                            if (kos1 == "bos")
                            {
                                kos1 = sonraki.Substring(0, sonraki.Length - 1);
                                sonraki = bag1; 
                            }
                            else if (kos2 == "bos")
                            {
                                if (kos1 == sonraki.Substring(0, sonraki.Length - 1)) { if (i == (baglantilar.Length - 1)) return false; sonraki = bag1; continue; }
                                kos2 = sonraki.Substring(0, sonraki.Length - 1);
                                if (kos1 == kos2) return false;

                                if (kos1.Substring(0, 3) != sonKsl && kos1 != sonKsl) kosE = baglantiKontrol(baglantilar, kos1, sonKsl);
                                else kosE = true;
                                
                                if (kos2.Substring(0, 3) != sonKsl && kos2 != sonKsl) kosH = baglantiKontrol(baglantilar, kos2, sonKsl);
                                else kosH = true;

                                if (kosE && kosH) return true; 
                                else  return false; 
                            }
                        }
                        else if (bag1.Substring(0, 3) == "don")//çıkan okları değil girenleri kotrol et --> saçma bir Düşünceydi
                        {
                            if (bag1.Substring(bag1.Length - 1, 1) == "E" || bag1.Substring(bag1.Length - 1, 1) == "H") bag1 = bag1.Substring(0, bag1.Length - 1);
                            char dngBag = Convert.ToChar(sonraki.Substring(sonraki.Length-1,1));
                           
                            if (dngBag == 'E')dngEvt= sonraki.Substring(0, sonraki.Length - 1);


                            if (kos1 == "bos")
                            {
                                kos1 = sonraki.Substring(0, sonraki.Length - 1);
                                sonraki = bag1;
                            }
                            else if (kos2 == "bos")
                            {
                                if (kos1 == sonraki.Substring(0, sonraki.Length - 1)) { if (i == (baglantilar.Length - 1)) return false; sonraki = bag1; continue; }
                                kos2 = sonraki.Substring(0, sonraki.Length - 1);
                                if (kos1 == kos2) return false;
                                if (kos1 == dngEvt)
                                {
                                    if (kos1.Substring(0, 3) != sonKsl && kos1 != sonKsl) kosE = baglantiKontrol(baglantilar, kos1, bag1);
                                    else kosE = true;

                                    if (kos2.Substring(0, 3) != sonKsl && kos2 != sonKsl) kosH = baglantiKontrol(baglantilar, kos2, sonKsl);
                                    else kosH = true;
                                }
                                if (kos2 == dngEvt)
                                {
                                    if (kos1.Substring(0, 3) != sonKsl && kos1 != sonKsl) kosE = baglantiKontrol(baglantilar, kos1, sonKsl);
                                    else kosE = true;

                                    if (kos2.Substring(0, 3) != sonKsl && kos2 != sonKsl) kosH = baglantiKontrol(baglantilar, kos2, bag1);
                                    else kosH = true;
                                }

                                if (kosE && kosH) return true;
                                else return false;
                            }
                            
                        }
                    }
                    if (kos1 == "bos" && sonraki.Length > 2 && (sonraki.Substring(0, 3) == sonKsl || sonraki == sonKsl)) return true;
                    i = 0;
                }
                if (i == (baglantilar.Length - 1)) return false;
            }
            return true;

        }
        private string BaglıSema(string[] baglantilar, string sonraki)
        {
            string sonrakibag = "null";
            int i = 0, bulunduguyer = -1;
            while (true)
            {

                if (bulunduguyer == i) return "null";
                if (baglantilar[i].Length >= sonraki.Length && baglantilar[i].Substring(0, sonraki.Length) == sonraki)
                {
                   // siraliBaglantilar.Add(baglantilar[i]);
                   // frm1.listBox1.Items.Add(baglantilar[i]);
                    sonrakibag = baglantilar[i].Substring(baglantilar[i].IndexOf('-') + 1, baglantilar[i].Length - baglantilar[i].IndexOf('-') - 1);
                    if (sonrakibag.Substring(sonrakibag.Length - 1, 1) == "E" || sonrakibag.Substring(sonrakibag.Length - 1, 1) == "H") sonrakibag = sonrakibag.Substring(0, sonrakibag.Length - 1);

                    return sonrakibag;
                }

                if (i < baglantilar.Length - 1) i++;
                else if (bulunduguyer != -1) i = 0;
                else return "null";
            }
        }
        public void calistir()
        {
           // frm1.listBox1.Items.Clear();
            string []baglantilar = baglayici.baglantilariVer();
            bool baslaBul = false,derlenebilir=true;
            string sonraki="basla";
            for (int i = 0; i < baglantilar.Length; i++)
            {
                if (!baslaBul && baglantilar[i].Length > 4 && baglantilar[i].Substring(0, 5) == "basla")
                {
                    bool baglantiDogrulandi = false;
                    baslaBul = true;
                    sonraki = baglantilar[i];

                    baglantiDogrulandi = baglantiKontrol(baglantilar, sonraki,"son");

                    if (baglantiDogrulandi) break;
                    else
                    {
                        if (frm1.button32.Text == "EN") MessageBox.Show("Şema bağlantılarında  hata var !", "Bağlantı Bildirimi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else MessageBox.Show("There is an error in the connection of Chart !", "Connection of Chart Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        derlenebilir = false; break;
                    }
                }
            }
            if (!baslaBul)
            {
                if (frm1.button32.Text == "EN") MessageBox.Show("Şema bağlantılarında  hata var !", "Bağlantı Bildirimi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("There is an error in the connection of Chart !", "Connection of Chart Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                derlenebilir = false;
            }
            
            if (derlenebilir)
            {
                dr = new Derleyici(siraliBaglantiVer(),semaKodVer(),frm1);
                dr.derle();
                derlendi = true;
            }
            siraliBaglantilar.Clear();
        }
        public void adimcalistir()
        {
            if (derlendi) dr.calistir();
        }
        public void durdur()
        {
           if(dr!=null) dr.durdur();
        }
        public void akisDuraklat()
        {
            if (dr != null) dr.islembitir = false;
        }
        public void akisDevam()
        {
            if (dr != null) dr.islembitir = true;
        }
        public ArrayList degiskenGoster()
        {
            if (derlendi)
            {
                return dr.degiskenGoster();
            }
            else return null;
        }

        public void boyutlandir(int byt,int yon)
        {
            if (aktifSema != null)
            {
                if (yon == 3)
                {
                    aktifSema.Height += byt;
                    aktifSema.Width += byt;
                }
                else if (yon == 2)
                {
                    aktifSema.Height += byt;
                }
                else
                {
                    aktifSema.Width += byt;
                }
                int result;
                if (aktifSema == null) return;
                string[] sayi = aktifSema.Tag.ToString().Split('%');

                if (sayi[0] != "")
                    for (int i = 0; sayi.Length > i; i++)
                        if (int.TryParse(sayi[i], out result)) baglayici.boyutGuncelle(Convert.ToInt16(sayi[i]), aktifSema.Width, aktifSema.Height);
            }
        }
        public void timerInterval(int tmrint)
        {
            if(derlendi)dr.gecikmeSuresi = tmrint;
        }
        public string[] siraliBaglantiVer()
        {

            string[] siraliBg = new string[siraliBaglantilar.Count];

            for (int i = 0; i < siraliBaglantilar.Count; i++)
                siraliBg[i] = siraliBaglantilar[i].ToString();

            return siraliBg;
        }
        public string[] semaKodVer()
        {

            string[] kod = new string[sematext.Count];

            for (int i = 0; i < sematext.Count; i++)
                kod[i] = sematext[i].ToString();

            return kod;
        }
        public void semaOlustur(string ad, Image img)
        {
            semaNo++;
         
            Point btnknm;
            Button btn = new Button();
            btn.Name = ad + semaNo.ToString();
            btn.Parent = frm1;
            btn.BackColor = Color.White;
            btn.BackgroundImage = img;
            btn.Size = new Size(100, 55);
            btn.Padding = new Padding(10, 1, 10, 1);
            btn.FlatAppearance.BorderColor = Color.DeepSkyBlue;
            btn.FlatAppearance.BorderSize = 2;
            btn.Font=new Font("Consolas",11);
            btn.FlatStyle = FlatStyle.Flat;
            btn.AutoSize = true;
            btnknm = konumButonu.Location;
            if(semaNo>1)
            {         

                 if ((konumButonu.Top + konumButonu.Height+50) > frm1.Height)
                 {
                     konumButonu.BackColor = Color.Black;
                     if (konumButonu.Left + konumButonu.Width < (frm1.Width - 40)) konumButonu.Left += konumButonu.Width + 20;
                     else { konumButonu.Location = new Point(150, 0); konumButonu.BackColor = Color.White; }

                 }
                 else if (konumButonu.Top -25 < 0 && konumButonu.BackColor==Color.Black)
                 {
                    
                     konumButonu.BackColor = Color.White;
                     if (konumButonu.Left + konumButonu.Width < (frm1.Width - 40)) konumButonu.Left += konumButonu.Width + 20;
                     else { konumButonu.Location = new Point(150, 0); }

                 }


                 if (konumButonu.BackColor == Color.Black)  btnknm = new Point(konumButonu.Left, konumButonu.Top -( konumButonu.Height + 20));
                 else    btnknm = new Point(konumButonu.Left, konumButonu.Top + konumButonu.Height + 20);

            }
            btn.Location = btnknm;
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            btn.Tag = "";
            if (ad == "son") {
                if (frm1.button32.Text == "EN") btn.Text = "Son";
                else btn.Text = "End";
            }
            else if (ad == "basla")
            {
                if (frm1.button32.Text == "EN") btn.Text = "Başla";
                else btn.Text = "Start";
            }
            btn.BringToFront();
            btn.MouseDown += new MouseEventHandler(btn_MouseDown);
            btn.MouseMove += new MouseEventHandler(btn_MouseMove);
            btn.MouseUp += new MouseEventHandler(btn_MouseUp);
            btn.Click += new EventHandler(btn_Click);
            btn.TextChanged += btn_TextChanged;
            konumButonu.Location = btnknm;

        }

        void btn_TextChanged(object sender, EventArgs e)
        {
            frm1.semaSatir = aktifSema.Text.Split('\n');
        }
        public void semaOlusturAc(string ad, Image img,int top,int left,int width,int height,string tag,Color bckclr,string text)
        {
            Button btn = new Button();
            btn.Name = ad;
            btn.Parent = frm1;
            btn.BackColor = bckclr;
            btn.BackgroundImage = img;
            btn.Height = height;
            btn.Width = width;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Consolas", 11);
            btn.FlatStyle = FlatStyle.Flat;
            btn.AutoSize = true;
            btn.Padding = new Padding(10, 1, 10, 1);
            btn.Left = left;
            btn.Top = top;
            btn.BackgroundImageLayout = ImageLayout.Stretch;
            btn.Tag = tag;
            btn.Text = text;
            btn.BringToFront();
            btn.MouseDown += new MouseEventHandler(btn_MouseDown);
            btn.MouseMove += new MouseEventHandler(btn_MouseMove);
            btn.MouseUp += new MouseEventHandler(btn_MouseUp);
            btn.Click += new EventHandler(btn_Click);
        }
        public void semaBaglantiAc(string l1, string l2, string g1, string g2, string y1, string y2, string t1, string t2, string bag)
        {
            baglayici.programAc( l1,  l2,  g1,  g2,  y1,  y2,  t1,  t2,  bag);
        }
        public void programKaydet(string yol)
        {
            baglayici.programKaydet(yol);
        }
        public void baglantilariSil()
        {
            
            baglayici.tumBaglantiSil();
            if(konumButonu!=null)konumButonu.Location = new Point((frm1.Width) / 2, 0);
        }
    }
}
