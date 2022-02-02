using System;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace language_ext.kata.Account
{
    public class UserService
    {
        private static Seq<User> _repository =
            Seq(new User(Guid.Parse("376510ae-4e7e-11ea-b77f-2e728ce88125"),
                    "bud.spencer@gmail.com",
                    "Bud Spencer",
                    "OJljaefp0')"),
                new User(Guid.Parse("37651306-4e7e-11ea-b77f-2e728ce88125"),
                    "terrence.hill@gmail.com",
                    "Terrence Hill",
                    "àu__udsv09Ll"));

        public async Task<User> FindById(Guid id)
            => await Task.FromResult(_repository.Filter(p => id.Equals(p.Id)).Single());

        public async Task UpdateTwitterAccountId(Guid id, string twitterAccountId) => await Task.CompletedTask;
    }
}
