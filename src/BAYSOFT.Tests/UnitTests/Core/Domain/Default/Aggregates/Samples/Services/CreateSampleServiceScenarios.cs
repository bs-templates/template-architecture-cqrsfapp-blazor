﻿using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Services;
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
	public class CreateSampleServiceScenarios
	{
		[TestMethod]
		public async Task CREATE_Sample_Should_Not_Return_Exception()
		{
			var contextData = SamplesCollections.GetDefaultCollection();

			using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples(contextData))
			{
				var reader = new DefaultDbContextReader(context);
				var writer = new DefaultDbContextWriter(context);

				var localizer = GenericHelper.CreateLocalizer<CreateSampleServiceRequestHandler>();

				var validator = new SampleValidator();
				var specification = new SampleDescriptionAlreadyExistsSpecification(reader);
				var specificationsValidator = new CreateSampleSpecificationsValidator(specification);

				var handler = new CreateSampleServiceRequestHandler(writer, localizer, validator, specificationsValidator);

				var sample = new Sample { Description = "Sample - 001 [new]" };

				var request = new CreateSampleServiceRequest(sample);

				var result = await handler.Handle(request, default);

				Assert.IsNotNull(result);
			}
		}
	}
}
