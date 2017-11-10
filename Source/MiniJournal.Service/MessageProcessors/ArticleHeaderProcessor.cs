using System;
using System.Collections.Generic;
using Infotecs.MiniJournal.Contracts;
using Infotecs.MiniJournal.Dal;
using Infotecs.MiniJournal.Models;
using Nelibur.ServiceModel.Services.Operations;

namespace Infotecs.MiniJournal.Service.MessageProcessors
{
    public sealed class ArticleHeaderProcessor : IGet<GetArticleHeadersRequest>
    {
        private readonly IArticleRepository articleRepository;
        private readonly IMapper mapper;

        public ArticleHeaderProcessor(
            IArticleRepository articleRepository,
            IMapper mapper)
        {
            this.articleRepository = articleRepository;
            this.mapper = mapper;
        }

        public object Get(GetArticleHeadersRequest request)
        {
            IList<Header> headers = articleRepository.GetHeaders();
            IEnumerable<HeaderData> headerDatas = mapper.Map<IList<Header>, IEnumerable<HeaderData>>(headers);

            return new GetArticleHeadersResponse
            {
                Headers = headerDatas
            };
        }
    }
}
