using System.Collections.Generic;

namespace Wordreference.Core.ViewModel.Abstract
{
    public interface ISettingsViewModel
    {
        IEnumerable<string> Themes { get; }
        string SelectedTheme { get; set; }
    }
}
