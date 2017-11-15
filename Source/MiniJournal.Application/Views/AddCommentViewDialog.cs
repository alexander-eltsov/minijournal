using System;
using Autofac;

namespace Infotecs.MiniJournal.Application.Views
{
    public class AddCommentViewDialog : IDialogView
    {
        private readonly IComponentContext componentContext;
        private AddCommentView view;

        public AddCommentViewDialog(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

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
                view = componentContext.Resolve<AddCommentView>();
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
