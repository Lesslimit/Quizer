using System.Collections.Generic;

namespace Quizer.Api.Models
{
    public class QuestionCreationModel
    {
        public string Title { get; set; }
        public IEnumerable<OptionCreationModel> Options { get; set; }
    }
}