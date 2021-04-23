using System;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using MahAppsMetroDependencyInjectionDummy.Internal;

namespace MahAppsMetroDependencyInjectionDummy
{
    public class MainWindowViewModel : ApplicationStyleViewModel
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IDummyInterface _dummyInterface;
        private readonly ISomeOtherInterface _someOtherInterface;

        public MainWindowViewModel([NotNull] IDialogCoordinator dialogCoordinator, [NotNull] IDummyInterface dummyInterface, [NotNull] ISomeOtherInterface someOtherInterface)
            : base(true)
        {
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _dummyInterface = dummyInterface ?? throw new ArgumentNullException(nameof(dummyInterface));
            _someOtherInterface = someOtherInterface ?? throw new ArgumentNullException(nameof(someOtherInterface));
            MainWindowViewModelClick = new DefaultCommand
                                       {
                                           Command = new RelayCommand(_ => MainWindowViewModelClickCommand())
                                       };
            SomeOtherInstanceClick = new DefaultCommand
                                     {
                                         Command = new RelayCommand(_ => _someOtherInterface.SomeOtherInstanceClickCommand(this))
                                     };
        }

        public ICommandViewModel MainWindowViewModelClick { get; set; }

        public ICommandViewModel SomeOtherInstanceClick { get; set; }


        private void MainWindowViewModelClickCommand()
        {
            _dialogCoordinator.ShowMessageAsync(this, "MainWindowViewModel Click",
                $"Current DateTime {_dummyInterface.Value}");
        }
    }

    public interface ISomeOtherInterface
    {
        void SomeOtherInstanceClickCommand(object context);
    }

    public class SomeOtherClass : ISomeOtherInterface
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        public SomeOtherClass([NotNull] IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
        }

        public void SomeOtherInstanceClickCommand(object context)
        {
            _dialogCoordinator.ShowMessageAsync(context, "SomeOtherInstance Click",
                "some stupid content");
        }
    }
}