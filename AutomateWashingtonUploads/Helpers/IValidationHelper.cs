namespace AutomateWashingtonUploads.Helpers
{
    public interface IValidationHelper
    {
        string ChangeSecondToLastCharacter(string s);
        string CheckForZero(string license);
    }
}