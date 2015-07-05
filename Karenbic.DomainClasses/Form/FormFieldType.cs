using System;

namespace Karenbic.DomainClasses
{
    [Flags]
    public enum FormFieldType
    {
        TextBox = 0,
        TextArea = 1,
        NumericStepper = 2,
        ColorPicker = 3,
        FileUploader = 4,
        Checkbox = 5,
        WebUrl = 6,
        DatePicker = 7,
        DropDown = 8,
        MultipleChoice = 9,
        CheckboxGroup = 10
    }
}
