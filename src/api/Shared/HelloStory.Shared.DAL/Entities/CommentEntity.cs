using HelloStory.Shared.DAL.Entities.Abstract;

namespace HelloStory.Shared.DAL.Entities;

internal sealed record CommentEntity(Guid UserId) : EntityBase
{
    public Guid PostId { get; init; }
    public PostEntity? Post { get; init; }
    public Guid ParentCommentId { get; init; }
    public CommentEntity? ParentComment { get; init; }
}