namespace Simulator.Model
{
    public interface ICopyPaste
    {
        string Copy();
        bool CanPaste();
        object Paste(string source);

    }
}