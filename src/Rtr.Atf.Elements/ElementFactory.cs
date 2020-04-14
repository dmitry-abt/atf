using NLog;
using Rtr.Atf.Core;
using System;
using System.Collections.Generic;

namespace Rtr.Atf.Elements
{
    public class ElementFactory : IElementFactory
    {
        private readonly ISettings settings;

        public ElementFactory(ISettings settings)
        {
            this.settings = settings;
        }

        public IEnumerable<(string key, object value)> GetDefaultSearchConditions(Type type)
        {
            if (this.settings.Host == Host.RRTE)
            {
                return this.GetDefaultRefHostSearchConditions(type);
            }

            throw new NotSupportedException($"Unknown {nameof(this.settings.Host)} value: {this.settings.Host}");
        }

        public IEnumerable<(string, object)> GetDefaultRefHostSearchConditions(Type type)
        {
            if (type == typeof(Element) || type == typeof(WindowRootElement))
            {
                // No default search criteria
            }
            else if (type == typeof(DataGridElement))
            {
                yield return (By.ClassNameProperty, "DataGrid");
            }
            else if (type == typeof(DataGridRowElement))
            {
                yield return (By.ClassNameProperty, "DataGridRow");
            }
            else if (type == typeof(DataGridCellElement))
            {
                yield return (By.ClassNameProperty, "DataGridCell");
            }
            else if (type == typeof(TextBlockElement))
            {
                yield return (By.ClassNameProperty, "TextBlock");
            }
            else if (type == typeof(TextBoxElement))
            {
                yield return (By.ClassNameProperty, "TextBox");
            }
            else if (type == typeof(TreeViewElement))
            {
                yield return (By.ClassNameProperty, "TreeView");
            }
            else if (type == typeof(TreeViewItemElement))
            {
                yield return (By.ClassNameProperty, "TreeViewItem");
            }
            else if (type == typeof(ButtonElement))
            {
                yield return (By.ClassNameProperty, "Button");
            }
            else if (type == typeof(TabControlElement))
            {
                yield return (By.ClassNameProperty, "TabControl");
            }
            else if (type == typeof(TabItemElement))
            {
                yield return (By.ClassNameProperty, "TabItem");
            }
            else if (type == typeof(CheckBoxElement))
            {
                yield return (By.ClassNameProperty, "CheckBox");
            }
            else if (type == typeof(ImageElement))
            {
                yield return (By.ClassNameProperty, "Image");
            }
            else if (type == typeof(ComboBoxElement))
            {
                yield return (By.ClassNameProperty, "ComboBox");
            }
            else if (type == typeof(ListBoxItemElement))
            {
                yield return (By.ClassNameProperty, "ListBoxItem");
            }

            yield break;
        }

        public T CreateElement<T>(IUiItemWrapper itemWrapper, IUiNavigationProvider uiNavigationProvider, IElementFactory elementFactory, IAwaitingService awaitingService, ILogger logger)
            where T : Element
        {
            var type = typeof(T);

            if (type == typeof(DataGridElement))
            {
                return new DataGridElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(DataGridRowElement))
            {
                return new DataGridRowElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(DataGridCellElement))
            {
                return new DataGridCellElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(TextBlockElement))
            {
                return new TextBlockElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(TextBoxElement))
            {
                return new TextBoxElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(TreeViewElement))
            {
                return new TreeViewElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(TreeViewItemElement))
            {
                return new TreeViewItemElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(ButtonElement))
            {
                return new ButtonElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(TabControlElement))
            {
                return new TabControlElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(TabItemElement))
            {
                return new TabItemElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(CheckBoxElement))
            {
                return new CheckBoxElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(ImageElement))
            {
                // todo: make separate configurable folder
                return new ImageElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger, this.settings.CommunicationLogFolderPathAlias) as T;
            }
            else if (type == typeof(ComboBoxElement))
            {
                return new ComboBoxElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }
            else if (type == typeof(ListBoxItemElement))
            {
                return new ListBoxItemElement(itemWrapper, uiNavigationProvider, elementFactory, awaitingService, logger) as T;
            }

            return default;
        }
    }
}
