namespace WebGesCommande.Core;

public interface ISession<T>
{
    void Set(string key, T value);
    T? Get(string key);
    void Remove(string key);
}