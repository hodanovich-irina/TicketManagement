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
    /// Providing methods for managing event seat.
    /// </summary>
    internal class EventSeatRepository : IModelRepository<EventSeat>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSeatRepository"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public EventSeatRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method for add event seat.
        /// </summary>
        /// <param name="entity">Object of event seat.</param>
        public async Task<EventSeat> AddAsync(EventSeat entity)
        {
            int result;
            var queryString = @"INSERT INTO EventSeat (EventAreaId, Row, Number, State)  
                Values(@EventAreaId, @Row, @Number, @State) SET @INSERTED_ID=SCOPE_IDENTITY()";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var addCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    addCommand.Parameters.AddWithValue("@EventAreaId", entity.EventAreaId);
                    addCommand.Parameters.AddWithValue("@Row", entity.Row);
                    addCommand.Parameters.AddWithValue("@Number", entity.Number);
                    addCommand.Parameters.AddWithValue("@State", (int)entity.State);

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
        /// Method for delete event seat.
        /// </summary>
        /// <param name="id">Id of event seat.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            bool result;
            var queryString = "DELETE FROM EventSeat WHERE Id = @Id";
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
        /// Method for edit event seat.
        /// </summary>
        /// <param name="entity">Object of event seat.</param>
        public async Task<bool> EditAsync(EventSeat entity)
        {
            bool result;
            var queryString = @"UPDATE EventSeat SET EventAreaId = @EventAreaId, Row = @Row,
                 Number = @Number, State = @State WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var updateCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    updateCommand.Parameters.AddWithValue("@Id", entity.Id);
                    updateCommand.Parameters.AddWithValue("@EventAreaId", entity.EventAreaId);
                    updateCommand.Parameters.AddWithValue("@Row", entity.Row);
                    updateCommand.Parameters.AddWithValue("@Number", entity.Number);
                    updateCommand.Parameters.AddWithValue("@State", (int)entity.State);

                    var res = await updateCommand.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for get all event seats with id.
        /// </summary>
        /// <param name="id">Id of event.</param>
        /// <returns>Collection of event seats.</returns>
        public async Task<IQueryable<EventSeat>> GetAllByParentIdAsync(int id)
        {
            string queryString = "SELECT Id, EventAreaId, Row, Number, State FROM EventSeat WHERE EventAreaId = @EventAreaId";
            var eventSeats = new List<EventSeat>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var selectCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    selectCommand.Parameters.AddWithValue("@EventAreaId", id);
                    using (var dataReader = selectCommand.ExecuteReader())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            eventSeats.Add(new EventSeat
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                EventAreaId = Convert.ToInt32(dataReader["EventAreaId"]),
                                Row = Convert.ToInt32(dataReader["Row"]),
                                Number = Convert.ToInt32(dataReader["Number"]),
                                State = (EventSeatState)dataReader["State"],
                            });
                        }

                        dataReader.Close();
                    }
                }
            }

            return eventSeats.AsQueryable();
        }

        /// <summary>
        /// Method for get event seat by id.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        /// <returns>Object of event seat.</returns>
        public async Task<EventSeat> GetByIdAsync(int id)
        {
            string queryString = "SELECT Id, EventAreaId, Row, Number, State FROM EventSeat WHERE Id = @Id";
            var eventSeat = new EventSeat();
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
                            eventSeat = new EventSeat
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                EventAreaId = Convert.ToInt32(dataReader["EventAreaId"]),
                                Row = Convert.ToInt32(dataReader["Row"]),
                                Number = Convert.ToInt32(dataReader["Number"]),
                                State = (EventSeatState)dataReader["State"],
                            };
                        }

                        dataReader.Close();
                    }
                }
            }

            return eventSeat;
        }
    }
}
