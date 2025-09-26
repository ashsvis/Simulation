namespace Simulator.Model.Interfaces
{
    public interface ICopyPaste
    {
        string Copy();
        bool CanPaste();
        object Paste(string source);

    }
}