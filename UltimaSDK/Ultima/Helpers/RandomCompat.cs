using System;
using System.Threading;

namespace Ultima.Ultima.Helpers
{
    public static class RandomCompat
    {
        private static readonly ThreadLocal<Random> _threadRandom = new ThreadLocal<Random>(() =>
        {
            // Ensure each Random gets a different seed (not time-based)
            return new Random(Guid.NewGuid().GetHashCode());
        });

        public static Random Shared => _threadRandom.Value;
    }
}
