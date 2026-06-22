using Repositories.DAOs;
using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly FunewsManagementContext _context;

        public TagRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public List<Tag> GetTags()
            => TagDAO.Instance.GetTags(_context);

        public List<Tag> GetAvailableTags()
            => TagDAO.Instance.GetAvailableTags(_context);

        public List<int> GetTakenTagIds()
            => TagDAO.Instance.GetTakenTagIds(_context);

        public List<Tag> GetTagsByIds(List<int> tagIds)
            => TagDAO.Instance.GetTagsByIds(_context, tagIds);
    }
}
