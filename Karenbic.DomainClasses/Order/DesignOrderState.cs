using System;

namespace Karenbic.DomainClasses
{
    [Flags]
    public enum DesignOrderState
    {
        Register = 0, // سفارش موقت
        Confirm = 1, // تآیید شده
        Paid = 2, // پرداخت شده
        Design = 3, //طراحی شده
        Finish = 4 // تحویل داده شده
    }
}
