using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using QianShi.Music.Common;

namespace QianShi.Music.ViewModels.Dialogs
{
    public class DescriptionDialogViewModel : BindableBase, IDialogHostAware
    {
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        private string _description;

        public string Description { get => _description; set => SetProperty(ref _description, value); }

        public string Title => throw new NotImplementedException();

        public DescriptionDialogViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }

        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Description"))
            {
                var description = parameters.GetValue<string>("Description");
                Description = description;
            }
        }
    }
}