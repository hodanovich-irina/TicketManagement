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
    /// Providing methods for managing area.
    /// </summary>
    internal class AreaRepository : IModelRepository<Area>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaRepository"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public AreaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method for add area.
        /// </summary>
        /// <param name="entity">Object of area.</param>
        public async Task<Area> AddAsync(Area entity)
        {
            int result;
            var queryString = @"INSERT INTO Area (LayoutId, Description, CoordX, CoordY) 
                Values( @LayoutId, @Description, @CoordX, @CoordY) SET @INSERTED_ID=SCOPE_IDENTITY()";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var addCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    addCommand.Parameters.AddWithValue("@LayoutId", entity.LayoutId);
                    addCommand.Parameters.AddWithValue("@Description", entity.Description);
                    addCommand.Parameters.AddWithValue("@CoordX", entity.CoordX);
                    addCommand.Parameters.AddWithValue("@CoordY", entity.CoordY);

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
        /// Method for delete area.
        /// </summary>
        /// <param name="id">Id of area.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            bool result;
            var queryString = "DELETE FROM Area WHERE Id = @Id";
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
        /// Method for edit area.
        /// </summary>
        /// <param name="entity">Object of area.</param>
        public async Task<bool> EditAsync(Area entity)
        {
            bool result;
            var queryString = @"UPDATE Area SET LayoutId = @LayoutId, Description = @Description,
                        CoordX = @CoordX, CoordY = @CoordY WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var updateCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    updateCommand.Parameters.AddWithValue("@Id", entity.Id);
                    updateCommand.Parameters.AddWithValue("@LayoutId", entity.LayoutId);
                    updateCommand.Parameters.AddWithValue("@Description", entity.Description);
                    updateCommand.Parameters.AddWithValue("@CoordX", entity.CoordX);
                    updateCommand.Parameters.AddWithValue("@CoordY", entity.CoordY);

                    var res = await updateCommand.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for get area by id.
        /// </summary>
        /// <param name="id">Id of area.</param>
        /// <returns>Object of area.</returns>
        public async Task<Area> GetByIdAsync(int id)
        {
            string queryString = "SELECT Id, LayoutId, Description, CoordX, CoordY FROM Area WHERE Id = @Id";
            var area = new Area();

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
                            area = new Area
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                LayoutId = Convert.ToInt32(dataReader["LayoutId"]),
                                Description = dataReader["Description"].ToString(),
                                CoordX = Convert.ToInt32(dataReader["CoordX"]),
                                CoordY = Convert.ToInt32(dataReader["CoordY"]),
                            };
                        }

                        dataReader.Close();
                    }
                }
            }

            return area;
        }

        /// <summary>
        /// Method for get all area with id.
        /// </summary>
        /// <param name="id">Id of venue.</param>
        /// <returns>Collection of area.</returns>
        public async Task<IQueryable<Area>> GetAllByParentIdAsync(int id)
        {
            string queryString = "SELECT Id, LayoutId, Description, CoordX, CoordY FROM Area WHERE LayoutId = @LayoutId";
            var areas = new List<Area>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var selectCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    selectCommand.Parameters.AddWithValue("@LayoutId", id);
                    using (var dataReader = selectCommand.ExecuteReader())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            areas.Add(new Area
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                LayoutId = Convert.ToInt32(dataReader["LayoutId"]),
                                Description = dataReader["Description"].ToString(),
                                CoordX = Convert.ToInt32(dataReader["CoordX"]),
                                CoordY = Convert.ToInt32(dataReader["CoordY"]),
                            });
                        }

                        dataReader.Close();
                    }
                }
            }

            return areas.AsQueryable();
        }
    }
}
