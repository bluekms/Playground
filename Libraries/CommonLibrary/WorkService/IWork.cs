using System.Threading.Tasks;

namespace CommonLibrary.Worker
{
    public interface IWork
    {
        Task RunAsync();
    }
}