using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Festival.Model;
using Festival.Services;
using Festival.Networking.Protobuf;
using Google.Protobuf; 

namespace Festival.Networking
{
    /// <summary>
    /// Server-side worker handling a single client connection via raw TCP sockets and Protocol Buffers.
    /// Translates incoming network RPCs into actual business logic calls.
    /// </summary>
    public class FestivalClientProtobufWorker : IFestivalObserver
    {
        private readonly IFestivalServices _server;
        private readonly TcpClient _connection;
        private readonly NetworkStream _stream;
        private volatile bool _connected;

        public FestivalClientProtobufWorker(IFestivalServices server, TcpClient connection)
        {
            _server = server;
            _connection = connection;
            _stream = connection.GetStream();
            _connected = true;
        }

        /// <summary>
        /// Starts the continuous listening loop for incoming client requests.
        /// </summary>
        public void Run()
        {
            while (_connected)
            {
                try
                {
                    // Blocking call until a Protobuf message is fully received
                    Request request = Request.Parser.ParseDelimitedFrom(_stream);
                    if (request != null)
                    {
                        Response response = HandleRequest(request);
                        if (response != null)
                        {
                            SendResponse(response);
                        }
                    }
                    else
                    {
                        _connected = false; // Stream closed
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[Worker Error] Connection dropped: {e.Message}");
                    _connected = false;
                }
            }
            
            // Clean up resources
            try { _stream.Close(); _connection.Close(); } catch { }
        }

        /// <summary>
        /// Routes the decoded Protobuf request to the appropriate service method.
        /// </summary>
        private Response HandleRequest(Request request)
        {
            Response response = new Response();
            try
            {
                switch (request.Type)
                {
                    case RequestType.Login:
                        Employee emp = ProtoUtils.GetEmployee(request.Employee);
                        _server.Login(emp, this); // Passing 'this' because the Worker is the Observer
                        response.Type = ResponseType.Ok;
                        break;
                        
                    case RequestType.Logout:
                        Employee empOut = ProtoUtils.GetEmployee(request.Employee);
                        _server.Logout(empOut, this);
                        _connected = false;
                        response.Type = ResponseType.Ok;
                        break;

                    case RequestType.GetAllShows:
                        var allShows = _server.GetAllShows();
                        response.Type = ResponseType.GetAllShowsResponse;
                        response.Shows.AddRange(allShows.Select(ProtoUtils.GetShowProto));
                        break;

                    case RequestType.GetShowsByDate:
                        DateTime date = DateTime.Parse(request.Date);
                        var filteredShows = _server.GetShowsByDate(date);
                        response.Type = ResponseType.GetShowsByDateResponse;
                        response.Shows.AddRange(filteredShows.Select(ProtoUtils.GetShowProto));
                        break;

                    case RequestType.BuyTicket:
                        Ticket ticket = ProtoUtils.GetTicket(request.Ticket);
                        _server.BuyTicket(ticket);
                        response.Type = ResponseType.Ok;
                        break;
                }
            }
            catch (FestivalException e)
            {
                response.Type = ResponseType.Error;
                response.Error = e.Message;
            }
            return response;
        }

        private void SendResponse(Response response)
        {
            lock (_stream) // Prevent concurrent writes to the same socket
            {
                response.WriteDelimitedTo(_stream);
                _stream.Flush();
            }
        }

        // --- IFestivalObserver Implementation (Triggered by the Server) ---

        public void TicketSold(Show updatedShow)
        {
            // When the server notifies us, we package the update and send it to the client
            Response updateResponse = new Response
            {
                Type = ResponseType.Update,
                Show = ProtoUtils.GetShowProto(updatedShow)
            };
            SendResponse(updateResponse);
        }
    }
}