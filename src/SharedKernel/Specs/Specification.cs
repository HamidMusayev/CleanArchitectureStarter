using System.Linq.Expressions;

namespace SharedKernel.Specs;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();
    public Func<T, bool> ToPredicate() => ToExpression().Compile();
}