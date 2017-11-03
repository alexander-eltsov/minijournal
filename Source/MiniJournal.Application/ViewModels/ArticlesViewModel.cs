using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Infotecs.MiniJournal.Application.ArticleServiceReference;
using Infotecs.MiniJournal.Application.Commands;

namespace Infotecs.MiniJournal.Application.ViewModels
{
    public class ArticlesViewModel : BaseViewModel
    {
        private readonly IArticleService articleService;
        private ArticleData selectedArticle;

        public ArticlesViewModel(IArticleService articleService)
        {
            if (articleService == null)
            {
                throw new ArgumentNullException(nameof(articleService));
            }

            this.articleService = articleService;
            Articles = new ObservableCollection<ArticleData>();

            LoadArticlesCommand = new RelayCommand(parameter => LoadArticles());
            AddArticleCommand = new RelayCommand(parameter => AddArticle());
            SaveArticleCommand = new RelayCommand(parameter => SaveSelectedArticle());
            DeleteArticleCommand = new RelayCommand(parameter => DeleteSelectedArticle());
        }

        public bool CanModifyArticle => SelectedArticle != null;

        public ICollection<ArticleData> Articles { get; }

        public ArticleData SelectedArticle
        {
            get => selectedArticle;
            set
            {
                selectedArticle = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanModifyArticle));
            }
        }

        public ICommand LoadArticlesCommand { get; }

        public ICommand AddArticleCommand { get; }

        public ICommand SaveArticleCommand { get; }

        public ICommand DeleteArticleCommand { get; }

        private void LoadArticles()
        {
            Articles.Clear();
            foreach (ArticleData articleData in articleService.GetAllArticles())
            {
                Articles.Add(articleData);
            }
        }

        private void AddArticle()
        {
            var newArticle = new ArticleData
            {
                Caption = "New Article",
                Text = string.Empty
            };

            articleService.CreateArticle(newArticle);

            Articles.Add(newArticle);
        }

        private void SaveSelectedArticle()
        {
            articleService.UpdateArticle(SelectedArticle);
        }

        private void DeleteSelectedArticle()
        {
            articleService.DeleteArticle(SelectedArticle);
            LoadArticles();
        }
    }
}
