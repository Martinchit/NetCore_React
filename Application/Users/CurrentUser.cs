using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interface;
using Application.ResponseObject;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Users
{
    public class CurrentUser
    {
        public class Query : IRequest<UserObject> { };

        public class Handler : IRequestHandler<Query, UserObject>
        {
            private readonly DataContext _dataContext;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
            }

            public async Task<UserObject> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                    return new UserObject
                    {
                        DisplayName = user.DisplayName,
                        Username = user.UserName,
                        Token = _jwtGenerator.CreateToken(user),
                        Image = null
                    };
                } catch (Exception ex)
                {
                    throw new RestException(
                            HttpStatusCode.Unauthorized,
                            new ErrorObject(HttpStatusCode.Unauthorized, "Error", ex.Message));
                }
                
            }
        }
    }
}
