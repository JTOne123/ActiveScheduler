// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ActiveLogging;
using ActiveOptions;
using ActiveScheduler.Configuration;
using ActiveScheduler.Models;
using ActiveScheduler.SqlServer.Internal.DependencyInjection;
using ActiveScheduler.SqlServer.Internal.SessionManagement;
using ActiveScheduler.SqlServer.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace ActiveScheduler.SqlServer
{
	public static class Add
	{
		public static BackgroundTaskBuilder AddSqlServerBackgroundTasksStore(this BackgroundTaskBuilder builder,
			string connectionString, ConnectionScope scope = ConnectionScope.ByThread,
			IConfiguration databaseConfig = null)
		{
			return builder.AddSqlServerBackgroundTasksStore(connectionString, scope, databaseConfig.FastBind);
		}

		public static BackgroundTaskBuilder AddSqlServerBackgroundTasksStore(this BackgroundTaskBuilder builder,
			string connectionString, ConnectionScope scope = ConnectionScope.ByThread,
			Action<SqlServerOptions> configureDatabase = null)
		{
			if (scope == ConnectionScope.ByRequest)
				builder.Services.AddHttpContextAccessor();

			builder.Services.AddSafeLogging();

			var extensions = new[] {new HttpAccessorExtension()};
			builder.Services.AddDatabaseConnection<BackgroundTaskBuilder, SqlServerConnectionFactory>(connectionString,
				scope, extensions);
			builder.Services.Replace(ServiceDescriptor.Singleton<IBackgroundTaskStore, SqlServerBackgroundTaskStore>());

			builder.Services.Configure(configureDatabase);

			var serviceProvider = builder.Services.BuildServiceProvider();
			var options = serviceProvider.GetRequiredService<IOptions<BackgroundTaskOptions>>();
			MigrateToLatest(connectionString, options.Value);

			return builder;
		}

		private static void MigrateToLatest(string connectionString, BackgroundTaskOptions options)
		{
			var runner = new SqlServerMigrationRunner(connectionString);

			if (options.Store.CreateIfNotExists)
			{
				runner.CreateDatabaseIfNotExists();
			}

			if (options.Store.MigrateOnStartup)
			{
				runner.MigrateUp(typeof(CreateBackgroundTasksSchema).Assembly,
					typeof(CreateBackgroundTasksSchema).Namespace);
			}
		}
	}
}