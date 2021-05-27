

namespace Wumpus_171220087_AhmetCanAydemir
{
    public enum KareTipi
    {
        Bilinmiyor,
        Altin,
        Wumpus,
        Cukur
    }
    public class Kare
    {
        public KareTipi KareTipi { get; set; }
        public bool ZiyaretEdildi { get; set; } = false;
        public bool Koku { get; set; } = false;
        public bool Esinti { get; set; } = false;

        public bool GuvenliMi { get; set; } = false;

        public bool BuKaredekiWumpusOlu { get; set; } = false;


        public Kare(KareTipi kareTipi)
        {
            KareTipi = kareTipi;
        }


    }
}
