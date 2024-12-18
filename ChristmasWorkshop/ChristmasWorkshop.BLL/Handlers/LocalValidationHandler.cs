using System.Threading.Tasks;

namespace ChristmasWorkshop.BLL.Handlers;

public class LocalValidationHandler : ValidationHandler
{
    public override Task<bool> ValidateAsync(double x, double y)
    {
        bool isValid = (170.3 - y) >= (1.357 * Math.Abs(x - 62.9));
        if (isValid && NextHandler != null)
        {
            return NextHandler.ValidateAsync(x, y);
        }
        return Task.FromResult(isValid);
    }
}