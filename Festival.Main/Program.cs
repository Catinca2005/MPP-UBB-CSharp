using System;
using System.Net.Mime;
using System.Windows.Forms;
using Festival.Repository;
using Festival.Domain;
using Festival.Service;
using Festival.Controller;
using Festival.Network.Utils;
using log4net.Config;

namespace Festival.Main
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            
            // 1. Aflăm unde este baza de date pe bune
            string dbPath = Path.GetFullPath("festival.db");
            Console.WriteLine("PROGRAMUL CAUTĂ BAZA AICI: " + dbPath);

            // 2. Verificăm dacă fișierul chiar există acolo
            if (!File.Exists(dbPath)) {
                Console.WriteLine("EROARE: Fișierul bazei de date NU există la calea de mai sus!");
            }
            
            // 1. Logging & Console
            XmlConfigurator.Configure();
            Console.WriteLine("========================================");
            Console.WriteLine("   FESTIVAL MANAGEMENT SYSTEM STARTING  ");
            Console.WriteLine("========================================");

            //UI settings
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
               //connexion test
                var connection = DbUtils.GetConnection();
                Console.WriteLine($"[SUCCESS] Database connected. State: {connection.State}");
                // -------------------------------

                //Injection
                IEmployeeRepository employeeRepo = new EmployeeDbRepository();
                IArtistRepository artistRepo = new ArtistDbRepository();
                IShowRepository showRepo = new ShowDbRepository();
                ITicketRepository ticketRepo = new TicketDbRepository();

                FestivalService service = new FestivalService(
                    employeeRepo, artistRepo, showRepo, ticketRepo);

                LoginController loginController = new LoginController(service);
                MainController mainController = new MainController(service);
                
                Console.WriteLine("--- Password Encoding Test ---");
        
                string rawPassword = "admin123";
                string securePassword = SecurityUtils.Encode(rawPassword);
        
                Console.WriteLine($"Raw text: {rawPassword}");
                Console.WriteLine($"Encoded (to be saved in DB): {securePassword}");
        
                // Test de verificare
                string backToNormal = SecurityUtils.Decode(securePassword);
                Console.WriteLine($"Decoded back: {backToNormal}");

                Console.WriteLine("[INFO] DI Container initialized. Opening Login Form...");

                
                Application.Run(new LoginForm(loginController, mainController));
                
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"\n[FATAL ERROR] {ex.Message}");
                MessageBox.Show($"Startup failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                log4net.LogManager.GetLogger("Main").Fatal("Application failed to start", ex);
            }
        }
    }
}