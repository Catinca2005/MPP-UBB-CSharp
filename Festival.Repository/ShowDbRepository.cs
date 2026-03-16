using System;
using System.Collections.Generic;
using System.Data;
using Festival.Domain;
using log4net;

namespace Festival.Repository
{
    /// <summary>
    /// SQLite implementation for Show persistence. 
    /// Handles complex date/time mapping and seat inventory management.
    /// </summary>
    public class ShowDbRepository : IShowRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ShowDbRepository));

        public IEnumerable<Show> FindByDate(DateTime date)
        {
            string dateStr = date.ToString("yyyy-MM-dd");
            Log.InfoFormat("Entering FindByDate with value: {0}", dateStr);
            IDbConnection con = DbUtils.GetConnection();
            IList<Show> shows = new List<Show>();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, artist_id, show_time, location, available_seats, sold_seats FROM shows WHERE show_date=@date";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@date"; p1.Value = dateStr; comm.Parameters.Add(p1);

                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        shows.Add(ExtractShowFromReader(reader, date));
                    }
                }
            }
            return shows;
        }

        public void Add(Show entity)
        {
            Log.InfoFormat("Entering Add show for Artist ID: {0}", entity.ArtistId);
            IDbConnection con = DbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO shows (artist_id, show_date, show_time, location, available_seats, sold_seats) " +
                                   "VALUES (@aid, @date, @time, @loc, @av, @sold)";

                SetShowParameters(comm, entity);
                comm.ExecuteNonQuery();
            }
        }

        public void Update(Show entity)
        {
            Log.InfoFormat("Entering Update show ID: {0}", entity.Id);
            IDbConnection con = DbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "UPDATE shows SET artist_id=@aid, show_date=@date, show_time=@time, " +
                                   "location=@loc, available_seats=@av, sold_seats=@sold WHERE id=@id";

                SetShowParameters(comm, entity);
                var pId = comm.CreateParameter(); pId.ParameterName = "@id"; pId.Value = entity.Id; comm.Parameters.Add(pId);
                comm.ExecuteNonQuery();
            }
        }

        public void Delete(long id)
        {
            IDbConnection con = DbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM shows WHERE id=@id";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@id"; p1.Value = id; comm.Parameters.Add(p1);
                comm.ExecuteNonQuery();
            }
        }

        public Show FindOne(long id)
        {
            IDbConnection con = DbUtils.GetConnection();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT artist_id, show_date, show_time, location, available_seats, sold_seats FROM shows WHERE id=@id";
                var p1 = comm.CreateParameter(); p1.ParameterName = "@id"; p1.Value = id; comm.Parameters.Add(p1);
                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime date = DateTime.Parse(reader.GetString(1));
                        return ExtractShowFromReader(reader, date, id);
                    }
                }
            }
            return null;
        }

        public IEnumerable<Show> FindAll()
        {
            IDbConnection con = DbUtils.GetConnection();
            IList<Show> shows = new List<Show>();
            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, artist_id, show_date, show_time, location, available_seats, sold_seats FROM shows";
                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime date = DateTime.Parse(reader.GetString(2));
                        shows.Add(new Show(reader.GetInt64(1), date, TimeSpan.Parse(reader.GetString(3)),
                            reader.GetString(4), reader.GetInt32(5), reader.GetInt32(6))
                        { Id = reader.GetInt64(0) });
                    }
                }
            }
            return shows;
        }

        private void SetShowParameters(IDbCommand comm, Show entity)
        {
            var p1 = comm.CreateParameter(); p1.ParameterName = "@aid"; p1.Value = entity.ArtistId; comm.Parameters.Add(p1);
            var p2 = comm.CreateParameter(); p2.ParameterName = "@date"; p2.Value = entity.Date.ToString("yyyy-MM-dd"); comm.Parameters.Add(p2);
            var p3 = comm.CreateParameter(); p3.ParameterName = "@time"; p3.Value = entity.Time.ToString(@"hh\:mm\:ss"); comm.Parameters.Add(p3);
            var p4 = comm.CreateParameter(); p4.ParameterName = "@loc"; p4.Value = entity.Location; comm.Parameters.Add(p4);
            var p5 = comm.CreateParameter(); p5.ParameterName = "@av"; p5.Value = entity.AvailableSeats; comm.Parameters.Add(p5);
            var p6 = comm.CreateParameter(); p6.ParameterName = "@sold"; p6.Value = entity.SoldSeats; comm.Parameters.Add(p6);
        }

        private Show ExtractShowFromReader(IDataReader reader, DateTime date, long? id = null)
        {
            long showId = id ?? reader.GetInt64(0);
            long artistId = (id == null) ? reader.GetInt64(1) : reader.GetInt64(0);
            int timeIdx = (id == null) ? 2 : 2;

            return new Show(artistId, date, TimeSpan.Parse(reader.GetString(2)),
                reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5))
            { Id = showId };
        }
    }
}