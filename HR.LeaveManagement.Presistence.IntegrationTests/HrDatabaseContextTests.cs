using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace HR.LeaveManagement.Presistence.UnitTests;

public class HrDatabaseContextTests
{
    private HRDatabaseContext _dbContext;

    public HrDatabaseContextTests()
    {
        var dbOptions = new DbContextOptionsBuilder<HRDatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        // _dbContext = new HRDatabaseContext(dbOptions.);
    }

    [Fact]
    public async void Test_Save_SetCreatedAtValue()
    {
        var leaveType = new LeaveType
        {
            Id = 1,
            DefaultDays = 10,
            Name = "Test ne` hehe"
        };

        await _dbContext.Set<LeaveType>().AddAsync(leaveType);
        await _dbContext.SaveChangesAsync();

        leaveType.CreatedAt.ShouldNotBeNull();
    }
    
    [Fact]
    public async void Test_Save_SetUpdatedAtValue()
    {
        var leaveType = new LeaveType
        {
            Id = 1,
            DefaultDays = 10,
            Name = "Test ne` hehe"
        };

        await _dbContext.Set<LeaveType>().AddAsync(leaveType);
        await _dbContext.SaveChangesAsync();

        leaveType.UpdatedAt.ShouldNotBeNull();
    }
}