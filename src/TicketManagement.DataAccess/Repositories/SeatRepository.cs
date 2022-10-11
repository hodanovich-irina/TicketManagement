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
    /// Providing methods for managing seat.
    /// </summary>
    internal class SeatRepository : IModelRepository<Seat>
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatRepository"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string to database.</param>
        public SeatRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Method for add seat.
        /// </summary>
        /// <param name="entity">Object to seat.</param>
        public async Task<Seat> AddAsync(Seat entity)
        {
            int result;
            var queryString = @"INSERT INTO Seat (AreaId, Row, Number) 
                Values( @AreaId, @Row, @Number) SET @INSERTED_ID=SCOPE_IDENTITY()";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var addCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    addCommand.Parameters.AddWithValue("@AreaId", entity.AreaId);
                    addCommand.Parameters.AddWithValue("@Row", entity.Row);
                    addCommand.Parameters.AddWithValue("@Number", entity.Number);
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
        /// Method for delete seat.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            bool result;
            var queryString = "DELETE FROM Seat WHERE Id = @Id";
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
        /// Method for edit seat.
        /// </summary>
        /// <param name="entity">Object of seat.</param>
        public async Task<bool> EditAsync(Seat entity)
        {
            bool result;
            var queryString = @"UPDATE Seat SET AreaId = @AreaId, Row = @Row, 
                Number = @Number WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var updateCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();

                    updateCommand.Parameters.AddWithValue("@Id", entity.Id);
                    updateCommand.Parameters.AddWithValue("@AreaId", entity.AreaId);
                    updateCommand.Parameters.AddWithValue("@Row", entity.Row);
                    updateCommand.Parameters.AddWithValue("@Number", entity.Number);

                    var res = await updateCommand.ExecuteNonQueryAsync();
                    result = Convert.ToBoolean(res);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for get seat by id.
        /// </summary>
        /// <param name="id">Id of seat.</param>
        /// <returns>Object of seat.</returns>
        public async Task<Seat> GetByIdAsync(int id)
        {
            string queryString = "SELECT Id, AreaId, Row, Number FROM Seat WHERE Id = @Id";
            var seat = new Seat();
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
                            seat = new Seat
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                AreaId = Convert.ToInt32(dataReader["AreaId"]),
                                Row = Convert.ToInt32(dataReader["Row"]),
                                Number = Convert.ToInt32(dataReader["Number"]),
                            };
                        }

                        dataReader.Close();
                    }
                }
            }

            return seat;
        }

        /// <summary>
        /// Method for get all seats with id.
        /// </summary>
        /// <param name="id">Id of area.</param>
        /// <returns>Collection of seat.</returns>
        public async Task<IQueryable<Seat>> GetAllByParentIdAsync(int id)
        {
            string queryString = "SELECT Id, AreaId, Row, Number FROM Seat WHERE AreaId = @AreaId";
            var seats = new List<Seat>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (var selectCommand = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    selectCommand.Parameters.AddWithValue("@AreaId", id);
                    using (var dataReader = selectCommand.ExecuteReader())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            seats.Add(new Seat
                            {
                                Id = Convert.ToInt32(dataReader["Id"]),
                                AreaId = Convert.ToInt32(dataReader["AreaId"]),
                                Row = Convert.ToInt32(dataReader["Row"]),
                                Number = Convert.ToInt32(dataReader["Number"]),
                            });
                        }

                        dataReader.Close();
                    }
                }
            }

            return seats.AsQueryable();
        }
    }
}
