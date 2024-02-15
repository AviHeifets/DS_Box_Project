using Ds;
using Ds.DataStructures;
using Ds.Models;
using Logic.Helpers;
using System.Collections.ObjectModel;

namespace Logic
{
    public class MainViewModel : ViewModelBase
    {
        private readonly DataService db;

        public MainViewModel()
        {
            db = DataService.Init;
            RefreshCollections();
            Timer();
        }

        #region Commands

        private RelayCommand? addBtn;
        public RelayCommand AddBtn
        {
            get
            {
                if (addBtn == null)
                    addBtn = new RelayCommand(execute => Add(), canExecute => CanAdd());
                return addBtn!;
            }

        }


        private RelayCommand? searchBtn;
        public RelayCommand SearchBtn
        {
            get
            {
                if (searchBtn == null)
                    searchBtn = new RelayCommand(execute => Search(), canExecute => CanSearch());
                return searchBtn;
            }

        }


        private RelayCommand? buyBtn;
        public RelayCommand BuyBtn
        {
            get
            {
                if (buyBtn == null)
                    buyBtn = new RelayCommand(execute => Buy(), canExecute => CanBuy());
                return buyBtn;
            }

        }


        private RelayCommand? deleteBtn;
        public RelayCommand DeleteBtn
        {
            get
            {
                if (deleteBtn == null)
                    deleteBtn = new RelayCommand(execute => Delete(), canExecute => CanDelete());
                return deleteBtn;
            }

        }

        private RelayCommand? deleteExpiredBtn;

        public RelayCommand DeleteExpiredBtn
        {
            get
            {
                if (deleteExpiredBtn == null)
                    deleteExpiredBtn = new RelayCommand(execute => DeleteExpired(), canExecute => CanDeleteExpired());
                return deleteExpiredBtn;
            }

        }


        #endregion

        #region Props
        private int buyAmount = 1;
        private double addX;
        private double addY;
        private int addAmount = 1;
        private double searchBase;
        private double searchY;
        private string? boxInfo;
        private CustomQueue<Box>? selectedBox;
        private readonly double maxBoxSize = 1000;
        private int selectedBoxCount = 0;


        public string? BoxInfo
        {
            get { return boxInfo; }
            set
            {
                if (boxInfo == value) return;
                boxInfo = value;
                OnPropertyChanged();
            }
        }
        public CustomQueue<Box> SelectedBox
        {
            get { return selectedBox!; }
            set
            {
                if (selectedBox == value) return;
                selectedBox = value;
                OnPropertyChanged();
                UpdateCommands();
            }
        }
        public int BuyAmount
        {
            get { return buyAmount!; }
            set
            {
                if (buyAmount == value) return;
                buyAmount = value;
                OnPropertyChanged();
                UpdateCommands();
            }
        }
        public double AddX
        {
            get { return addX!; }
            set
            {
                if (addX == value) return;
                addX = value;
                OnPropertyChanged();
                UpdateCommands();

            }
        }
        public double AddY
        {
            get { return addY; }
            set
            {
                if (addY == value) return;
                addY = value;
                OnPropertyChanged();
                UpdateCommands();

            }
        }
        public int AddAmount
        {
            get { return addAmount; }
            set
            {
                if (addAmount == value) return;
                addAmount = value;
                OnPropertyChanged();
                UpdateCommands();
            }
        }
        public double SearchBase
        {
            get { return searchBase; }
            set
            {
                if (searchBase == value) return;
                searchBase = value;
                OnPropertyChanged();
                UpdateCommands();
            }
        }
        public double SearchY
        {
            get { return searchY; }
            set
            {
                if (searchY == value) return;
                searchY = value;
                OnPropertyChanged();
                UpdateCommands();
            }

        }


        private ObservableCollection<Box>? boxes;

