using System.Threading.Tasks;

namespace ChristmasWorkshop.BLL.Handlers;

public abstract class ValidationHandler
{
    protected ValidationHandler NextHandler;

    public void SetNext(ValidationHandler handler)
    {
        NextHandler = handler;
    }

    public abstract Task<bool> ValidateAsync(double x, double y);
}
