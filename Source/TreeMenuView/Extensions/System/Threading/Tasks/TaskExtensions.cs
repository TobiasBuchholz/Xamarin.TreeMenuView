using System.Threading.Tasks;

namespace TreeMenuView.Extensions.System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static void Ignore(this Task @this)
        {
        }

        public static void Ignore<T>(this Task<T> @this)
        {
        }
    }
}
