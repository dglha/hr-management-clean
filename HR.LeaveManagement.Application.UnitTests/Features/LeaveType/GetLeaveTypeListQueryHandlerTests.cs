using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTests.Features.LeaveType;

public class GetLeaveTypeListQueryHandlerTests
{
    private readonly Mock<ILeaveTypeRepository> _mock;
    private readonly IMapper _mapper;
    private Mock<IAppLogger<GetLeaveTypesQueryHandler>> _logger;
    
    public GetLeaveTypeListQueryHandlerTests()
    {
        _mock = MockLeaveTypeRepository.GetLeaveType();
        var mapperConfig = new MapperConfiguration(config =>
        {
            config.AddProfile<LeaveTypeProfile>();
        });

        _mapper = mapperConfig.CreateMapper();
        _logger = new Mock<IAppLogger<GetLeaveTypesQueryHandler>>();
    }

    [Fact]
    public async Task GetLeaveTypeListTest()
    {
        var handler = new GetLeaveTypesQueryHandler(_mapper, _mock.Object);
        var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
        
        result.ShouldBeOfType<List<LeaveTypeDTO>>();
        result.Count().ShouldBe(3);
    }
}