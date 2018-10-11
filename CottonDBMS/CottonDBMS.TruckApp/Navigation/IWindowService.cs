//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.TruckApp.Messages;
using CottonDBMS.TruckApp.ViewModels;
using CottonDBMS.TruckApp.Windows;

namespace CottonDBMS.TruckApp.Navigation
{
    public enum WindowType {MainWindow, AddFieldWindow, PickupWindow, LoadingWindow, LoadingIncorrectModuleWindow, WaitingForUnloadWindow, ChangeListWindow, UnloadingAtGin, LoadingAtGin, UnloadCorrectionWindow, TruckSetupWindow};

    public interface IWindowService
    {       
        void LaunchDirections(double? lat, double? lng, double? myLat, double? myLong);
        void ShowModalWindow(WindowType windowType, object vm);
        void CloseModalWindow(WindowType windowType);
        void FocusLast(WindowType activatedWindowType);
        bool IsWindowOpen(WindowType windowType);
    }

    /// <summary>
    /// This class is used to manage open and closed windows in the application
    /// </summary>
    public class WindowService : IWindowService {

        AddFieldWindow addFieldWindow = null;
        PickupWindow pickupWindow = null;
        LoadingWindow loadingWindow = null;
        LoadingIncorrectModuleWindow loadingIncorrectModuleWindow = null;
        WaitingForUnloadWindow waitingForUnloadWindow = null;
        ChangeFieldWindow changeFieldWindow = null;
        LoadingAtGin loadingAtGinWindow = null;
        UnloadingWindow unloadingAtGinWindow = null;
        UnloadingCorrectionWindow unloadCorrectionWindow = null;
        FirstRunWindow firstRunWindow = null;

        private List<WindowType> openWindows = new List<WindowType>();

