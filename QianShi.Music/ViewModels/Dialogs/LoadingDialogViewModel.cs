using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Services.Dialogs;

using QianShi.Music.Common;

namespace QianShi.Music.ViewModels.Dialogs
{
    public class LoadingDialogViewModel : IDialogHostAware
    {
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }
    }
}