using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Core.Domain.Exceptions;
using BAYSOFT.Abstractions.Crosscutting.Helpers;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Resources;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModelWrapper;
using ModelWrapper.Extensions.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Aggregates.Samples.Queries
{
    public class GetSampleByIdQuery : ApplicationRequest<Sample, GetSampleByIdQueryResponse>
    {
        public GetSampleByIdQuery()
        {
            ConfigKeys(x => x.Id);

            // Configures supressed properties & response properties
            //ConfigSuppressedProperties(x => x);
            //ConfigSuppressedResponseProperties(x => x);  
        }
    }
    public class GetSampleByIdQueryResponse : ApplicationResponse<Sample>
    {
        public GetSampleByIdQueryResponse(Tuple<int, int, WrapRequest<Sample>, Dictionary<string, object>, Dictionary<string, object>, string, long?> tuple) : base(tuple)
        {
        }

        public GetSampleByIdQueryResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    [InheritStringLocalizer(typeof(EntitiesSamples), Priority = 2)]
    public class GetSampleByIdQueryHandler : ApplicationRequestHandler<Sample, GetSampleByIdQuery, GetSampleByIdQueryResponse>
    {
        private ILoggerFactory Logger { get; set; }
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextReader Reader { get; set; }
        public GetSampleByIdQueryHandler(
            ILoggerFactory logger,
            IMediator mediator,
            IStringLocalizer<GetSampleByIdQueryHandler> localizer,
            IDefaultDbContextReader reader
        )
        {
            Logger = logger;
            Mediator = mediator;
            Localizer = localizer;
            Reader = reader;
        }
        public override async Task<GetSampleByIdQueryResponse> Handle(GetSampleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                long resultCount = 1;

                var id = request.Project(x => x.Id);

                var data = await Reader
                    .Query<Sample>()
                    .Where(x => x.Id == id)
                    .Select(request)
                    .SingleOrDefaultAsync();

                if (data == null)
                {
                    throw new EntityNotFoundException<Sample>(Localizer);
                }

                return new GetSampleByIdQueryResponse(request, data, Localizer["Successful operation!"], resultCount);
            }
            catch (Exception exception)
            {
                Logger.CreateLogger<GetSampleByIdQueryHandler>().Log(LogLevel.Error, exception, exception.Message);

                return new GetSampleByIdQueryResponse(ExceptionResponseHelper.CreateTuple(Localizer, request, exception));
            }
        }
    }
}
