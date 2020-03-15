using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class EditActivity
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime? Date { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty().NotNull();
                RuleFor(x => x.Description).NotEmpty().NotNull().MaximumLength(60).MinimumLength(5);
                RuleFor(x => x.Category).NotEmpty().NotNull().WithMessage("Category cannot be null or empty.").IsInEnum().WithMessage("Category value is invalid.");
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
                // handler logic                

                var activity = await _dataContext.Activities.FindAsync(request.Id);

                if (activity == null)
                {
                    throw new Exception("Could not found activity");
                }
                activity.Title = request.Title ?? activity.Title;
                activity.Description = request.Description ?? activity.Description;
                activity.Category = request.Category ?? activity.Category;
                activity.Date = request.Date ?? activity.Date;
                activity.City = request.City ?? activity.City;
                activity.Venue = request.Venue ?? activity.Venue;

                var success = await _dataContext.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }   
    }
}