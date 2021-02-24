using MediatorPattern.Events;
using MediatorPattern.IServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorPattern.Handlers
{
    public class SendMessageHandler : INotificationHandler<AddCustomerEvent>
    {
        private readonly IMessageService messageService;

        public SendMessageHandler(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public Task Handle(AddCustomerEvent notification, CancellationToken cancellationToken)
        {
            messageService.Send(notification.Customer.Email, "Hello World!");

            return Task.CompletedTask;
        }
    }
}
