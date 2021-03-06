// Copyright 2007-2012 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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

using System;
using Burrows.Configuration;
using Burrows.Configuration.BusConfigurators;
using Magnum.TestFramework;
using NUnit.Framework;
using Burrows.Transports.Configuration.Configurators;
using Burrows.Transports.Configuration.Extensions;

namespace Burrows.Tests.RabbitMq
{
    [Scenario]
    public abstract class Given_two_rabbitmq_buses
    {
        protected Uri LocalUri { get; set; }
        protected Uri RemoteUri { get; set; }

        protected IServiceBus LocalBus { get; private set; }
        protected IServiceBus RemoteBus { get; private set; }
        protected Uri LocalErrorUri { get; private set; }

        [TestFixtureSetUp]
        public void LocalAndRemoteTestFixtureSetup()
        {
            LocalUri = new Uri("rabbitmq://localhost:5672/test_queue");
            LocalErrorUri = new Uri("rabbitmq://localhost:5672/test_queue_error");

            RemoteUri = new Uri("rabbitmq://localhost:5672/test_remote_queue");

            LocalBus = SetupServiceBus(LocalUri, ConfigureLocalBus);
            RemoteBus = SetupServiceBus(RemoteUri, ConfigureRemoteBus);
        }

        [TestFixtureTearDown]
        public void LocalAndRemoteTestFixtureTeardown()
        {
            LocalBus.Dispose();
            LocalBus = null;

            RemoteBus.Dispose();
            RemoteBus = null;
        }

        protected virtual void ConfigureLocalBus(IServiceBusConfigurator configurator)
        {
        }

        protected virtual void ConfigureRemoteBus(IServiceBusConfigurator configurator)
        {
        }

        protected virtual void ConfigureRabbitMq(ITransportFactoryConfigurator configurator)
        {
        }

        protected IServiceBus SetupServiceBus(Uri uri, Action<IServiceBusConfigurator> configure)
        {
            IServiceBus bus = ServiceBusFactory.New(x =>
                {
                    x.ReceiveFrom(uri);
                    x.UseRabbitMq(ConfigureRabbitMq);

                    configure(x);
                });

            return bus;
        }
    }
}