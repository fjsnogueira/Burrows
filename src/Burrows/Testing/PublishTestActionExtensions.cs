﻿// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace Burrows.Testing
{
    using System;
    using ActionConfigurators;
    using Context;
    using TestInstanceConfigurators;

    public static class PublishTestActionExtensions
	{
		public static void Publish<TMessage>(this ITestInstanceConfigurator<IBusTestScenario> configurator, TMessage message)
			where TMessage : class
		{
			var actionConfigurator = new PublishTestActionConfigurator<IBusTestScenario, TMessage>(x => x.Bus, message);

			configurator.AddActionConfigurator(actionConfigurator);
		}

		public static void Publish<TMessage>(this ITestInstanceConfigurator<IBusTestScenario> configurator, TMessage message,
		                                     Action<IPublishContext<TMessage>> callback)
			where TMessage : class
		{
			var actionConfigurator = new PublishTestActionConfigurator<IBusTestScenario, TMessage>(x => x.Bus, message,
				(scenario, context) => callback(context));

			configurator.AddActionConfigurator(actionConfigurator);
		}

		public static void Publish<TMessage>(this ITestInstanceConfigurator<IBusTestScenario> configurator, TMessage message,
		                                     Action<IBusTestScenario, IPublishContext<TMessage>> callback)
			where TMessage : class
		{
			var actionConfigurator = new PublishTestActionConfigurator<IBusTestScenario, TMessage>(x => x.Bus, message,
				callback);

			configurator.AddActionConfigurator(actionConfigurator);
		}

		public static void Publish<TMessage>(this ITestInstanceConfigurator<ILocalRemoteTestScenario> configurator,
		                                     TMessage message)
			where TMessage : class
		{
			var actionConfigurator = new PublishTestActionConfigurator<ILocalRemoteTestScenario, TMessage>(x => x.LocalBus,
				message);

			configurator.AddActionConfigurator(actionConfigurator);
		}

		public static void Publish<TMessage>(this ITestInstanceConfigurator<ILocalRemoteTestScenario> configurator,
		                                     TMessage message,
		                                     Action<IPublishContext<TMessage>> callback)
			where TMessage : class
		{
			var actionConfigurator = new PublishTestActionConfigurator<ILocalRemoteTestScenario, TMessage>(x => x.LocalBus,
				message,
				(scenario, context) => callback(context));

			configurator.AddActionConfigurator(actionConfigurator);
		}

		public static void Publish<TMessage>(this ITestInstanceConfigurator<ILocalRemoteTestScenario> configurator,
		                                     TMessage message,
		                                     Action<ILocalRemoteTestScenario, IPublishContext<TMessage>> callback)
			where TMessage : class
		{
			var actionConfigurator = new PublishTestActionConfigurator<ILocalRemoteTestScenario, TMessage>(x => x.LocalBus,
				message,
				callback);

			configurator.AddActionConfigurator(actionConfigurator);
		}
	}
}