
namespace Dope.DDXX.UserInterface
{
    public interface IMenuControl : IWindowControl
    {
        void ClearOptions();
        void Next();
        int NumOptions { get; }
        void Previous();
        int Selected { get; set; }
    }

    public interface IMenuControl<T> : IMenuControl
    {
        T Action { get; }
        void AddOption(string text, T action);
    }
}
