using CottonDBMS.Bridges.Shared.Navigation;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.Bridges.Shared.ViewModels
{
    public class BasePageViewModel : ViewModelBase
    {
        protected INavigationService NavService = null;


        public BasePageViewModel(INavigationService navService) : base()
        {
            NavService = navService;
        }
    }
}
