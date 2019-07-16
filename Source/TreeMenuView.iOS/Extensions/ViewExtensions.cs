using System.Collections.Generic;
using System.Linq;
using Foundation;

namespace TreeMenuView.iOS.Extensions
{
    public static class ViewExtensions
    {
		public static IEnumerable<NSIndexPath> ToNSIndexPaths(this IEnumerable<int> @this, int section = 0)
		{
			return @this.Select(x => NSIndexPath.FromRowSection(x, section));
		}
    }
}