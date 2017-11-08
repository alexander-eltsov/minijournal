﻿using System;
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

                LoadedArticle = selectedHeader == null
                    ? null
                    : articleService.GetArticle(selectedHeader.Id);
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

        private void LoadHeaders()
        {
            Headers.Clear();
            var headersFromService = new HeaderData[0];

            try
            {
                headersFromService = articleService.GetArticleHeaders();
            }
            catch (Exception exception)
            {
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }

            foreach (HeaderData header in headersFromService)
            {
                Headers.Add(header);
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
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }

            LoadHeaders();
        }

        private void SaveSelectedArticle()
        {
            try
            {
                articleService.UpdateArticle(LoadedArticle);
            }
            catch (Exception exception)
            {
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }

            LoadHeaders();
        }

        private void DeleteSelectedArticle()
        {
            try
            {
                articleService.DeleteArticle(LoadedArticle.Id);
            }
            catch (EndpointNotFoundException exception)
            {
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }

            LoadHeaders();
        }

        private void AddComment()
        {
            throw new NotImplementedException();
        }

        private void DeleteComment()
        {
            try
            {
                articleService.RemoveComment(LoadedArticle.Id, SelectedArticleComment.Id);
            }
            catch (EndpointNotFoundException exception)
            {
                logger.LogError(exception);
                // TODO: inform user, consider using some IDialogService
            }

            LoadHeaders();
        }
    }
}
