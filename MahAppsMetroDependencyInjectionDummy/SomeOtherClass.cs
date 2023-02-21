using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;

namespace MahAppsMetroDependencyInjectionDummy;

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