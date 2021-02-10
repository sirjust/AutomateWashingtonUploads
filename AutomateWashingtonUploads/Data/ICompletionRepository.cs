using System.Collections.Generic;

namespace AutomateWashingtonUploads.Data
{
    interface ICompletionRepository
    {
        IEnumerable<Completion> Completions { get; set; }
    }
}
