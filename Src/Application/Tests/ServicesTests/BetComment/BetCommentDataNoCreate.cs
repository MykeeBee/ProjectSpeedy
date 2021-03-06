using System.Threading.Tasks;
using ProjectSpeedy.Models.BetComment;
using ProjectSpeedy.Services;

namespace ProjectSpeedy.Tests.ServicesTests
{
    public class BetCommentData : IBetComment
    {
        /// <inheritdoc />
        public Task<bool> CreateAsync(string projectId, string problemId, string betId, BetCommentNewUpdate form)
        {
            return Task.FromResult(true);
        }

        /// <inheritdoc />
        public bool Delete(string projectId, string problemId, string betId, string commentId)
        {
            return true;
        }

        /// <inheritdoc />
        public bool Update(string projectId, string problemId, string betId, BetCommentNewUpdate form)
        {
            return true;
        }
    }
}