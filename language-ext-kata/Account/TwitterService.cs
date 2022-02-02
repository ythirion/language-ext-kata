using System.Threading.Tasks;

namespace language_ext.kata.Account
{
    public class TwitterService
    {
        public async Task<string> Register(string email, string name) => await Task.FromResult("TwitterAccountId");
        public async Task<string> Authenticate(string email, string password) => await Task.FromResult("ATwitterToken");
        public async Task<string> Tweet(string token, string message) => await Task.FromResult("TweetUrl");
    }
}
