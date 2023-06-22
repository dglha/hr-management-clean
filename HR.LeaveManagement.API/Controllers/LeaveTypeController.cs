using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetLeaveTypeDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        // GET: api/LeaveType
        public LeaveTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<LeaveTypeDTO>> Get()
        {
            var leaveTypes = await _mediator.Send(new GetLeaveTypesQuery());

            return leaveTypes;
        }

        // GET: api/LeaveType/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<LeaveTypeDetailDTO>> Get(int id)
        {
            var leaveType = await _mediator.Send(new GetLeaveTypesDetailsQuery(id));
            return Ok(leaveType);
        }

        // POST: api/LeaveType
        [HttpPost]
        // TODO: A filter that specifies the type of the value and status code returned by the action.
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(CreateLeaveTypeCommand leaveType)
        {
            var response = await _mediator.Send(leaveType);
            return CreatedAtAction(nameof(Get), new { id = response });
        }

        // PUT: api/LeaveType/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put(UpdateLeaveTypeCommand leaveType)
        {
            await _mediator.Send(leaveType);

            return NoContent();
        }

        // DELETE: api/LeaveType/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            var leaveDeleteCommand = new DeleteLeaveTypeCommand { Id = id };
            await _mediator.Send(leaveDeleteCommand);
            return NoContent();
        }
    }
}
