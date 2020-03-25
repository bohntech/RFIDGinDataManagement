using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.BridgeApp.ViewModels;
using CottonDBMS.BridgeApp.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CottonDBMS.Bridges.Shared.ViewModels;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.Messages;

namespace CottonDBMS.BridgeApp.Navigation
{
    

    public class NavigationService : INavigationService
    {
        public PageType _currentPageType = PageType.NONE;
        public BasePageViewModel currentViewModel;
        public Page currentPage;

        public Page previousPage;
        public BasePageViewModel previousViewModel;
        public PageType _previousPageType = PageType.NONE;
        
        public NavigationService()
        {

        }

        public bool IsOpen(PageType pageType)
        {
            return (pageType == _currentPageType);
        }

        public void ShowPage(PageType pageType, bool keepViewModel=false, BasePageViewModel dataContext = null)
        {
            if (pageType == _currentPageType)
            {
                //we are already on this page so don't show another
                return;
            }

            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (currentViewModel != null && !keepViewModel)
                {
                    currentViewModel.Cleanup();
                }
                else if (keepViewModel)
                {
                    previousPage = currentPage;
                    previousViewModel = currentViewModel;
                }

                _currentPageType = pageType;
                if (pageType == PageType.IDLE_PAGE)
                {
                    currentPage = new IdlePage();
                    currentViewModel = (BasePageViewModel)dataContext;
                    currentPage.DataContext = currentViewModel;
                    Messenger.Default.Send<ContentPageChangedMessage>(new ContentPageChangedMessage { Content = currentPage });
                }
                else if (pageType ==  PageType.SETTINGS_PAGE)
                {
                    currentPage = new SettingsPage();
                    currentViewModel = (BasePageViewModel)dataContext;
                    currentPage.DataContext = currentViewModel;
                    Messenger.Default.Send<ContentPageChangedMessage>(new ContentPageChangedMessage { Content = currentPage });
                }
                else if (pageType == PageType.WEIGHT_IN_PAGE)
                {
                    currentPage = new WeighInPage();
                    currentViewModel = (BasePageViewModel)dataContext;
                    currentPage.DataContext = currentViewModel;
                    Messenger.Default.Send<ContentPageChangedMessage>(new ContentPageChangedMessage { Content = currentPage });
                }
                else if (pageType == PageType.LOAD_PAGE)
                {
                    currentPage = new LoadPage();
                    currentViewModel = (BasePageViewModel)dataContext;
                    currentPage.DataContext = currentViewModel;
                    Messenger.Default.Send<ContentPageChangedMessage>(new ContentPageChangedMessage { Content = currentPage });
                }
                else if (pageType == PageType.LOAD_LIST_PAGE)
                {
                    currentPage = new LoadListPage();
                    currentViewModel = (BasePageViewModel)dataContext;
                    currentPage.DataContext = currentViewModel;
                    Messenger.Default.Send<ContentPageChangedMessage>(new ContentPageChangedMessage { Content = currentPage });
                }
                else if (pageType == PageType.EXIT_SCALE_PAGE)
                {
                    currentPage = new ExitScalePage();
                    currentViewModel = (BasePageViewModel)dataContext;
                    currentPage.DataContext = currentViewModel;
                    Messenger.Default.Send<ContentPageChangedMessage>(new ContentPageChangedMessage { Content = currentPage });
                }
            }));
        }
        public void Pop()
        {
            System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                var temp = currentViewModel;
                if (temp != null)
                {
                    temp.Cleanup();
                }
                currentViewModel = previousViewModel;
                currentPage = previousPage;
                _currentPageType = _previousPageType;
                Messenger.Default.Send<ContentPageChangedMessage>(new ContentPageChangedMessage { Content = currentPage });
            }));
        }
        public void Dispose()
        {
            if (currentViewModel != null)
            {
                currentViewModel.Cleanup();
            }
            
        }
    }
}
