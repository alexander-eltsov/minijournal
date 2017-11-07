using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Windows.Input;
using Infotecs.MiniJournal.Application.ArticleServiceReference;
using Infotecs.MiniJournal.Application.Commands;

namespace Infotecs.MiniJournal.Application.ViewModels
{
    public class ArticlesViewModel : BaseViewModel
    {
        private readonly IArticleService articleService;
        private readonly ILogger logger;
        private ArticleData selectedArticle;

        public ArticlesViewModel(
            IArticleService articleService,
            ILogger logger)
        {
            if (articleService == null)
            {
                throw new ArgumentNullException(nameof(articleService));
            }

            this.articleService = articleService;
            this.logger = logger;
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
            var articlesFromService = new ArticleData[0];

            try
            {
                articlesFromService = articleService.GetAllArticles();
            }
            catch (EndpointNotFoundException exception)
            {
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }

            foreach (ArticleData articleData in articlesFromService)
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

            try
            {
                articleService.CreateArticle(newArticle);
            }
            catch (EndpointNotFoundException exception)
            {
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }

            Articles.Add(newArticle);
        }

        private void SaveSelectedArticle()
        {
            try
            {
                articleService.UpdateArticle(SelectedArticle);
            }
            catch (EndpointNotFoundException exception)
            {
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }
        }

        private void DeleteSelectedArticle()
        {
            try
            {
                articleService.DeleteArticle(SelectedArticle.Id);
            }
            catch (EndpointNotFoundException exception)
            {
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }

            LoadArticles();
        }
    }
}
