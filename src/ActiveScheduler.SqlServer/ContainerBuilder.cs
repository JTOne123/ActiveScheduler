// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace ActiveScheduler.SqlServer
{
	public sealed class ContainerBuilder
	{
		public ContainerBuilder(IServiceCollection services)
		{
			Services = services;
		}

		public IServiceCollection Services { get; }
	}
}