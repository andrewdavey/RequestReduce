using System;

namespace Spriting
{
    public class Registry
    {
        public static Action<Exception> CaptureErrorAction { get; set; }
    }
}