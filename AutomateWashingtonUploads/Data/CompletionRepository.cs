using System.Collections.Generic;

namespace AutomateWashingtonUploads.Data
{
    public class CompletionRepository : ICompletionRepository
    {
        public IEnumerable<Completion> Completions { get; set; }
    }
}
