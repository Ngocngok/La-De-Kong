namespace LongDK.Scenes
{
    public interface ILoadingScreen
    {
        /// <summary>
        /// Called every frame during loading with progress (0.0 to 1.0).
        /// </summary>
        void SetProgress(float progress);
    }
}