using QianShi.Music.Common;

namespace QianShi.Music.ViewModels.Dialogs
{
    public class LoadingDialogViewModel : IDialogHostAware
    {
        public string? DialogHostName { get; set; }

        public LoadingDialogViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
            SaveCommand = new DelegateCommand(Save);
        }

        public void OnDialogOpend(IDialogParameters parameters)
        {
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }

        private void Save()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Yes));
        }
    }
}