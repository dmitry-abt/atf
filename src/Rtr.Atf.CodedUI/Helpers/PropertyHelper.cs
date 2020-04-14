using System.Windows.Automation;

namespace Rtr.Atf.CodedUI
{
    internal static class PropertyHelper
    {
        public static string RuntimeIdBytesToString(int[] bytes)
        {
            var res = string.Empty;

            foreach (var i in bytes)
            {
                res += i.ToString() + ".";
            }

            return res.Substring(0, res.Length - 1);
        }

        public static string ExpandCollapseStateToString(ExpandCollapseState state)
        {
            switch (state)
            {
                case ExpandCollapseState.Collapsed:
                    return "Collapsed";
                case ExpandCollapseState.Expanded:
                    return "Expanded";
                case ExpandCollapseState.LeafNode:
                    return "LeafNode";
                default:
                    return string.Empty;
            }
        }
    }
}
