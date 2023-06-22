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

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<CreateLeaveTypeCommandHandler> _logger;

        public CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository, IAppLogger<CreateLeaveTypeCommandHandler> logger)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
        }
        public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveValidator = new CreateLeaveTypeCommandValidator(_leaveTypeRepository);
            var leaveValidationResult = await leaveValidator.ValidateAsync(request, cancellationToken);

            if (leaveValidationResult.Errors.Any())
            {
                _logger.LogWarning("Errors in create request for {0}", nameof(LeaveType));
                throw new BadRequestException("Invalid LeaveType", leaveValidationResult);
            }
            var leaveTypeToCreate = _mapper.Map<Domain.LeaveType>(request);

            await _leaveTypeRepository.CreateAsync(leaveTypeToCreate);

            return leaveTypeToCreate.Id; 
        }
    }
}
