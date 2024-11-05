namespace Counter
{
    public partial class MainPage
    {
        private string filePath = Path.Combine(FileSystem.AppDataDirectory, "counters.txt");
        public List<Counter> Counters { get; } = new List<Counter>();

        public MainPage()
        {
            InitializeComponent();
            LoadCounters();
            CounterListView.ItemsSource = Counters;
        }

        private void LoadCounters()
        {
            if (File.Exists(filePath))
                {
                var lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(';');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                        {
                            Counters.Add(new Counter(parts[0], count));
                        }
                    }
                }
        }

        private void SaveCounters()
        {
            var counterStrings = Counters.Select(c => $"{c.Name};{c.Count}");
            File.WriteAllLines(filePath, counterStrings);
           
        }

        private void OnAddCounterClicked(object sender, EventArgs e)
        {
            string name = CounterNameEntry.Text;
            string initialValue = InitialValueEntry.Text;

            if (!string.IsNullOrEmpty(name))
            {
                int startValue = 0;
                if (!string.IsNullOrEmpty(initialValue) && int.TryParse(initialValue, out int value))
                {
                    startValue = value;
                }

                Counters.Add(new Counter(name, startValue));
                CounterNameEntry.Text = "";
                InitialValueEntry.Text = "";
                UpdateListView();
                SaveCounters();
            }
        }

        private void OnIncreaseClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string name)
            {
                var counter = Counters.FirstOrDefault(c => c.Name == name);
                if (counter != null)
                {
                    counter.Count++;
                    UpdateListView();
                    SaveCounters();
                }
            }
        }

        private void OnDecreaseClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string name)
            {
                var counter = Counters.FirstOrDefault(c => c.Name == name);
                if (counter != null)
                {
                    counter.Count--;
                    UpdateListView();
                    SaveCounters();
                }
            }
        }

        private void UpdateListView()
        {
            CounterListView.ItemsSource = null;
            CounterListView.ItemsSource = Counters;
        }
    }

    public class Counter
    {
        public string Name { get; }
        public int Count { get; set; }
        public string DisplayText => $"Licznik '{Name}': {Count}";

        public Counter(string name, int initialCount = 0)
        {
            Name = name;
            Count = initialCount;
        }
    }
}