        public ObservableCollection<Box>? Boxes
        {
            get { return boxes; }
            set
            {
                boxes = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Box>? expiredBoxes;

        public ObservableCollection<Box>? ExpiredBoxes
        {
            get { return expiredBoxes; }
            set
            {
                expiredBoxes = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Functions
        private void Search()
        {
            double SearchX = Math.Sqrt(SearchBase);
            CustomQueue<Box> resultQueue = db.Search(SearchX, SearchY);
            if (resultQueue == null || resultQueue.Items.Count == 0)
                BoxInfo = "There are no boxes with these parameters";

            else if (SearchX == resultQueue.X && SearchY == resultQueue.Y)
            {
                int boxes = 0;
                foreach (var box in resultQueue.Items)
                {
                    boxes += box.Count;
                }
                selectedBoxCount = boxes;
                BoxInfo = $"{selectedBoxCount} Boxes in storage";
            }
            else
                BoxInfo = $"There are no boxes with these parameters\nThe closest box we have is {Math.Round((resultQueue.X * resultQueue.X), 2)}, {Math.Round(resultQueue.Y, 2)}";
            SelectedBox = resultQueue!;
            SearchBase = 0;
            SearchY = 0;
        }
        private bool CanSearch()
        {
            if (SearchBase < 1 || SearchBase > maxBoxSize * maxBoxSize || SearchY < 1 || SearchY > maxBoxSize)
                return false;
            return true;
        }

        private void Add()
        {
            Box box = new Box() { X = AddX, Y = AddY, Count = AddAmount };
            db.Add(box);
      
            Boxes!.Add(box);

            AddAmount = 1;
            AddX = 0;
            AddY = 0;
            RefreshCollections();
        }
        private bool CanAdd()
        {
            if (AddX < 1 || AddX > maxBoxSize || AddY < 1 || AddY > maxBoxSize)
                return false;
            return true;
        }

        private void Delete() => db.Delete(SelectedBox);
        private bool CanDelete()
        {
            return true;
        }

        private void Buy()
        {
            List<Box> Removed;
            Removed = db.Buy(SelectedBox, buyAmount);
            selectedBoxCount -= buyAmount;
            BoxInfo = $"{selectedBoxCount} Boxes in storage";
            BuyAmount = 1;
            SelectedBox = null!;

            if (Removed == null)
                return;

            foreach (var removedBox in Removed)
            {
                if (Boxes != null)
                {
                    Boxes.Remove(removedBox);
                }
            }

            ObservableCollection<Box> temp = Boxes!;
            Boxes = null!;
            Boxes = temp;
        }
        private bool CanBuy()
        {
            if (SelectedBox == null)
                return false;

            if (selectedBoxCount < buyAmount)
                return false;
            return true;
        }

        private void DeleteExpired()
        {
            db.ClearExpired();
            ExpiredBoxes?.Clear();
        }
        private bool CanDeleteExpired()
        {
            return true;
        }

        private void UpdateCommands()
        {
            SearchBtn.RaiseCanExecuteChanged();
            AddBtn.RaiseCanExecuteChanged();
            BuyBtn.RaiseCanExecuteChanged();
            DeleteBtn.RaiseCanExecuteChanged();
        }

        private void RefreshCollections()
        {
            List<Box> expiredBoxes;
            Boxes = new ObservableCollection<Box>(db.GetAllBoxes(out expiredBoxes));
            ExpiredBoxes = new ObservableCollection<Box>(expiredBoxes);
        }

        private void Timer()
        {
            DateTime now = DateTime.Now;
            DateTime nextRunTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0); // Set the time to midnight
            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1); // Move to the next day if it's already past midnight
            }

            double millisecondsUntilFirstRun = (nextRunTime - now).TotalMilliseconds;

            // Create a timer that runs MyTask every 24 hours (86400000 milliseconds)
            Timer timer = new Timer(_ => RefreshCollections(), null, (int)millisecondsUntilFirstRun, 86400000);

        }

        #endregion
    }
}
