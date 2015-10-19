using System;

namespace Karenbic.DomainClasses
{
    [Flags]
    public enum TransportType
    {
        None = 0,
        Bus = 1,
        Tipax = 2,
        BileCycle = 3,
        Physical = 4
    }
}
