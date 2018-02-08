using System;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.DiscontDelivery.Models
{
	public class ConfigurationModel : BaseNopModel
	{
		public int ActiveStoreScopeConfiguration { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.DiscontDelivery.DateTimeTo")]
		[AllowHtml]
		public DateTime DateTimeTo { get; set; }
		public bool DateTimeTo_OverrideForStore { get; set; }

		[NopResourceDisplayName("Plugins.Widgets.DiscontDelivery.DiscontPercent")]
		[AllowHtml]
		public int DiscontPercent { get; set; }
		public bool DiscontPercent_OverrideForStore { get; set; }

	}
}