using GalaSoft.MvvmLight.Command;

namespace Wordreference.Core.ViewModel.Abstract
{
    public interface IAboutViewModel
    {
        RelayCommand GoToTwitterCommand { get; }
    }
}
