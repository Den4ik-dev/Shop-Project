using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Validation;
public interface IProductCommentsService
{
  public Task<Comment?> FindComment(Expression<Func<Comment, bool>> predicate);
  public Task<Comment> AddComment(AddedCommentDto addedComment, int customerId);
  public Task DeleteComment(Comment deletedComment);
  public IEnumerable<CommentDto> GetAllComments(int productId);
  public IEnumerable<CommentDto> GetRangeOfComments(int productId, int limit, int page);
  public int GetCountComments(int productId);
  public Task ChangeComment(Comment initialComment, string content);
}
