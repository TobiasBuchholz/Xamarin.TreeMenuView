namespace System
{
	public static class FloatExtensions
	{
		public static bool NearlyEquals(this float first, float second)
		{
			return Math.Abs(first - second) < 1E-15;
		}
	}
}
