using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Infotecs.MiniJournal.Application.Commands;
using Infotecs.MiniJournal.Application.Views;
using Infotecs.MiniJournal.Contracts;

namespace Infotecs.MiniJournal.Application.ViewModels
{
    public class ArticlesViewModel : BaseViewModel
    {
        private readonly IArticleService articleService;
        private readonly ILogger logger;
        private HeaderData selectedHeader;
        private ArticleData loadedArticle;
        private CommentData selectedArticleComment;

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
            Headers = new ObservableCollection<HeaderData>();

            LoadHeadersCommand = new RelayCommand(parameter => LoadHeaders());
            AddArticleCommand = new RelayCommand(parameter => AddArticle());
            SaveArticleCommand = new RelayCommand(parameter => SaveSelectedArticle());
            DeleteArticleCommand = new RelayCommand(parameter => DeleteSelectedArticle());
            AddCommentCommand = new RelayCommand(parameter => AddComment());
            DeleteCommentCommand = new RelayCommand(parameter => DeleteComment());
        }

        public bool CanModifyArticle => SelectedHeader != null;

        public ICollection<HeaderData> Headers { get; }

        public HeaderData SelectedHeader
        {
            get => selectedHeader;
            set
            {
                selectedHeader = value; 
                OnPropertyChanged();
                ReloadArticle();
            }
        }

        private void ReloadArticle()
        {
            LoadedArticle = selectedHeader == null
                ? null
                : articleService.GetArticle(selectedHeader.Id);
        }

        public ArticleData LoadedArticle
        {
            get => loadedArticle;
            set
            {
                loadedArticle = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanModifyArticle));
            }
        }

        public CommentData SelectedArticleComment
        {
            get => selectedArticleComment;
            set
            {
                selectedArticleComment = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadHeadersCommand { get; }

        public ICommand AddArticleCommand { get; }

        public ICommand SaveArticleCommand { get; }

        public ICommand DeleteArticleCommand { get; }

        public ICommand AddCommentCommand { get; }

        public ICommand DeleteCommentCommand { get; }

        private void LoadHeaders()
        {
            try
            {
                Headers.Clear();

                IEnumerable<HeaderData> headersFromService = articleService.GetArticleHeaders();

                foreach (HeaderData header in headersFromService)
                {
                    Headers.Add(header);
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception);
                MiniJournalDependencyResolver.Instance().Resolve<INotificationService>().NotifyError(exception.Message);
            }
        }

        private void AddArticle()
        {
            var newArticle = new ArticleData
            {
                Caption = $"New Article {Headers.Count + 1}",
                Text = string.Empty
            };

            try
            {
                articleService.CreateArticle(newArticle);

                LoadHeaders();
            }
            catch (Exception exception)
            {
                logger.LogError(exception);
                MiniJournalDependencyResolver.Instance().Resolve<INotificationService>().NotifyError(exception.Message);
            }
        }

        private void SaveSelectedArticle()
        {
            try
            {
                articleService.UpdateArticle(LoadedArticle);

                ReloadArticle();
            }
            catch (Exception exception)
            {
                logger.LogError(exception);
                MiniJournalDependencyResolver.Instance().Resolve<INotificationService>().NotifyError(exception.Message);
            }
        }

        private void DeleteSelectedArticle()
        {
            try
            {
                articleService.DeleteArticle(LoadedArticle.Id);

                LoadHeaders();
            }
            catch (Exception exception)
            {
                logger.LogError(exception);
                MiniJournalDependencyResolver.Instance().Resolve<INotificationService>().NotifyError(exception.Message);
            }
        }

        private void AddComment()
        {
            var viewModel = MiniJournalDependencyResolver.Instance().Resolve<AddCommentViewModel>();
            var view = MiniJournalDependencyResolver.Instance().Resolve<IDialogView>();

            viewModel.User = Environment.UserName;
            view.BindViewModel(viewModel);
            view.ShowDialog();

            if (!viewModel.DialogResult)
            {
                return;
            }

            try
            {
                var newComment = new CommentData
                {
                    User = viewModel.User,
                    Text = viewModel.Comment
                };
                articleService.AddComment(LoadedArticle.Id, newComment);

                ReloadArticle();
            }
            catch (Exception exception)
            {
                logger.LogError(exception);
                MiniJournalDependencyResolver.Instance().Resolve<INotificationService>().NotifyError(exception.Message);
            }
        }

        private void DeleteComment()
        {
            try
            {
                articleService.RemoveComment(LoadedArticle.Id, SelectedArticleComment.Id);

                ReloadArticle();
            }
            catch (Exception exception)
            {
                logger.LogError(exception);
                MiniJournalDependencyResolver.Instance().Resolve<INotificationService>().NotifyError(exception.Message);
            }
        }
    }
}
