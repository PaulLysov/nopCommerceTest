using System;
using System.Collections.Generic;
using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.DiscontDelivery
{
	public class DiscontDeliveryPlugin : BasePlugin, IWidgetPlugin
	{
		//widget settings
		private readonly ISettingService _settingService;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="settingService">widget settings</param>
		public DiscontDeliveryPlugin(ISettingService settingService)
		{
			this._settingService = settingService;
		}

		/// <summary>
		/// gets list of places where widget rendered
		/// </summary>
		/// <returns> widget zones </returns>
		public IList<string> GetWidgetZones()
		{
			return new List<string> { "productbox_addinfo_middle", "producttemplate_simple_delivery_after" };
		}

		/// <summary>
		/// gets a route for provider configuration
		/// </summary>
		/// <param name="actionName">action name</param>
		/// <param name="controllerName">controller name</param>
		/// <param name="routeValues">route values</param>
		public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
		{
			actionName = "Configure";
			controllerName = "WidgetsDiscontDelivery";
			routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Widgets.DiscontDelivery.Controllers" }, { "area", null } };
		}

		public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName,
			out RouteValueDictionary routeValues)
		{
			actionName = "PublicInfo";
			controllerName = "WidgetsDiscontDelivery";
			routeValues = new RouteValueDictionary
			{
				{"Namespaces", "Nop.Plugin.Widgets.DiscontDelivery.Controllers"},
				{"area", null},
				{"widgetZone", widgetZone}
			};
		}

		/// <summary>
		/// Install plugin
		/// </summary>
		public override void Install()
		{
			//settings
			var settings = new DiscontDeliverySettings()
			{
				DiscontPercent = 25,
				DateTimeTo = DateTime.Now.AddMonths(1).Date
			};
			_settingService.SaveSetting(settings);

			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.DiscontDelivery.Settings", "Settings");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.DiscontDelivery.Text", "If you order before {0} discount for shipping {1}%");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.DiscontDelivery.DateTimeTo", "DateTime to");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.DiscontDelivery.DateTimeTo.Hint", "Enter the datetime to which the action will operate");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.DiscontDelivery.DiscontPercent", "Discount percent");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.DiscontDelivery.DiscontPercent.Hint", "Please enter discount delivery percent");
			this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.DiscontDelivery.Attention", "Discont on delivery");

			base.Install();
		}

		/// <summary>
		/// Uninstall plugin
		/// </summary>
		public override void Uninstall()
		{
			//settings
			_settingService.DeleteSetting<DiscontDeliverySettings>();

			//remove plugin resources

			this.DeletePluginLocaleResource("Plugins.Widgets.DiscontDelivery.Settings");
			this.DeletePluginLocaleResource("Plugins.Widgets.DiscontDelivery.Text");
			this.DeletePluginLocaleResource("Plugins.Widgets.DiscontDelivery.DateTimeTo");
			this.DeletePluginLocaleResource("Plugins.Widgets.DiscontDelivery.DateTimeTo.Hint");
			this.DeletePluginLocaleResource("Plugins.Widgets.DiscontDelivery.DiscontPercent");
			this.DeletePluginLocaleResource("Plugins.Widgets.DiscontDelivery.DiscontPercent.Hint");
			this.DeletePluginLocaleResource("Plugins.Widgets.DiscontDelivery.Attention");

			base.Uninstall();
		}
	}
}
