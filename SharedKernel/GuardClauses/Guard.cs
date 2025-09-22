namespace SharedKernel.GuardClauses;

public static class Guard
{
    public static T NotNull<T>(T? value, string paramName) where T : class =>
        value ?? throw new ArgumentNullException(paramName);
}