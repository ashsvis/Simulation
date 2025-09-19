namespace Simulator.Model
{
    public interface ICopyPaste
    {
        void Copy();
        bool CanPaste();
        object Paste();

    }
}