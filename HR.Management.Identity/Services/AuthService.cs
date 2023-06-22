using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Exceptions;
using HR.Management.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HR.Management.Identity.Services;

public class AuthService : IAuthService

{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwsSettings _jwsSettings;

    public AuthService(UserManager<ApplicationUser> userManager, JwsSettings jwsSettings)
    {
        _userManager = userManager;
        _jwsSettings = jwsSettings;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new NotFoundException($"User with {request.Email} not found.", request.Email);
            
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            throw new BadRequestException($"Credentials for '{request.Email}' aren't valid.");
        }

        JwtSecurityToken leaveJwtSecurityToken = await GenerateToken(user);

        var response = new AuthResponse
        {
            Id = user.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(leaveJwtSecurityToken),
            Email = user.Email,
            UserName = user.UserName,
        };
        return response;
    }

    public async  Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            StringBuilder errors = new StringBuilder();
            foreach (var err in result.Errors)
            {
                errors.AppendFormat($"{err.Description}\n");
            }

            throw new BadRequestException($"{errors}");
        }

        await _userManager.AddToRoleAsync(user, "Employee");
        return new RegistrationResponse() { UserId = user.Id };
    }


    private async Task<JwtSecurityToken> GenerateToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

        var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }.Union(userClaims)
            .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwsSettings.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwsSettings.Issuser,
            audience: _jwsSettings.Audience,
            claims: claim,
            expires: DateTime.Now.AddMinutes(_jwsSettings.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken; 
    }
}