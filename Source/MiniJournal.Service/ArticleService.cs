﻿using System;
using System.Collections.Generic;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;

namespace Infortecs.MiniJournal.Service
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository ArticleRepository { get; }

        public ArticleService(IArticleRepository articleRepository)
        {
            if (articleRepository == null)
            {
                throw new ArgumentNullException(nameof(articleRepository));
            }

            ArticleRepository = articleRepository;
        }

        public IEnumerable<ArticleData> GetAllArticles()
        {
            return ArticleRepository.GetArticles();
        }
    }
}
