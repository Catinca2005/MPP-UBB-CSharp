using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Festival.Model;
using Festival.Services;
using Festival.Networking.Protobuf;
using Google.Protobuf;

namespace Festival.Networking
{
    /// <summary>
    /// Client-side proxy representing the remote server. 
    /// Marshals local method calls into Protobuf network requests and manages asynchronous updates.
    /// </summary>
    public class FestivalServerProtobufProxy : IFestivalServices
    {
        private readonly string _host;
        private readonly int _port;
        private IFestivalObserver _clientObserver;
        
        private TcpClient _connection;
        private NetworkStream _stream;
        private volatile bool _finished;
        
        // Blocking queue to sync request-response pairs safely
        private readonly BlockingCollection<Response> _responses;

        public FestivalServerProtobufProxy(string host, int port)
        {
            _host = host;
            _port = port;
            _responses = new BlockingCollection<Response>();
        }

        private void InitializeConnection()
        {
            try
            {
                _connection = new TcpClient(_host, _port);
                _stream = _connection.GetStream();
                _finished = false;
                
                // Start a background thread to listen for server messages
                Task.Run(StartReader);
            }
            catch (Exception e)
            {
                throw new FestivalException("Failed to connect to the festival server.", e);
            }
        }

        private void CloseConnection()
        {
            _finished = true;
            try { _stream.Close(); _connection.Close(); } catch { }
            _clientObserver = null;
        }

        private void SendRequest(Request request)
        {
            try
            {
                request.WriteDelimitedTo(_stream);
                _stream.Flush();
            }
            catch (Exception e)
            {
                throw new FestivalException("Error sending data to server.", e);
            }
        }

        private Response ReadResponse()
        {
            Response response = null;
            try
            {
                // Blocks until a response is put into the queue by the Reader thread
                response = _responses.Take();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Proxy] Error reading response: {e.Message}");
            }
            return response;
        }

        private void StartReader()
        {
            while (!_finished)
            {
                try
                {
                    Response response = Response.Parser.ParseDelimitedFrom(_stream);
                    if (response == null) continue;

                    // If it's an asynchronous live update, handle it immediately
                    if (response.Type == ResponseType.Update)
                    {
                        HandleUpdate(response);
                    }
                    else // Otherwise, it's a direct response to a synchronous request
                    {
                        _responses.Add(response);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("[Proxy Reader] Server connection lost.");
                    _finished = true;
                }
            }
        }

        private void HandleUpdate(Response response)
        {
            // Fire the observer method on the client
            Show updatedShow = ProtoUtils.GetShow(response.Show);
            _clientObserver?.TicketSold(updatedShow);
        }

        // --- IFestivalServices Implementation (Called by UI Controllers) ---

        public void Login(Employee employee, IFestivalObserver client)
        {
            InitializeConnection(); // Connect on login
            
            Request req = new Request
            {
                Type = RequestType.Login,
                Employee = ProtoUtils.GetEmployeeProto(employee)
            };
            
            SendRequest(req);
            Response response = ReadResponse();
            
            if (response.Type == ResponseType.Error)
            {
                CloseConnection();
                throw new FestivalException(response.Error);
            }
            
            _clientObserver = client;
        }

        public void Logout(Employee employee, IFestivalObserver client)
        {
            Request req = new Request
            {
                Type = RequestType.Logout,
                Employee = ProtoUtils.GetEmployeeProto(employee)
            };
            
            SendRequest(req);
            Response response = ReadResponse();
            
            if (response.Type == ResponseType.Error)
            {
                throw new FestivalException(response.Error);
            }
            
            CloseConnection(); // Clean disconnect
        }

        public IEnumerable<Show> GetAllShows()
        {
            Request req = new Request { Type = RequestType.GetAllShows };
            SendRequest(req);
            Response res = ReadResponse();
            
            if (res.Type == ResponseType.Error) throw new FestivalException(res.Error);
            
            return res.Shows.Select(ProtoUtils.GetShow).ToList();
        }

        public IEnumerable<Show> GetShowsByDate(DateTime date)
        {
            Request req = new Request 
            { 
                Type = RequestType.GetShowsByDate, 
                Date = date.ToString("yyyy-MM-dd") 
            };
            
            SendRequest(req);
            Response res = ReadResponse();
            
            if (res.Type == ResponseType.Error) throw new FestivalException(res.Error);
            
            return res.Shows.Select(ProtoUtils.GetShow).ToList();
        }

        public void BuyTicket(Ticket ticket)
        {
            Request req = new Request
            {
                Type = RequestType.BuyTicket,
                Ticket = ProtoUtils.GetTicketProto(ticket)
            };
            
            SendRequest(req);
            Response res = ReadResponse();
            
            if (res.Type == ResponseType.Error) throw new FestivalException(res.Error);
        }
    }
}