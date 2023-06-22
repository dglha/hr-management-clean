using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR.LeaveManagement.Application.Contracts.Logging;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<DeleteLeaveTypeCommandHandler> _logger;

        public DeleteLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository, IAppLogger<DeleteLeaveTypeCommandHandler> logger)
        {
            this._leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveTypeToDelete = await _leaveTypeRepository.GetAsync(request.Id);
            // TODO: Why nameof (Where leave type define (application or domain))
            if (leaveTypeToDelete == null)
            {
                _logger.LogWarning("Errors in delete request for {0} - {1}", nameof(LeaveType), request.Id);
                throw new NotFoundException(nameof(LeaveType), request.Id);
            }

            await _leaveTypeRepository.DeleteAsync(leaveTypeToDelete);

            return Unit.Value;
        }
    }
}
