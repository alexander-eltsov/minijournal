using System;
using System.Windows.Input;
using Infotecs.MiniJournal.Application.Commands;

namespace Infotecs.MiniJournal.Application.ViewModels
{
    public class AddCommentViewModel : BaseViewModel
    {
        private string user;
        private string comment;

        public AddCommentViewModel()
        {
            AddCommand = new RelayCommand(parameter => Add());
            CancelCommand = new RelayCommand(parameter => Cancel());
        }

        public string User
        {
            get => user;
            set
            {
                user = value; 
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get => comment;
            set
            {
                comment = value; 
                OnPropertyChanged();
            }
        }

        public bool DialogResult { get; private set; }

        public ICommand AddCommand { get; }

        public ICommand CancelCommand { get; }

        private void Add()
        {
            DialogResult = true;
        }

        private void Cancel()
        {
            DialogResult = false;
        }
    }
}
