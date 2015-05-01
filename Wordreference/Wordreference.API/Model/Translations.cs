using System.Collections.ObjectModel;

namespace Wordreference.API.Model
{
    public class Translations
    {
        #region Properties

        private ObservableCollection<Translation> _traductionsPrincipales = new ObservableCollection<Translation>();
        public ObservableCollection<Translation> TraductionsPrincipales
        {
            get { return _traductionsPrincipales; }
            set { _traductionsPrincipales = value; }
        }

        private ObservableCollection<Translation> _traductionsAdditionnelles = new ObservableCollection<Translation>();
        public ObservableCollection<Translation> TraductionsAdditionnelles
        {
            get { return _traductionsAdditionnelles; }
            set { _traductionsAdditionnelles = value; }
        }

        private ObservableCollection<Translation> _formesComposees = new ObservableCollection<Translation>();
        public ObservableCollection<Translation> FormesComposees
        {
            get { return _formesComposees; }
            set { _formesComposees = value; }
        }

        #endregion


        #region Methods

        public void Clear()
        {
            TraductionsPrincipales.Clear();
            TraductionsAdditionnelles.Clear();
            FormesComposees.Clear();
        }

        public void Populate(Translations anotherTranslations)
        {
            foreach (Translation traductionPrincipale in anotherTranslations.TraductionsPrincipales)
                TraductionsPrincipales.Add(traductionPrincipale);

            foreach (Translation traductionsdditionnelle in anotherTranslations.TraductionsAdditionnelles)
                TraductionsAdditionnelles.Add(traductionsdditionnelle);

            foreach (Translation formeComposee in anotherTranslations.FormesComposees)
                FormesComposees.Add(formeComposee);
        }

        #endregion
    }
}
