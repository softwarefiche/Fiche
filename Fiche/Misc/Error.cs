using System;
using System.Threading.Tasks;

using Fiche.Extensions;

namespace Fiche.Misc
{
    internal static class Error
    {
        public static void ThrowArgumentException(string paramName)
            => throw new ArgumentException(null, paramName);
        public static void ThrowArgumentException(string paramName, string message)
            => throw new ArgumentException(message, paramName);
        public static void ThrowArgumentException(bool condition, string paramName)
        {
            if (condition)
                throw new ArgumentException(null, paramName);
        }
        public static void ThrowArgumentException(bool condition, string paramName, string message)
        {
            if (condition)
                throw new ArgumentException(message, paramName);
        }
        public static void ThrowIfNull<T>(T source, string argumentName)
        {
            if (source.IsNull())
                throw new ArgumentNullException(argumentName);
        }
        public static void ThrowIfNull<T>(T source)
        {
            if (source.IsNull())
                throw new ArgumentNullException();
        }
        public static void ThrowObjectDisposed(string objectName)
            => throw new ObjectDisposedException(objectName);
        public static void ThrowObjectDisposed(string objectName, string message)
            => throw new ObjectDisposedException(objectName, message);
        public static void ThrowObjectDisposed(bool condition, string objectName)
        {
            if (condition)
                throw new ObjectDisposedException(objectName);
        }
        public static void ThrowObjectDisposed(bool condition, string objectName, string message)
        {
            if (condition)
                throw new ObjectDisposedException(objectName, message);
        }
        public static void ThrowArgumentOutOfRange()
            => throw new ArgumentOutOfRangeException();
        public static void ThrowArgumentOutOfRange(string paramName)
            => throw new ArgumentOutOfRangeException(paramName);
        public static void ThrowArgumentOutOfRange(string paramName, string message)
            => throw new ArgumentOutOfRangeException(paramName, message);
        public static void ThrowArgumentOutOfRange(bool condition, string paramName)
        {
            if (condition)
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowArgumentOutOfRange(bool condition, string paramName, string message)
        {
            if (condition)
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        #region Numeric Ranges
        public static void ThrowIfOutOfRange(short obj, short lowerBoundSbj, short upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfOutOfRange(short obj, string paramName, short lowerBoundSbj, short upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfOutOfRange(short obj, string paramName, short lowerBoundSbj, short upperBoundSbj, string message)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfOutOfRange(int obj, int lowerBoundSbj, int upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfOutOfRange(int obj, string paramName, int lowerBoundSbj, int upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfOutOfRange(int obj, string paramName, int lowerBoundSbj, int upperBoundSbj, string message)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfOutOfRange(long obj, long lowerBoundSbj, long upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfOutOfRange(long obj, string paramName, long lowerBoundSbj, long upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfOutOfRange(long obj, string paramName, long lowerBoundSbj, long upperBoundSbj, string message)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfOutOfRange(float obj, float lowerBoundSbj, float upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfOutOfRange(float obj, string paramName, float lowerBoundSbj, float upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfOutOfRange(float obj, string paramName, float lowerBoundSbj, float upperBoundSbj, string message)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfOutOfRange(double obj, double lowerBoundSbj, double upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfOutOfRange(double obj, string paramName, double lowerBoundSbj, double upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfOutOfRange(double obj, string paramName, double lowerBoundSbj, double upperBoundSbj, string message)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfOutOfRange(decimal obj, decimal lowerBoundSbj, decimal upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfOutOfRange(decimal obj, string paramName, decimal lowerBoundSbj, decimal upperBoundSbj)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfOutOfRange(decimal obj, string paramName, decimal lowerBoundSbj, decimal upperBoundSbj, string message)
        {
            if (obj.IsLessThan(lowerBoundSbj) || obj.IsGreaterThan(upperBoundSbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }

        public static void ThrowIfBelowRange(short obj, short sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfBelowRange(short obj, string paramName, short sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfBelowRange(short obj, string paramName, short sbj, string message)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfAboveRange(short obj, short sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfAboveRange(short obj, string paramName, short sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfAboveRange(short obj, string paramName, short sbj, string message)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfBelowRange(int obj, int sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfBelowRange(int obj, string paramName, int sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfBelowRange(int obj, string paramName, int sbj, string message)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfAboveRange(int obj, int sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfAboveRange(int obj, string paramName, int sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfAboveRange(int obj, string paramName, int sbj, string message)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfBelowRange(long obj, long sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfBelowRange(long obj, string paramName, long sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfBelowRange(long obj, string paramName, long sbj, string message)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfAboveRange(long obj, long sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfAboveRange(long obj, string paramName, long sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfAboveRange(long obj, string paramName, long sbj, string message)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfBelowRange(float obj, float sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfBelowRange(float obj, string paramName, float sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfBelowRange(float obj, string paramName, float sbj, string message)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfAboveRange(float obj, float sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfAboveRange(float obj, string paramName, float sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfAboveRange(float obj, string paramName, float sbj, string message)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfBelowRange(double obj, double sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfBelowRange(double obj, string paramName, double sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfBelowRange(double obj, string paramName, double sbj, string message)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfAboveRange(double obj, double sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfAboveRange(double obj, string paramName, double sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfAboveRange(double obj, string paramName, double sbj, string message)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfBelowRange(decimal obj, decimal sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfBelowRange(decimal obj, string paramName, decimal sbj)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfBelowRange(decimal obj, string paramName, decimal sbj, string message)
        {
            if (obj.IsLessThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        public static void ThrowIfAboveRange(decimal obj, decimal sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException();
        }
        public static void ThrowIfAboveRange(decimal obj, string paramName, decimal sbj)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName);
        }
        public static void ThrowIfAboveRange(decimal obj, string paramName, decimal sbj, string message)
        {
            if (obj.IsGreaterThan(sbj))
                throw new ArgumentOutOfRangeException(paramName, message);
        }
        #endregion

        public static void ThrowInvalidOperation()
            => throw new InvalidOperationException();
        public static void ThrowInvalidOperation(string message)
            => throw new InvalidOperationException(message);
        public static void ThrowInvalidOperation(bool condition)
        {
            if (condition)
                throw new InvalidOperationException();
        }
        public static void ThrowInvalidOperation(bool condition, string message)
        {
            if (condition)
                throw new InvalidOperationException(message);
        }
        public static void ThrowTaskCanceled()
            => throw new TaskCanceledException();
        public static void ThrowTaskCanceled(Task task)
            => throw new TaskCanceledException(task);
        public static void ThrowNotSupported()
            => throw new NotSupportedException();
        public static void ThrowNotSupported(string message)
            => throw new NotSupportedException(message);

    }
}
