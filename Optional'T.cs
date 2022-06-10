using System;
using System.Collections;
using System.Collections.Generic;

namespace Optional;

public class Optional<T>
{
    private readonly T value;
    private readonly bool hasValue;

    private static readonly Optional<T> empty = new Optional<T>();
    public static Optional<T> Empty => empty;

    public Optional(T value)
    {
        this.value = value;
        this.hasValue = true;
    }

    private Optional()
    {
        this.value = default;
        this.hasValue = false;
    }

    public Optional<T> Or(T value) => hasValue ? this : new Optional<T>(value);

    public Optional<T> Or(Func<T> supplier) => hasValue ? this : new Optional<T>(supplier());

    public T ValueOr(T value) => hasValue ? this.value : value;

    public T ExpectValue() => hasValue ? value : throw new InvalidOperationException();

    public Optional<(T, T2)> Intersect<T2>(Optional<T2> other)
    {
        if(!hasValue || !other.hasValue) return Optional.None<(T, T2)>();

        return new Optional<(T, T2)>((this.value, other.value));
    }

    public bool Any()
    {
        return hasValue;
    }

    public Optional<T> OnAny(Action block)
    {
        if(Any()) block();
        return this;
    }

    public Optional<T> OnAny(Action<T> block)
    {
        if(Any()) block(value);
        return this;
    }

    public Optional<T> OnNone(Action block)
    {
        if(!Any()) block();
        return this;
    }

    public Optional<T> Where(Predicate<T> filter)
    {
        return !Any() || filter(value) ? this : Optional.None<T>();
    }

    public Optional<TOther> Select<TOther>(Func<T, TOther> selector)
    {
        if(!Any()) return Optional.None<TOther>();
        return Optional.Some(selector(value));
    }

    public Optional<TOther> Select<TOther>(Func<T, Optional<TOther>> selector)
    {
        if(!Any()) return Optional.None<TOther>();
        return selector(value);
    }

    public override string ToString()
    {
        if(hasValue)
            return "{ " + value.ToString() + " }";
        else
            return "{ Empty }";
    }
}