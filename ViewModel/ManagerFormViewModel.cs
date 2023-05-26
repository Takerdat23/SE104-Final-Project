﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf_Karaokay.ViewModel
{
    public class ManagerFormViewModel : BaseViewModel
    {
        public ICommand CashierCmd { get; private set; }
        public ICommand RoomCmd { get; private set; }
        public ICommand MenuCmd { get; private set; }
        public ICommand EmployCmd { get; private set; }

        




        public ManagerFormViewModel() {
            
           
            NavigationService.RegisterWindow("RoomsWindow", typeof(RoomsWindows), new RoomsWindowViewModel());
            NavigationService.RegisterPage("CashierPage", typeof(CashierPage), new CashierViewModel());

            CashierCmd = new RelayCommand(SwitchToRoomWindows);
            RoomCmd = new RelayCommand(SwitchToEditRoom);
            MenuCmd = new RelayCommand(SwitchToEditMenu);
            EmployCmd = new RelayCommand(SwitchToEmployee);
        }

        private void SwitchToRoomWindows(object parameter)
        {



            // Close the current window
           
            NavigationService.NavigateToWindow("RoomsWindow");
          
           // Show the new window
            //roomsWindow.Show();
        }

        private void SwitchToEditRoom(object parameter)
        {
            
            
            NavigationService.NavigateToWindow("EditRoom"); 

            //EditRoom room = new EditRoom();
            Application.Current.MainWindow.Close();
            //room.Show();

        }

        private void SwitchToEditMenu(object parameter)
        {
            
            
            NavigationService.NavigateToWindow("EditMenu");
            Application.Current.MainWindow.Close();

        }

        private void SwitchToEmployee(object parameter)
        {
            MessageBox.Show("No employee window yet!!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
