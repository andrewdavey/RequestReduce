using System;

namespace Spriting
{
    public interface IReducer : IDisposable
    {
        Type SupportedResourceType { get; }
        string Process(Guid key, string urls);
        string Process(string urls);
    }
}