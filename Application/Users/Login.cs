using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interface;
using Application.ResponseObject;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users
{
    public class Login
    {
        public class Query : IRequest<UserObject>
        {
            public string Email { get; set; }

            public string Password { get; set; }
        };

        public class QueryValidator: AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }
        

        public class Handler : IRequestHandler<Query, UserObject>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<UserObject> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    throw new RestException(
                        HttpStatusCode.Unauthorized,
                        new ErrorObject(HttpStatusCode.Unauthorized, "Login Fail", "Invalid Email or Password"));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (result.Succeeded)
                {
                    return new UserObject
                    {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Image = null
                    };
                }

                throw new RestException(
                       HttpStatusCode.Unauthorized,
                       new ErrorObject(HttpStatusCode.Unauthorized, "Login Fail", "Invalid Email or Password"));
            }
        }
    }
}
