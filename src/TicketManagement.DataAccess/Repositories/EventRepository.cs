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
    /// Providing methods for managing event.
    /// </summary>
    internal class EventRepository : IModelRepository<Event>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventRepository"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public EventRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method for add event.
        /// </summary>
        /// <param name="entity">Object of event.</param>
        public async Task<Event> AddAsync(Event entity)
        {
            int result;
            var queryString = "InsertEvent";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var insertProcedure = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    insertProcedure.CommandType = CommandType.StoredProcedure;
                    insertProcedure.Parameters.AddWithValue("@Name", entity.Name);
                    insertProcedure.Parameters.AddWithValue("@Description", entity.Description);
                    insertProcedure.Parameters.AddWithValue("@LayoutId", entity.LayoutId);
                    insertProcedure.Parameters.AddWithValue("@DateStart", entity.DateStart);
                    insertProcedure.Parameters.AddWithValue("@DateEnd", entity.DateEnd);
                    insertProcedure.Parameters.AddWithValue("@ImageURL", entity.ImageURL);
                    insertProcedure.Parameters.AddWithValue("@ShowTime", entity.ShowTime);
                    var addedId = new SqlParameter
                    {
                        ParameterName = "@AddedId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output,
                    };
                    insertProcedure.Parameters.Add(addedId);
                    await insertProcedure.ExecuteNonQueryAsync();
                    result = Convert.ToInt32(insertProcedure.Parameters["@AddedId"].Value);
                    entity.Id = result;
                }
            }

            return entity;
        }

        /// <summary>
        /// Method for delete event.
        /// </summary>
        /// <param name="id">Id of event.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            bool result;
            var queryString = "DeleteEvent";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var deleteProcedure = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    deleteProcedure.CommandType = CommandType.StoredProcedure;
                    deleteProcedure.Parameters.AddWithValue("@Id", id);

                    var res = await deleteProcedure.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for edit event.
        /// </summary>
        /// <param name="entity">Object of event.</param>
        public async Task<bool> EditAsync(Event entity)
        {
            bool result;
            var queryString = "UpdateEvent";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var updateProcedure = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    updateProcedure.CommandType = CommandType.StoredProcedure;
                    updateProcedure.Parameters.AddWithValue("@Id", entity.Id);
                    updateProcedure.Parameters.AddWithValue("@Name", entity.Name);
                    updateProcedure.Parameters.AddWithValue("@Description", entity.Description);
                    updateProcedure.Parameters.AddWithValue("@LayoutId", entity.LayoutId);
                    updateProcedure.Parameters.AddWithValue("@DateStart", entity.DateStart);
                    updateProcedure.Parameters.AddWithValue("@DateEnd", entity.DateEnd);
                    updateProcedure.Parameters.AddWithValue("@ImageURL", entity.ImageURL);
                    updateProcedure.Parameters.AddWithValue("@ShowTime", entity.ShowTime);

                    var res = await updateProcedure.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for get event by id.
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>Object of event.</returns>
        public async Task<Event> GetByIdAsync(int id)
        {
            string queryString = "SELECT Id, Name, Description, LayoutId, DateStart, DateEnd FROM Event WHERE Id = @Id";
            var newEvent = new Event();
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
                            newEvent = new Event
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                Name = dataReader["Name"].ToString(),
                                Description = dataReader["Description"].ToString(),
                                LayoutId = Convert.ToInt32(dataReader["LayoutId"]),
                                DateStart = Convert.ToDateTime(dataReader["DateStart"]),
                                DateEnd = Convert.ToDateTime(dataReader["DateEnd"]),
                            };
                        }

                        dataReader.Close();
                    }
                }
            }

            return newEvent;
        }

        /// <summary>
        /// Method for get all event with id.
        /// </summary>
        /// <param name="id">Id of layout.</param>
        /// <returns>Collection of event.</returns>
        public async Task<IQueryable<Event>> GetAllByParentIdAsync(int id)
        {
            string queryString = "SELECT Id, Name, Description, LayoutId, DateStart, DateEnd FROM Event WHERE LayoutId = @LayoutId";
            var events = new List<Event>();
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
                            events.Add(new Event
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                Name = dataReader["Name"].ToString(),
                                Description = dataReader["Description"].ToString(),
                                LayoutId = Convert.ToInt32(dataReader["LayoutId"]),
                                DateStart = Convert.ToDateTime(dataReader["DateStart"]),
                                DateEnd = Convert.ToDateTime(dataReader["DateEnd"]),
                            });
                        }

                        dataReader.Close();
                    }
                }
            }

            return events.AsQueryable();
        }
    }
}
