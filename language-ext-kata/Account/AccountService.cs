using System;
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

        private Try<RegistrationContext> RetrieveUserDetails(Guid userId) =>
            Try(() => _userService.FindById(userId))
                .Map(user => user.ToContext());

        private Try<RegistrationContext> RegisterOnTwitter(RegistrationContext context) =>
            Try(() => _twitterService.Register(context.Email, context.Name))
                .Map(twitterAccountId => context with {AccountId = twitterAccountId});

        private Try<RegistrationContext> AuthenticateOnTwitter(RegistrationContext context) =>
            Try(() => _twitterService.Authenticate(context.Email, context.Password))
                .Map(token => context with {Token = token});

        private Try<RegistrationContext> Tweet(RegistrationContext context) =>
            Try(() => _twitterService.Tweet(context.Token, "Hello I am " + context.Name))
                .Map(tweetUrl => context with {Url = tweetUrl});

        private Try<RegistrationContext> UpdateUser(RegistrationContext context) =>
            Try(() =>
            {
                _userService.UpdateTwitterAccountId(context.Id, context.AccountId);
                return context;
            });

        public Option<string> Register(Guid id)
        {
            return RetrieveUserDetails(id)
                .Bind(RegisterOnTwitter)
                .Bind(AuthenticateOnTwitter)
                .Bind(Tweet)
                .Bind(UpdateUser)
                .Do(context => _businessLogger.LogSuccessRegister(context.Id))
                .Map(context => context.Url)
                .IfFail(failure =>
                {
                    _businessLogger.LogFailureRegister(id, failure);
                    return null;
                });
        }
    }
}
