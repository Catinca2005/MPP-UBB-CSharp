namespace Festival.Domain
{
    public class Bilet : IIdentifiable<long>
    {
        public long Id { get; set; }
        public long SpectacolId { get; set; }
        public string NumeCumparator { get; set; }
        public int NrLocuri { get; set; }

        public Bilet(long spectacolId, string numeCumparator, int nrLocuri)
        {
            SpectacolId = spectacolId;
            NumeCumparator = numeCumparator;
            NrLocuri = nrLocuri;
        }
        public void ActualizareNrLocuri(int nrNou)
        {
            NrLocuri = nrNou;
        }
    }
}