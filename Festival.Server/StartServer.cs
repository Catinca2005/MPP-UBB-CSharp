using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Festival.Persistence;
using Festival.Services;
using Festival.Networking;

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
                IEmployeeRepository employeeRepo = new EmployeeDbRepository();
                IArtistRepository artistRepo = new ArtistDbRepository();
                IShowRepository showRepo = new ShowDbRepository();
                ITicketRepository ticketRepo = new TicketDbRepository();

                // 2. Initialize the core Business Logic
                IFestivalServices serviceImpl = new FestivalServicesImpl(employeeRepo, artistRepo, showRepo, ticketRepo);

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