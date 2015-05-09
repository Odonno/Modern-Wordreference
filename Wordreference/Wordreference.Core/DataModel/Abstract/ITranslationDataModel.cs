using Wordreference.Model;

namespace Wordreference.Core.DataModel.Abstract
{
    public interface ITranslationDataModel
    {
        Term TermeOriginal { get; set; }
        Term FirstTranslation { get; set; }
        Term SecondTranslation { get; set; }
        string GroupName { get; set; }
    }
}
