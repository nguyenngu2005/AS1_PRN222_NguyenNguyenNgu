using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public interface INewsArticleRepository
    {
        List<NewsArticle> GetNewsArticles();
        List<NewsArticle> SearchNewsArticles(string keyword);
        NewsArticle? GetNewsArticleById(string id);
        void AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(NewsArticle newsArticle);
        List<NewsArticle> GetNewsArticlesByCreator(short accountId);
        List<NewsArticle> SearchNewsArticlesByCreator(short accountId, string keyword);
        List<NewsArticle> GetNewsArticlesByPeriod(System.DateTime startDate, System.DateTime endDate);
    }
}
