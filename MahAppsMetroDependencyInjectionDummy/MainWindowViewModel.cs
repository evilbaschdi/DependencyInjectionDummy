using EvilBaschdi.Core.Wpf;
using EvilBaschdi.Core.Wpf.Mvvm.ViewModel;
using EvilBaschdi.Core.Wpf.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using MahAppsMetroDependencyInjectionDummy.Internal;

namespace MahAppsMetroDependencyInjectionDummy;

public class MainWindowViewModel : ApplicationStyleViewModel
{
    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly IDummyInterface _dummyInterface;

    public MainWindowViewModel([NotNull] IDialogCoordinator dialogCoordinator,
                               [NotNull] IDummyInterface dummyInterface,
                               [NotNull] ISomeOtherInterface someOtherInterface,
                               IApplicationStyle applicationStyle
    )
        : base(applicationStyle)
    {
        _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
        _dummyInterface = dummyInterface ?? throw new ArgumentNullException(nameof(dummyInterface));
        var someOtherInterfaceLocal = someOtherInterface ?? throw new ArgumentNullException(nameof(someOtherInterface));
        MainWindowViewModelClick = new DefaultCommand
                                   {
                                       Command = new RelayCommand(_ => MainWindowViewModelClickCommand())
                                   };
        SomeOtherInstanceClick = new DefaultCommand
                                 {
                                     Command = new RelayCommand(_ => someOtherInterfaceLocal.SomeOtherInstanceClickCommand(this))
                                 };
    }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ICommandViewModel MainWindowViewModelClick { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ICommandViewModel SomeOtherInstanceClick { get; }

    private void MainWindowViewModelClickCommand()
    {
        _dialogCoordinator.ShowMessageAsync(this, "MainWindowViewModel Click",
            $"Current DateTime {_dummyInterface.Value}");
    }
}