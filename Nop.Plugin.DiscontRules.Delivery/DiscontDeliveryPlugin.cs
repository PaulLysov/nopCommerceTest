using System;
using System.Collections.Generic;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Cms;

namespace Nop.Plugin.DiscontRules.Delivery
{
    public class DiscontDeliveryPlugin: IWidgetPlugin
    {
	    public PluginDescriptor PluginDescriptor { get; set; }
	    public void Install()
	    {
		    throw new NotImplementedException();
	    }

	    public void Uninstall()
	    {
		    throw new NotImplementedException();
	    }

	    public IList<string> GetWidgetZones()
	    {
		    throw new NotImplementedException();
	    }

	    public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
	    {
		    throw new NotImplementedException();
	    }

	    public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName,
		    out RouteValueDictionary routeValues)
	    {
		    throw new NotImplementedException();
	    }
    }
}
