using Fiche.Misc;

namespace Fiche.Extensions
{
    public static class IDisposablePatternExtensions
    {
        /// <summary>
        /// Throws an exception if the <see cref="IDisposablePattern.Disposed"/> was true.
        /// </summary>
        public static void AssertNotDisposed(this IDisposablePattern disposable)
        {
            if (disposable is null)
                Error.ThrowIfNull(nameof(disposable));

            string objectName;
            //Avoid excpetions that might be thrown by an overridden ToString().
            try
            { objectName = disposable.ToString(); }
            catch
            { objectName = disposable.GetType().ToString(); }

            Error.ThrowObjectDisposed(disposable.Disposed, objectName);
        }
    }
}
