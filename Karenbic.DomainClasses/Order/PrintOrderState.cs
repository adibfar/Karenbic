using System;

namespace Karenbic.DomainClasses
{
    [Flags]
    public enum PrintOrderState
    {
        Register = 0, // سفارش موقت
        Confirm = 1, // تآیید شده
        Paid = 2, // پرداخت شده
        Print = 3, // چاپ شده
        FinishServes = 4, // خدمات چاپ انجام شده
        Finish = 5 // تحویل داده شده
    }
}
