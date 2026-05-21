using Microsoft.AspNetCore.SignalR;

namespace Festival.RestServices.Hubs
{
    /// <summary>
    /// The WebSocket endpoint for real-time notifications.
    /// It acts as a broadcasting tower, keeping track of all connected web clients.
    /// </summary>
    public class ShowHub : Hub
    {
        // This class is intentionally left empty.
        // Data mutations (Create/Update/Delete) happen via REST Controllers.
        // The ShowController will use this Hub's context behind the scenes to broadcast updates.
    }
}