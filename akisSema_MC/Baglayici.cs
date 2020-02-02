using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace akisSema_MC
{
    class Baglayici
    {
        public Baglayici(Form f)
        {
            frm1 = f;

        }

        
        public Pen firca = new Pen(System.Drawing.Color.Black, 3);
        ArrayList top1 = new ArrayList(), top2 = new ArrayList(), left1 = new ArrayList(), left2 = new ArrayList();
        ArrayList yukseklik1 = new ArrayList(), yukseklik2 = new ArrayList(), genislik1 = new ArrayList(), genislik2 = new ArrayList(), baglanti = new ArrayList();
        static Form frm1;
        Graphics grafiknesne;
        public void bagla(Point konum1,Point konum2,Size boyut1,Size boyut2)//şu anda boyut tam anlamıyla kullanılmıyor
        {
            grafiknesne = frm1.CreateGraphics();
            int toport, widthort;
            //poligon kullanılarak da çizilebilir

            widthort = konum1.X;
            toport = konum1.Y + (boyut1.Height  / 2);
            grafiknesne.DrawLine(firca, widthort, toport, konum2.X + (boyut2.Width / 2), toport);

            widthort = konum2.X + (boyut2.Width / 2);
            toport = konum1.Y + (boyut1.Height  / 2);
            grafiknesne.DrawLine(firca, widthort, toport, widthort, konum2.Y);

            #region ok ucu
            if ((konum1.Y - konum2.Y) < 0 && (konum1.Y - konum2.Y) < -15)
            {
                grafiknesne.DrawLine(firca, widthort + 8, konum2.Y - 14, widthort, konum2.Y - 1);
                grafiknesne.DrawLine(firca, widthort - 8, konum2.Y - 14, widthort, konum2.Y - 1);

            }
            else if ((konum1.Y - konum2.Y) > 0 && (konum1.Y - konum2.Y) > 15)
            {
                grafiknesne.DrawLine(firca, widthort + 8, (konum2.Y + boyut2.Height) + 14, widthort, konum2.Y + boyut2.Height);
                grafiknesne.DrawLine(firca, widthort - 8, (konum2.Y + boyut2.Height) + 14, widthort, konum2.Y + boyut2.Height);

            }
            else if ((konum1.X - konum2.X) < 0 && (konum1.X - konum2.X) < -15)
            {
                grafiknesne.DrawLine(firca, konum2.X - 14, toport + 8, konum2.X - 1, toport);
                grafiknesne.DrawLine(firca, konum2.X - 14, toport - 8, konum2.X - 1, toport);

            }
            else if ((konum1.X - konum2.X) > 0 && (konum1.X - konum2.X) > 15)
            {
                grafiknesne.DrawLine(firca, (konum2.X + boyut2.Width) + 14, toport + 8, (konum2.X + boyut2.Width) + 1, toport);
                grafiknesne.DrawLine(firca, (konum2.X + boyut2.Width) + 14, toport - 8, (konum2.X + boyut2.Width) + 1, toport);

            }
            #endregion

        }
        public void programKaydet(string yol)
        {       

            for (int i = 0; i < top1.Count; i++)
            {      
                    StreamWriter SW = File.AppendText(yol);

                    SW.WriteLine(Sifrele(left1[i].ToString()));
                    SW.WriteLine(Sifrele(left2[i].ToString()));
                    SW.WriteLine(Sifrele(genislik1[i].ToString()));
                    SW.WriteLine(Sifrele(genislik2[i].ToString()));
                    SW.WriteLine(Sifrele(yukseklik1[i].ToString()));
                    SW.WriteLine(Sifrele(yukseklik2[i].ToString()));
                    SW.WriteLine(Sifrele(top1[i].ToString()));
                    SW.WriteLine(Sifrele(top2[i].ToString()));
                    SW.WriteLine(Sifrele(baglanti[i].ToString()));
                    SW.WriteLine(Sifrele("-*-SaTiRSoNu+-."));
                    SW.Close();
                
            } 
        }
        public void tumBaglantiSil()
        {
            if (grafiknesne != null)
            {
                grafiknesne = frm1.CreateGraphics();
                grafiknesne.Clear(Color.White);
                grafiknesne.Dispose();
            }
            top1.Clear();
            top2.Clear();
            left1.Clear();
            left2.Clear();
            yukseklik1.Clear();
            yukseklik2.Clear();
            genislik1.Clear();
            genislik2.Clear();
            baglanti.Clear();

            top1.TrimToSize();
            top2.TrimToSize();
            left1.TrimToSize();
            left2.TrimToSize();
            yukseklik1.TrimToSize();
            yukseklik2.TrimToSize();
            genislik1.TrimToSize();
            genislik2.TrimToSize();
            baglanti.TrimToSize();
        }
        public void programAc(string l1,string l2,string g1,string g2,string y1,string y2,string t1,string t2,string bag)
        {
            grafiknesne = frm1.CreateGraphics();
            top1.Add(t1);
            top2.Add(t2);
            left1.Add(l1);
            left2.Add(l2);
            yukseklik1.Add(y1);
            yukseklik2.Add(y2);
            genislik1.Add(g1);
            genislik2.Add(g2);
            baglanti.Add(bag);
        }

        public void ciz()
        {
            
           /* firca = new Pen(System.Drawing.Color.DeepSkyBlue, 1);
            firca.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            for (int i = 0; i < frm1.Width/30; i++)
            {
                grafiknesne.DrawLine(firca, i * 30, 0, i * 30, frm1.Height);
                grafiknesne.DrawLine(firca, 0, i * 30, frm1.Width, i * 30);
            }
            firca.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            firca = new Pen(System.Drawing.Color.Black, 3);*/
            for (int i = 0; i < top1.Count; i++)
            {
                if (top1[i].ToString() != "atla")
                {
                    Point konum1 = new Point(Convert.ToInt32(left1[i]), Convert.ToInt32(top1[i]));
                    Point konum2 = new Point(Convert.ToInt32(left2[i]), Convert.ToInt32(top2[i]));
                    Size boyut1 = new Size(Convert.ToInt32(genislik1[i]), Convert.ToInt32(yukseklik1[i]));
                    Size boyut2 = new Size(Convert.ToInt32(genislik2[i]), Convert.ToInt32(yukseklik2[i]));

                    string[] baglanan = baglanti[i].ToString().Split('-');

                    if (baglanan.Length > 1)
                    {
                        if (kosulOrdongu(baglanan[0]) && baglanan[1].Substring(baglanan[1].Length - 1, 1) == "E") firca.Color = Color.Green;
                        else if (kosulOrdongu(baglanan[0]) && baglanan[1].Substring(baglanan[1].Length - 1, 1) == "H") firca.Color = Color.Red;
                        else firca.Color = Color.Black;
                    }

                    bagla(konum1, konum2, boyut1, boyut2);
                }
            }
            //grafiknesne.Dispose();

        }
        public void Yenidenciz()
        {
            grafiknesne.Clear(Color.White);
            
            for (int i = 0; i < top1.Count; i++)
            {
                if (top1[i].ToString() != "atla")
                {
                 
                    Point konum1 = new Point(Convert.ToInt32(left1[i]), Convert.ToInt32(top1[i]));
                    Point konum2 = new Point(Convert.ToInt32(left2[i]), Convert.ToInt32(top2[i]));
                    Size boyut1 = new Size(Convert.ToInt32(genislik1[i]), Convert.ToInt32(yukseklik1[i]));
                    Size boyut2 = new Size(Convert.ToInt32(genislik2[i]), Convert.ToInt32(yukseklik2[i]));

                    string[] baglanan = baglanti[i].ToString().Split('-');

                    if (baglanan.Length > 1)
                    {
                        if (kosulOrdongu(baglanan[0]) && baglanan[1].Substring(baglanan[1].Length - 1, 1) == "E") firca.Color = Color.Green;
                        else if (kosulOrdongu(baglanan[0]) && baglanan[1].Substring(baglanan[1].Length - 1, 1) == "H") firca.Color = Color.Red;
                        else firca.Color = Color.Black;
                    }

                    bagla(konum1, konum2, boyut1, boyut2);
                }
            }


        }
        public void konumGuncelle(int indis, int x, int y)
        {
            grafiknesne = frm1.CreateGraphics();

            if (indis > 0)
            {
                indis--;
                if (indis < top1.Count && top1[indis].ToString() != "atla")
                {
                    Point konum1 = new Point(Convert.ToInt32(left1[indis]), Convert.ToInt32(top1[indis]));
                    Point konum2 = new Point(Convert.ToInt32(left2[indis]), Convert.ToInt32(top2[indis]));
                    Size boyut1 = new Size(Convert.ToInt32(genislik1[indis]), Convert.ToInt32(yukseklik1[indis]));
                    Size boyut2 = new Size(Convert.ToInt32(genislik2[indis]), Convert.ToInt32(yukseklik2[indis]));
                    firca.Color = Color.White;
                    bagla(konum1, konum2, boyut1, boyut2);
                    top1[indis] = y;
                    left1[indis] = x;

                }
            }
            else if (indis < 0)
            {
                indis++;
                indis = indis * -1;
                if (indis < top1.Count && top1[indis].ToString() != "atla")
                {
                    Point konum1 = new Point(Convert.ToInt32(left1[indis]), Convert.ToInt32(top1[indis]));
                    Point konum2 = new Point(Convert.ToInt32(left2[indis]), Convert.ToInt32(top2[indis]));
                    Size boyut1 = new Size(Convert.ToInt32(genislik1[indis]), Convert.ToInt32(yukseklik1[indis]));
                    Size boyut2 = new Size(Convert.ToInt32(genislik2[indis]), Convert.ToInt32(yukseklik2[indis]));
                    firca.Color = Color.White;
                    bagla(konum1, konum2, boyut1, boyut2);
                    top2[indis] = y;
                    left2[indis] = x;

                }
            }
          //  grafiknesne.Clear(Color.White);
            ciz();
        }
        public void boyutGuncelle(int indis, int x, int y)
        {
            grafiknesne.Clear(Color.White);
            if (indis > 0)
            {
                indis--;
                if (indis < top1.Count && top1[indis].ToString() != "atla")
                {
                    yukseklik1[indis] = y;
                    genislik1[indis] = x;
                }
            }
            else if (indis < 0)
            {
                indis++;
                indis = indis * -1;
                if (indis < top1.Count && top1[indis].ToString() != "atla")
                {
                    yukseklik2[indis] = y;
                    genislik2[indis] = x;
                }
            }
            ciz();
        }
        private bool kosulOrdongu(string isim)
        {
            if (isim.Length > 4 && (isim.Substring(0, 5) == "kosul" || isim.Substring(0, 5) == "dongu")) return true;
            else return false;
        }
        static public string Sifrele(string veri)
        {

            // gelen veri byte dizisine aktarılıyor

            byte[] veriByteDizisi = System.Text.ASCIIEncoding.UTF8.GetBytes(veri);

            // base64 şifreleme algoritmasına göre şifreleniyor.

            string sifrelenmisVeri = System.Convert.ToBase64String(veriByteDizisi);

            return sifrelenmisVeri;
        }
        public bool baglantiKontrol(string baglanan1, string baglanan2)
        {
            if (baglanti.IndexOf(baglanan1 + "-" + baglanan2) == -1 && (baglanti.IndexOf(baglanan2 + "-" + baglanan1) == -1 || baglanan2.IndexOf("dongu") > -1|| baglanan1.IndexOf("kosul")>-1))
                return true;
            return false;
        }
        public int baglantiKaydet(Point konum1,Point konum2,Size boyut1,Size boyut2,string baglanan1,string baglanan2)
        {
                    top1.Add(konum1.Y);
                    top2.Add(konum2.Y);
                    left1.Add(konum1.X);
                    left2.Add(konum2.X);
                    yukseklik1.Add(boyut1.Height);
                    yukseklik2.Add(boyut2.Height);
                    genislik1.Add(boyut1.Width);
                    genislik2.Add(boyut2.Width);
                    baglanti.Add(baglanan1 + "-" + baglanan2);
                    return baglanti.Count;
        }
        public int baglantiSil(string baglananlar)
        {
            int indis = baglanti.IndexOf(baglananlar);
            if (indis > -1)
            {
                if (indis < top1.Count && top1[indis].ToString() != "atla")
                {
                    Point konum1 = new Point(Convert.ToInt32(left1[indis]), Convert.ToInt32(top1[indis]));
                    Point konum2 = new Point(Convert.ToInt32(left2[indis]), Convert.ToInt32(top2[indis]));
                    Size boyut1 = new Size(Convert.ToInt32(genislik1[indis]), Convert.ToInt32(yukseklik1[indis]));
                    Size boyut2 = new Size(Convert.ToInt32(genislik2[indis]), Convert.ToInt32(yukseklik2[indis]));
                    firca.Color = Color.White;
                    bagla(konum1, konum2, boyut1, boyut2);
                }
                top1[indis] = "atla";
                top2[indis] = "atla";
                left1[indis] = "atla";
                left2[indis] = "atla";
                yukseklik1[indis] = "atla";
                yukseklik2[indis] = "atla";
                genislik1[indis] = "atla";
                genislik2[indis] = "atla";
                baglanti[indis] = "atla";
                ciz();     return indis;
            }
            else MessageBox.Show("Bağlantı bulunamadı !", "Bağlantı Bildirimi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return -1;
        }
        public int kosulDonguBaglantiSil(string baglananlar)
        {
            int HorE = 0;
            int indis = baglanti.IndexOf(baglananlar + 'E');

            if (indis < 0) { indis = baglanti.IndexOf(baglananlar + 'H'); HorE = 2; }
            else HorE = 1;

            if (indis > -1)
            {
                if (indis < top1.Count && top1[indis].ToString() != "atla")
                {
                    Point konum1 = new Point(Convert.ToInt32(left1[indis]), Convert.ToInt32(top1[indis]));
                    Point konum2 = new Point(Convert.ToInt32(left2[indis]), Convert.ToInt32(top2[indis]));
                    Size boyut1 = new Size(Convert.ToInt32(genislik1[indis]), Convert.ToInt32(yukseklik1[indis]));
                    Size boyut2 = new Size(Convert.ToInt32(genislik2[indis]), Convert.ToInt32(yukseklik2[indis]));
                    firca.Color = Color.White;
                    bagla(konum1, konum2, boyut1, boyut2);
                }
                top1[indis] = "atla";
                top2[indis] = "atla";
                left1[indis] = "atla";
                left2[indis] = "atla";
                yukseklik1[indis] = "atla";
                yukseklik2[indis] = "atla";
                genislik1[indis] = "atla";
                genislik2[indis] = "atla";
                baglanti[indis] = "atla";
                ciz();
                if (HorE == 1) return indis;
                else return indis * -1;
            }
            else
            {
                MessageBox.Show("Bağlantı bulunamadı !", "Bağlantı Bildirimi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return 0;
        }
        public string[] baglantilariVer()
        {
            baglanti.TrimToSize();
            top1.TrimToSize();
            top2.TrimToSize();
            left1.TrimToSize();
            left2.TrimToSize();
            yukseklik1.TrimToSize();
            yukseklik2.TrimToSize();
            genislik1.TrimToSize();
            genislik2.TrimToSize();

            string[] baglantilar = new string[baglanti.Count];

            for (int i = 0; i < baglanti.Count; i++)
                baglantilar[i] = baglanti[i].ToString();

                return baglantilar;
        }
    }
}
