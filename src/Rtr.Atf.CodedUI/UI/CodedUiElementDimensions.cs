using Rtr.Atf.Core;

namespace Rtr.Atf.CodedUI.UI
{
    public class CodedUiElementDimensions : IElementDimensions
    {
        public CodedUiElementDimensions(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public double Width { get; }

        public double Height { get; }

        public override string ToString()
        {
            return $"(Width = {this.Width}, Height = {this.Height}) [{nameof(CodedUiElementDimensions)}]";
        }
    }
}
