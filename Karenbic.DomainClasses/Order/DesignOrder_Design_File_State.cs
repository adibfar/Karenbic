using System;

namespace Karenbic.DomainClasses
{
    [Flags]
    public enum DesignOrder_Design_File_State
    {
        None = 1,
        Accept = 2,
        Maybe = 3,
        UnAccept = 4
    }
}
