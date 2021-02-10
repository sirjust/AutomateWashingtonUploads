using System.Collections.Generic;

namespace AutomateWashingtonUploads
{
    public interface IUploader
    {
        void InputCompletions(IEnumerable<Completion> completions);

        void LoginToWebsite();
    }
}
