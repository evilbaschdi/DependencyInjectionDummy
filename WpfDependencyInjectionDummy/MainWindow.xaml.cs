using System.Windows;
using WpfDependencyInjectionDummy.Internal;

namespace WpfDependencyInjectionDummy
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDummyInterface _dummyInterface;

        public MainWindow(IDummyInterface dummyInterface)
        {
            InitializeComponent();
            _dummyInterface = dummyInterface;
        }

        private void DummyButtonOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_dummyInterface.Value);
        }
    }
}