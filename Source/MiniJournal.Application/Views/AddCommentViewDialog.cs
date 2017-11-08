using System;

namespace Infotecs.MiniJournal.Application.Views
{
    public class AddCommentViewDialog : IDialogView
    {
        private AddCommentView view;

        public void BindViewModel<TViewModel>(TViewModel viewModel)
        {
            GetDialog().DataContext = viewModel;
        }

        public void ShowDialog()
        {
            GetDialog().ShowDialog();
        }

        private AddCommentView GetDialog()
        {
            if (view == null)
            {
                view = MiniJournalDependencyResolver.Instance().Resolve<AddCommentView>();
                view.Closed += new EventHandler(ViewClosed);
            }
            return view;
        }

        private void ViewClosed(object sender, EventArgs e)
        {
            view = null;
        }

    }
}
