using System;

namespace language_ext.kata.Account;

public record RegistrationContext(
    Guid Id,
    string Email,
    string Name,
    string Password,
    string AccountId = "",
    string Token = "",
    string Url = ""
);

public static class RegistrationExtensions
{
    public static RegistrationContext ToContext(this User user)
        => new(user.Id, user.Email, user.Name, user.Password);
}
