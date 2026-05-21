using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Festival.Services;
using Festival.Networking;
using Festival.Persistence;

namespace Festival.Server
{
    /// <summary>
    /// Entry point for the Festival Server.
    /// Initializes repositories, the business logic implementation, and starts the TCP listener.
    /// </summary>
    class StartServer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("       FESTIVAL SERVER STARTING...      ");
            Console.WriteLine("========================================");

            try
            {
                // 1. Initialize Persistence (Database Repositories)
                
                // --- NEW ORM PERSISTENCE (Entity Framework Core) ---
                // --- ORM PERSISTENCE (Entity Framework Core) ---
                IEmployeeRepository repoEmployee = new EmployeeOrmRepository();
                IShowRepository repoShow = new ShowOrmRepository();

                // --- OLD ADO.NET PERSISTENCE ---
                // We keep these for the entities that are not mapped via ORM yet
                IArtistRepository repoArtist = new ArtistDbRepository();
                ITicketRepository repoTicket = new TicketDbRepository();

                // 2. Initialize the core Business Logic
                // Passed strictly in the correct order: Employee, Show, Artist, Ticket
                IFestivalServices serviceImpl = new FestivalServicesImpl(repoEmployee,repoArtist,repoShow, repoTicket);
               

                // 3. Set up the Network Listener
                int port = 55555;
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                
                Console.WriteLine($"[SUCCESS] Server is listening on port {port}.");
                Console.WriteLine("[INFO] Waiting for client connections...\n");

                // 4. The Infinite Loop to accept incoming clients
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine($"[NETWORK] New client connected from {client.Client.RemoteEndPoint}");

                    // Create a dedicated worker for this specific client using Protobuf
                    FestivalClientProtobufWorker worker = new FestivalClientProtobufWorker(serviceImpl, client);

                    // Start a new thread so the server can immediately go back to waiting for other clients
                    Thread clientThread = new Thread(new ThreadStart(worker.Run));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FATAL ERROR] {ex.Message}");
                Console.ReadLine(); // Pause so we can read the error before the console closes
            }
        }
    }
}