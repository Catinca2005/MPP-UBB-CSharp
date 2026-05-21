using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Festival.Model;

namespace Festival.RestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting C# REST Client Test Pipeline...\n");

            // 1. Setup HttpClient with the Interceptor attached
            using var handler = new LoggingInterceptor(new HttpClientHandler());
            using var client = new HttpClient(handler)
            {
                // Ensure this port matches your running Web API port!
                BaseAddress = new Uri("http://localhost:5050/")
            };

            try
            {
                // --- TEST 1: GET ALL ---
                Console.WriteLine(">>> EXECUTING TEST 1: GET ALL SHOWS <<<");
                var allShows = await client.GetFromJsonAsync<Show[]>("festival/shows");

                // --- TEST 2: POST (CREATE) ---
                Console.WriteLine(">>> EXECUTING TEST 2: CREATE NEW SHOW <<<");
                Show newShow = new Show(
                    1,                                // artistId
                    new DateTime(2026, 12, 31),        // date
                    new TimeSpan(23, 59, 59),          // time
                    "Test Arena",                      // location
                    1000,                              // availableSeats
                    0                                  // soldSeats
                );
                
                // Requirement 3: Client does not send ID. The server generates it and returns the full object.
                var postResponse = await client.PostAsJsonAsync("festival/shows", newShow);
                postResponse.EnsureSuccessStatusCode();
                
                Show createdShow = await postResponse.Content.ReadFromJsonAsync<Show>();
                Console.WriteLine($"[CLIENT SCRIPT] Success! Server generated ID: {createdShow.Id}");

                // --- TEST 3: GET BY ID ---
                Console.WriteLine($"\n>>> EXECUTING TEST 3: GET SHOW BY ID ({createdShow.Id}) <<<");
                var fetchedShow = await client.GetFromJsonAsync<Show>($"festival/shows/{createdShow.Id}");

                // --- TEST 4: PUT (UPDATE) ---
                Console.WriteLine($"\n>>> EXECUTING TEST 4: UPDATE SHOW ({createdShow.Id}) <<<");
                createdShow.Location = "Updated Test Arena";
                createdShow.SoldSeats = 150;
                
                var putResponse = await client.PutAsJsonAsync($"festival/shows/{createdShow.Id}", createdShow);
                putResponse.EnsureSuccessStatusCode();

                // --- TEST 5: GET (FILTER) ---
                Console.WriteLine("\n>>> EXECUTING TEST 5: FILTER SHOWS BY DATE <<<");
                string filterDate = "2026-12-31";
                var filteredShows = await client.GetFromJsonAsync<Show[]>($"festival/shows/filter?date={filterDate}");

                // --- TEST 6: DELETE ---
                Console.WriteLine($"\n>>> EXECUTING TEST 6: DELETE SHOW ({createdShow.Id}) <<<");
                var deleteResponse = await client.DeleteAsync($"festival/shows/{createdShow.Id}");
                deleteResponse.EnsureSuccessStatusCode();

                Console.WriteLine("\n[CLIENT SCRIPT] All REST operations completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[CLIENT SCRIPT ERROR] {ex.Message}");
            }
            
            Console.ReadLine(); // Keep console open
        }
    }
}