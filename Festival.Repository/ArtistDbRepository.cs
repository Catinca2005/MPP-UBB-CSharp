using System;
using System.Collections.Generic;
using System.Data;
using Festival.Domain;
using log4net;

namespace Festival.Repository
{
    /// <summary>
    /// SQLite-specific implementation of the IArtistRepository interface.
    /// Manages persistence logic for Artist entities using ADO.NET.
    /// </summary>
    public class ArtistDbRepository : IArtistRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ArtistDbRepository));
        public void Add(Artist entity)
        {
            Log.InfoFormat("Entering Add with artist: {0}", entity.Name);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO artists (name) VALUES (@name)";

                var paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = entity.Name;
                comm.Parameters.Add(paramName);

                var result = comm.ExecuteNonQuery();
                Log.InfoFormat("Exiting Add - rows affected: {0}", result);
            }
        }
        public IEnumerable<Artist> FindAll()
        {
            Log.Info("Entering FindAll artists");
            IDbConnection con = DbUtils.GetConnection();
            IList<Artist> artists = new List<Artist>();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT id, name FROM artists";

                using (var reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(0);
                        string name = reader.GetString(1);
                        Artist artist = new Artist(name) { Id = id };
                        artists.Add(artist);
                    }
                }
            }
            Log.InfoFormat("Exiting FindAll - found {0} artists", artists.Count);
            return artists;
        }

        public Artist FindOne(long id)
        {
            Log.InfoFormat("Entering FindOne with id: {0}", id);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "SELECT name FROM artists WHERE id=@id";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var reader = comm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(0);
                        Artist artist = new Artist(name) { Id = id };
                        Log.InfoFormat("Exiting FindOne - artist found: {0}", name);
                        return artist;
                    }
                }
            }
            Log.Info("Exiting FindOne - no artist found");
            return null;
        }
        public void Update(Artist entity)
        {
            Log.InfoFormat("Entering Update for artist ID: {0}", entity.Id);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "UPDATE artists SET name=@name WHERE id=@id";

                var paramName = comm.CreateParameter();
                paramName.ParameterName = "@name";
                paramName.Value = entity.Name;
                comm.Parameters.Add(paramName);

                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.Id;
                comm.Parameters.Add(paramId);

                var result = comm.ExecuteNonQuery();
                Log.InfoFormat("Exiting Update - rows affected: {0}", result);
            }
        }
        public void Delete(long id)
        {
            Log.InfoFormat("Entering Delete for artist ID: {0}", id);
            IDbConnection con = DbUtils.GetConnection();

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "DELETE FROM artists WHERE id=@id";

                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                var result = comm.ExecuteNonQuery();
                Log.InfoFormat("Exiting Delete - rows affected: {0}", result);
            }
        }
    }
}