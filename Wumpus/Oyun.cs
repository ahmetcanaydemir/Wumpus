using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wumpus_171220087_AhmetCanAydemir
{
    public class Oyun
    {
        public int Boyut { get; set; } = 4;
        public Kare[,] Kareler { get; set; }
        public Oyuncu Oyuncu{ get; set; }
        public Kare OyuncununBulunduguKare => Kareler[(int)Oyuncu.Konum.X, (int)Oyuncu.Konum.Y];

        public int AltinAdet { get; set; } = 1;
        public int WumpusAdet { get; set; } = 1;
        public int CukurAdet { get; set; } = 3;


        public void OyunuBaslat()
        {
            Oyuncu = new Oyuncu(Boyut,WumpusAdet);

            // n * n boyutunda bir Kare tipinde matris oluşturduk.
            Kareler = new Kare[Boyut, Boyut];

            // Kareler null olmaması için tüm kareleri boş olarak ekliyoruz.
            for (int i = 0; i < Boyut; i++)
                for (int j = 0; j < Boyut; j++)
                    Kareler[i, j] = new Kare(KareTipi.Bilinmiyor);

            Kareler =  KareleriDoldur();
        }

        public string EtrafindakilerGuvenliMi()
        {
            StringBuilder stringBuilder = new StringBuilder();

            List<Kare> kontrolEdilecekKareler = new List<Kare>();
            var konum = KareninKonumunuBul(OyuncununBulunduguKare);
            int x = (int)konum.X;
            int y = (int)konum.Y;

            // Oyuncunun bulunduğu karenin sağı solu üstü ve altı ziyaret edilmediyse listeye alındı.
            if (x + 1 < Boyut && x + 1 >= 0 && !Kareler[x + 1, y].ZiyaretEdildi)
                    kontrolEdilecekKareler.Add(Kareler[x + 1, y]);
            if (x - 1 < Boyut && x - 1 >= 0 && !Kareler[x - 1, y].ZiyaretEdildi)
                    kontrolEdilecekKareler.Add(Kareler[x - 1, y]);
            if (y + 1 < Boyut && y + 1 >= 0 && !Kareler[x, y + 1].ZiyaretEdildi)
                    kontrolEdilecekKareler.Add(Kareler[x, y + 1]);
            if (y - 1 < Boyut && y - 1 >= 0 && !Kareler[x, y - 1].ZiyaretEdildi)
                    kontrolEdilecekKareler.Add(Kareler[x, y - 1]);

            stringBuilder.Append($"\nŞİMDİKİ KARE [{x},{y}]:\nKomşuları kontrol ediliyor.");

            foreach (var kare in kontrolEdilecekKareler)
            {
                stringBuilder.Append($"\nKARE [{x},{y}]:\n");
                // Çukur kontrolü
                konum = KareninKonumunuBul(kare);
                x = (int)konum.X;
                y = (int)konum.Y;
                int taranan = 0;
                int esintiSayisi = 0;
                // Sağ
                if (x + 1 < Boyut && x + 1 >= 0 && !Kareler[x + 1, y].ZiyaretEdildi)
                {
                    taranan++;
                    if (Kareler[x + 1, y].Esinti)
                        esintiSayisi++;
                }
                // Sol
                if (x - 1 < Boyut && x - 1 >= 0 && !Kareler[x - 1, y].ZiyaretEdildi)
                {
                    taranan++;
                    if (Kareler[x - 1, y].Esinti)
                        esintiSayisi++;
                }
                // Yukarı
                if (y + 1 < Boyut && y + 1 >= 0 && !Kareler[x, y + 1].ZiyaretEdildi)
                {
                    taranan++;
                    if (Kareler[x, y + 1].Esinti)
                        esintiSayisi++;
                }
                // Aşağı
                if (y - 1 < Boyut && y - 1 >= 0 && !Kareler[x, y - 1].ZiyaretEdildi)
                {
                    taranan++;
                    if (Kareler[x, y - 1].Esinti)
                        esintiSayisi++;
                }
                stringBuilder.Append($"Çukur Kontrolü \n");
                stringBuilder.Append($"Ziyaret edilmiş:{taranan}\nEsinti:{esintiSayisi}");

                // Eğer 1'den fazla tarama yapılmasına rağmen esinti o karenin çevresinde 0 veya 1 ise o karede çukur olamaz
                bool kesinCukurYok = taranan > 1 && esintiSayisi < 2;
                if (kesinCukurYok)
                {
                    stringBuilder.Append($"\nEsinti sayısı en az bir kenarda yok, kesinlikle çukur yok.");
                }
                else
                {
                    stringBuilder.Append($"\nBu karede kesin çukur yok denemez.");

                }

                // Wumpus kontrolü

                taranan = 0;
                int kokuSayisi = 0;
                if (x + 1 < Boyut && x + 1 >= 0 && !Kareler[x + 1, y].ZiyaretEdildi)
                {
                    taranan++;
                    if (Kareler[x + 1, y].Koku)
                        kokuSayisi++;
                }
                if (x - 1 < Boyut && x - 1 >= 0 && !Kareler[x - 1, y].ZiyaretEdildi)
                {
                    taranan++;
                    if (Kareler[x - 1, y].Koku)
                        kokuSayisi++;
                }
                if (y + 1 < Boyut && y + 1 >= 0 && !Kareler[x, y + 1].ZiyaretEdildi)
                {
                    taranan++;
                    if (Kareler[x, y + 1].Koku)
                        kokuSayisi++;
                }
                if (y - 1 < Boyut && y - 1 >= 0 && !Kareler[x, y - 1].ZiyaretEdildi)
                {
                    taranan++;
                    if (Kareler[x, y - 1].Koku)
                        kokuSayisi++;
                }


                stringBuilder.Append($"Wumpus Kontrolü \n");
                stringBuilder.AppendLine($"Ziyaret edilmiş:{taranan}\nKötü koku:{kokuSayisi}");
                // Eğer 1'den fazla tarama yapılmasına rağmen kötü koku o karenin çevresinde 0 veya 1 ise o karede wumpus olamaz
                bool kesinWumpusYok = taranan > 1 && kokuSayisi < 2;
                if (kesinCukurYok)
                {
                    stringBuilder.Append($"\nKötü koku sayısı en az bir kenarda yok, kesinlikle wumpus yok.");
                }
                else
                {
                    stringBuilder.Append($"\nBu karede kesin wumpus yok denemez.");
                }

                //Eğer hem wumpus yok hem de çukur yoksa o kare güvenlidir.
                if (kesinCukurYok && kesinWumpusYok)
                {
                    kare.GuvenliMi = true;
                    stringBuilder.Append($"\n+==+ Bu karede kesin wumpus ve çukur yok o halde bu kare güvenlidir.");
                }
                else
                {
                    stringBuilder.Append($"\n~==~ Bu kare güvenli olabilir ama güvenli olmayadabilir.");
                }

            }
            stringBuilder.AppendLine("\n=============");
            return stringBuilder.ToString();


        }

        private Point KareninKonumunuBul(Kare kare)
        {
            for (int i = 0; i < Boyut; i++)
            {
                for (int j = 0; j < Boyut; j++)
                {
                    if (Kareler[i, j] == kare)
                        return new Point(i, j);
                }
            }
            return new Point(0, 0);
        }

        /// <summary>
        /// Oyundaki tüm kareleri sıfırdan dolduruyoruz.-
        /// </summary>
        private Kare[,] KareleriDoldur()
        {

            // Oyuna eklenecek altın, wumpus ve çukur sayılarına ulaşıp ulaşmadığımızı kontrol etmek için sayaçlar oluşturuldu.
            int eklenenAltin, eklenenWumpus, eklenenCukur;
            eklenenAltin = eklenenWumpus = eklenenCukur = 0;

            // Matris içerisinde geziyor.
            for(int i = 0; i < Boyut; i++)
            {
                for (int j = 0; j < Boyut; j++)
                {
                    // Oyuncunun başladığı karenin güvenli olduğu garanti ediliyor.
                    if(i == 0 && j == 0)
                    {
                        Kareler[i, j] = new Kare(KareTipi.Bilinmiyor)
                        {
                            GuvenliMi = true,
                            ZiyaretEdildi = true
                        };
                        continue;
                    }

                    // Sırayla istenilen kadar altın daha sonra wumpus ve çukur ekleniyor. Diğer tüm kareler ise boş olduğu için güvenli olarak işaretleniyor.
                    if (eklenenAltin++ < AltinAdet)
                        Kareler[i, j] = new Kare(KareTipi.Altin);
                    else if (eklenenWumpus++ < WumpusAdet)
                        Kareler[i, j] = new Kare(KareTipi.Wumpus);
                    else if (eklenenCukur++ < CukurAdet)
                        Kareler[i, j] = new Kare(KareTipi.Cukur);
                    else
                        Kareler[i, j] = new Kare(KareTipi.Bilinmiyor);

                }
            }
            // Kareler oluşturuldu fakat altın, wumpus ve çukur yanyana. Bunları dağıtmak için matris karıştırılıyor.
            KareleriKaristir();

            //Kareler karıştırıldıktan sonra wumpus ve cukur için esinti ve kötü kokular karelerin yanlarına ekleniyor.
            for (int i = 0; i < Boyut; i++)
            {
                for (int j = 0; j < Boyut; j++)
                {
                    var kare = Kareler[i, j];
                    if (kare.KareTipi == KareTipi.Wumpus)
                        WumpusEkle(i, j);
                    else if (kare.KareTipi == KareTipi.Cukur)
                        CukurEkle(i, j);
                }
            }

            return Kareler;
        }

        /// <summary>
        /// Belirlenenkomşu karelere kötü koku ekler.
        /// </summary>
        /// <param name="i">Karenin x koordinatı</param>
        /// <param name="j">Karenin y koordinatı</param>
        private void WumpusEkle(int i, int j)
        {
            // Wumpus sağ sol yukarı ve aşağı karelerine kötü koku ekleniyor.
            if (i + 1 < Boyut && i + 1 >= 0)
                Kareler[i + 1, j].Koku = true;
            if (i - 1 < Boyut && i - 1 >= 0)
                Kareler[i - 1, j].Koku = true;
            if (j + 1 < Boyut && j + 1 >= 0)
                Kareler[i, j + 1].Koku = true;
            if (j - 1 < Boyut && j - 1 >= 0)
                Kareler[i, j - 1].Koku = true;
        }

        /// <summary>
        /// Belirlenen komşu karelere  esinti ekler.
        /// </summary>
        /// <param name="i">Karenin x koordinatı</param>
        /// <param name="j">Karenin y koordinatı</param>
        private void CukurEkle(int i, int j)
        {
            // Wumpus sağ sol yukarı ve aşağı karelerine kötü koku ekleniyor.
            if (i + 1 < Boyut && i + 1 >= 0)
                Kareler[i + 1, j].Esinti = true;
            if (i - 1 < Boyut && i - 1 >= 0)
                Kareler[i - 1, j].Esinti = true;
            if (j + 1 < Boyut && j + 1 >= 0)
                Kareler[i, j + 1].Esinti = true;
            if (j - 1 < Boyut && j - 1 >= 0)
                Kareler[i, j - 1].Esinti = true;
        }


       

        /// <summary>
        /// Kareler matrisini rastgele bir şekilde karıştıran fonksiyon.
        /// </summary>
        private void KareleriKaristir()
        {
            Random rand = new Random();
            for (int i = 0; i < (Boyut * Boyut) - 1; i++)
            {
                int j = rand.Next(i, Boyut*Boyut);

                int row_i = i / Boyut;
                int col_i = i % Boyut;
                int row_j = j / Boyut;
                int col_j = j % Boyut;

                // Oyuncunun başladığı karenin güvenli olduğu garanti ediliyor.
                if ((row_i == 0 && col_i == 0) || (row_j == 0 && col_j == 0))
                    continue;

                var temp = Kareler[row_i, col_i];
                Kareler[row_i, col_i] = Kareler[row_j, col_j];
                Kareler[row_j, col_j] = temp;
            }
        }


    }
}
