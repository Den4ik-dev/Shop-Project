using Microsoft.EntityFrameworkCore;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using server.Infrastructure.Validation;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class ProductCommentsService : IProductCommentsService
{
  private ApplicationContext _db;
  public ProductCommentsService(ApplicationContext db) =>
    _db = db;

  public async Task<Comment?> FindComment(Expression<Func<Comment, bool>> predicate) =>
    await _db.Comments.FirstOrDefaultAsync(predicate);

  public async Task<Comment> AddComment(AddedCommentDto addedComment, int customerId)
  {
    Comment comment = _db.Comments.Add(new Comment()
    {
      Content = addedComment.Content,
      ProductId = addedComment.ProductId,
      CustomerId = customerId
    }).Entity;

    await _db.SaveChangesAsync();

    return comment;
  }

  public async Task DeleteComment(Comment deletedComment)
  {
    _db.Comments.Remove(deletedComment);

    await _db.SaveChangesAsync();
  }

  public IEnumerable<CommentDto> GetAllComments(int productId) =>
    _db.Comments
      .Where(c => c.ProductId == productId)
      .OrderByDescending(c => c.Id)
      .Select(c => new CommentDto()
      {
        Id = c.Id,
        Content = c.Content,
        CreationDate = c.CreationDate,
        ProductId = c.ProductId,
        CustomerId = c.CustomerId
      });

  public IEnumerable<CommentDto> GetRangeOfComments(int productId, int limit, int page) =>
    _db.Comments
      .Where(c => c.ProductId == productId)
      .OrderByDescending(c => c.Id)
      .Skip(limit * page)
      .Take(limit)
      .Select(c => new CommentDto()
      {
        Id = c.Id,
        Content = c.Content,
        CreationDate = c.CreationDate,
        ProductId = c.ProductId,
        CustomerId = c.CustomerId
      });

  public int GetCountComments(int productId) => 
    _db.Comments.Count(c => c.ProductId == productId);

  public async Task ChangeComment(Comment initialComment, string content)
  {
    initialComment.Content = content;

    await _db.SaveChangesAsync();
  }
}