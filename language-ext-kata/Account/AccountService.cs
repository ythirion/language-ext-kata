using System;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace language_ext.kata.Account
{
    public class AccountService
    {
        private readonly IBusinessLogger _businessLogger;
        private readonly TwitterService _twitterService;
        private readonly UserService _userService;

        public AccountService(
            UserService userService,
            TwitterService twitterService,
            IBusinessLogger businessLogger)
        {
            _userService = userService;
            _twitterService = twitterService;
            _businessLogger = businessLogger;
        }

        private TryAsync<RegistrationContext> CreateContext(Guid userId)
            => TryAsync(() => _userService.FindById(userId))
                .Map(user => user.ToContext());

        private TryAsync<RegistrationContext> RegisterOnTwitter(RegistrationContext context)
            => TryAsync(() => _twitterService.Register(context.Email, context.Name))
                .Map(twitterAccountId => context with {AccountId = twitterAccountId});

        private TryAsync<RegistrationContext> AuthenticateOnTwitter(RegistrationContext context)
            => TryAsync(() => _twitterService.Authenticate(context.Email, context.Password))
                .Map(token => context with {Token = token});

        private TryAsync<RegistrationContext> Tweet(RegistrationContext context)
            => TryAsync(() => _twitterService.Tweet(context.Token, "Hello I am " + context.Name))
                .Map(tweetUrl => context with {Url = tweetUrl});

        private TryAsync<RegistrationContext> UpdateUser(RegistrationContext context) =>
            (async () =>
            {
                await _userService.UpdateTwitterAccountId(context.Id, context.AccountId);
                return context;
            });

        public async Task<Option<string>> Register(Guid id)
        {
            return await CreateContext(id)
                .Bind(RegisterOnTwitter)
                .Bind(AuthenticateOnTwitter)
                .Bind(Tweet)
                .Bind(UpdateUser)
                .Do(context => _businessLogger.LogSuccessRegister(context.Id))
                .Map(context => context.Url)
                .IfFail(failure =>
                {
                    _businessLogger.LogFailureRegister(id, failure);
                    return (string) null;
                });
        }
    }
}
