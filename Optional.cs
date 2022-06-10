namespace Optional;

public static class Optional
{
    public static Optional<T> None<T>() => Optional<T>.Empty;

    public static Optional<T> SomeNotNull<T>(this T value) => value is null ? throw new ArgumentNullException(nameof(value)) : new Optional<T>(value);

    public static Optional<T> Some<T>(this T value) => value is null ? Optional.None<T>() : new Optional<T>(value);
}