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
    /// Providing methods for managing layout.
    /// </summary>
    internal class LayoutRepository : IModelRepository<Layout>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRepository"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public LayoutRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method for add layout.
        /// </summary>
        /// <param name="entity">Object of layout.</param>
        public async Task<Layout> AddAsync(Layout entity)
        {
            int result;
            var queryString = @"INSERT INTO Layout (VenueId, Description, Name) 
                Values( @VenueId, @Description, @Name) SET @INSERTED_ID=SCOPE_IDENTITY()";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var addCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    addCommand.Parameters.AddWithValue("@VenueId", entity.VenueId);
                    addCommand.Parameters.AddWithValue("@Description", entity.Description);
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
        /// Method for delete layout.
        /// </summary>
        /// <param name="id">Id of layout.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            bool result;
            var queryString = "DELETE FROM Layout WHERE Id = @Id";
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
        /// Method for edit layout.
        /// </summary>
        /// <param name="entity">Object of layout.</param>
        public async Task<bool> EditAsync(Layout entity)
        {
            bool result;
            var queryString = @"UPDATE Layout SET VenueId = @VenueId, Description = @Description, 
                  Name = @Name WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var updateCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    updateCommand.Parameters.AddWithValue("@Id", entity.Id);
                    updateCommand.Parameters.AddWithValue("@VenueId", entity.VenueId);
                    updateCommand.Parameters.AddWithValue("@Description", entity.Description);
                    updateCommand.Parameters.AddWithValue("@Name", entity.Name);

                    var res = await updateCommand.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for get layout by id.
        /// </summary>
        /// <param name="id">Id of layout.</param>
        /// <returns>Object of layout.</returns>
        public async Task<Layout> GetByIdAsync(int id)
        {
            string queryString = "SELECT Id, VenueId, Description, Name FROM Layout WHERE Id = @Id";
            var layout = new Layout();
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
                            layout = new Layout
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                VenueId = Convert.ToInt32(dataReader["VenueId"]),
                                Description = dataReader["Description"].ToString(),
                                Name = dataReader["Name"].ToString(),
                            };
                        }

                        dataReader.Close();
                    }
                }
            }

            return layout;
        }

        /// <summary>
        /// Method for get all layout with id.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        /// <returns>Collection of layout.</returns>
        public async Task<IQueryable<Layout>> GetAllByParentIdAsync(int id)
        {
            string queryString = "SELECT Id, VenueId, Description, Name FROM Layout WHERE VenueId = @VenueId";
            var layouts = new List<Layout>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var selectCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    selectCommand.Parameters.AddWithValue("@VenueId", id);
                    using (var dataReader = selectCommand.ExecuteReader())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            layouts.Add(new Layout
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                VenueId = Convert.ToInt32(dataReader["VenueId"]),
                                Description = dataReader["Description"].ToString(),
                                Name = dataReader["Name"].ToString(),
                            });
                        }

                        dataReader.Close();
                    }
                }
            }

            return layouts.AsQueryable();
        }
    }
}
