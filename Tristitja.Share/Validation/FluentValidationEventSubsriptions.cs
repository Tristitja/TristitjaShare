using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;

namespace Tristitja.Share.Validation;

internal sealed class FluentValidationEventSubsriptions : IDisposable
{
    private readonly EditContext _editContext;
    private readonly ValidationMessageStore _messageStore;
    private readonly IValidator _validator;
    
    public FluentValidationEventSubsriptions(EditContext editContext, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(editContext);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        
        _editContext = editContext;
        _messageStore = new ValidationMessageStore(editContext);
        
        var modelType = editContext.Model.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(modelType);
        _validator = (IValidator)serviceProvider.GetRequiredService(validatorType);
        
        _editContext.OnFieldChanged += OnFieldChanged;
        _editContext.OnValidationRequested += OnValidationRequested;
    }

    private void OnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        // FluentValidations does not support single property validation as far as I know
        // So on field change I will just validate entire model and add only revelant messages for given field.
        // I can't add all of them because then it will add validation messages for not touched field, which is not
        // poggers at all.
        
        var validationResult = _validator.Validate(new ValidationContext<object>(_editContext.Model));

        _messageStore.Clear(e.FieldIdentifier);

        foreach (var error in validationResult.Errors)
        {
            if (error.PropertyName == e.FieldIdentifier.FieldName)
            {
                _messageStore.Add(e.FieldIdentifier, error.ErrorMessage);
            }
        }
        
        _editContext.NotifyValidationStateChanged();
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        var validationResult = _validator.Validate(new ValidationContext<object>(_editContext.Model));

        _messageStore.Clear();

        foreach (var error in validationResult.Errors)
        {
            _messageStore.Add(_editContext.Field(error.PropertyName), error.ErrorMessage);
        }
        
        _editContext.NotifyValidationStateChanged();
    }
    
    public void Dispose()
    {
        _messageStore.Clear();
        _editContext.OnFieldChanged -= OnFieldChanged;
        _editContext.OnValidationRequested -= OnValidationRequested;
        _editContext.NotifyValidationStateChanged();
    }
}