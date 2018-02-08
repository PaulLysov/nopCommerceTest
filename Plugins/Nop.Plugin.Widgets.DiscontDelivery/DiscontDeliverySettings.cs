using System;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.DiscontDelivery
{
	public class DiscontDeliverySettings : ISettings
	{
		public DateTime DateTimeTo { get; set; }
		public int DiscontPercent { get; set; }

		//public string DiscountText { get; set; }
		//public int BackgroundPictureId { get;set; }
	}
}