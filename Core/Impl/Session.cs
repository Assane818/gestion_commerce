using Newtonsoft.Json;

namespace WebGesCommande.Core.Impl;

public class Session<T> : ISession<T>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Session(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Set(string key, T value)
    {
        var serializedValue = JsonConvert.SerializeObject(value);
        _httpContextAccessor.HttpContext?.Session.SetString(key, serializedValue);
    }

    public T? Get(string key)
    {
        var serializedValue = _httpContextAccessor.HttpContext?.Session.GetString(key);
        return serializedValue == null ? default : JsonConvert.DeserializeObject<T>(serializedValue);
    }

    public void Remove(string key)
    {
        _httpContextAccessor.HttpContext?.Session.Remove(key);
    }
}