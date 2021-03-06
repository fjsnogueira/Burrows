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

using Burrows.Configuration;
using Burrows.Endpoints;
using System;
using System.Transactions;
using Burrows.Tests.Framework.Messages;
using Burrows.Transports;
using NUnit.Framework;

namespace Burrows.Tests.Framework.Transports
{
    public abstract class TransportContract<TTransportFactory>
		where TTransportFactory : class, ITransportFactory, new()
	{
		IEndpoint _endpoint;

		protected TransportContract(Uri uri)
		{
			Address = uri;
		}

		public Uri Address { get; set; }
		public Action<Uri> VerifyMessageIsNotInQueue { get; set; }


		[SetUp]
		public void SetUp()
		{
			IEndpointCache endpointCache = EndpointCacheFactory.New(x => x.AddTransportFactory<TTransportFactory>());

			_endpoint = endpointCache.GetEndpoint(Address);
		}

		[TearDown]
		public void TearDown()
		{
			_endpoint.Dispose();
			_endpoint = null;
		}


		[Test]
		public void While_writing_it_should_perisist_on_complete()
		{
			using (var trx = new TransactionScope())
			{
				_endpoint.Send(new DeleteMessage());
				trx.Complete();
			}

			VerifyMessageIsInQueue(_endpoint);
		}

		[Test]
		public void While_writing_it_should_perisist_even_on_rollback()
		{
			using (new TransactionScope())
			{
				_endpoint.Send(new DeleteMessage());
				//no complete
			}

			VerifyMessageIsInQueue(_endpoint);
		}

		//outside transaction
		[Test]
		public void While_writing_it_should_persist()
		{
			_endpoint.Send(new DeleteMessage());

			VerifyMessageIsInQueue(_endpoint);
		}


		public void VerifyMessageIsInQueue(IEndpoint ep)
		{
			ep.ShouldContain<DeleteMessage>();
		}
	}
}