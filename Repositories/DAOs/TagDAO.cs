using Repositories.Models;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.DAOs
{
    public class TagDAO
    {
        private static TagDAO? instance;
        private static readonly object instanceLock = new object();

        private TagDAO() { }

        public static TagDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TagDAO();
                    }

                    return instance;
                }
            }
        }

        public List<Tag> GetTags(FunewsManagementContext context)
        {
            return context.Tags.ToList();
        }

        public List<Tag> GetAvailableTags(FunewsManagementContext context)
        {
            return context.Tags
                .Where(t => !t.NewsArticles.Any())
                .ToList();
        }

        public List<int> GetTakenTagIds(FunewsManagementContext context)
        {
            return context.Tags
                .Where(t => t.NewsArticles.Any())
                .Select(t => t.TagId)
                .ToList();
        }

        public List<Tag> GetTagsByIds(FunewsManagementContext context, List<int> tagIds)
        {
            if (tagIds == null || !tagIds.Any())
            {
                return new List<Tag>();
            }

            return context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToList();
        }
    }
}
