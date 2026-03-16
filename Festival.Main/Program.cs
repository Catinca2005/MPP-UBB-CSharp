using System;
using Festival.Repository;
using Festival.Domain;
using log4net.Config;

namespace Festival.Main
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            Console.WriteLine("========================================");
            Console.WriteLine("   FESTIVAL MANAGEMENT SYSTEM STARTING  ");
            Console.WriteLine("========================================");

            try
            {
                var connection = DbUtils.GetConnection();
                Console.WriteLine($"[SUCCESS] Database connected. State: {connection.State}");

                Console.WriteLine("\n--- Testing Artist Persistence ---");
                IArtistRepository artistRepo = new ArtistDbRepository();

                string testArtistName = "Arctic Monkeys " + DateTime.Now.ToString("HH:mm:ss");
                Artist newArtist = new Artist(testArtistName);

                artistRepo.Add(newArtist);
                Console.WriteLine($"[INFO] Successfully added: {testArtistName}");

                Console.WriteLine("\n--- Current Artists in Database ---");
                foreach (var artist in artistRepo.FindAll())
                {
                    Console.WriteLine($"ID: {artist.Id} | Name: {artist.Name}");
                }

                Console.WriteLine("\nReady for operations.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[FATAL ERROR] {ex.Message}");
                log4net.LogManager.GetLogger("Main").Fatal("Application failed to start", ex);
            }

            Console.WriteLine("\nPress any key to close this window and view logs...");
            Console.ReadKey();
        }
    }
}