
namespace Dope.DDXX.UserInterface
{
    public interface IMenuControl<T> : IWindowControl
    {
        T Action { get; }
        void AddOption(string text, T action);
        void ClearOptions();
        void Next();
        int NumOptions { get; }
        void Previous();
        int Selected { get; set; }
    }
}
