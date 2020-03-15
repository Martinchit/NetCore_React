using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;
using Application.Errors;

namespace Application.Activities
{
    public class DeleteActivity
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                    throw new RestException(HttpStatusCode.NotFound, new ErrorObject(HttpStatusCode.NotFound, "Invalid Activity ID", "Failed to delete as activity not found"));
                }

                _dataContext.Remove(activity);

                var success = await _dataContext.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem saving changes");
            }
        }
    }
}
