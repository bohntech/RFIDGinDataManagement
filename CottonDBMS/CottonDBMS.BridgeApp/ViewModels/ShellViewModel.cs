using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.Tasks;
using CottonDBMS.Bridges.Shared.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.BridgeApp.ViewModels
{
    public class ShellViewModel : ShellViewModelBase 
    {
        public ShellViewModel(INavigationService navService) : base(navService)
        {

        }

        protected override void OpenFirstRunPage()
        {
            //settings have not been saved so let's open the first run wizard
            INavigationService navService = SimpleIoc.Default.GetInstance<INavigationService>();
            SettingsPageViewModel vm = new SettingsPageViewModel(navService);
            vm.AllowPortSelection = true;
            vm.AllowPortBarCodeSelection = true;
            vm.AllowWeighInPageTimeOutSelection = true;
            vm.IsFirstLaunch = true;            
            NavService.ShowPage(PageType.SETTINGS_PAGE, false, vm);
            vm.Initialize();

            BridgeSyncProcessHelper.SetAppName("BridgeApp");
        }

        protected override void OpenStartPage()
        {
            Task.Run(() =>
            {
                BridgeSyncProcessHelper.Init("BridgeApp");
            });

            IdlePageViewModel vm = new IdlePageViewModel(NavService);
            NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
            vm.Initialize();
            
        }

        protected override void HandleSettingsSavedMessage(SettingsSavedMessage msg)
        {
            base.HandleSettingsSavedMessage(msg);

            if (msg.IsFirstLaunch)
            {
                var vm = new IdlePageViewModel(NavService);
                NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
                vm.Initialize();
            }
        }
    }
}
