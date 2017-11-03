using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Infotecs.MiniJournal.Application.ArticleServiceReference;
using Infotecs.MiniJournal.Application.Commands;

namespace Infotecs.MiniJournal.Application.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IArticleService articleService;

        public MainViewModel(IArticleService articleService)
        {
            if (articleService == null)
            {
                throw new ArgumentNullException(nameof(articleService));
            }

            this.articleService = articleService;
            Articles = new ObservableCollection<ArticleData>();
            LoadArticlesCommand = new RelayCommand(parameter => LoadArticles());
        }

        public ICollection<ArticleData> Articles { get; }

        public ICommand LoadArticlesCommand { get; private set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadArticles()
        {
            foreach (ArticleData articleData in articleService.GetAllArticles())
            {
                Articles.Add(articleData);
            }
        }
    }
}
