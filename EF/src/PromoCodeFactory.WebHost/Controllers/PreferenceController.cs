using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferenceController
        : ControllerBase
    {
        private readonly IRepository<Preference> _repository;
        private readonly IMapper _mapper;

        public PreferenceController(IRepository<Preference> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все предпочтения
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PreferenceResponse>>> GetPreferences(CancellationToken ct)
        {
            var preferences = await _repository.GetAllAsync(ct);
            if (!preferences.ToList().Any()) { return NotFound(); }
            return Ok(preferences.Select(_mapper.Map<PreferenceResponse>));
        }
    }
}
