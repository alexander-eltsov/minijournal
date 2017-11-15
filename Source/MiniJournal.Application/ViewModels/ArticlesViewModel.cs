using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Markup;
using Autofac;
using Infotecs.MiniJournal.Application.Commands;
using Infotecs.MiniJournal.Application.Interfaces;
using Infotecs.MiniJournal.Application.Views;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Contracts.Notification;

namespace Infotecs.MiniJournal.Application.ViewModels
{
    public class ArticlesViewModel : BaseViewModel, IDisposable
    {
        private readonly SynchronizationContext synchronizationContext;
        private readonly IArticleService articleService;
        private readonly INotificationService notificationService;
        private readonly IComponentContext componentContext;
        private HeaderData selectedHeader;
        private ArticleData loadedArticle;
        private CommentData selectedArticleComment;

        public ArticlesViewModel(
            IArticleService articleService,
            INotificationService notificationService,
            IComponentContext componentContext)
        {
            if (articleService == null)
            {
                throw new ArgumentNullException(nameof(articleService));
            }

            synchronizationContext = SynchronizationContext.Current;

            this.articleService = articleService;
            this.notificationService = notificationService;
            this.componentContext = componentContext;
            Headers = new ObservableCollection<HeaderData>();

            LoadHeadersCommand = new RelayCommand(parameter => ReloadHeaders());
            AddArticleCommand = new RelayCommand(parameter => AddArticle());
            SaveArticleCommand = new RelayCommand(parameter => SaveSelectedArticle());
            DeleteArticleCommand = new RelayCommand(parameter => DeleteSelectedArticle());
            AddCommentCommand = new RelayCommand(parameter => AddComment());
            DeleteCommentCommand = new RelayCommand(parameter => DeleteComment());

            SubscribeEvents();
        }

        public bool CanModifyArticle => SelectedHeader != null;

        public ObservableCollection<HeaderData> Headers { get; }

        public HeaderData SelectedHeader
        {
            get => selectedHeader;
            set
            {
                selectedHeader = value; 
                OnPropertyChanged();
                try
                {
                    ReloadArticle();
                }
                catch (Exception exception)
                {
                    notificationService.NotifyError(exception.Message);
                }
            }
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

        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            notificationService.Subscribe<ArticleCreatedMessage>(OnArticleCreated);
            notificationService.Subscribe<ArticleUpdatedMessage>(OnArticleUpdated);
            notificationService.Subscribe<ArticleDeletedMessage>(OnArticleDeleted);
            notificationService.Subscribe<CommentMessage>(OnCommentChanged);
        }

        private void UnsubscribeEvents()
        {
            notificationService.Unsubscribe<ArticleCreatedMessage>(OnArticleCreated);
            notificationService.Unsubscribe<ArticleUpdatedMessage>(OnArticleUpdated);
            notificationService.Unsubscribe<ArticleDeletedMessage>(OnArticleDeleted);
            notificationService.Unsubscribe<CommentMessage>(OnCommentChanged);
        }

        private void OnArticleCreated(ArticleCreatedMessage message)
        {
            synchronizationContext.Post(state => ReloadHeaders(), null);
        }

        private void OnArticleUpdated(ArticleUpdatedMessage message)
        {
            synchronizationContext.Post(state =>
            {
                var msg = (ArticleUpdatedMessage) state;
                if (loadedArticle != null && loadedArticle.Id == msg.ArticleId)
                {
                    ReloadArticle();
                }
                ReloadHeaders();
            }, message);
        }

        private void OnArticleDeleted(ArticleDeletedMessage message)
        {
            synchronizationContext.Post(state =>
            {
                var msg = (ArticleDeletedMessage) state;
                if (loadedArticle != null && loadedArticle.Id == msg.ArticleId)
                {
                    LoadedArticle = null;
                }
                ReloadHeaders();
            }, message);
        }

        private void OnCommentChanged(CommentMessage message)
        {
            synchronizationContext.Post(state =>
            {
                var msg = (CommentMessage) state;
                if (loadedArticle != null && loadedArticle.Id == msg.ParentId)
                {
                    ReloadArticle();
                }
            }, message);
        }

        private void ReloadArticle()
        {
            LoadedArticle = selectedHeader == null
                ? null
                : articleService.GetArticle(selectedHeader.Id);
        }

        private void ReloadHeaders()
        {
            try
            {
                var headerBeforeRefresh = SelectedHeader;

                Headers.Clear();
                IEnumerable<HeaderData> headersFromService = articleService.GetArticleHeaders();
                foreach (HeaderData header in headersFromService)
                {
                    Headers.Add(header);
                }

                SelectedHeader = headerBeforeRefresh == null
                    ? Headers.FirstOrDefault()
                    : Headers.FirstOrDefault(header => header.Id == headerBeforeRefresh.Id);
            }
            catch (Exception exception)
            {
                notificationService.NotifyError(exception.Message);
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
            }
            catch (Exception exception)
            {
                notificationService.NotifyError(exception);
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
                notificationService.NotifyError(exception.Message);
            }
        }

        private void DeleteSelectedArticle()
        {
            try
            {
                articleService.DeleteArticle(LoadedArticle.Id);
            }
            catch (Exception exception)
            {
                notificationService.NotifyError(exception.Message);
            }
        }

        private void AddComment()
        {
            var viewModel = componentContext.Resolve<AddCommentViewModel>();
            var view = componentContext.Resolve<IDialogView>();

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
                notificationService.NotifyError(exception.Message);
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
                notificationService.NotifyError(exception.Message);
            }
        }
    }
}
