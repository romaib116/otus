using MassTransit;
using Pcf.Contracts;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using System;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class AdministrationGateway
        : IAdministrationGateway
    {
        private readonly IBusControl _busControl;

        public AdministrationGateway(
            IBusControl busControl
            )
        {
            _busControl = busControl;
        }

        public async Task NotifyAdminAboutPartnerManagerPromoCode(Guid partnerManagerId)
        {
            await _busControl.Publish(new MessageDto { Uid = partnerManagerId});
        }
    }
}