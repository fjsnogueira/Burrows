﻿using System.Reflection;
using Autofac;
using Burrows.Autofac;
using Burrows.Configuration;
using Burrows.Log4Net;
using Burrows.Publishing;
using Magnum.Extensions;
using Module = Autofac.Module;

namespace Burrows.Tests.PublishSubscribeConsole
{
    public class AutofacModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(t => t.Implements<IConsumer>())
                   .AsSelf();

            //builder.Register(c => ServiceBusFactory.New(sbc => sbc.Configure(@"rabbitmq://localhost/SubscribeConsole",
            //    subs => subs.LoadFrom(c.Resolve<ILifetimeScope>())))).SingleInstance();

            builder.Register(c => new Publisher(sbc => sbc.Configure(@"rabbitmq://localhost/PublishSubscribeConsole", subs => subs.LoadFrom(c.Resolve<ILifetimeScope>())).UseLog4Net(), 
                ps => ps.UsePublisherConfirms("PublishSubscribeConsole").WithFileBackingStore())).As<IPublisher>().SingleInstance();


            //This is the same as:
            //builder.Register(c => ServiceBusFactory.New(sbc =>
            //{
            //    sbc.ReceiveFrom(@"rabbitmq://localhost/SubscribeConsole");
            //    sbc.UseRabbitMq();
            //    sbc.UseControlBus();
            //    sbc.Subscribe(subs => subs.LoadFrom(c.Resolve<ILifetimeScope>()));
            //})).SingleInstance();

        }
    }
}