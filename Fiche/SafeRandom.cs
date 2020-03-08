//Contents of this file were originally found in this stack overflow answer: https://stackoverflow.com/a/24648788/3602352
using System;
using System.Security.Cryptography;

using Fiche.Misc;

namespace Fiche
{
    /// <summary>
    /// A thread-safe and cryptographically strong implementation of Random.
    /// <para>
    /// This class inherits from <see cref="Random"/>.
    /// </para>
    /// </summary>
    public class SafeRandom : Random
    {
        private const int PoolSize = 2048;

        private static readonly Lazy<RandomNumberGenerator> Rng =
            new Lazy<RandomNumberGenerator>(() => new RNGCryptoServiceProvider());

        private static readonly Lazy<object> PositionLock =
            new Lazy<object>(() => new object());

        private static readonly Lazy<byte[]> Pool =
            new Lazy<byte[]>(() => GeneratePool(new byte[PoolSize]));

        private static int bufferPosition;

        public static int GetNext()
        {
            while (true)
            {
                int result = (int)(GetRandomUInt32() & int.MaxValue);

                if (result != int.MaxValue)
                {
                    return result;
                }
            }
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public static int GetNext(int maxValue)
        {
            Error.ThrowIfBelowRange(maxValue, nameof(maxValue), 1);
            return GetNext(0, maxValue);
        }

        /// <exception cref="ArgumentException"/>
        public static int GetNext(int minValue, int maxValue)
        {
            const long Max = 1 + (long)uint.MaxValue;
            Error.ThrowArgumentException(minValue >= maxValue, $"{nameof(minValue)}, {nameof(maxValue)}", $"{nameof(minValue)} cannot be greater than or equal to {nameof(maxValue)}.");

            long diff = maxValue - minValue;
            long limit = Max - (Max % diff);

            while (true)
            {
                uint rand = GetRandomUInt32();
                if (rand < limit)
                {
                    return (int)(minValue + (rand % diff));
                }
            }
        }

        /// <exception cref="ArgumentNullException"/>
        public static void GetNextBytes(byte[] buffer)
        {
            Error.ThrowIfNull(buffer, nameof(buffer));
            if (buffer.Length < PoolSize)
            {
                lock (PositionLock.Value)
                {
                    if ((PoolSize - bufferPosition) < buffer.Length)
                    {
                        GeneratePool(Pool.Value);
                    }

                    Buffer.BlockCopy(
                        Pool.Value,
                        bufferPosition,
                        buffer,
                        0,
                        buffer.Length);
                    bufferPosition += buffer.Length;
                }
            }
            else
            {
                Rng.Value.GetBytes(buffer);
            }
        }

        public static double GetNextDouble() => GetRandomUInt32() / (1.0 + uint.MaxValue);

        public override int Next() => GetNext();

        public override int Next(int maxValue) => GetNext(0, maxValue);

        public override int Next(int minValue, int maxValue) => GetNext(minValue, maxValue);

        public override void NextBytes(byte[] buffer) => GetNextBytes(buffer);

        public override double NextDouble() => GetNextDouble();

        private static byte[] GeneratePool(byte[] buffer)
        {
            bufferPosition = 0;
            Rng.Value.GetBytes(buffer);
            return buffer;
        }

        private static uint GetRandomUInt32()
        {
            uint result;
            lock (PositionLock.Value)
            {
                if ((PoolSize - bufferPosition) < sizeof(uint))
                {
                    GeneratePool(Pool.Value);
                }

                result = BitConverter.ToUInt32(
                    Pool.Value,
                    bufferPosition);
                bufferPosition += sizeof(uint);
            }

            return result;
        }
    }
}