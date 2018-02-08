using System;
using System.Web.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.DiscontDelivery.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.DiscontDelivery.Controllers
{
	public class WidgetsDiscontDeliveryController : BasePluginController
	{
		private readonly IWorkContext _workContext;
		private readonly IStoreContext _storeContext;
		private readonly IStoreService _storeService;
		private readonly ISettingService _settingService;
		private readonly ILocalizationService _localizationService;

		public WidgetsDiscontDeliveryController(IWorkContext workContext,
			IStoreContext storeContext,
			IStoreService storeService,
			ISettingService settingService,
			ILocalizationService localizationService)
		{
			_workContext = workContext;
			_storeContext = storeContext;
			_storeService = storeService;
			_settingService = settingService;
			_localizationService = localizationService;
		}

		[AdminAuthorize]
		[ChildActionOnly]
		public ActionResult Configure()
		{
			//load settings for a chosen store scope
			var storeScope = GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var discontDeliverySettings = _settingService.LoadSetting<DiscontDeliverySettings>(storeScope);
			var model = new ConfigurationModel
			{
				DateTimeTo = discontDeliverySettings.DateTimeTo ,
				DiscontPercent = discontDeliverySettings.DiscontPercent,
				ActiveStoreScopeConfiguration = storeScope
			};

			if (storeScope > 0)
			{
				model.DateTimeTo_OverrideForStore = _settingService.SettingExists(discontDeliverySettings, x => x.DateTimeTo, storeScope);
				model.DiscontPercent_OverrideForStore = _settingService.SettingExists(discontDeliverySettings, x => x.DiscontPercent, storeScope);
			}

			return View("~/Plugins/Widgets.DiscontDelivery/Views/Configure.cshtml", model);
		}

		[HttpPost]
		[AdminAuthorize]
		[ChildActionOnly]
		public ActionResult Configure(ConfigurationModel model)
		{
			//load settings for a chosen store scope
			var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
			var discontDeliverySettings = _settingService.LoadSetting<DiscontDeliverySettings>(storeScope);
		
			discontDeliverySettings.DateTimeTo = model.DateTimeTo;
			discontDeliverySettings.DiscontPercent = model.DiscontPercent;

			//save settings 
			_settingService.SaveSettingOverridablePerStore(discontDeliverySettings, x => x.DateTimeTo, model.DateTimeTo_OverrideForStore, storeScope, false);
			_settingService.SaveSettingOverridablePerStore(discontDeliverySettings, x => x.DiscontPercent, model.DiscontPercent_OverrideForStore, storeScope, false);			

			//now clear settings cache
			_settingService.ClearCache();

			SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
			return Configure();
		}

		[ChildActionOnly]
		public ActionResult PublicInfo(string widgetZone, object additionalData = null)
		{
			var discontDeliverySettings = _settingService.LoadSetting<DiscontDeliverySettings>(_storeContext.CurrentStore.Id);

			var model = new PublicInfoModel
			{
				DateTimeTo = discontDeliverySettings.DateTimeTo,
				DiscontPercent = discontDeliverySettings.DiscontPercent
			};
			
			//check all parameters exist 
			//if (!model.DateTimeTo.HasValue || !model.DiscontPercent.HasValue)
			//	return Content("");

			return View("~/Plugins/Widgets.DiscontDelivery/Views/PublicInfo.cshtml", model);
		}
	}
}