using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Providing methods for managing venue.
    /// </summary>
    internal class VenueRepository : IVenueRepository<Venue>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenueRepository"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public VenueRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method for add venue.
        /// </summary>
        /// <param name="entity">Object of venue.</param>
        public async Task<Venue> AddAsync(Venue entity)
        {
            int result;
            var queryString = @"INSERT INTO Venue (Description, Address, Phone, Name) 
                Values(@Description, @Address, @Phone, @Name) SET @INSERTED_ID=SCOPE_IDENTITY()";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var addCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    addCommand.Parameters.AddWithValue("@Description", entity.Description);
                    addCommand.Parameters.AddWithValue("@Address", entity.Address);
                    addCommand.Parameters.AddWithValue("@Phone", entity.Phone);
                    addCommand.Parameters.AddWithValue("@Name", entity.Name);
                    SqlParameter id = new SqlParameter
                    {
                        ParameterName = "INSERTED_ID",
                        Size = 4,
                        Direction = ParameterDirection.Output,
                    };
                    addCommand.Parameters.Add(id);

                    await addCommand.ExecuteNonQueryAsync();

                    result = Convert.ToInt32(id.Value);
                    entity.Id = result;
                }
            }

            return entity;
        }

        /// <summary>
        /// Method for delete venue.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            bool result;
            var queryString = "DELETE FROM Venue WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var deleteCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    deleteCommand.Parameters.AddWithValue("@Id", id);

                    var res = await deleteCommand.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for edit venue.
        /// </summary>
        /// <param name="entity">Object of venue.</param>
        public async Task<bool> EditAsync(Venue entity)
        {
            bool result;
            var queryString = @"UPDATE Venue SET Description = @Description, Address = @Address,
                 Phone = @Phone, Name = @Name WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var updateCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    updateCommand.Parameters.AddWithValue("@Id", entity.Id);
                    updateCommand.Parameters.AddWithValue("@Description", entity.Description);
                    updateCommand.Parameters.AddWithValue("@Address", entity.Address);
                    updateCommand.Parameters.AddWithValue("@Phone", entity.Phone);
                    updateCommand.Parameters.AddWithValue("@Name", entity.Name);

                    var res = await updateCommand.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for get all venue with id.
        /// </summary>
        /// <returns>Collection of venue.</returns>
        public async Task<IQueryable<Venue>> GetAllAsync()
        {
            string queryString = "SELECT Id, Description, Address, Phone, Name FROM Venue";
            var venues = new List<Venue>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var selectCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    using (var dataReader = selectCommand.ExecuteReader())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            venues.Add(new Venue
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                Description = dataReader["Description"].ToString(),
                                Address = dataReader["Address"].ToString(),
                                Phone = dataReader["Phone"].ToString(),
                                Name = dataReader["Name"].ToString(),
                            });
                        }

                        dataReader.Close();
                    }
                }
            }

            return venues.AsQueryable();
        }

        /// <summary>
        /// Method for venue all by id.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        /// <returns>Object of venue.</returns>
        public async Task<Venue> GetByIdAsync(int id)
        {
            string queryString = "SELECT Id, Description, Address, Phone, Name FROM Venue WHERE Id = @Id";
            var venue = new Venue();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var selectCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    selectCommand.Parameters.AddWithValue("@Id", id);
                    using (var dataReader = selectCommand.ExecuteReader())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            venue = new Venue
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                Description = dataReader["Description"].ToString(),
                                Address = dataReader["Address"].ToString(),
                                Phone = dataReader["Phone"].ToString(),
                                Name = dataReader["Name"].ToString(),
                            };
                        }

                        dataReader.Close();
                    }
                }
            }

            return venue;
        }
    }
}
