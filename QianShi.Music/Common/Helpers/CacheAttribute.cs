
using Metalama.Framework.Aspects;

namespace QianShi.Music.Common.Helpers
{
    public class CacheAttribute : OverrideMethodAspect
    {
        public override dynamic? OverrideMethod()
        {
            return meta.Proceed();
        }
    }
}
