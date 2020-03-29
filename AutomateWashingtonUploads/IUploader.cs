using System.Collections.Generic;

namespace AutomateWashingtonUploads
{
    public interface IUploader
    {
        void InputCompletions(List<Completion> completions);

        void LoginToWebsite();
    }
}
