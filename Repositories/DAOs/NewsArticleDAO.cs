using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.DAOs
{
    public class NewsArticleDAO
    {
        private static NewsArticleDAO? instance;
        private static readonly object instanceLock = new object();

        private NewsArticleDAO() { }

        public static NewsArticleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new NewsArticleDAO();
                    }
                    return instance;
                }
            }
        }

        // Lấy danh sách bài viết (kèm theo thông tin Category, người tạo, và Tags)
        public List<NewsArticle> GetNewsArticles(FunewsManagementContext context)
        {
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .ToList();
        }

        public List<NewsArticle> SearchNewsArticles(FunewsManagementContext context, string keyword)
        {
            var query = context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .AsQueryable();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return query.ToList();
            }

            keyword = keyword.Trim().ToLower();

            return query
                .Where(n => n.NewsArticleId.ToLower().Contains(keyword)
                         || (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword))
                         || n.Headline.ToLower().Contains(keyword)
                         || (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword))
                         || (n.NewsSource != null && n.NewsSource.ToLower().Contains(keyword))
                         || (n.Category != null && n.Category.CategoryName.ToLower().Contains(keyword))
                         || (n.CreatedBy != null && n.CreatedBy.AccountName != null && n.CreatedBy.AccountName.ToLower().Contains(keyword))
                         || n.Tags.Any(t => t.TagName != null && t.TagName.ToLower().Contains(keyword)))
                .ToList();
        }

        public List<NewsArticle> GetNewsArticlesByCreator(FunewsManagementContext context, short accountId)
        {
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.CreatedById == accountId)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }

        public List<NewsArticle> SearchNewsArticlesByCreator(FunewsManagementContext context, short accountId, string keyword)
        {
            var query = context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.CreatedById == accountId);

            if (string.IsNullOrWhiteSpace(keyword))
            {
                return query
                    .OrderByDescending(n => n.CreatedDate)
                    .ToList();
            }

            keyword = keyword.Trim().ToLower();

            return query
                .Where(n => n.NewsArticleId.ToLower().Contains(keyword)
                         || (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword))
                         || n.Headline.ToLower().Contains(keyword)
                         || (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword))
                         || (n.NewsSource != null && n.NewsSource.ToLower().Contains(keyword))
                         || (n.Category != null && n.Category.CategoryName.ToLower().Contains(keyword))
                         || n.Tags.Any(t => t.TagName != null && t.TagName.ToLower().Contains(keyword)))
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }

        public NewsArticle? GetNewsArticleById(FunewsManagementContext context, string id)
        {
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .SingleOrDefault(n => n.NewsArticleId == id);
        }

        public void AddNewsArticle(FunewsManagementContext context, NewsArticle newsArticle)
        {
            context.NewsArticles.Add(newsArticle);
            context.SaveChanges();
        }

        public void UpdateNewsArticle(FunewsManagementContext context, NewsArticle newsArticle)
        {
            var tracked = context.NewsArticles.Local.FirstOrDefault(entry => entry.NewsArticleId == newsArticle.NewsArticleId);
            if (tracked != null && tracked != newsArticle)
            {
                context.Entry(tracked).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            context.NewsArticles.Update(newsArticle);
            context.SaveChanges();
        }

        public void DeleteNewsArticle(FunewsManagementContext context, NewsArticle newsArticle)
        {
            context.NewsArticles.Remove(newsArticle);
            context.SaveChanges();
        }
        // Hàm bổ sung cho chức năng làm Report của Admin
        public List<NewsArticle> GetNewsArticlesByPeriod(FunewsManagementContext context, System.DateTime startDate, System.DateTime endDate)
        {
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }
    }
}
