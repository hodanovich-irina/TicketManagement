using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Automapper;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Providing methods for managing seat with validation and business logic.
    /// </summary>
    internal class SeatService : AutoMapperService, IService<SeatDto>
    {
        private readonly IEFRepository<Seat> _seatEFRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IValidator<SeatDto> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatService"/> class.
        /// </summary>
        /// <param name="seatEFRepository">Object of seat ef repository.</param>
        /// <param name="seatRepository">Object of seat repository.</param>
        /// <param name="validator">Object of seat validator.</param>
        public SeatService(IEFRepository<Seat> seatEFRepository, IRepository<Seat> seatRepository, IValidator<SeatDto> validator)
        {
            _seatEFRepository = seatEFRepository;
            _seatRepository = seatRepository;
            _validator = validator;
        }

        /// <summary>
        /// Logic for add seat.
        /// </summary>
        /// <param name="entity">Object of class seat.</param>
        public async Task<SeatDto> AddAsync(SeatDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            var allAreaSeats = await _seatEFRepository.GetAsync(seat => seat.AreaId.Equals(entity.AreaId));
            var isRowAndNumExists = allAreaSeats.Any(seatRowAndNum => seatRowAndNum.Number.Equals(entity.Number) && allAreaSeats.Any(seatRowAndNum => seatRowAndNum.Row.Equals(entity.Row)));
            if (isRowAndNumExists)
            {
                throw new InvalidOperationException("You can't add a new seat. Seat with this row and number alredy exsist in this area");
            }

            var seat = await _seatRepository.AddAsync(Mapper.Map<Seat>(entity));
            return Mapper.Map<SeatDto>(seat);
        }

        /// <summary>
        /// Logic for edit seat.
        /// </summary>
        /// <param name="entity">Object of class seat.</param>
        public async Task<bool> EditAsync(SeatDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            _validator.ValidateId(entity.Id);
            var allAreaSeats = await _seatEFRepository.GetAsync(seat => seat.AreaId.Equals(entity.AreaId));
            var isRowAndNumAndIdExists = allAreaSeats.Any(seatRowAndNum => seatRowAndNum.Row.Equals(entity.Row) && seatRowAndNum.Number.Equals(entity.Number) && seatRowAndNum.Id.Equals(entity.Id));
            var isRowAndNumExists = allAreaSeats.Any(seatRowAndNum => seatRowAndNum.Row.Equals(entity.Row) && seatRowAndNum.Number.Equals(entity.Number));
            if (isRowAndNumAndIdExists || !isRowAndNumExists)
            {
                return await _seatRepository.EditAsync(Mapper.Map<Seat>(entity));
            }
            else
            {
                throw new InvalidOperationException("You can't edit this seat. Seat with this row and number alredy exist in this area");
            }
        }

        /// <summary>
        /// Logic for delete seat.
        /// </summary>
        /// <param name="id">Id of seat object.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            _validator.ValidateId(id);
            var seat = await _seatRepository.GetByIdAsync(id);
            if (seat is null)
            {
                throw new InvalidOperationException("You can't delete seat. Incorrect data");
            }

            return await _seatRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Logic for get seat by id.
        /// </summary>
        /// <param name="id">Id of seat object.</param>
        public async Task<SeatDto> GetByIdAsync(int id)
        {
            _validator.ValidateId(id);
            return Mapper.Map<SeatDto>(await _seatRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Logic for get all seat.
        /// </summary>
        /// <param name="id">Area Id of seat object.</param>
        /// <returns>Collection of seat object.</returns>
        public async Task<IEnumerable<SeatDto>> GetAsync(int id)
        {
            _validator.ValidateId(id);
            var seats = await _seatEFRepository.GetAsync(seat => seat.AreaId.Equals(id));
            return seats.Select(seat => Mapper.Map<SeatDto>(seat)).AsEnumerable();
        }

        /// <summary>
        /// Logic for get all seat.
        /// </summary>
        /// <returns>Collection of seat.</returns>
        public async Task<IEnumerable<SeatDto>> GetAllAsync()
        {
            var seats = await _seatEFRepository.GetAllAsync();
            return seats.Select(seat => Mapper.Map<SeatDto>(seat)).AsEnumerable();
        }
    }
}
