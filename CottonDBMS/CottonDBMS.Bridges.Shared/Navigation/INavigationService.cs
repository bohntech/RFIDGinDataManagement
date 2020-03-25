using CottonDBMS.Bridges.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CottonDBMS.Bridges.Shared.Navigation
{
    public enum PageType { NONE = 0, IDLE_PAGE, LOAD_PAGE, MESSAGE_PAGE, SCAN_GIN_TAG_PAGE, SETTINGS_PAGE, UNLOCK_PAGE, EXIT_SCALE_PAGE, WEIGHT_IN_PAGE, FEEDER_MODULE_PAGE, LOAD_LIST_PAGE }

    public class PopPageMessage
    {
        public Page ContentPage { get; set; }
    }

    public interface INavigationService
    {

        void ShowPage(PageType page, bool keepViewModel = false, BasePageViewModel dataContext = null);
        void Pop();
        bool IsOpen(PageType pageType);
        void Dispose();
        //void ShowPage(int iDLE_PAGE, bool v, BridgeApp.ViewModels.IdlePageViewModel vm);
    }
}
