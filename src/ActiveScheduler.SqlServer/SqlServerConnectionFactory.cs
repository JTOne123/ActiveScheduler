// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data;
using System.Data.SqlClient;
using ActiveScheduler.SqlServer.Internal.SessionManagement;

namespace ActiveScheduler.SqlServer
{
	public class SqlServerConnectionFactory : IConnectionFactory
	{
		public string ConnectionString { get; set; }
		public IDbConnection CreateConnection() => new SqlConnection(ConnectionString);
	}
}