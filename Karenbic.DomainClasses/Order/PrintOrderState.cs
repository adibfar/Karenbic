using System;

namespace Karenbic.DomainClasses
{
    [Flags]
    public enum PrintOrderState
    {
        Register = 0,
        Confirm = 1,
        Paid = 2
    }
}