        public void ShowModalWindow(WindowType windowType, object vm)
        {
            if (openWindows.Contains(windowType))
            {
                return;
            };

            openWindows.Add(windowType);
            Messenger.Default.Send<DialogOpenedMessage>(new DialogOpenedMessage { Sender = this });

            if (windowType == WindowType.AddFieldWindow)
            {                
                addFieldWindow = new AddFieldWindow();
                addFieldWindow.DataContext = vm;
                addFieldWindow.Show();
            }
            else if (windowType == WindowType.PickupWindow)
            {                
                pickupWindow = new PickupWindow();
                pickupWindow.DataContext = vm;
                pickupWindow.Show();
            }
            else if (windowType == WindowType.LoadingWindow)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {                    
                    loadingWindow = new LoadingWindow();
                    loadingWindow.DataContext = vm;
                    loadingWindow.Show();
                }));
            }
            else if (windowType == WindowType.LoadingAtGin)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    loadingAtGinWindow = new LoadingAtGin();
                    loadingAtGinWindow.DataContext = vm;
                    loadingAtGinWindow.Show();
                }));
            }
            else if (windowType == WindowType.UnloadingAtGin)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    unloadingAtGinWindow = new UnloadingWindow();
                    unloadingAtGinWindow.DataContext = vm;
                    unloadingAtGinWindow.Show();
                }));
            }
            else if (windowType == WindowType.LoadingIncorrectModuleWindow)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    loadingIncorrectModuleWindow = new LoadingIncorrectModuleWindow();
                    loadingIncorrectModuleWindow.DataContext = vm;
                    loadingIncorrectModuleWindow.Show();
                }));
            }
            else if (windowType == WindowType.WaitingForUnloadWindow)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    waitingForUnloadWindow = new WaitingForUnloadWindow();
                    waitingForUnloadWindow.DataContext = vm;
                    waitingForUnloadWindow.Show();
                }));
            }
            else if (windowType == WindowType.UnloadCorrectionWindow)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    unloadCorrectionWindow = new UnloadingCorrectionWindow();
                    unloadCorrectionWindow.DataContext = vm;
                    unloadCorrectionWindow.Show();
                }));
            }
            else if (windowType == WindowType.ChangeListWindow)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    loadingIncorrectModuleWindow.StopBeep();
                    changeFieldWindow = new ChangeFieldWindow();
                    changeFieldWindow.DataContext = vm;
                    changeFieldWindow.Show();
                }));
            }

            else if (windowType == WindowType.TruckSetupWindow)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {                   
                    firstRunWindow = new FirstRunWindow();
                    firstRunWindow.DataContext = vm;
                    firstRunWindow.Show();
                }));
            }
        }

        public void CloseModalWindow(WindowType windowType)
        {
            openWindows.RemoveAll(w => w == windowType);
            Messenger.Default.Send<DialogClosedMessage>(new DialogClosedMessage { Sender = this });

            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (windowType == WindowType.AddFieldWindow)
                {
                    addFieldWindow.Close();
                }
                else if (windowType == WindowType.PickupWindow)
                {
                    pickupWindow.Close();
                }
                else if (windowType == WindowType.LoadingWindow)
                {
                    loadingWindow.Close();
                }
                else if (windowType == WindowType.UnloadingAtGin)
                {
                    unloadingAtGinWindow.Close();
                }
                else if (windowType == WindowType.LoadingAtGin)
                {
                    loadingAtGinWindow.Close();
                }
                else if (windowType == WindowType.LoadingIncorrectModuleWindow)
                {
                    loadingIncorrectModuleWindow.Close();
                }
                else if (windowType == WindowType.WaitingForUnloadWindow)
                {
                    waitingForUnloadWindow.Close();
                }
                else if (windowType == WindowType.UnloadCorrectionWindow)
                {
                    unloadCorrectionWindow.Close();
                }
                else if (windowType == WindowType.ChangeListWindow)
                {                    
                    changeFieldWindow.Close();                    
                }
                else if (windowType == WindowType.TruckSetupWindow)
                {
                    firstRunWindow.Close();
                }
            }));
        }     

        public void LaunchDirections(double? lat, double? lng, double? myLat, double? myLong)
        {
            //switch to use 
            if (lat.HasValue && lng.HasValue && myLat.HasValue && myLong.HasValue)
                System.Diagnostics.Process.Start(string.Format("bingmaps:?rtp=pos.{0}_{1}~pos.{2}_{3}", myLat.Value, myLong.Value, lat.Value, lng.Value));
            else
                MessageBox.Show("Unable to launch Maps for directions.  Not enough GPS information.");
        }

        public bool IsWindowOpen(WindowType windowType)
        {
            return openWindows.Contains(windowType);
        }

        public void FocusLast(WindowType activatedWindowType)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (openWindows.Count > 0)
                {
                    var windowType = openWindows.Last();

                    if (windowType != activatedWindowType)
                    {

                        if (windowType == WindowType.AddFieldWindow)
                        {
                            addFieldWindow.Activate();
                        }
                        else if (windowType == WindowType.PickupWindow)
                        {
                            if (pickupWindow != null)
                                pickupWindow.Activate();
                        }
                        else if (windowType == WindowType.LoadingWindow)
                        {
                            loadingWindow.Activate();
                        }
                        else if (windowType == WindowType.LoadingIncorrectModuleWindow)
                        {
                            loadingIncorrectModuleWindow.Activate();
                        }
                        else if (windowType == WindowType.WaitingForUnloadWindow)
                        {
                            waitingForUnloadWindow.Activate();
                        }
                        else if (windowType == WindowType.UnloadCorrectionWindow)
                        {
                            unloadCorrectionWindow.Activate();
                        }
                        else if (windowType == WindowType.ChangeListWindow)
                        {
                            changeFieldWindow.Activate();
                        }
                        else if (windowType == WindowType.LoadingAtGin)
                        {
                            loadingAtGinWindow.Activate();
                        }
                        else if (windowType == WindowType.UnloadingAtGin)
                        {
                            unloadingAtGinWindow.Activate();
                        }
                        else if (windowType == WindowType.TruckSetupWindow)
                        {
                            firstRunWindow.Activate();
                        }
                    }
                }
            }));          
        }
    }
}
