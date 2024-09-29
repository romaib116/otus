using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(IRepository<Customer> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение всех клиентов
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Коллекция</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync(CancellationToken ct)
        {
            var result = (await _customerRepository.GetAllAsync(ct)).Select(_mapper.Map<CustomerShortResponse>);
            return Ok(result);
        }

        /// <summary>
        /// Получить клиента по ИДу
        /// </summary>
        /// <param name="id">ИД</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Клиент если был найден</returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(id, ct);
            if (customer == null)
                return NotFound();
            else 
                return Ok(_mapper.Map<CustomerResponse>(customer));
        }

        /// <summary>
        /// Создать клиента
        /// </summary>
        /// <param name="request">Клиент</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>ИДн</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Guid>> CreateCustomerAsync(CreateOrEditCustomerRequest request, CancellationToken ct)
        {
            var entity = _mapper.Map<Customer>(request);
            return Ok(await _customerRepository.CreateAsync(entity, ct));
        }

        /// <summary>
        /// Изменить клиента по ИДу
        /// </summary>
        /// <param name="id">ИД</param>
        /// <param name="request">Клиент</param>
        /// <param name="ct">Токен отмены</param>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(id, ct);
            if (customer is null)
                return NotFound();
            else
            {
                var entity = _mapper.Map<Customer>(request);
                entity.Id = id;
                await _customerRepository.UpdateAsync(entity, ct);
                return Ok();
            }
        }

        /// <summary>
        /// Удалить клиента по ИДу
        /// </summary>
        /// <param name="id">ИД</param>
        /// <param name="ct">Токен отмены</param>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCustomer(Guid id, CancellationToken ct)
        {
            var customer = await _customerRepository.GetByIdAsync(id, ct);
            if (customer is null)
                return NotFound();
            else
            {
                await _customerRepository.DeleteAsync(id, ct);
                return Ok();
            }
        }
    }
}