
namespace Game.UI
{
    public interface ITextLabel
    {
        /// <summary>
        /// Sets the text contents of the text label
        /// </summary>
        /// <param name="text">The new text to be displayed on the label</param>
        void SetText(string text);

        /// <summary>
        /// Shows the label in the viewport
        /// </summary>
        void DisplayText();

        /// <summary>
        /// Hides the label in the viewport
        /// </summary>
        void HideText();
    }
}