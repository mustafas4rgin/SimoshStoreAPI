namespace SimoshStoreAPI;

public interface IResultService
{
    public int GetUserCount();
    public int GetProductCount();
    public int GetCategoryCount();
    int GetOrderCount();
    int GetProductCommentCount();
    int GetBlogCommentCount();
}
