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
namespace Burrows.Tests.Saga
{
    using System;
    using System.Threading;
    using Magnum.Extensions;
    using Messages;

    public class RegisterUserController :
		Consumes<UserRegistrationPending>.For<Guid>,
		Consumes<UserRegistrationComplete>.For<Guid>
	{
		readonly IServiceBus _bus;
		readonly ManualResetEvent _registrationComplete = new ManualResetEvent(false);
		readonly Guid _correlationId;
		readonly ManualResetEvent _registrationPending = new ManualResetEvent(false);

		public RegisterUserController(IServiceBus bus)
		{
			_bus = bus;
			_correlationId = Guid.NewGuid();
		}

		public void Consume(UserRegistrationComplete message)
		{
			_registrationComplete.Set();
		}

		public Guid CorrelationId
		{
			get { return _correlationId; }
		}

		public void Consume(UserRegistrationPending message)
		{
			_registrationPending.Set();
		}

		public bool RegisterUser(string username, string password, string displayName, string email)
		{
			var message = new RegisterUser(_correlationId, username, password, displayName, email);

		    _bus.Publish(message);

		    return _registrationPending.WaitOne(8.Seconds());
		}

		public bool ValidateUser()
		{
		    _bus.Publish(new UserValidated(CorrelationId));

		    return WaitOn(_registrationComplete, 8.Seconds(), "Timeout waiting for registration to complete");
		}

        bool WaitOn(WaitHandle handle, TimeSpan timeout, string message)
        {
            int result = WaitHandle.WaitAny(new[] { handle }, timeout, true);
            if (result == 0)
                return true;

            if (result == 1)
                return false;

            throw new ApplicationException(message);
        }
	}
}