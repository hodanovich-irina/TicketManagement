using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueAPI.Automapper;
using TicketManagement.VenueAPI.Dto;
using TicketManagement.VenueAPI.Interfaces;

namespace TicketManagement.VenueAPI.Services
{
    /// <summary>
    /// Providing methods for managing area with validation and business logic.
    /// </summary>
    internal class AreaService : AutoMapperService, IService<AreaDto>
    {
        private readonly IRepository<Area> _areaRepository;
        private readonly IEFRepository<Area> _areaEFRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IEFRepository<Seat> _seatEFRepository;
        private readonly IValidator<AreaDto> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaService"/> class.
        /// Constructor with params.
        /// </summary>
        /// <param name="areaRepository">Object of area repository.</param>
        /// <param name="areaEFRepository">Object of area ef repository.</param>
        /// <param name="seatRepository">Object of seat repository.</param>
        /// <param name="seatEFRepository">Object of seat ef repository.</param>
        /// <param name="validator">Object of area validator.</param>
        public AreaService(IRepository<Area> areaRepository, IEFRepository<Area> areaEFRepository, IRepository<Seat> seatRepository,
            IEFRepository<Seat> seatEFRepository, IValidator<AreaDto> validator)
        {
            _areaRepository = areaRepository;
            _areaEFRepository = areaEFRepository;
            _seatRepository = seatRepository;
            _seatEFRepository = seatEFRepository;
            _validator = validator;
        }

        /// <summary>
        /// Logic for add area.
        /// </summary>
        /// <param name="entity">Object of class area.</param>
        public async Task<AreaDto> AddAsync(AreaDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            var allLayoutAreas = await _areaEFRepository.GetAsync(area => area.LayoutId.Equals(entity.LayoutId));
            var isDescriptionExists = allLayoutAreas.Any(areaDescription => areaDescription.Description.Equals(entity.Description));
            if (isDescriptionExists)
            {
                throw new InvalidOperationException("You can't add a new area. This area description alredy exist in this layout");
            }

            var area = await _areaRepository.AddAsync(Mapper.Map<Area>(entity));
            var areaDto = Mapper.Map<AreaDto>(area);

            return areaDto;
        }

        /// <summary>
        /// Logic for edit area.
        /// </summary>
        /// <param name="entity">Object of class area.</param>
        public async Task<bool> EditAsync(AreaDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            _validator.ValidateId(entity.Id);

            var allLayoutAreas = await _areaEFRepository.GetAsync(area => area.LayoutId.Equals(entity.LayoutId));
            var isDescriptionAndIdExists = allLayoutAreas.Any(areaDescription => areaDescription.Description.Equals(entity.Description) && areaDescription.Id.Equals(entity.Id));
            var isDescriptionExists = allLayoutAreas.Any(areaDescription => areaDescription.Description.Equals(entity.Description));
            if (isDescriptionAndIdExists || !isDescriptionExists)
            {
                return await _areaRepository.EditAsync(Mapper.Map<Area>(entity));
            }
            else
            {
                throw new InvalidOperationException("You can't edit this area. This area description alredy exist in this layout");
            }
        }

        /// <summary>
        /// Logic for delete area.
        /// </summary>
        /// <param name="id">Id of area object.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            _validator.ValidateId(id);
            var areaForDelete = await _areaRepository.GetByIdAsync(id);
            if (areaForDelete is null)
            {
                throw new InvalidOperationException("Incorrect data");
            }

            await DeleteSeatsInAreaAsync(id);
            return await _areaRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Logic for get area by id.
        /// </summary>
        /// <param name="id">Id of area object.</param>
        public async Task<AreaDto> GetByIdAsync(int id)
        {
            _validator.ValidateId(id);
            return Mapper.Map<AreaDto>(await _areaRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Logic for get all area.
        /// </summary>
        /// <param name="id">Layout Id of area object.</param>
        /// <returns>Collection of area object.</returns>
        public async Task<IEnumerable<AreaDto>> GetAsync(int id)
        {
            _validator.ValidateId(id);
            var areas = await _areaEFRepository.GetAsync(area => area.LayoutId.Equals(id));
            return areas.Select(area => Mapper.Map<AreaDto>(area)).AsEnumerable();
        }

        private async Task DeleteSeatsInAreaAsync(int areaId)
        {
            var seats = await _seatEFRepository.GetAsync(seat => seat.AreaId.Equals(areaId));
            foreach (var seat in seats.ToList())
            {
                await _seatRepository.DeleteAsync(seat.Id);
            }
        }

        public async Task<IEnumerable<AreaDto>> GetAllAsync()
        {
            var areas = await _areaEFRepository.GetAllAsync();
            return areas.Select(area => Mapper.Map<AreaDto>(area)).AsEnumerable();
        }
    }
}
