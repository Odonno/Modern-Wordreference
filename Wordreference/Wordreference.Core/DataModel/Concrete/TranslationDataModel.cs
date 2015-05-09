using Wordreference.Core.DataModel.Abstract;
using Wordreference.Model;

namespace Wordreference.Core.DataModel.Concrete
{
    internal class TranslationDataModel : ITranslationDataModel
    {
        public Term TermeOriginal { get; set; }
        public Term FirstTranslation { get; set; }
        public Term SecondTranslation { get; set; }
        public string GroupName { get; set; }


        public TranslationDataModel(Translation translation)
        {
            FirstTranslation = translation.FirstTranslation;
            SecondTranslation = translation.SecondTranslation;
            TermeOriginal = translation.TermeOriginal;
        }
    }
}
