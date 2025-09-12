using BuildingBlocks.Core.Entities;
using BuildingBlocks.EventBus.Interfaces.User;
using BuildingBlocks.MassTransit.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Product.Infrastructure.Consumers.Events;

public class UserRegisteredEventConsumer : IConsumer<IUserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredEventConsumer> _logger;
    private readonly UserManager<User> _userManager;
    public UserRegisteredEventConsumer(ILogger<UserRegisteredEventConsumer> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task Consume(ConsumeContext<IUserRegisteredEvent> context)
    {

        var identityUser = new User
        {
            Id = context.Message.UserId,
            UserName = context.Message.UserName,
            Email = context.Message.Email,
            EmailConfirmed = true           
        };

        var userByUserName = await _userManager.FindByNameAsync(identityUser.UserName);
        var userByEmail = await _userManager.FindByEmailAsync(identityUser.Email);

        // User is already exists in database
        if (userByUserName != default || userByEmail != default)
        {
            return;
        }


        var result = await _userManager.CreateAsync(identityUser, context.Message.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User created.");
        }
        else
        {
            throw new Exception($"'{identityUser.Email}' - User could not be registered.");
        }
    }
}