

namespace JobFlowProject.Business.Exceptions.BaseExeption;


public class ItemNotFoundException : BaseBusinessException
{
    public ItemNotFoundException(string message = "Item not found.")
        : base(message, "ItemNotFound_404")
    {
    }
}