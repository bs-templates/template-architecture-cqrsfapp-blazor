﻿using BAYSOFT.Core.Application.Default.Aggregates.Samples.Notifications;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;
using MediatR;
using Moq;

namespace BAYSOFT.Tests.UnitTests.Core.Application.Default.Aggregates.Samples.Notifications
{
	[TestClass]
	public class PutSampleNotificationScenarios
	{
		[TestMethod]
		public async Task PostSampleNotification_Should_Not_Return_Exception()
		{
			var contextData = SamplesCollections.GetDefaultCollection();

			using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples(contextData))
			{
				var mockedLoggerFactory = GenericHelper.MockILoggerFactory<PutSampleNotificationHandler>();

				var mockedMediator = new Mock<IMediator>();

				var handler = new PutSampleNotificationHandler(
					mockedLoggerFactory.Object,
					mockedMediator.Object);

				var entity = contextData.First();

				var notification = new PutSampleNotification(entity);

				await handler.Handle(notification, default);
			}
		}
	}
}
