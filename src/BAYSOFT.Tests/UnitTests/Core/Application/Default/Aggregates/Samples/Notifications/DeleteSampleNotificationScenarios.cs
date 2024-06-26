﻿using BAYSOFT.Core.Application.Default.Aggregates.Samples.Notifications;
using BAYSOFT.Tests.Helpers;
using BAYSOFT.Tests.Helpers.Data.Default;
using BAYSOFT.Tests.Helpers.Data.Default.Samples;
using MediatR;
using Moq;

namespace BAYSOFT.Tests.UnitTests.Core.Application.Default.Aggregates.Samples.Notifications
{
	[TestClass]
	public class DeleteSampleNotificationScenarios
	{
		[TestMethod]
		public async Task DeleteSampleNotification_Should_Not_Return_Exception()
		{
			var contextData = SamplesCollections.GetDefaultCollection();

			using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples(contextData))
			{
				var mockedLoggerFactory = GenericHelper.MockILoggerFactory<DeleteSampleNotificationHandler>();

				var mockedMediator = new Mock<IMediator>();

				var handler = new DeleteSampleNotificationHandler(
					mockedLoggerFactory.Object,
					mockedMediator.Object);

				var entity = contextData.First();

				var notification = new DeleteSampleNotification(entity);

				await handler.Handle(notification, default);
			}
		}
	}
}
