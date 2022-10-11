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
    internal class EventAreaRepository : IModelRepository<EventArea>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaRepository"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public EventAreaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method for add evnet area.
        /// </summary>
        /// <param name="entity">Object of event area.</param>
        public async Task<EventArea> AddAsync(EventArea entity)
        {
            int result;
            var queryString = @"INSERT INTO EventArea (EventId, Description, CoordX, CoordY, Price) 
                           Values( @EventId, @Description, @CoordX, @CoordY, @Price) SET @INSERTED_ID=SCOPE_IDENTITY()";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var addCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    addCommand.Parameters.AddWithValue("@EventId", entity.EventId);
                    addCommand.Parameters.AddWithValue("@Description", entity.Description);
                    addCommand.Parameters.AddWithValue("@CoordX", entity.CoordX);
                    addCommand.Parameters.AddWithValue("@CoordY", entity.CoordY);
                    addCommand.Parameters.AddWithValue("@Price", entity.Price);

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

        public Task AddEFAsync(Area area)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method for delete event area.
        /// </summary>
        /// <param name="id">Id of event area.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            bool result;
            var queryString = "DELETE FROM EventArea WHERE Id = @Id";
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
        /// Method for edit event area.
        /// </summary>
        /// <param name="entity">Object of event area.</param>
        public async Task<bool> EditAsync(EventArea entity)
        {
            bool result;
            var queryString = @"UPDATE EventArea SET EventId = @EventId, Description = @Description,
                            CoordX = @CoordX, CoordY = @CoordY, Price = @Price WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var updateCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    updateCommand.Parameters.AddWithValue("@Id", entity.Id);
                    updateCommand.Parameters.AddWithValue("@EventId", entity.EventId);
                    updateCommand.Parameters.AddWithValue("@Description", entity.Description);
                    updateCommand.Parameters.AddWithValue("@CoordX", entity.CoordX);
                    updateCommand.Parameters.AddWithValue("@CoordY", entity.CoordY);
                    updateCommand.Parameters.AddWithValue("@Price", entity.Price);

                    var res = await updateCommand.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for get all event area with id.
        /// </summary>
        /// <param name="id">Id of area.</param>
        /// <returns>Collection of event area.</returns>
        public async Task<IQueryable<EventArea>> GetAllByParentIdAsync(int id)
        {
            string queryString = "SELECT Id, EventId, Description, CoordX, CoordY, Price FROM EventArea WHERE EventId = @EventId";
            var eventArea = new List<EventArea>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var selectCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    selectCommand.Parameters.AddWithValue("@EventId", id);
                    using (var dataReader = selectCommand.ExecuteReader())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            eventArea.Add(new EventArea
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                EventId = Convert.ToInt32(dataReader["EventId"]),
                                Description = dataReader["Description"].ToString(),
                                CoordX = Convert.ToInt32(dataReader["CoordX"]),
                                CoordY = Convert.ToInt32(dataReader["CoordY"]),
                                Price = Convert.ToDecimal(dataReader["Price"]),
                            });
                        }

                        dataReader.Close();
                    }
                }
            }

            return eventArea.AsQueryable();
        }

        /// <summary>
        /// Method for get event area by id.
        /// </summary>
        /// <param name="id">Id of event area.</param>
        /// <returns>Object of event area.</returns>
        public async Task<EventArea> GetByIdAsync(int id)
        {
            string queryString = "SELECT Id, EventId, Description, CoordX, CoordY, Price FROM EventArea WHERE Id = @Id";
            var eventArea = new EventArea();
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
                            eventArea = new EventArea
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                EventId = Convert.ToInt32(dataReader["EventId"]),
                                Description = dataReader["Description"].ToString(),
                                CoordX = Convert.ToInt32(dataReader["CoordX"]),
                                CoordY = Convert.ToInt32(dataReader["CoordY"]),
                                Price = Convert.ToDecimal(dataReader["Price"]),
                            };
                        }

                        dataReader.Close();
                    }
                }
            }

            return eventArea;
        }
    }
}
