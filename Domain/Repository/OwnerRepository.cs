using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Status;
using System.Data.SQLite;

namespace Domain.Repository
{
    public class OwnerRepository : IRepository<Owner>
    {
        //string path = Directory.GetCurrentDirectory();
        SQLiteConnection _connectionString; 

        public StatusRequest Status { get; private set; }
        public List<string> ErrorMessage { get; private set; } 

        public OwnerRepository()
        {
            _connectionString = new SQLiteConnection("Data Source=database.db;Version=3;Compress=True;");
            ErrorMessage = new List<string>();
        }

        public OwnerRepository(string dbPath)
        {
            _connectionString = new SQLiteConnection(string.Format("Data Source={0};Version=3;Compress=True;", dbPath));
            ErrorMessage = new List<string>();
        }

        public void CatchedExeption(Exception ex)
        {
            ErrorMessage.Add(ex.Message);
            Status = StatusRequest.BadRequest;
            _connectionString.Close();
        }

        public IEnumerable<Owner> GetList()
        {
            List<Owner> owners = new List<Owner>();

            try
            {
                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();
                cmd.CommandText = @"SELECT o.id, o.name, IFNULL(sub.petCount, 0) AS petCount
                                    FROM owner o LEFT JOIN
                                    (SELECT COUNT(p.name) as petCount, p.OwnerId
                                    FROM pet p
                                    GROUP BY p.OwnerId
                                    )sub on sub.OwnerId = o.id";
                SQLiteDataReader datareader = cmd.ExecuteReader();


                while (datareader.Read())
                {
                    owners.Add(new Owner()
                    {
                        Id = datareader.GetInt32(0),
                        Name = datareader.GetString(1),
                        PetCount = datareader.GetInt32(2),
                    });
                }
                _connectionString.Close();

                Status = StatusRequest.Ok;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }

            return owners;
        }

        public Owner GetElement(int id)
        {
            Owner owner = null;

            try
            {
                if (id <= 0)
                {
                    ErrorMessage.Add("id must be more than 0");
                    Status = StatusRequest.BadRequest;
                    return null;
                }


                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();
                cmd.CommandText = @"SELECT o.id, o.name, IFNULL(sub.petCount, 0) AS petCount
                                    FROM  [Owner] o JOIN 
                                    (SELECT COUNT(p.name) as petCount, p.OwnerId
                                    FROM [pet] p 
                                    GROUP BY p.OwnerId
                                    )sub on sub.OwnerId = o.id 
                                    WHERE o.Id = " + id;
                SQLiteDataReader datareader = cmd.ExecuteReader();


                while (datareader.Read())
                {
                    owner = new Owner()
                    {
                        Id = datareader.GetInt32(0),
                        Name = datareader.GetString(1),
                        PetCount = datareader.GetInt32(2),
                    };
                }
                _connectionString.Close();

                Status = StatusRequest.Found;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }

            return owner;
        }

        public void Create(Owner item)
        {
            try
            {
                if (item == null)
                {
                    ErrorMessage.Add("sended object is not valid");
                    Status = StatusRequest.BadRequest;
                    return;
                }
                   


                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();
                cmd.CommandText = string.Format("INSERT INTO owner (name) VALUES('{0}');", item.Name);
                cmd.ExecuteNonQuery();

                _connectionString.Close();

                Status = StatusRequest.Created;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }
        }

        public void Update(Owner item)
        {
            try
            {
                if (item == null || item.Id <= 0)
                {
                    ErrorMessage.Add("sended object is not valid");
                    Status = StatusRequest.BadRequest;
                    return;
                }

                bool wasInsideIf = false;
                string command = "UPDATE owner SET ";

                if (!string.IsNullOrEmpty(item.Name))
                {
                    command += string.Format(" name = '{0}'", item.Name);

                    wasInsideIf = true;
                }


                if (!wasInsideIf)
                {
                    Status = StatusRequest.NotChanged;
                    return;
                }
                    

                command += "WHERE id = " + item.Id;

                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();

                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
                _connectionString.Close();

                Status = StatusRequest.Updated;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    ErrorMessage.Add("id must be more than 0");
                    Status = StatusRequest.BadRequest;
                    return;
                }
                  


                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();

                cmd.CommandText = string.Format("DELETE FROM pet WHERE ownerId = {0}", id);
                cmd.ExecuteNonQuery();

                cmd.CommandText = string.Format("DELETE FROM owner WHERE Id = {0}", id);
                cmd.ExecuteNonQuery();

                _connectionString.Close();

                Status = StatusRequest.Deleted;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }
        }
    }
}
