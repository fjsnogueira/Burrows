﻿using System;
using Burrows.Tests.Messages;

namespace Burrows.Tests.SubscribeConsole.Consumers
{
    public class PublisherConsumer : Consumes<SimpleMessage>.All
    {
        public void Consume(SimpleMessage message)
        {
            Console.WriteLine("Just got a message");

           //throw new Exception("I failed!!!!!");
        }
    }
}