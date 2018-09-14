using System;
using MassTransit;
using MessageTransitHub.Config;

namespace MessageTransitHub
{
    public class Initializer
    {
	    private readonly BrokerConfig _brokerConfig;
	    private IBusControl _busControl;

	    public Initializer(BrokerConfig brokerConfig)
	    {
		    _brokerConfig = brokerConfig;
			InitilaizeBus();
	    }

	    private void InitilaizeBus()
	    {
		    _busControl = Bus.Factory.CreateUsingRabbitMq(sbc =>
		    {
			    var sbcHost = sbc.Host(new Uri(_brokerConfig.ServiceUrl), host =>
			    {
				    host.Username(_brokerConfig.Username);
				    host.Password(_brokerConfig.Password);
			    });
		    });
	    }
    }
}
