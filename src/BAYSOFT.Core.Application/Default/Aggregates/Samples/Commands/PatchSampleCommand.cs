using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Core.Domain.Exceptions;
using BAYSOFT.Abstractions.Crosscutting.Helpers;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Application.Default.Aggregates.Samples.Notifications;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Resources;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Services;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModelWrapper;
using ModelWrapper.Extensions.Patch;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Aggregates.Samples.Commands
{
	public class PatchSampleCommand : ApplicationRequest<Sample, PatchSampleCommandResponse>
    {
        public PatchSampleCommand()
        {
            ConfigKeys(x => x.Id);

            // Configures supressed properties & response properties
            //ConfigSuppressedProperties(x => x);
            //ConfigSuppressedResponseProperties(x => x);
        }
    }
    public class PatchSampleCommandResponse : ApplicationResponse<Sample>
    {
        public PatchSampleCommandResponse(Tuple<int, int, WrapRequest<Sample>, Dictionary<string, object>, Dictionary<string, object>, string, long?> tuple) : base(tuple)
        {
        }

        public PatchSampleCommandResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
            : base(request, data, message, resultCount)
        {
        }
    }

    [InheritStringLocalizer(typeof(Messages), Priority = 0)]
    [InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
    [InheritStringLocalizer(typeof(EntitiesSamples), Priority = 2)]
    public class PatchSampleCommandHandler : ApplicationRequestHandler<Sample, PatchSampleCommand, PatchSampleCommandResponse>
    {
        private ILoggerFactory Logger { get; set; }
        private IMediator Mediator { get; set; }
        private IStringLocalizer Localizer { get; set; }
        private IDefaultDbContextWriter Writer { get; set; }
        public PatchSampleCommandHandler(
            ILoggerFactory logger,
            IMediator mediator,
            IStringLocalizer<PatchSampleCommandHandler> localizer,
            IDefaultDbContextWriter writer
        )
        {
            Logger = logger;
            Mediator = mediator;
            Localizer = localizer;
            Writer = writer;
        }
        public override async Task<PatchSampleCommandResponse> Handle(PatchSampleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.IsValid(Localizer, true);

                long resultCount = 1;

                var id = request.Project(x => x.Id);

                var data = await Writer
                    .Query<Sample>()
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (data == null)
                {
                    throw new EntityNotFoundException<Sample>(Localizer);
                }

                request.Patch(data);

                await Mediator.Send(new UpdateSampleServiceRequest(data));

                await Writer.CommitAsync(cancellationToken);

                await Mediator.Publish(new PatchSampleNotification(data));

                return new PatchSampleCommandResponse(request, data, Localizer["Successful operation!"], resultCount);
            }
            catch (Exception exception)
            {
                Logger.CreateLogger<PatchSampleCommandHandler>().Log(LogLevel.Error, exception, exception.Message);

                return new PatchSampleCommandResponse(ExceptionResponseHelper.CreateTuple(Localizer, request, exception));
            }
        }
    }
}