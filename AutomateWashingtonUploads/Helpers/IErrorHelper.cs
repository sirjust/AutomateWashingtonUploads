namespace AutomateWashingtonUploads.Helpers
{
    public interface IErrorHelper
    {
        bool CourseNumberNotFound();
        bool CourseOutOfDateRange();
        bool HasAlreadyUsedCourse();
        bool HasInvalidLicense();
        bool LienseAlreadyOnRoster();
    }
}