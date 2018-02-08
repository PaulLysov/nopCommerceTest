using System;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.DiscontDelivery.Models
{
	public class PublicInfoModel : BaseNopModel
	{
		public DateTime DateTimeTo { get; set; }
		public int DiscontPercent { get; set; }
	}
}