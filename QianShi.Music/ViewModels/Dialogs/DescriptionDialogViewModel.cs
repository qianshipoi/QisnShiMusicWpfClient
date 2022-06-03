using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using QianShi.Music.Common;

namespace QianShi.Music.ViewModels.Dialogs
{
    public class DescriptionDialogViewModel : BindableBase, IDialogHostAware
    {
        public const string DescriptionParameterName = "Description";

        private string _description;
        public DescriptionDialogViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
        }

        public DelegateCommand CancelCommand { get; set; }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string DialogHostName { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public string Title => throw new NotImplementedException();
        public void OnDialogOpend(IDialogParameters parameters)
        {
            if (parameters.ContainsKey(DescriptionParameterName))
            {
                var description = parameters.GetValue<string>(DescriptionParameterName);
                Description = description;
            }
        }

        private void Cancel()
        {
            if (DialogHost.IsDialogOpen(DialogHostName))
                DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
        }
    }
}