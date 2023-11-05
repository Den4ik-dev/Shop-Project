using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;
using server.Infrastructure.Validation;

namespace server.Web.Controllers;
[ApiController, Route("api/products/comments")]
public class ProductCommentsController : ControllerBase
{
  private IProductsService _productsService;
  private IProductCommentsService _productCommentsService;
  public ProductCommentsController(IProductsService productsService, 
    IProductCommentsService productCommentsService)
  {
    _productsService = productsService;
    _productCommentsService = productCommentsService;
  }


  [HttpPost, Authorize]
  public async Task<IActionResult> Add([FromBody] AddedCommentDto addedComment,
    [FromServices] IValidator<AddedCommentDto> addedCommentValidator)
  {
    var addedCommentValidatorResult = addedCommentValidator.Validate(addedComment);
    if (!addedCommentValidatorResult.IsValid)
      return BadRequest(new { Message = addedCommentValidatorResult.Errors.First().ErrorMessage });

    Product? product = await _productsService.FindProduct(p => p.Id == addedComment.ProductId);

    if (product == null)
      return BadRequest(new { Message = "Продукт с данным идентификатором не найден" });

    int customerId = int.Parse(User.Identity.Name);
    Comment comment = await _productCommentsService.AddComment(addedComment, customerId);

    return Ok(new CommentDto()
    {
      Id = comment.Id,
      Content = comment.Content,
      CreationDate = comment.CreationDate,
      ProductId = comment.ProductId,
      CustomerId = comment.CustomerId
    });
  }

  [HttpDelete("{id:int}"), Authorize]
  public async Task<IActionResult> Delete(int id)
  {
    Comment? comment = await _productCommentsService.FindComment(c => c.Id == id);

    if (comment == null)
      return NotFound(new { Message = "Комментарий не найден" });

    if (comment.CustomerId != int.Parse(User.Identity.Name))
      return BadRequest(new { Message = "Вам не принадлежит данный комментарий" });

    await _productCommentsService.DeleteComment(comment);

    return Ok(new { Message = "Комментарий успешно удалён" });
  }

  [HttpGet, Route("all")]
  public IActionResult GetAll(int productId) =>
    Ok(_productCommentsService.GetAllComments(productId));

  [HttpGet]
  public IActionResult GetRange(int productId, int limit, int page)
  {
    Response.Headers.Add(
      "x-total-count",
      _productCommentsService.GetCountComments(productId).ToString());

    return Ok(_productCommentsService.GetRangeOfComments(productId, limit, page));
  }

  [HttpPut("{id:int}"), Authorize]
  public async Task<IActionResult> Change(int id,
    [FromBody] ChangedCommentDto changedComment,
    [FromServices] IValidator<ChangedCommentDto> changedCommentValidator)
  {
    var changedCommentValidatorResult = changedCommentValidator.Validate(changedComment);
    if (!changedCommentValidatorResult.IsValid)
      return BadRequest(new { Message = changedCommentValidatorResult.Errors.First().ErrorMessage });

    Comment? comment = await _productCommentsService.FindComment(c => c.Id == id);

    if (comment == null)
      return NotFound(new { Message = "Комментарий не найден" });

    if (comment.CustomerId != int.Parse(User.Identity.Name))
      return BadRequest(new { Message = "Вам не принадлежит данный комментарий" });

    await _productCommentsService.ChangeComment(comment, changedComment.Content);

    return Ok(new { Message = "Комментарий успешно изменён" });
  }

  [HttpDelete("admin/{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> AdminDelete(int id)
  {
    Comment? comment = await _productCommentsService.FindComment(c => c.Id == id);

    if (comment == null)
      return NotFound(new { Message = "Комментарий не найден" });

    await _productCommentsService.DeleteComment(comment);

    return Ok(new { Message = "Комментарий успешно удалён" });
  }

  [HttpPut("admin/{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> AdminChange(int id,
    [FromBody] ChangedCommentDto changedComment,
    [FromServices] IValidator<ChangedCommentDto> changedCommentValidator)
  {
    var changedCommentValidatorResult = changedCommentValidator.Validate(changedComment);
    if (!changedCommentValidatorResult.IsValid)
      return BadRequest(new { Message = changedCommentValidatorResult.Errors.First().ErrorMessage });

    Comment? comment = await _productCommentsService.FindComment(c => c.Id == id);

    if (comment == null)
      return NotFound(new { Message = "Комментарий не найден" });

    await _productCommentsService.ChangeComment(comment, changedComment.Content);

    return Ok(new { Message = "Комментарий успешно изменён" });
  }
}