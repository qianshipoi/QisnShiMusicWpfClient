namespace QianShi.Music.Extensions
{
    public static class SnackbarMessageQueueExtensions
    {
        public static void Enqueue(this ISnackbarMessageQueue snackbarMessageQueue, object content, TimeSpan timeSpan)
        {
            snackbarMessageQueue.Enqueue(content, null, null, null, false, true, timeSpan);
        }
    }
}
