﻿using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Services;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Specifications;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Validations.DomainValidations;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Validations.EntityValidations;
using BAYSOFT.Infrastructures.Data.Default;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;

namespace BAYSOFT.Tests.UnitTests.Core.Domain.Default.Aggregates.Samples.Services
{
	[TestClass]
	public class UpdateSampleServiceScenarios
	{
		[TestMethod]
		public async Task UPDATE_Sample_Should_Not_Return_Exception()
		{
			var contextData = SamplesCollections.GetDefaultCollection();

			using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples(contextData))
			{
				var reader = new DefaultDbContextReader(context);
				var writer = new DefaultDbContextWriter(context);

				var localizer = GenericHelper.CreateLocalizer<UpdateSampleServiceRequestHandler>();

				var validator = new SampleValidator();
				var specification = new SampleDescriptionAlreadyExistsSpecification(reader);
				var specificationsValidator = new UpdateSampleSpecificationsValidator(specification);

				var handler = new UpdateSampleServiceRequestHandler(writer, localizer, validator, specificationsValidator);

				var sample = contextData.First();
				sample.Description += " [updated]";

				var request = new UpdateSampleServiceRequest(sample);

				var result = await handler.Handle(request, default);

				Assert.IsNotNull(result);
			}
		}
	}
}
