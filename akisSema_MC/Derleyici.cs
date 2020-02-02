using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Timers;
using System.Drawing;

namespace akisSema_MC
{
    class Derleyici
    {
        ArrayList degiskenler = new ArrayList();
        ArrayList degiskenAd = new ArrayList();
        int gecikme = 0;
        Form1 frm;
        string[] baglantilar;
        int say = 0;
        public bool islembitir=false;
        string[] text;
        int j = 0;
        bool ksl = false, dgskngster = false,dngksl = false, kslkontrol = false, dngkslkontrol=false;
        string kosulAd = "nll", dngAd = "nll";
        ArrayList dnguad = new ArrayList();
        ArrayList kslAd = new ArrayList();
        ArrayList dngsayac = new ArrayList();
        ArrayList kslSayac = new ArrayList();
        string[] semakod;
        Button tmpbtn;
        Button[] semalar;
        public int gecikmeSuresi
        {
            set
            {
              gecikme = value;
            }
        }
        public Derleyici(string[] siralibaglanti, string[] kod, Form1 frm1)
        {
            frm = frm1;
            semakod = kod;
            baglantilar = new string[siralibaglanti.Length* 2];//null değer gelmesini engelle
            int sayac = 0;
            for (int i = 0; i < siralibaglanti.Length; i++)
            {
                string[] temp;
                string dallanma="yok";
                if (siralibaglanti[i] != null) temp = siralibaglanti[i].Split('-');
                else break;
                if (sayac > 0) dallanma = baglantilar[sayac - 1].Substring(baglantilar[sayac - 1].Length - 1, 1);
                if (dallanma == "E" || dallanma == "H") dallanma = baglantilar[sayac - 1].Substring(0, baglantilar[sayac - 1].Length - 1);
                else dallanma = "yok";

                if (sayac > 0 && temp[0] != baglantilar[sayac - 1] && temp[0] != dallanma) baglantilar[sayac++] = temp[0];
                if(sayac>0)frm1.listBox1.Items.Add(baglantilar[sayac-1]);

                baglantilar[sayac++] = temp[1];  
            }


            for (int i = 0; i < baglantilar.Length; i++)
            {
                if (baglantilar[i] == null || i == baglantilar.Length - 1) { semalar = new Button[i]; break; }
            }

            int j = 0;
            foreach (Control c in frm.Controls)
            {

                if (baglantilar.Contains(c.Name) || baglantilar.Contains(c.Name+"E") || baglantilar.Contains(c.Name+"H"))
                {
                    semalar[j] = (Button)c; j++;
                }
            }
             //   listeyeyaz(baglantilar);
                //MessageBox.Show("");

             }

