using System;

namespace CommonLibrary.Worker
{
    public interface IWorkServiceOptions<T>
    {
        TimeSpan Delay { get; }
        TimeSpan Interval { get; }
    }
}