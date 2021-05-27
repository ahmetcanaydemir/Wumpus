using System;
using System.Windows;

namespace Wumpus_171220087_AhmetCanAydemir
{
    public class Oyuncu
    {
        public int Puan { get; set; } = 0;
        public int Boyut{ get; set; }
        public int KalanOk { get; set; }
        public int ToplananAltinSayisi { get; set; } = 0;

        public bool Olu { get; set; } = false;

        public bool OkAtilacak { get; set; }

        public Point Konum { get; set; } = new Point(0, 0);

        /// <summary>
        /// Sınırların bilinmesi açısından boyut alınıyor.
        /// </summary>
        /// <param name="boyut">Karenin boyutu</param>
        public Oyuncu(int boyut, int kalanOk)
        {
            Boyut = boyut;
            KalanOk = kalanOk;
        }

        #region Hareket Ettirme Fonksiyonları
        public void YukariGit()
        {
            if (Konum.Y > 0)
            {
                Konum = new Point(Konum.X, Konum.Y - 1);
                Puan -= 1;
                GerekiyorsaOkuAt();
            }
        }
        public void AsagiGit()
        {
            if (Konum.Y < Boyut - 1)
            {
                Konum = new Point(Konum.X, Konum.Y + 1);
                Puan -= 1;
                GerekiyorsaOkuAt();
            }
            
        }
        public void SagaGit ()
        {
            if (Konum.X < Boyut - 1)
            {
                Konum = new Point(Konum.X + 1, Konum.Y);
                Puan -= 1;
                GerekiyorsaOkuAt();
            }
        }
        public void SolaGit()
        {
            if (Konum.X > 0)
            {
                Konum = new Point(Konum.X-1 , Konum.Y);
                Puan -= 1;
                GerekiyorsaOkuAt();
            }
                
        }

        /// <summary>
        /// <see cref="OkAtilacak"/> değişkenini true yapar.
        /// </summary>
        public void OkuHazirla()
        {
            if(KalanOk > 0)
                OkAtilacak = true;
        }

        /// <summary>
        /// Oku Fırlatır ve <see cref="OkAtilacak"/> değişkenini false yapar ve <see cref="KalanOk"/> değişkenini 1 azaltır.
        /// </summary>
        private void GerekiyorsaOkuAt()
        {
            if (OkAtilacak)
            {
                KalanOk--;
                Puan -= 10;
            }
            OkAtilacak = false;
        }

        /// <summary>
        /// Altini Yerden Alır
        /// </summary>
        public void AltiniAl()
        {
            Puan += 1000;
            ToplananAltinSayisi++;
        }

        /// <summary>
        /// Karakter öldü.
        /// </summary>
        public void Ol()
        {
            Puan -= 1000;
        }

        #endregion

    }
}
