using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace language_ext.kata.Account
{
    public class UserService
    {
        private readonly Seq<User> repository =
            Seq(
                new User
                {
                    Id = new Guid("376510ae-4e7e-11ea-b77f-2e728ce88125"),
                    Email = "bud.spencer@gmail.com",
                    Name = "Bud Spencer",
                    Password = "OJljaefp0')",
                },
                new User
                {
                    Id = new Guid("37651306-4e7e-11ea-b77f-2e728ce88125"),
                    Email = "terrence.hill@gmail.com",
                    Name = "Terrence Hill",
                    Password = "àu__udsv09Ll",
                });

        public User FindById(Guid id) => repository.Filter(p => id.Equals(p.Id)).Single();

        public void UpdateTwitterAccountId(Guid id, string twitterAccountId)
        {

        }
    }
}