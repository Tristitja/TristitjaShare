using Microsoft.AspNetCore.Components.Forms;

namespace Tristitja.Share.Validation;

public static class EditContextValidationExtensions
{
    extension(EditContext editContext)
    {
        public IDisposable EnableFluentValidation(IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));
            return new FluentValidationEventSubsriptions(editContext, serviceProvider);
        }
    }
}
