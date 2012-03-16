namespace Spritastic.Selector
{
    public interface ICssSelectorAnalyzer
    {
        bool IsInScopeOfTarget(string targetSelector, string comparableSelector);
    }
}