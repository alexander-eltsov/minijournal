using System;

namespace Infotecs.MiniJournal.Application.Views
{
    public interface IDialogView
    {
        void BindViewModel<TViewModel>(TViewModel viewModel);

        void ShowDialog();
    }
}
