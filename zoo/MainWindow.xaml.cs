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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace zoo
{
    
    public partial class MainWindow : Window
    {
        Random random = new Random();

        List<Animal> MonkeyList { get; set; }
        List<Animal> LionList { get; set; }
        List<Animal> ElephantList { get; set; }

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        int DayTimeTicks;

        public MainWindow()
        {

            MonkeyList = new List<Animal>();
            LionList = new List<Animal>();
            ElephantList = new List<Animal>();


            DayTimeTicks = 0;

            InitializeComponent();

            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            dispatcherTimer.Start();

        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            DayTimeTicks += 1;
            if (DayTimeTicks == 41)
            {
                NextDay();
            }
            UpdateTimeBar();
            
        }

        public void UpdateTimeBar()
        {
            if (DayTimeTicks == 41)
            {
                DayTimeTicks = 0;
            }
            TimeBar.Width = DayTimeTicks*(19.8/2);
        }

        private void AddMonkey_Click(object sender, RoutedEventArgs e)
        {
            Animal animal = new Monkey();
            MonkeyList.Add(animal);

            UpdateViewList(MergeList(MonkeyList, MergeList(ElephantList, LionList)));

        }
        
        private void AddLion_Click(object sender, RoutedEventArgs e)
        {
            Animal animal = new Lion();
            LionList.Add(animal);

            UpdateViewList(MergeList(MonkeyList, MergeList(ElephantList, LionList)));
        }

        private void AddElephant_Click(object sender, RoutedEventArgs e)
        {
            Animal animal = new Elephant();
            ElephantList.Add(animal);

            UpdateViewList(MergeList(MonkeyList,MergeList(ElephantList,LionList)));
        }

        private void FeedAll_Click(object sender, RoutedEventArgs e)
        {
            FeedAnimal(MonkeyList);
            FeedAnimal(LionList);
            FeedAnimal(ElephantList);
        }

        public void FeedAnimal(List<Animal> animalList) {
            for (int i = 0; i < animalList.Count; i++)
            {
                animalList[i].FeedAnimal(15);
            }
            UpdateViewList(MergeList(MonkeyList, MergeList(ElephantList, LionList)));
        }


        public void NextDay()
        {
            CheckBoxIfDead(MonkeyList);
            CheckBoxIfDead(LionList);
            CheckBoxIfDead(ElephantList);
            UseEnergy(MonkeyList);
            UseEnergy(LionList);
            UseEnergy(ElephantList);
        }

        public void CheckBoxIfDead(List<Animal> animalList)
        {
            List<int> ToRemove = new List<int>();
            for (int i = 0; i < animalList.Count; i++)
            {
                if (animalList[i].Energy  <= 0 )
                {
                    ToRemove.Add(i);
                }
            }
            ToRemove.Reverse();
            foreach (var item in ToRemove)
            {
                animalList.RemoveAt(item);
            }
            UpdateViewList(MergeList(MonkeyList, MergeList(ElephantList, LionList)));
        }

        public void UseEnergy(List<Animal> animalList)
        {
            for (int i = 0; i < animalList.Count; i++)
            {
                animalList[i].UseEnergy();
            }
            UpdateViewList(MergeList(MonkeyList, MergeList(ElephantList, LionList)));
        }

        public List<Animal> MergeList(List<Animal> animals1, List<Animal> animals2) {
            animals1 = new List<Animal>(animals1.ToArray());
            List<Animal> returnList = animals1;
            returnList.AddRange(animals2);
            return returnList;
        }

        public void UpdateViewList(List<Animal> animals)
        {
            AnimalListView.Items.Clear();
            foreach (var animal in animals)
            {
                AnimalListView.Items.Add(animal);
            }
        }

        private void FeedMonkey_Click(object sender, RoutedEventArgs e)
        {
            FeedAnimal(MonkeyList);
            
        }

        private void FeedLion_Click(object sender, RoutedEventArgs e)
        {
            FeedAnimal(LionList);
            
        }

        private void FeedElephant_Click(object sender, RoutedEventArgs e)
        {
            FeedAnimal(ElephantList);

        }

        
    }

    abstract public class Animal
    {
        public string Name { get; set; }
        public int Energy { get; set; }
        public string Species { get; set; }
        public string Gender { get; set; }

        Random random = new Random();

        public abstract void FeedAnimal(int FeedAmount);

        public abstract void UseEnergy();

        public string GenerateName(List<string> names)
        {
            
            Name = "";
            int nameGenerateAmount = random.Next(3) + 2;
            for (int i = 0; i < nameGenerateAmount; i++)
            {
                int rndNamePiece = random.Next(names.Count);
                Name += names[rndNamePiece];
                if (i == 1 && nameGenerateAmount > 3)
                {
                    Name += " ";
                }
            }
            Name = Name[0].ToString().ToUpper() + Name.Substring(1);

            return Name;
        }
    }

    public class Monkey : Animal
    {

        public Monkey()
        {
            

            Energy = 20;
            Species = "Monkey";

            Random random = new Random();
            if (random.Next(2) == 1)
            {
                Gender = "Male";
            }
            else
            {
                Gender = "Female";
            }

            List<string> names = new List<string> { "bo", "ko", "ji" };
            Name = GenerateName(names);
        }

        public override void FeedAnimal(int FeedAmount)
        {
            Energy = Math.Min(Energy + FeedAmount, 100);
        }

        public override void UseEnergy()
        {
            Energy = Energy - 20;
        }
    }

    public class Lion : Animal
    {

        public Lion()
        {


            Energy = 10;
            Species = "Lion";

            Random random = new Random();
            if (random.Next(2) == 1)
            {
                Gender = "Male";
            }
            else
            {
                Gender = "Female";
            }

            List<string> names = new List<string> { "sim", "ba", "na", "la" };
            Name = GenerateName(names);
        }

        public override void FeedAnimal(int FeedAmount)
        {
            Energy = Math.Min(Energy + FeedAmount, 225);
        }

        public override void UseEnergy()
        {
            Energy = Energy - 50;
        }
    }

    public class Elephant : Animal
    {

        public Elephant()
        {


            Energy = 10;
            Species = "Elephant";

            Random random = new Random();
            if (random.Next(2) == 1)
            {
                Gender = "Male";
            }
            else
            {
                Gender = "Female";
            }

            List<string> names = new List<string> { "to", "mo", "ko" };
            Name = GenerateName(names);
        }

        public override void FeedAnimal(int FeedAmount)
        {
            Energy = Math.Min(Energy + FeedAmount, 750);
        }

        public override void UseEnergy()
        {
            Energy = Energy - 75;
        }
    }
}



