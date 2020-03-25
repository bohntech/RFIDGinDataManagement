using CottonDBMS.BridgeFeederApp.ViewModels;
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

namespace CottonDBMS.BridgePBIApp.ViewModels
{
    public class ShellViewModel : ShellViewModelBase 
    {
        public ShellViewModel(INavigationService navService) : base(navService)
        {
            EnableProcessSync = false;            
        }

        protected override void OpenFirstRunPage()
        {
            //settings have not been saved so let's open the first run wizard
            INavigationService navService = SimpleIoc.Default.GetInstance<INavigationService>();
            PBISettingsPageViewModel vm = new PBISettingsPageViewModel(navService);         
            vm.IsFirstLaunch = true;            
            NavService.ShowPage(PageType.SETTINGS_PAGE, false, vm);
            vm.Initialize();            
        }

        protected override void ExecuteOpenSettings()
        {
            if (!NavService.IsOpen(PageType.SETTINGS_PAGE))
            {
                INavigationService navService = SimpleIoc.Default.GetInstance<INavigationService>();
                var vm = new PBISettingsPageViewModel(navService);               
                vm.IsFirstLaunch = false;
                NavService.ShowPage(PageType.SETTINGS_PAGE, true, vm);
                vm.Initialize();
            }
        }

        protected override void OpenStartPage()
        {           
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
