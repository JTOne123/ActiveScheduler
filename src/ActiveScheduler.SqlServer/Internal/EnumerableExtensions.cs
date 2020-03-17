﻿// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ActiveScheduler.SqlServer.Internal
{
	internal static class EnumerableExtensions
	{
		public static SelfEnumerable<T> Enumerate<T>(this List<T> inner)
		{
			return new SelfEnumerable<T>(inner);
		}

		public static FuncEnumerable<T, TResult> Enumerate<T, TResult>(this List<T> inner, Func<T, TResult> func)
		{
			return new FuncEnumerable<T, TResult>(inner, func);
		}
	}
}