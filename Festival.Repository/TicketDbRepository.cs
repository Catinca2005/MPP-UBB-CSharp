using System;
using System.Collections.Generic;
using System.Data;
using Festival.Domain;
using log4net;

namespace Festival.Repository
{
    /// <summary>
    /// SQLite implementation for Ticket persistence. 
    /// Manages ticket sales records and links them to specific shows.
    /// </summary>
    public class TicketDbRepository : ITicketRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TicketDbRepository));

        public void Add(Ticket entity)
        {
            Log.InfoFormat("Entering Add ticket for Buyer: {0}", entity.BuyerName);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO tickets (show_id, buyer_name, number_of_seats) VALUES (@sid, @name, @seats)";

                var p1 = comm.CreateParameter(); p1.ParameterName = "@sid"; p1.Value = entity.ShowId; comm.Parameters.Add(p1);
                var p2 = comm.CreateParameter(); p2.ParameterName = "@name"; p2.Value = entity.BuyerName; comm.Parameters.Add(p2);
                var p3 = comm.CreateParameter(); p3.ParameterName = "@seats"; p3.Value = entity.NumberOfSeats; comm.Parameters.Add(p3);

                comm.ExecuteNonQuery();
            }
            Log.Info("Exiting Add");
        }

        public IEnumerable<Ticket> FindAllByShow(long showId)
        {
            Log.InfoFormat("Entering FindAllByShow with Show ID: {0}", showId);
            IDbConnection con = DbUtils.GetConnection();
            IList<Ticket> tickets = new List<Ticket>();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, buyer_name, number_of_seats FROM tickets WHERE show_id=@sid";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@sid"; p1.Value = showId; comm.Parameters.Add(p1);

                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tickets.Add(new Ticket(showId, reader.GetString(1), reader.GetInt32(2)) { Id = reader.GetInt64(0) });
                    }
                }
            }
            Log.InfoFormat("Exiting FindAllByShow - found {0} tickets", tickets.Count);
            return tickets;
        }

        public Ticket FindOne(long id)
        {
            Log.InfoFormat("Entering FindOne for Ticket ID: {0}", id);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT show_id, buyer_name, number_of_seats FROM tickets WHERE id=@id";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@id"; p1.Value = id; comm.Parameters.Add(p1);

                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Ticket(reader.GetInt64(0), reader.GetString(1), reader.GetInt32(2)) { Id = id };
                    }
                }
            }
            return null;
        }

        public IEnumerable<Ticket> FindAll()
        {
            Log.Info("Entering FindAll tickets");
            IDbConnection con = DbUtils.GetConnection();
            IList<Ticket> tickets = new List<Ticket>();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, show_id, buyer_name, number_of_seats FROM tickets";
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tickets.Add(new Ticket(reader.GetInt64(1), reader.GetString(2), reader.GetInt32(3)) { Id = reader.GetInt64(0) });
                    }
                }
            }
            return tickets;
        }

        public void Update(Ticket entity)
        {
            Log.InfoFormat("Entering Update for ticket ID: {0}", entity.Id);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "UPDATE tickets SET show_id=@sid, buyer_name=@name, number_of_seats=@seats WHERE id=@id";

                var p1 = comm.CreateParameter(); p1.ParameterName = "@sid"; p1.Value = entity.ShowId; comm.Parameters.Add(p1);
                var p2 = comm.CreateParameter(); p2.ParameterName = "@name"; p2.Value = entity.BuyerName; comm.Parameters.Add(p2);
                var p3 = comm.CreateParameter(); p3.ParameterName = "@seats"; p3.Value = entity.NumberOfSeats; comm.Parameters.Add(p3);
                var p4 = comm.CreateParameter(); p4.ParameterName = "@id"; p4.Value = entity.Id; comm.Parameters.Add(p4);

                comm.ExecuteNonQuery();
            }
        }

        public void Delete(long id)
        {
            Log.InfoFormat("Entering Delete for ticket ID: {0}", id);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM tickets WHERE id=@id";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@id"; p1.Value = id; comm.Parameters.Add(p1);
                comm.ExecuteNonQuery();
            }
        }
    }
}