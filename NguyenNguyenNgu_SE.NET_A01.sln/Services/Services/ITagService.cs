using Repositories.Models;
using System.Collections.Generic;

namespace Services
{
    public interface ITagService
    {
        List<Tag> GetTags();
        List<Tag> GetAvailableTags();
        List<int> GetTakenTagIds();
        List<Tag> GetTagsByIds(List<int> tagIds);
    }
}
