using Repositories.Models;
using Repositories.Repositories;
using System.Collections.Generic;

namespace Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;

        public TagService(ITagRepository repository)
        {
            _repository = repository;
        }

        public List<Tag> GetTags()
            => _repository.GetTags();

        public List<Tag> GetAvailableTags()
            => _repository.GetAvailableTags();

        public List<int> GetTakenTagIds()
            => _repository.GetTakenTagIds();

        public List<Tag> GetTagsByIds(List<int> tagIds)
            => _repository.GetTagsByIds(tagIds);
    }
}
