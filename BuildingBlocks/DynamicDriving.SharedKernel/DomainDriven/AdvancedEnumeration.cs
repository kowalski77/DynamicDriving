#pragma warning disable CS8618
using System.Reflection;

namespace DynamicDriving.SharedKernel.DomainDriven;

public class AdvancedEnumeration<T>
{
    protected AdvancedEnumeration() { }

    protected AdvancedEnumeration(int id, string name)
    {
        (this.Id, this.Name) = (id, name);
    }

    public int Id { get; }

    public string Name { get; }

    protected static IEnumerable<T> All => LazyEnumeration.Value;

    private static readonly Lazy<IEnumerable<T>> LazyEnumeration = new(() =>
    {
        return typeof(T).GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.DeclaredOnly)
            .Select(x => x.GetValue(null))
            .Cast<T>();
    });
}