        private void listeyeyaz(string[] yazidizini)
        {
            frm.listBox1.Items.Clear();
            for (int i = 0; i < baglantilar.Length; i++)
            {
                if (baglantilar[i] != null) frm.listBox1.Items.Add(baglantilar[i]);
                else break;
            }
        }
        public void derle()
        {

            if (!frm.checkBox3.Checked) gecikme = frm.trackBar1.Value * 10;
            else gecikme = -1;

            text = new string[baglantilar.Length];
            for (int i = 0; i < baglantilar.Length; i++)
            {
                if (baglantilar[i] == null) break;
                foreach(Control c in frm.Controls)
                {
                    string yolayrimi = "";
                    if (baglantilar[i].Substring(baglantilar[i].Length - 1, 1) == "E" || baglantilar[i].Substring(baglantilar[i].Length - 1, 1) == "H") yolayrimi = baglantilar[i].Substring(0, baglantilar[i].Length - 1);
                    else yolayrimi = baglantilar[i];

                    if (yolayrimi == c.Name)
                    {
                         text[j] = baglantilar[i] + "\n" + c.Text;
                        j++;
                      
                        break;
                    }
                }
            }

            calistir();
        }
        public  void calistir()
        {
            if (frm.checkBox3.Checked)  gecikme= -1; 
            else    say =0;

            if (say >= baglantilar.Length) say = 0;
            islembitir = true;
            semaisle();
        }
        public void durdur()
        {
            islembitir = false;
            frm.calismaBitti();
            listeyeyaz(semakod);
        }
       private void semaisle()
        {
            while (islembitir)
            {
                Application.DoEvents();
                for (int k = 0; k < semalar.Length; k++)
                {
                    if (semalar[k] != null && (semalar[k].Name == baglantilar[say] || baglantilar[say] == semalar[k].Name + "E" || baglantilar[say] == semalar[k].Name + "H"))
                    {
                       
                        string semaKosulu = (baglantilar[say].Substring(baglantilar[say].Length - 1, 1));

                        if (kslkontrol)
                        {
                            if (say == 0) break;
                            if (baglantilar[say - 1] != kosulAd) { break; }
                            if (semaKosulu != "E" && semaKosulu != "H") break;

                            if (ksl == true && semaKosulu == "H") { break; }
                            if (ksl == false && semaKosulu == "E") { break; }

                            string bagkslbul=baglantilar[say].Substring(0, baglantilar[say].Length - 1);
                            for (int j = 0; j < baglantilar.Length; j++)
                            {
                                if (baglantilar[j] ==bagkslbul ) { say = j - 1; break; }
                            }
                            kslkontrol = false;
                            break;
                        }
                        if (dngkslkontrol)
                        {
                            if (say == 0) break;
                            if (baglantilar[say - 1] != dngAd) { break; }
                            if (semaKosulu != "E" && semaKosulu != "H") break;

                            if (dngksl == true && semaKosulu == "H") { break; }
                            if (dngksl == false && semaKosulu == "E") { break; }

                            if (baglantilar[say].Substring(0, 3) == "son") { islembitir = false; gecikme = 0; }

                            string bagkslbul = baglantilar[say].Substring(0, baglantilar[say].Length - 1);
                            for (int j = 0; j < baglantilar.Length; j++)
                            {
                                if (baglantilar[j] == bagkslbul) { say = j - 1; break; }
                            }
                                dngkslkontrol = false;
                                break;
                        }
                        if (gecikme > 0) System.Threading.Thread.Sleep(gecikme);
                        else if (gecikme < 0) islembitir = false;


                        if (tmpbtn != null) tmpbtn.FlatAppearance.BorderSize = 0;
                        semalar[k].FlatAppearance.BorderColor = Color.Green;
                        semalar[k].FlatAppearance.BorderSize = 2;
                        tmpbtn = semalar[k];
                        if (frm.degiskengosteraktif) frm.degiskenGuncelle();

                        if (baglantilar[say].Substring(0, 3) == "bas") { }
                        else if (baglantilar[say].Substring(0, 3) == "isl")
                        {
                      
                            islem(semalar[k].Text);
                        }
                        else if (baglantilar[say].Substring(0, 3) == "kos")
                        {
                            ksl = kosul(semalar[k].Text); kslkontrol = true; kosulAd = (baglantilar[say]);
                            if (semaKosulu == "H" || semaKosulu == "E") kosulAd = kosulAd.Substring(0, kosulAd.Length - 1);
                        }
                        else if (baglantilar[say].Substring(0, 3) == "don")
                        {
                          
                            dngksl = dongu(semalar[k].Text); dngkslkontrol = true; dngAd = (baglantilar[say]);
                            if (semaKosulu == "H" || semaKosulu == "E") dngAd = dngAd.Substring(0, dngAd.Length - 1); break;
                        }
                        else if (baglantilar[say].Substring(0, 3) == "gir")
                        {
                            giris(semalar[k].Text);
                        }
                        else if (baglantilar[say].Substring(0, 3) == "cik")
                        {
                            frm.richTextBox1.Text += cikti(semalar[k].Text);
                        }
                        else if (baglantilar[say].Substring(0, 3) == "son" && !kslkontrol) { islembitir = false; gecikme = 0; }

                    }
                }
                   if (say < baglantilar.Length - 1) say++;
                   else say = 0;
            }

            if(gecikme==0)frm.calismaBitti();
        }

