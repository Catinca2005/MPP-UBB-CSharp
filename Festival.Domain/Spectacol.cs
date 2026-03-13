using System;

namespace Festival.Domain
{
    public class Spectacol : IIdentifiable<long>
    {
        public long Id { get; set; }
        public long ArtistId { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Ora { get; set; }
        public string Locatia { get; set; }
        public int LocuriDisponibile { get; set; }
        public int LocuriVandute { get; set; }

        public Spectacol(long artistId, DateTime data, TimeSpan ora, string locatia, int disp, int vand)
        {
            ArtistId = artistId;
            Data = data;
            Ora = ora;
            Locatia = locatia;
            LocuriDisponibile = disp;
            LocuriVandute = vand;
        }

        public void ScadeLocuriDisp(int nr)
        {
            LocuriDisponibile -= nr;
            LocuriVandute += nr;
        }

        public void AdaugaLocuriDisp(int nr)
        {
            LocuriDisponibile += nr;
            LocuriVandute -= nr;
        }
    }
}