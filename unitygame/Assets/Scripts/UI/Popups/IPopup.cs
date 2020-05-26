namespace UI
{
    public interface IPopup
    {
        float DestroyDelay { get; }
        void OnShow();
        void OnHide();
    }
}