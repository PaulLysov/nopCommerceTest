using System;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.DiscontDelivery.Models;
using Nop.Services.Catalog;
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
		private readonly IProductService _productService;
		private readonly IStoreService _storeService;
		private readonly ISettingService _settingService;

		private readonly ILocalizationService _localizationService;

		public WidgetsDiscontDeliveryController(IWorkContext workContext,
			IStoreContext storeContext,
			IStoreService storeService,
			IProductService productService,
			ISettingService settingService,
			ILocalizationService localizationService)
		{
			_workContext = workContext;
			_storeContext = storeContext;
			_storeService = storeService;
			_productService = productService;
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
				DateTimeTo = discontDeliverySettings.DateTimeTo,
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
			//it is necessary product id
			if (additionalData == null)
				return new EmptyResult();

			var discontDeliverySettings = _settingService.LoadSetting<DiscontDeliverySettings>(_storeContext.CurrentStore.Id);

			var productId = Convert.ToInt32(additionalData);
			var product = _productService.GetProductById(productId);

			//check product and is not free shipping
			if (product == null || product.Deleted || product.IsFreeShipping)
				return new EmptyResult();

			var model = new PublicInfoModel
			{
				DateTimeTo = discontDeliverySettings.DateTimeTo,
				DiscontPercent = discontDeliverySettings.DiscontPercent
			};

			switch (widgetZone)
			{
				case "productbox_addinfo_middle":
					return View("~/Plugins/Widgets.DiscontDelivery/Views/PublicInfoProductBox.cshtml");
				case "producttemplate_simple_delivery_after":
					return View("~/Plugins/Widgets.DiscontDelivery/Views/PublicInfoProductSimple.cshtml", model);
				default:
					return new EmptyResult();
			}


		}
	}
}