using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Wpf_Karaokay.Model;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace Wpf_Karaokay.ViewModel
{
    public class CashierViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<BillDetail> BillDetails { get; set; }
        public ICommand TimerCommand { get; private set; }
        public ICommand BackCommand { get; set; }
        public ICommand DecreaseQuantityCommand { get; private set; }
        public ICommand IncreaseQuantityCommand { get; private set; }

        public ObservableCollection<Item> FoodItems { get; set; }
        public ObservableCollection<Item> BeverageItems { get; set; }
        public ObservableCollection<Item> OthersItems { get; set; }

        public Room CurrentRoom { get; set; }

        public String RoomName { get; set; }

        public bill CurrentBill { get; set; }

        public List<BillDetail> CurrentReceipt { get; set; }

        public string receiptText { get; set; }



        public CashierViewModel()
        {
            //Load the current room 
            Items = new ObservableCollection<Item>(DataProvider.Ins.DB.Items.ToList());
            FoodItems = new ObservableCollection<Item>(Items.Where(i => i.itemType == "food"));
            BeverageItems = new ObservableCollection<Item>(Items.Where(i => i.itemType == "beverage"));
            OthersItems = new ObservableCollection<Item>(Items.Where(i => i.itemType == "others"));

            DecreaseQuantityCommand = new RelayCommand<Item>(DecreaseQuantity);
            IncreaseQuantityCommand = new RelayCommand<Item>(IncreaseQuantity);
            TimerCommand = new RelayCommand<Item>(StartTimer);
            BackCommand = new RelayCommand<Window>(BackButton);
        }


        private void DecreaseQuantity(Item item)
        {
            BillDetail BillDetailWithOrder = DataProvider.Ins.DB.BillDetails.FirstOrDefault(bd => bd.OrderID == item.itemID);
            if (BillDetailWithOrder == null)
            {
                BillDetail BillDetailForItem = new BillDetail();
                BillDetailForItem.BillID = CurrentBill.BillID;
                BillDetailForItem.OrderID = item.itemID;
                BillDetailForItem.Quantity = 0;
                BillDetailForItem.TotalPrice = item.itemPrice * BillDetailForItem.Quantity;
                CurrentReceipt.Add(BillDetailForItem);
                DataProvider.Ins.DB.BillDetails.Add(BillDetailForItem);
                DataProvider.Ins.DB.SaveChanges();
                this.PrintReceipt(CurrentReceipt);
            }
            else
            {
                BillDetailWithOrder.Quantity--;
                if (BillDetailWithOrder.Quantity < 0)
                {
                    BillDetailWithOrder.Quantity = 0;
                }
                DataProvider.Ins.DB.SaveChanges();
                this.PrintReceipt(CurrentReceipt);
            }
        }

        private void IncreaseQuantity(Item item)
        {


            //get the orderID 
            BillDetail BillDetailWithOrder = DataProvider.Ins.DB.BillDetails.FirstOrDefault(bd => bd.OrderID == item.itemID);
            if (BillDetailWithOrder == null)
            {
                BillDetail BillDetailForItem = new BillDetail();
                BillDetailForItem.BillID = CurrentBill.BillID;
                BillDetailForItem.OrderID = item.itemID;
                BillDetailForItem.Quantity = 1;
                BillDetailForItem.TotalPrice = item.itemPrice * BillDetailForItem.Quantity;
                CurrentReceipt.Add(BillDetailForItem);
                DataProvider.Ins.DB.BillDetails.Add(BillDetailForItem);
                DataProvider.Ins.DB.SaveChanges();
                this.PrintReceipt(CurrentReceipt);
            }
            else
            {
                BillDetailWithOrder.Quantity++;
                DataProvider.Ins.DB.SaveChanges();
                this.PrintReceipt(CurrentReceipt);
            }
        }


        private void StartTimer(Item item)
        {
            this.CreateBillDetails();
        }

        private void StopTimer(object paramter)
        {

        }

        public void LoadSavedBillDetails(Room room)
        {
            //Load current room 
            CurrentRoom = room;
            RoomName = "Room " + CurrentRoom.RmId;

            // get the bill info 
            CurrentBill = DataProvider.Ins.DB.bills.FirstOrDefault(b => (b.RmId == room.RmId) && (b.Billed == false));
            if (CurrentBill != null)
            {
                CurrentReceipt = DataProvider.Ins.DB.BillDetails.Where(bd => bd.BillID == CurrentBill.BillID).ToList();
                if (CurrentReceipt != null)
                {
                    this.PrintReceipt(CurrentReceipt);
                }
                // load to the receipt text box and calculate the total order price 
            }
            else
            {
                MessageBox.Show("No bill have been made", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                room.RMStatus = 0;

                DataProvider.Ins.DB.SaveChanges();
            }
        }


        public void PrintReceipt(List<BillDetail> billdetails)
        {
            StringBuilder receiptBuilder = new StringBuilder();

            foreach (BillDetail billDetail in billdetails)
            {
                // get item base on the OrderId 
                Item item = DataProvider.Ins.DB.Items.FirstOrDefault(i => (i.itemID == billDetail.OrderID));
                //calculate the bill orderprice 
                CurrentBill.OrderPice += item.itemPrice * billDetail.Quantity;
                receiptBuilder.AppendLine($"Item: {item.itemName}, Quantity: {billDetail.Quantity}, Price: {item.itemPrice * billDetail.Quantity}");
            }

            string receipt = receiptBuilder.ToString();
            receiptText = receipt;
        }


        private void CreateBillDetails()
        {
            CurrentBill = new bill();
            string billId = Guid.NewGuid().ToString();
            DateTime billDate = DateTime.Now;
            CurrentBill.BillID = billId;
            CurrentBill.BillDate = billDate;
            CurrentBill.Billed = false;
            CurrentBill.OrderPice = 0;
            CurrentBill.TotalPrice = 0;
            CurrentBill.RmId = CurrentRoom.RmId;

            DataProvider.Ins.DB.bills.Add(CurrentBill);
            DataProvider.Ins.DB.SaveChanges();
        }



        private void BackButton(object parameter)
        {
            
            NavigationService.NavigateToWindow("RoomsWindow");
        }
        

    }
}