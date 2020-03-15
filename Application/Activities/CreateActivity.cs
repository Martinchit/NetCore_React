using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class CreateActivity
    {
        public class Command : IRequest
        {
            [Required(ErrorMessage = "Activity id should not be null or empty")]
            public Guid Id { get; set; }

            [Required(ErrorMessage = "Activity title should not be null or empty")]
            public string Title { get; set; }

            [Required(ErrorMessage = "Activity description should not be null or empty")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Activity category should not be null or empty")]
            public string Category { get; set; }

            [Required(ErrorMessage = "Activity date should not be null or empty")]
            public DateTime Date { get; set; }

            [Required(ErrorMessage = "Activity city should not be null or empty")]
            public string City { get; set; }

            [Required(ErrorMessage = "Activity venue should not be null or empty")]
            public string Venue { get; set; }
        };

        public class CommandValidator: AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty().NotNull();
                RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(60).MinimumLength(5);
                RuleFor(x => x.Category).NotEmpty().NotNull().WithMessage("Category must not be Null or Empty");
                RuleFor(x => x.Date).NotEmpty();
                RuleFor(x => x.City).NotEmpty().NotNull();
                RuleFor(x => x.Venue).NotEmpty().NotNull();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = new Activity
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    Category = request.Category,
                    Date = request.Date,
                    City = request.City,
                    Venue = request.Venue
                };

                _dataContext.Activities.Add(activity);
                var success = await _dataContext.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
