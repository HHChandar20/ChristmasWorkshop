using System.Threading.Tasks;

namespace ChristmasWorkshop.BLL.Handlers;

public abstract class ValidationHandler
{
    protected ValidationHandler nextHandler;

    public void SetNext(ValidationHandler handler)
    {
        this.nextHandler = handler;
    }

    public abstract Task<bool> ValidateAsync(double x, double y);
}
