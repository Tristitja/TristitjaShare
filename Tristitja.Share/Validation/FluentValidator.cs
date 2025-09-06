using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Tristitja.Share.Validation;

public sealed class FluentValidator : ComponentBase, IDisposable
{
    private IDisposable? _subscription;
    private EditContext? _originalEditContext;
    
    [CascadingParameter] private EditContext? CurrentEditContext { get; set; }

    [Inject] private IServiceProvider ServiceProvider { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(FluentValidator)} requires a cascading " +
                                                $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(FluentValidator)} " +
                                                $"inside an EditForm.");
        }

        _subscription = CurrentEditContext.EnableFluentValidation(ServiceProvider);
        _originalEditContext = CurrentEditContext;
    }

    protected override void OnParametersSet()
    {
        if (CurrentEditContext != _originalEditContext)
        {
            throw new InvalidOperationException($"{GetType()} does not support changing the " +
                                                $"{nameof(EditContext)} dynamically.");
        }
    }

    void IDisposable.Dispose()
    {
        _subscription?.Dispose();
        _subscription = null;
    }
}
