using Rtr.Atf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Rtr.Atf.CodedUI
{
    internal class PropertyMapper
    {
        public static AutomationProperty ToCodedUiAutomationProperty(string propertyName)
        {
            if (propertyName == By.ClassNameProperty)
            {
                return AutomationElement.ClassNameProperty;
            }

            if (propertyName == By.NameProperty)
            {
                return AutomationElement.NameProperty;
            }

            if (propertyName == By.HelpTextProperty)
            {
                return AutomationElement.HelpTextProperty;
            }

            if (propertyName == By.AutomationIdProperty)
            {
                return AutomationElement.AutomationIdProperty;
            }

            if (propertyName == By.RuntimeIdProperty)
            {
                return AutomationElement.RuntimeIdProperty;
            }

            if (propertyName == "IsEnabled")
            {
                return AutomationElement.IsEnabledProperty;
            }

            if (propertyName == "ExpandCollapse.ExpandCollapseState")
            {
                return ExpandCollapsePattern.ExpandCollapseStateProperty;
            }

            if (propertyName == "Value.Value")
            {
                return ValuePattern.ValueProperty;
            }

            if (propertyName == "Value.IsReadOnly")
            {
                return ValuePattern.IsReadOnlyProperty;
            }

            if (propertyName == "Selection.Selection")
            {
                return SelectionPattern.SelectionProperty;
            }

            if (propertyName == "SelectionItem.IsSelected")
            {
                return SelectionItemPattern.IsSelectedProperty;
            }

            if (propertyName == "Toggle.ToggleState")
            {
                return TogglePattern.ToggleStateProperty;
            }

            if (propertyName == "Grid.RowCount")
            {
                return GridPattern.RowCountProperty;
            }

            throw new Exception($"No keyword '{propertyName}'");
        }
    }
}
