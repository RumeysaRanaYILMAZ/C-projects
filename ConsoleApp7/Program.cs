using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace K_means
{
    //We develop this class for produce random number in close times. And we add double randomize option.  
    class rastgele
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public int GetRandomNumber(int minimum, int maximum)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(minimum,maximum);
            }
        }
    }
    //Center class is k-means algorithm's k value's class. This class contain data in an arraylist.
    class Center
    {

        rastgele rst = new rastgele();
        public Center eskiMerkez;//this object keeps because of comparing old and new center. 
        public double[] foo1;
        public ArrayList liste = new ArrayList();//this list contains data objects
        public Center(ArrayList datalar)// küme merkezleri classıdır. Bu classdan türetilen merkez objeleri kendisine en yakın verileri liste array listinde tutar.
        {
            int rd = rst.GetRandomNumber(0, datalar.Count);
            data alım=(data)datalar[rd];
            
        }
        public Center(data dtt)
        {
            foo1 =(Double[]) dtt.foo.Clone();
        }
        public Center(Center eski)
        {
            this.foo1=(double[])eski.foo1.Clone();
        }
        public double[] ort()
        {
            //mean algorithm
            int len = liste.Count;
            double[] orta = new double[foo1.Length];
            foreach (data flower in this.liste)
            {
                for (int g=0;g<foo1.Length;g++)
                {
                    orta[g] += flower.foo[g];
                }
            }
            for (int u= 0;u < orta.Length;u++)
            {
                orta[u] = Math.Round(orta[u] / len,1);
            }
            return orta;
        }
        
        public bool finish(int r)
        {
            //algorithm stop conditions
            bool end = true;
            for (int y = 0; y < foo1.Length; y++)
            {
                if (this.foo1[y] != eskiMerkez.foo1[y])
                {
                    end = false;
                    break;
                }
            }
            if (r > 100)
            {
                end = true;
            }
            return end;
        }
        public void New_Center()
        {
            //find the mean of arraylist of points that closest center and assign as new center
            this.eskiMerkez = new Center(this);
            this.foo1 = this.ort();
        }
        public string tostring()
        {
            string print = "";
            foreach (double d in foo1)
            {
                print = print + d + "-";
            }
            return print;
        }


    }
    class data
    {
        //keep the data's attribute in a class because of more proper to oop concepts.
        public double[] foo;//veri setlerinin tutulduğu class
        public String isim;


        public data(double[] deger,String name)
        {
            foo = deger;
            this.isim = name;
        }
        public data(double[] deger)
        {
            foo = deger;
            
        }

        public string print()
        {
            string bas = string.Format("{0:0.0}", this.foo[0]);
            for (int v=1;v<foo.Length;v++)
            {
                bas = bas + "-" + string.Format("{0:0.0}", this.foo[v]);
            }
            bas = bas + "," + isim;
            return bas;
        }
        public double uzaklık(Center c)
        {
            double dist = 0;
            for (int n=0;n<foo.Length;n++)
            {
                dist+=Math.Pow((foo[n]-c.foo1[n]),2);
            }
            return Math.Sqrt(dist);
        }

    } 


    class Program
    {
        static void Main(string[] args)
        {
            int oz_nitelik = 4;
            rastgele rand = new rastgele();
            ArrayList datalist = new ArrayList();
            //pull data by string type from the link 
            Console.Write("Veri Tipi Seçiniz(Çiçek-1/Sosyal Medya-2):");
            int t = Convert.ToInt32(Console.ReadLine());
            Console.Write("Merkez Sayısı Giriniz:");
            int merkez_say = Convert.ToInt32(Console.ReadLine());
            StreamReader sw = new System.IO.StreamReader(@"C:\Users\ahmetcan\Desktop\ConsoleApp7\ConsoleApp7\veri2.txt");
            oz_nitelik = 3;
            if (t==1)
            {
                sw = new System.IO.StreamReader(@"C:\Users\ahmetcan\Desktop\ConsoleApp7\ConsoleApp7\veri3.txt");
                oz_nitelik = 4;
            }
            
            
            string satir;


            //find max and min values for first center's random method.

            
            while ((satir = sw.ReadLine()) !=null)//dosyadan veriler okunur,split yöntemiyle bölünür,bu verilerle obje oluşturulur
            {

                string[] koordinat = satir.Split(',');
                //convert string to double and create data object 
                if (oz_nitelik == 4)
                {



                    string[] koordinatlar = new string[oz_nitelik+1];
                    for (int c = 0; c < oz_nitelik+1; c++)
                    {
                        koordinatlar[c] = koordinat[c];
                    }
                    double[] veri = new double[oz_nitelik];
                    for (int s = 0; s < oz_nitelik; s++)
                    {
                        veri[s] = Double.Parse(koordinatlar[s], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    String isim2 = koordinatlar[4];

                    data yaprak = new data(veri,isim2);
                    datalist.Add(yaprak);
                }
                else
                {
                    string[] koordinatlar = new string[oz_nitelik];
                    for (int c = 0; c < oz_nitelik; c++)
                    {
                        koordinatlar[c] = koordinat[c];
                    }
                    double[] veri = new double[oz_nitelik];
                    for (int s = 0; s < oz_nitelik; s++)
                    {
                        veri[s] = Double.Parse(koordinatlar[s], System.Globalization.CultureInfo.InvariantCulture);
                    }

                    data yaprak = new data(veri);
                    datalist.Add(yaprak);

                }
            }
            sw.Close();
            Center[] choose()
            {
                Center[] Merkezi = new Center[merkez_say];
                for (int i = 0; i < merkez_say; i++)
                {
                    int rd = rand.GetRandomNumber(0, datalist.Count);
                    data alım = (data)datalist[rd];
                    Merkezi[i] = new Center(alım);
                }
                return Merkezi;
            }
            Center[] Merkezler = choose();
            bool cc= true;

            int r = 0;

            do
            {  
            bool isempty =true;
            //cluster all of data by attribute 
            while (isempty==true)
                {
                    foreach (data a in datalist)
                    {

                        double m1;
                        double min = 1000000;
                        Center min1 = null;
                        for (int i = 0; i < merkez_say; i++)
                        {
                            //minimum euclid distance for clustring process 
                            m1 = a.uzaklık(Merkezler[i]);

                            if (m1 < min)
                            {
                                min1 = Merkezler[i];

                                min = m1;
                            }

                        }
                        min1.liste.Add(a);
                    }
                    foreach (Center ctr in Merkezler)
                    {
                        if (ctr.liste == null)
                        {
                            Merkezler = choose();
                            isempty = true;
                            break;
                        }
                        isempty = false;
                    }
                }
                
                for (int p = 0; p < merkez_say; p++)
                {
                    Merkezler[p].New_Center();
                    
                }
                foreach (Center m in Merkezler)
                    {
                    //print the console datas by clusters
                    Console.WriteLine("------------------------------------");
                    foreach(data fl in m.liste)
                    {

                        Console.WriteLine("Öznitelik:"+fl.print()+"    Merkez Uzaklık:" + String.Format("{0:0.000}", fl.uzaklık(m))+"    Bulunduğu Merkez:"+m.tostring());
                    }
                }
                for(int i = 0; i< merkez_say; i++)
                {

                }
                foreach (Center m in Merkezler)
                {
                    //clean all center's data list for new clustering
                    m.liste.Clear();
                }
                foreach(Center cp in Merkezler)
                {
                    //control finish condition
                    if (cp.finish(r)==false)
                    {

                        cc =false;
                        
                        break;
                    }
                    cc = true;
                    
                }
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                r++;
            }
            while (cc!=true);
            Console.Write("Yeni Veri Girişi Yapmak İster misiniz?(Evet=e/Hayır=h):");
            string d = Console.ReadLine();
            while (d.Equals("e"))
            {
                double mini = 3000;
                double[] degg = new double[oz_nitelik];
                for (int o = 0; o < oz_nitelik; o++)
                {
                    Console.Write("Veri Giriniz: ");
                    degg[o] = Convert.ToDouble(Console.ReadLine(), System.Globalization.CultureInfo.InvariantCulture);
                }
                data ff = new data(degg);
                double mesafe = 10000;
                Center sec = null;
                foreach (Center crr in Merkezler)
                {
                    mesafe = ff.uzaklık(crr);
                    if (mesafe < mini)
                    {
                        sec = crr;
                        mini = mesafe;
                    }
                }
                sec.liste.Add(ff);
                Console.WriteLine("Öznitelik:" + ff.print() + "    Merkez Uzaklık:" + String.Format("{0:0.000}", mesafe) + "    Bulunduğu Merkez:" + sec.tostring());
                Console.ReadKey();
                Console.Write("Yeni Veri Girişi Yapmak İster misiniz?(Evet=e/Hayır=h):");
                d = Console.ReadLine();
            }
            
        }
    }
}

