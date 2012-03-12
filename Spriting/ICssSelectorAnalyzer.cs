namespace Spriting
{
    public interface ICssSelectorAnalyzer
    {
        bool IsInScopeOfTarget(string targetSelector, string comparableSelector);
    }
}