       /* private void semaisle1()
        {
            while (islembitir)
            {
                Application.DoEvents();
                if (!dur)
                {
                    if (gecikme > 0) System.Threading.Thread.Sleep(gecikme);
                    say++;
                    if (kslAd != null)
                        if (baglantilar[say] == null) say = 0;
                    if ((kosulAd == "nll" && dngad == "nll" && baglantilar[say].Length > 2 && baglantilar[say].Substring(0, 3) == "son")) break;

                    if (dngad != "nll" && say > 0)//bu kodu optimize et 
                    {
                        if ((dngad == baglantilar[say - 1] || dngad + "E" == baglantilar[say - 1] || dngad + "H" == baglantilar[say - 1]))
                        {
                            if ((dngksl && baglantilar[say].Substring(baglantilar[say].Length - 1, 1) == "E") || (!dngksl && baglantilar[say].Substring(baglantilar[say].Length - 1, 1) == "H"))
                            {
                                if (baglantilar[say].Substring(0, 3) == "son") break;
                                dngad = "nll";
                            }
                        }
                        else break;
                    }
                    if (kosulAd != "nll" && say > 0)
                    {
                        if ((kosulAd == baglantilar[say - 1] || kosulAd + "E" == baglantilar[say - 1] || kosulAd + "H" == baglantilar[say - 1]))
                        {
                            if ((ksl && baglantilar[say].Substring(baglantilar[say].Length - 1, 1) == "E") || (!ksl && baglantilar[say].Substring(baglantilar[say].Length - 1, 1) == "H"))
                            {
                                if (baglantilar[say].Substring(0, 3) == "son") break;

                                kslSayac.Insert(kslAd.IndexOf(kosulAd), say - 1);
                                kosulAd = "nll";
                            }
                        }
                        else break;
                    }
                    foreach (Control c in frm.Controls)
                    {
                        string yolayrimi = "", semaText = ""; ;
                        if (baglantilar[say].Substring(baglantilar[say].Length - 1, 1) == "E" || baglantilar[say].Substring(baglantilar[say].Length - 1, 1) == "H") yolayrimi = baglantilar[say].Substring(0, baglantilar[say].Length - 1);
                        else yolayrimi = baglantilar[say];

                        if (yolayrimi == c.Name)
                        {
                            semaText = baglantilar[say] + "\n" + c.Text;

                            if (dngad == "nll" && kosulAd == "nll" && semaText.Length >= baglantilar[say].Length && semaText.Substring(0, baglantilar[say].Length) == baglantilar[say])
                            {
                                if (tempbtn != null) tempbtn.FlatAppearance.BorderSize = 0;
                                tempbtn = (Button)c;

                                tempbtn.FlatAppearance.BorderColor = Color.Green;
                                tempbtn.FlatAppearance.BorderSize = 2;

                                string komut = semaText.Substring(baglantilar[say].Length + 1, semaText.Length - (baglantilar[say].Length + 1));

                                if (komut == "") break;
                                if (komut.Substring(0, 1) == "\n") komut = komut.Substring(1, komut.Length - 1);// Bu durumu engelle komutun başına \n gelmemeli

                                string duzelt = "";
                                if (baglantilar[say].Substring(baglantilar[say].Length - 1, 1) == "E" || baglantilar[say].Substring(baglantilar[say].Length - 1, 1) == "H") duzelt = baglantilar[say].Substring(0, baglantilar[say].Length - 1);
                                else duzelt = baglantilar[say];

                                if (duzelt.Length > 4)//kosulu 2 kez işleme alıyor bunu hallet(nedeni bağlantılarda iki kez olması)
                                {
                                    islembitir = false;
                                    if (duzelt.Substring(0, 5) == "islem") islem(komut);
                                    else if (duzelt.Substring(0, 5) == "giris") giris(komut);
                                    else if (duzelt.Substring(0, 5) == "cikti") { frm.richTextBox1.Text += cikti(komut); }
                                    else if (duzelt.Substring(0, 5) == "kosul")
                                    {
                                        ksl = kosul(komut); kosulAd = duzelt;
                                        if (!kslAd.Contains(duzelt))
                                        {
                                            kslAd.Add(duzelt);
                                            kslSayac.Add(say);
                                        }
                                        else
                                        {
                                            say = Convert.ToInt16(kslSayac[kslAd.IndexOf(duzelt)]);

                                        }
                                        break;
                                    }
                                    else if (duzelt.Substring(0, 5) == "dongu")
                                    {
                                        dngksl = dongu(komut); dngad = duzelt;
                                        if (!dnguad.Contains(duzelt))
                                        {
                                            dnguad.Add(duzelt);
                                            dngsayac.Add(say);
                                        }
                                        else
                                        {
                                            say = Convert.ToInt16(dngsayac[dnguad.IndexOf(duzelt)]);
                                        }
                                        break;
                                    }
                                    if (frm.listView2.Visible) frm.degiskenGuncelle();
                                }
                                break;
                            }
                        }
                    }
                }
                else say = -1;
                if (!frm.checkBox3.Checked) islembitir = true;
            }
            durdur();
        }*/

