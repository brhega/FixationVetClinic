using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PawsAndClaws
{
    public class DbAccess
    {
        //probably need to make some of this code reusable
        public DbAccess()
        {

        }

        public List<Model.Appointment> GetAllAppointments()
        {
            List<Model.Appointment> allAppointments = new List<Model.Appointment>();
            string phoneNumber;
            string reason;

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"select appt.ID, appt.PetID, appt.OwnerID, appt.ApptTime, appt.reason, pet.Name petName, own.Name ownerName, own.phoneNumber
                                from Appointment appt
                                join Pet pet on appt.PetID = pet.ID
                                join Owner own on appt.OwnerID = own.ID";

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["phoneNumber"] is System.DBNull)
                                {
                                    phoneNumber = "";
                                }
                                else
                                {
                                    phoneNumber = (string)reader["phoneNumber"];
                                }

                                if (reader["reason"] is System.DBNull)
                                {
                                    reason = "";
                                }
                                else
                                {
                                    reason = (string)reader["reason"];
                                }

                                Model.Pet pet = new Model.Pet()
                                {
                                    ID = (int)reader["PetID"],
                                    name = (string)reader["petName"]
                                };

                                Model.Owner owner = new Model.Owner()
                                {
                                    ID = (int)reader["OwnerID"],
                                    name = (string)reader["ownerName"],
                                    phoneNumber = phoneNumber
                                };

                                Model.Appointment appointment = new Model.Appointment()
                                {
                                    appointmentID = (int) reader["ID"],
                                    scheduledPet = pet,
                                    scheduledOwner = owner,
                                    scheduledTime = (DateTime) reader["ApptTime"],
                                    reason = reason
                                };
                                allAppointments.Add(appointment);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }

            return allAppointments;
        }

        public void AddAppointment(Model.Appointment appointment)
        {
            appointment.scheduledOwner.ID = AddOwner(appointment.scheduledOwner);
            appointment.scheduledPet.ID = AddPet(appointment.scheduledPet, appointment.scheduledOwner);

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"insert into Appointment (PetID, OwnerID, ApptTime, reason)
                            values (@petID, @ownerID, @apptTime, @reason)";

                        command.Parameters.Add("@petID", SqlDbType.Int).Value = appointment.scheduledPet.ID;
                        command.Parameters.Add("@ownerID", SqlDbType.Int).Value = appointment.scheduledOwner.ID;
                        
                        if (appointment.scheduledTime != null)
                        {
                            command.Parameters.Add("@apptTime", SqlDbType.DateTime).Value = appointment.scheduledTime;
                        }
                        else
                        {
                            command.Parameters.Add("@apptTime", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        if (appointment.reason != null)
                        {
                            command.Parameters.Add("@reason", SqlDbType.Text).Value = appointment.reason;
                        }
                        else
                        {
                            command.Parameters.Add("@reason", SqlDbType.Text).Value = DBNull.Value;
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }
        }

        public int AddPet(Model.Pet pet, Model.Owner owner)
        {
            int petID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"insert into Pet (OwnerID, Name)
                            values (@ownerID, @name)
                            select SCOPE_IDENTITY() as thisID;";

                        command.Parameters.Add("@ownerID", SqlDbType.Int).Value = owner.ID;
                        command.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = pet.name;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                petID = int.Parse(reader["thisID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }

            return petID;
        }

        public int AddOwner(Model.Owner owner)
        {
            int ownerID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"insert into Owner (Name, phoneNumber)
                            values (@name, @phoneNumber)
                            select SCOPE_IDENTITY() as thisID;";

                        command.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = owner.name;

                        if (owner.phoneNumber != null) 
                        { 
                            command.Parameters.Add("@phoneNumber", SqlDbType.VarChar, 30).Value = owner.phoneNumber; 
                        }
                        else
                        {
                            command.Parameters.Add("@phoneNumber", SqlDbType.VarChar, 30).Value = DBNull.Value;
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //weird fix but okay, I'll keep it for now
                                ownerID = int.Parse(reader["thisID"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }

            return ownerID;
        }

        public Model.Appointment GetAppointment(int appointmentID)
        {
            Model.Appointment appointment = new Model.Appointment();
            string phoneNumber;
            string reason;

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"select appt.ID, appt.PetID, appt.OwnerID, appt.ApptTime, appt.reason, pet.Name petName, own.Name ownerName, own.phoneNumber
                                from Appointment appt
                                join Pet pet on appt.PetID = pet.ID
                                join Owner own on appt.OwnerID = own.ID
                                where appt.ID = @appointmentID";

                        command.Parameters.Add("@appointmentID", SqlDbType.Int).Value = appointmentID;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["phoneNumber"] is System.DBNull)
                                {
                                    phoneNumber = "";
                                }
                                else
                                {
                                    phoneNumber = (string)reader["phoneNumber"];
                                }

                                if (reader["reason"] is System.DBNull)
                                {
                                    reason = "";
                                }
                                else
                                {
                                    reason = (string)reader["reason"];
                                }

                                Model.Pet pet = new Model.Pet()
                                {
                                    ID = (int)reader["PetID"],
                                    name = (string)reader["petName"]
                                };

                                Model.Owner owner = new Model.Owner()
                                {
                                    ID = (int)reader["OwnerID"],
                                    name = (string)reader["ownerName"],
                                    phoneNumber = phoneNumber
                                };

                                appointment.appointmentID = (int)reader["ID"];
                                appointment.scheduledPet = pet;
                                appointment.scheduledOwner = owner;
                                appointment.scheduledTime = (DateTime)reader["ApptTime"];
                                appointment.reason = reason;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }

            return appointment;
        }

        public void EditOwner(Model.Owner owner)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"update Owner
                                set Name = @name, phoneNumber = @phoneNumber
                                where ID = @ownerID";

                        command.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = owner.name;

                        if (owner.phoneNumber != null)
                        {
                            command.Parameters.Add("@phoneNumber", SqlDbType.VarChar, 30).Value = owner.phoneNumber;
                        }
                        else
                        {
                            command.Parameters.Add("@phoneNumber", SqlDbType.VarChar, 30).Value = DBNull.Value;
                        }
                        command.Parameters.Add("@ownerID", SqlDbType.Int).Value = owner.ID;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }
        }

        public void EditPet(Model.Pet pet, Model.Owner owner)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"update Pet
                                set OwnerID = @ownerID, Name = @name
                                where ID = @petID";

                        command.Parameters.Add("@ownerID", SqlDbType.Int).Value = owner.ID;
                        command.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = pet.name;
                        command.Parameters.Add("@petID", SqlDbType.Int).Value = pet.ID;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }
        }

        public void EditAppointment(Model.Appointment appointment)
        {
            EditOwner(appointment.scheduledOwner);
            EditPet(appointment.scheduledPet, appointment.scheduledOwner);

            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"update Appointment
                                set PetID = @petID, OwnerID = @ownerID, ApptTime = @apptTime, reason = @reason
                                where ID = @apptID";

                        command.Parameters.Add("@petID", SqlDbType.Int).Value = appointment.scheduledPet.ID;
                        command.Parameters.Add("@ownerID", SqlDbType.Int).Value = appointment.scheduledOwner.ID;
                        command.Parameters.Add("@apptID", SqlDbType.Int).Value = appointment.appointmentID;

                        if (appointment.scheduledTime != null)
                        {
                            command.Parameters.Add("@apptTime", SqlDbType.DateTime).Value = appointment.scheduledTime;
                        }
                        else
                        {
                            command.Parameters.Add("@apptTime", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        if (appointment.reason != null)
                        {
                            command.Parameters.Add("@reason", SqlDbType.Text).Value = appointment.reason;
                        }
                        else
                        {
                            command.Parameters.Add("@reason", SqlDbType.Text).Value = DBNull.Value;
                        }

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }
        }

        public void DeleteAppointment(int appointmentID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Config.CONNECTIONSTRING))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            @"delete 
                                from Appointment
                                where ID = @apptID";

                        command.Parameters.Add("@apptID", SqlDbType.Int).Value = appointmentID;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                //Do whatever with error message
            }
        }
    }
}
