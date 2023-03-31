using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthLibrary.Feature.Session;

public sealed class SessionBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (!bindingContext.IsTopLevelObject)
        {
            throw new InvalidOperationException("Is not TopLevelObject");
        }

        var session = bindingContext.HttpContext.Features.Get<SessionInfo>();
        bindingContext.Result = ModelBindingResult.Success(session);
        return Task.CompletedTask;
    }
}