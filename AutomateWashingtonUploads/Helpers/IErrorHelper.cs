namespace AutomateWashingtonUploads.Helpers
{
    public interface IErrorHelper
    {
        bool CourseNumberNotFound(string text);
        bool CourseOutOfDateRange(string text);
        bool HasAlreadyUsedCourse(string text);
        bool HasInvalidLicense(string text);
        bool LienseAlreadyOnRoster(string text);
    }
}