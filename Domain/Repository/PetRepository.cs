using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Status;
using System.Data.SQLite;

namespace Domain.Repository
{
    public class PetRepository : IPetRepository
    {
        SQLiteConnection _connectionString;
 
        public StatusRequest Status { get; private set; }
        public List<string> ErrorMessage { get; private set; }

        public PetRepository()
        {
            _connectionString = new SQLiteConnection("Data Source=database.db;Version=3;Compress=True;");
            ErrorMessage = new List<string>();
        }

        public PetRepository(string dbPath)
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

        public IEnumerable<Pet> GetList()
        {
            List<Pet> pets = new List<Pet>();

            try
            {
                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();
                cmd.CommandText = @"SELECT p.id, p.name, p.ownerId, o.name AS owner 
                                    FROM pet p 
                                    JOIN owner o on p.ownerId == o.id";
                SQLiteDataReader datareader = cmd.ExecuteReader();


                while (datareader.Read())
                {
                    pets.Add(new Pet()
                    {
                        Id = datareader.GetInt32(0),
                        Name = datareader.GetString(1),
                        OwnerId = datareader.GetInt32(2),
                        OwnerName = datareader.GetString(3),
                    });
                }
                _connectionString.Close();

                Status = StatusRequest.Ok;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }

            return pets;
        }
        public List<Pet> GetOwnerPetsList(int ownerId)
        {
            List<Pet> pets = new List<Pet>();

            try
            {
                if (ownerId <= 0)
                {
                    ErrorMessage.Add("ownerId must be more than 0");
                    Status = StatusRequest.BadRequest;
                    return pets;
                }

                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();
                cmd.CommandText = @"SELECT IFNULL(p.id, 0) AS id, p.name, o.id, o.name AS owner 
                                    FROM owner o 
                                    LEFT JOIN  pet p on p.ownerId == o.id
                                    WHERE o.id == " + ownerId;

                //cmd.CommandText = @"SELECT p.id, p.name, p.ownerId, o.name AS owner 
                //                    FROM pet p 
                //                    JOIN owner o on p.ownerId == o.id
                //                    WHERE p.ownerId == " + ownerId;

                SQLiteDataReader datareader = cmd.ExecuteReader();


                while (datareader.Read())
                {
                    int Id = datareader.GetInt32(0);
                    string Name = null;
                    if (Id != 0)
                    {
                        Name = datareader.GetString(1);
                    }
                    int OwnerId = ownerId;
                    string OwnerName = datareader.GetString(3);

                    pets.Add(new Pet()
                    {
                        Id = Id,
                        Name = Name,
                        OwnerId = OwnerId,
                        OwnerName = OwnerName,
                    });
                }
                _connectionString.Close();

                Status = StatusRequest.Ok;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }

            return pets;
        }
        public Pet GetElement(int id)
        {
            Pet pet = null;

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
                cmd.CommandText = cmd.CommandText = @"SELECT p.id, p.name, p.ownerId, o.name AS owner 
                                    FROM pet p 
                                    JOIN owner o on p.ownerId == o.id
                                    WHERE p.id == " + id;
                SQLiteDataReader datareader = cmd.ExecuteReader();


                while (datareader.Read())
                {
                    pet = new Pet()
                    {
                        Id = datareader.GetInt32(0),
                        Name = datareader.GetString(1),
                        OwnerId = datareader.GetInt32(2),
                    };
                }
                _connectionString.Close();

                Status = StatusRequest.Found;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }

            return pet;
        }

        public void Create(Pet item)
        {
            try
            {
                if (item == null || item.OwnerId < 0)
                {
                    ErrorMessage.Add("sended object is no valid");
                    Status = StatusRequest.BadRequest;
                    return;
                }


                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();
                cmd.CommandText = string.Format("INSERT INTO pet (name, ownerid) VALUES('{0}', '{1}');", item.Name, item.OwnerId);
                cmd.ExecuteNonQuery();

                _connectionString.Close();

                Status = StatusRequest.Created;
            }
            catch (Exception ex)
            {
                CatchedExeption(ex);
            }
        }

        public void Update(Pet item)
        {
            try
            {
                if (item == null || item.Id <= 0)
                {
                    ErrorMessage.Add("sended object is no valid");
                    Status = StatusRequest.BadRequest;
                    return;
                }

                bool wasInsideIf = false;

                string command = "UPDATE pet SET ";

                if (!string.IsNullOrEmpty(item.Name))
                {
                    command += string.Format(" name = '{0}'", item.Name);

                    wasInsideIf = true;
                }


                if (item.OwnerId > 0)
                {
                    if (wasInsideIf)
                    {
                        command += ',';
                    }

                    command += " ownerid = " + item.OwnerId;

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
                    ErrorMessage.Add("id must mbe more than 0");
                    Status = StatusRequest.BadRequest;
                    return;
                }


                _connectionString.Open();
                SQLiteCommand cmd = _connectionString.CreateCommand();
                cmd.CommandText = string.Format("DELETE FROM pet WHERE Id = {0}", id);
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
