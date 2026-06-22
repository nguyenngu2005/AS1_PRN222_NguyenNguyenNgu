using Repositories.Models;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetTags();
        List<Tag> GetAvailableTags();
        List<int> GetTakenTagIds();
        List<Tag> GetTagsByIds(List<int> tagIds);
    }
}
