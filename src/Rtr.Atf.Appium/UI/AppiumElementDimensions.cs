using Rtr.Atf.Core;

namespace Rtr.Atf.Appium
{
    public class AppiumElementDimensions : IElementDimensions
    {
        public AppiumElementDimensions(double width, double height)
        {
            this.Width = width;
            this.Height = height;
        }

        public double Width { get; }

        public double Height { get; }

        public override string ToString()
        {
            return $"(Width = {this.Width}, Height = {this.Height}) [{nameof(AppiumElementDimensions)}]";
        }
    }
}
