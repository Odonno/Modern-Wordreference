namespace Wordreference.Core.Services.Abstract
{
    public interface IRatingService
    {
        #region Properties

        int StartedCount { get; }
        bool ReviewedBefore { get; }
        string LastVersionStarted { get; }

        #endregion


        #region Methods

        void AskForRating();

        #endregion
    }
}
