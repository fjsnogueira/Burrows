// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Burrows.Transports
{
    using System;
    using System.Runtime.Serialization;
    using Exceptions;

    [Serializable]
	public class InvalidConnectionException : TransportException
	{
		public InvalidConnectionException()
		{
		}

		public InvalidConnectionException(Uri uri)
			: base(uri)
		{
		}

		public InvalidConnectionException(Uri uri, string message)
			: base(uri, message)
		{
		}

		public InvalidConnectionException(Uri uri, Exception innerException)
			: base(uri, "The connection was invalid", innerException)
		{
		}

		public InvalidConnectionException(Uri uri, string message, Exception innerException)
			: base(uri, message, innerException)
		{
		}

		protected InvalidConnectionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}