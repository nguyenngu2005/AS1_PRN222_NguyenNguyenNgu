using Repositories.Models;
using System.Collections.Generic;

namespace Services
{
    public interface INewsArticleService
    {
        List<NewsArticle> GetNewsArticles();
        List<NewsArticle> SearchNewsArticles(string keyword);
        NewsArticle? GetNewsArticleById(string id);
        void AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(string id);
        List<NewsArticle> GetNewsArticlesByCreator(short accountId);
        List<NewsArticle> SearchNewsArticlesByCreator(short accountId, string keyword);
        List<NewsArticle> GetNewsArticlesByPeriod(System.DateTime startDate, System.DateTime endDate);
    }
}
