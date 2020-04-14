namespace Rtr.Atf.Core
{
    /// <summary>
    /// Provides a set of known criteria keys, used for navigating through visual tree.
    /// </summary>
    public static class By
    {
        /// <summary>
        /// Gets a criteria key for searching by 'Name' property.
        /// </summary>
        public static string NameProperty => "Name";

        /// <summary>
        /// Gets a criteria key for searching by 'ClassName' property.
        /// </summary>
        public static string ClassNameProperty => "ClassName";

        /// <summary>
        /// Gets a criteria key for searching by 'ControlType' property.
        /// </summary>
        public static string ControlTypeProperty => "ControlType";

        /// <summary>
        /// Gets a criteria key for searching by 'LocalizedControlType' property.
        /// </summary>
        public static string LocalizedControlTypeProperty => "LocalizedControlType";

        /// <summary>
        /// Gets a criteria key for searching by 'HelpText' property.
        /// </summary>
        public static string HelpTextProperty => "HelpText";

        /// <summary>
        /// Gets a criteria key for searching by 'AutomationId' property.
        /// </summary>
        public static string AutomationIdProperty => "AutomationId";

        /// <summary>
        /// Gets a criteria key for searching an item with parent with desired 'ParentId' property value.
        /// </summary>
        public static string ParentRuntimeId => "ParentRuntimeId";

        /// <summary>
        /// Gets a criteria key for searching by 'RuntimeId' property.
        /// </summary>
        public static string RuntimeIdProperty => "RuntimeId";

        /// <summary>
        /// Gets a criteria key for searching by 'IsSelected' property.
        /// </summary>
        public static string IsSelectedProperty => "SelectionItem.IsSelected";
    }
}