        private void degiskenolustur(string s)
        {
            degiskenAd.Add(s);
            degiskenler.Add(0);
        }
        private string degerAktar(string s)
        {
            s = s.TrimStart();
            s = s.TrimEnd();
            if (degiskenAd.Contains(s)) return degiskenler[degiskenAd.IndexOf(s)].ToString();
            else return s;
        }
        private string islemyap(string s,short islem)
        {
            string deg1="", deg2="" ;
            Random rnd =new Random();
            deg1 = s.Substring(0, s.IndexOf("'"));
            deg2 = s.Substring(deg1.Length + 5, s.Length - (deg1.Length + 5));

            deg1 =degerAktar(deg1);
            deg2 = degerAktar(deg2);

            double n1=0,n2=0;
            bool isNumeric = (double.TryParse(deg1, out n1) && double.TryParse(deg2, out n2));

            if (isNumeric)
            {
                if (islem == 1) return (n1 + n2).ToString();
                else if (islem == 2) return (n1 - n2).ToString();
                else if (islem == 3) return (n1 / n2).ToString();
                else if (islem == 4) return (n1 * n2).ToString();
                else if (islem == 6) return (n1 % n2).ToString();
                else if (islem == 5) return (Math.Pow(n1, n2)).ToString();
                else if (islem ==7)
                {
                    if (n1 > n2) {/* aTimer.Stop();*/ MessageBox.Show("Minimun değer maximum değerden küçük olmalıdır!", "Aritmetik İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning); return "0"; }
                    else return (rnd.Next((int)n1, (int)n2)).ToString();
                }
            }
            else if (islem ==1) return deg1 + deg2;
            else {/* aTimer.Stop();*/ MessageBox.Show("Bu işlem yapılamıyor!", "Aritmetik İşlem Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning); } return "0";
        }    
        private void islem(string kmt)
        {
            string[] satir = kmt.Split('\n');

            for (int i = 0; i < satir.Length; i++)
            {
                if (satir[i] == "") break;
                string degisken = satir[i].Substring(0, satir[i].IndexOf(" = "));
                if (!degiskenAd.Contains(degisken)) degiskenolustur(degisken);

                string islem = satir[i].Substring(satir[i].IndexOf(" = ")+4, satir[i].Length - (satir[i].IndexOf(" = ")+5));

                if (islem.Contains("' + '")) degiskenler[degiskenAd.IndexOf(degisken)] = islemyap(islem, 1);
                else if (islem.Contains("' - '")) degiskenler[degiskenAd.IndexOf(degisken)] = islemyap(islem, 2);
                else if (islem.Contains("' / '")) degiskenler[degiskenAd.IndexOf(degisken)] = islemyap(islem,3);
                else if (islem.Contains("' * '")) degiskenler[degiskenAd.IndexOf(degisken)] = islemyap(islem,4);
                else if (islem.Contains("' ^ '")) degiskenler[degiskenAd.IndexOf(degisken)] = islemyap(islem, 5);
                else if (islem.Contains("' % '")) degiskenler[degiskenAd.IndexOf(degisken)] = islemyap(islem, 6);
                else if (islem.Contains("' ~ '")) degiskenler[degiskenAd.IndexOf(degisken)] = islemyap(islem, 7);
                else degiskenler[degiskenAd.IndexOf(degisken)] = islem;
            }
        }
        private void giris(string kmt)
        {
        
            string[] satir = kmt.Split('\n');

            for (int i = 0; i < satir.Length; i++)
            {
                if (satir[i] == "") break;
                string text = satir[i].Substring(0, satir[i].IndexOf(" | "));
                string degisken = satir[i].Substring(satir[i].IndexOf(" | ") + 3, satir[i].Length - (satir[i].IndexOf(" | ") + 3));

                if (!degiskenAd.Contains(degisken)) degiskenolustur(degisken);

                string inputbx = Interaction.InputBox(text, "Veri Girişi");

                degiskenler[degiskenAd.IndexOf(degisken)] = inputbx;
                frm.richTextBox1.Text += "  G >> "+ text+inputbx+"\n" ;
            }
          
        }
        private bool kosul(string kmt)
        {
            short yankosul = 0;
            string[] satir = kmt.Split('\n');

            if (satir.Length > 1)
            {
                if (satir[1].IndexOf("&& ")!=-1) yankosul = 1;
                else if (satir[1].IndexOf("|| ")!=-1) yankosul = 2;
            }

            for (int i = 0; i < satir.Length; i++)
            {
                string kosul = "", deg1 = "", deg2 = "";
                if (satir[i] == "") break;
                if (satir[i].Contains(" = ")) kosul = " = ";
                else if (satir[i].Contains(" < ")) kosul = " < ";
                else if (satir[i].Contains(" > ")) kosul = " > ";
                else if (satir[i].Contains(" <= ")) kosul = " <= ";
                else if (satir[i].Contains(" >= ")) kosul = " >= ";
                else if (satir[i].Contains(" != ")) kosul = " != ";

                if (i == 0) deg1 = satir[i].Substring(0, satir[i].IndexOf(kosul));
                else if (yankosul == 1) deg1 = satir[i].Substring(5, satir[i].IndexOf(kosul) - 5);
                else if (yankosul == 2) deg1 = satir[i].Substring(3, satir[i].IndexOf(kosul) - 3);

                deg2 = satir[i].Substring(satir[i].IndexOf(kosul) + 3, satir[i].Length - (satir[i].IndexOf(kosul) + 3));

                deg1 = degerAktar(deg1);
                deg2 = degerAktar(deg2);

                if (kosul == " = " || kosul == " != ")
                {
                    if (yankosul == 2)
                    {
                        if (kosul == " = ")
                        {
                            if (deg1 == deg2) return true;
                            else continue;
                        }
                        else
                        {
                            if (deg1 != deg2) return true;
                            else continue;
                        }

                    }
                    else
                    {
                        if (kosul == " = " && deg1 == deg2) continue;
                        else if (kosul == " = " && deg1 != deg2) return false;
                        else if (kosul == " != " && deg1 != deg2) continue;
                        else if (kosul == " != " && deg1 == deg2) return false; ;
                    }
                }
                else
                {
                    double n1 = 0, n2 = 0;
                    bool isNumeric = (double.TryParse(deg1, out n1) && double.TryParse(deg2, out n2));
                    if (isNumeric)
                    {
                        if (yankosul == 2)
                        {
                            if (kosul == " < ")
                            {
                                if (n1 < n2) return true;
                                else continue;
                            }
                            if (kosul == " > ")
                            {
                                if (n1 > n2) return true;
                                else continue;
                            }
                            if (kosul == " <= ")
                            {
                                if (n1 <= n2) return true;
                                else continue;
                            }
                            if (kosul == " >= ")
                            {
                                if (n1 >= n2) return true;
                                else continue;
                            }
                        }
                        else
                        {
                            if (kosul == " < " && (n1 < n2)) continue;
                            else if (kosul == " < " && (n1 <= n2)) return false;
                            else if (kosul == " > " && (n1 > n2)) continue;
                            else if (kosul == " > " && (n1 <= n2)) return false;
                            else if (kosul == " <= " && (n1 <= n2)) continue;
                            else if (kosul == " <= " && (n1 > n2)) return false;
                            else if (kosul == " >= " && (n1 >= n2)) continue;
                            else if (kosul == " >= " && (n1 < n2)) return false;
                        }
                    }
                    else MessageBox.Show("Bu karşılaştırma geçersiz!", "Karşılaştırma Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false;
                }
            }
            if (yankosul == 2) return false;
            else return true;
        }
        private bool dongu(string kmt)
        {
           
            string deg1,deg11, deg2, deg3, deg4,deg5,kosul="";
            bool ilkdeger = false, dngkosul = false ;
            if (kmt == "") return false;

            if (kmt.Contains("<=")) kosul = "<=";
            else if (kmt.Contains(">=")) kosul = ">=";
            else if (kmt.Contains("<")) kosul = "<";
            else if (kmt.Contains(">")) kosul = ">";
            else if (kmt.Contains("!=")) kosul = "!=";
            else if (kmt.Contains("=")) kosul = "=";
          
            deg11 = kmt.Substring(0, kmt.IndexOf(" = "));
            deg2 = kmt.Substring(kmt.IndexOf(" = ") + 3, kmt.IndexOf(" ; ") - (kmt.IndexOf(" = ") + 3));
            deg3 = kmt.Substring(kmt.IndexOf(" ; ") + 3, kmt.IndexOf(kosul) - (kmt.IndexOf(" ; ") + 4));
            deg4 = kmt.Substring(kmt.IndexOf(kosul)+kosul.Length+1,kmt.LastIndexOf(";")-(kmt.IndexOf(kosul)+kosul.Length+2));
            deg5 = kmt.Substring(kmt.LastIndexOf(";") + 2, kmt.Length - (kmt.LastIndexOf(";") + 2));

            double degkontrol;

            if (!degiskenAd.Contains(deg11)) { degiskenolustur(deg11); ilkdeger = true; }// bu değişken olarak girilmek zorunda tanımlarken kontrol et
            if (!double.TryParse(deg2, out degkontrol) && !degiskenAd.Contains(deg2)) degiskenolustur(deg2);
            if (!double.TryParse(deg3, out degkontrol) && !degiskenAd.Contains(deg3)) degiskenolustur(deg3);  
            if (!double.TryParse(deg4, out degkontrol) && !degiskenAd.Contains(deg4)) degiskenolustur(deg4);
            if (!double.TryParse(deg5, out degkontrol) && !degiskenAd.Contains(deg5)) degiskenolustur(deg5);
            if (deg11 == deg3) dngkosul = true;

            deg1 = degerAktar(deg11);
            deg2 = degerAktar(deg2);
            deg3 = degerAktar(deg3);
            deg4 = degerAktar(deg4);
            deg5 = degerAktar(deg5);

            double n1 = 0, n2 = 0, n3 = 0, n4 = 0,n5=0 ;
            bool isNumericZ = (double.TryParse(deg1, out n1) && double.TryParse(deg2, out n2) && double.TryParse(deg5, out n5));
            bool isNumeric = (double.TryParse(deg3, out n3) && double.TryParse(deg4, out n4));

            if (isNumericZ)
            {
                if (ilkdeger) { n1 = n2 - n5; }
                if (dngkosul) n3 = n1+n5;
                if (isNumeric)
                {
                    n1 += n5;
                    if (kosul == "<" && (n3 < n4)) { degiskenler[degiskenAd.IndexOf(deg11)] = n1; return true; }
                    else if (kosul == ">" && (n3 > n4)) { degiskenler[degiskenAd.IndexOf(deg11)] = n1; return true; }
                    else if (kosul == "<=" && (n3 <= n4)) {degiskenler[degiskenAd.IndexOf(deg11)] = n1; return true; }
                    else if (kosul == ">=" && (n3 >= n4)) { degiskenler[degiskenAd.IndexOf(deg11)] = n1; return true; }
                    else { degiskenler.RemoveAt(degiskenAd.IndexOf(deg11)); degiskenAd.Remove(deg11); return false; }
                }
                else
                {
                    if (kosul == " = " && deg3 == deg4) {  n1 += n5; degiskenler[degiskenAd.IndexOf(deg11)] = n1; return true; }
                    else if (kosul == " != " && deg3 != deg4) {  n1 += n5; degiskenler[degiskenAd.IndexOf(deg11)] = n1; return true; }
                    else { degiskenler.RemoveAt(degiskenAd.IndexOf(deg11)); degiskenAd.Remove(deg11); return false; }
                }
            }
            else { degiskenler.RemoveAt(degiskenAd.IndexOf(deg11)); degiskenAd.Remove(deg11); return false; }
         
        }
        private string cikti(string kmt)
        {
            string[] satir = kmt.Split('\n');
            string cikti = "";
            for (int i = 0; i < satir.Length; i++)
            {
                if (satir[i] == "") break;
                string text = satir[i].Substring(1, satir[i].IndexOf(" | ")-2);
                string degisken = satir[i].Substring(satir[i].IndexOf(" | ") + 3, satir[i].Length - (satir[i].IndexOf(" | ") + 3));

                degisken = degerAktar(degisken);
                cikti += "  Ç >> " + text + degisken + "\n";
            }
            return cikti;
        }
        public ArrayList degiskenGoster()
        {
            if (!dgskngster) { dgskngster = true; return degiskenAd; }
            else { dgskngster = false; return degiskenler; }
        }
    }
}
