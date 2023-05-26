using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Wpf_Karaokay.Model;

namespace Wpf_Karaokay.ViewModel
{
    public class RoomsWindowViewModel : BaseViewModel
    {


        public Room SelectedRoom { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }
        public ICommand RoomButtonCommand { get; private set; }
        public ICommand BackButtonCommand { get; set; }

       

        public RoomsWindowViewModel()
        {
            // Fetch your rooms from the database and populate the collection

           
            List<Room> roomsFromDatabase = DataProvider.Ins.DB.Rooms.ToList();
            Rooms = new ObservableCollection<Room>(roomsFromDatabase);
            RoomButtonCommand = new RelayCommand<Room>(NavigateToCashierPage);
            BackButtonCommand = new RelayCommand<Room>(BackButton);
          
        }
        private void NavigateToCashierPage(Room room)
        {
            SelectedRoom = room;

            // Check the room status
            if (SelectedRoom.RMStatus == 1)
            {
                // Room is active, navigate to the cashier page and load the saved bill details
                CashierPage cashierPage = new CashierPage();
                CashierViewModel cashierPageViewModel = new CashierViewModel();
                cashierPageViewModel.LoadSavedBillDetails(room);
                cashierPage.DataContext = cashierPageViewModel;
             
                NavigationService.NavigateToPage("CashierPage");
            }
            else
            {
                // navigate to empty CashierPage 
                
                NavigationService.NavigateToPage("CashierPage");
            }
        }

        public void BackButton(Room room )
        {

            NavigationService.RegisterWindow("ManagerForm", typeof(ManagerForm), new ManagerFormViewModel());
            NavigationService.NavigateToWindow("ManagerForm"); 
        }
    }
}
