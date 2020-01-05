namespace MonoKle.Core.Conversion
{
    /// <summary>
    /// Converts base data types to byte arrays, and byte arrays to base data types. Uses an arbitrary standard for converting to a byte array
    /// and should only be used when endian-agnosticism is wanted.
    /// </summary>
    public static class ByteConverter
    {
        /// <summary>
        /// Returns the boolean value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Boolean value.</returns>
        public static bool ToBoolean(byte[] byteArray) => ToBoolean(byteArray, 0);

        /// <summary>
        /// Returns the boolean value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Boolean value</returns>
        public static bool ToBoolean(byte[] byteArray, int startIndex) => byteArray[startIndex] > 0;

        /// <summary>
        /// Stores the byte array representation of the boolean input into an existing array.
        /// </summary>
        /// <param name="value">Boolean input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static void ToBytes(bool value, byte[] destination, int startIndex)
        {
            if (value)
            {
                destination[startIndex] = 0x01;
            }
            else
            {
                destination[startIndex] = 0x00;
            }
        }

        /// <summary>
        /// Returns a byte array containing the representation of the string input. The string input may maximally have a length of ushort.MaxValue.
        /// </summary>
        /// <param name="value">String input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(string value)
        {
            if (value.Length > ushort.MaxValue)
            {
                throw new System.ArgumentException("Max allowed length of string is ushort.MaxValue.");
            }
            byte[] ret = new byte[sizeof(ushort) + value.Length * sizeof(char)];
            ByteConverter.ToBytes(value, ret, 0);
            return ret;
        }

        /// <summary>
        /// Stores the byte array representation of the string input into an existing array. The string input may maximally have a length of ushort.MaxValue.
        /// </summary>
        /// <param name="value">String input.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index to store string at.</param>
        public static void ToBytes(string value, byte[] destination, int startIndex)
        {
            if (value.Length > ushort.MaxValue)
            {
                throw new System.ArgumentException("Max allowed length of string is ushort.MaxValue.");
            }
            ByteConverter.ToBytes((ushort)value.Length, destination, startIndex);
            System.Buffer.BlockCopy(value.ToCharArray(), 0, destination, startIndex + sizeof(ushort), value.Length * sizeof(char));
        }

        /// <summary>
        /// Returns the string value from the provided byte representation.
        /// </summary>
        /// <param name="bytes">Byte representation.</param>
        /// <returns>String.</returns>
        public static string ToString(byte[] bytes) => ByteConverter.ToString(bytes, 0);

        /// <summary>
        /// Returns the string value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="bytes">Byte representation.</param>
        /// <param name="startIndex">Index to start at.</param>
        /// <returns>String.</returns>
        public static string ToString(byte[] bytes, int startIndex)
        {
            ushort length = ByteConverter.ToUInt16(bytes, startIndex);
            char[] chars = new char[length];
            System.Buffer.BlockCopy(bytes, startIndex + sizeof(ushort), chars, 0, chars.Length * sizeof(char));
            return new string(chars);
        }

        /// <summary>
        /// Returns the string value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="bytes">Byte representation.</param>
        /// <param name="startIndex">Index to start at.</param>
        /// <param name="bytesRead">Bytes read.</param>
        /// <returns>String.</returns>
        public static string ToString(byte[] bytes, int startIndex, out int bytesRead)
        {
            ushort length = ByteConverter.ToUInt16(bytes, startIndex);
            bytesRead = sizeof(ushort) + length * sizeof(char);
            return ByteConverter.ToString(bytes, startIndex);
        }

        /// <summary>
        /// Returns a byte array containing the representation of the bool input.
        /// </summary>
        /// <param name="value">Bool input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(bool value)
        {
            byte[] ret = new byte[sizeof(bool)];
            ToBytes(value, ret, 0);
            return ret;
        }

        /// <summary>
        /// Returns a byte array containing the representation of the signed short input.
        /// </summary>
        /// <param name="value">Signed short input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(short value)
        {
            byte[] ret = new byte[sizeof(short)];
            ToBytes(value, ret, 0);
            return ret;
        }

        /// <summary>
        /// Stores the byte array representation of the signed short input into an existing array.
        /// </summary>
        /// <param name="value">Signed short input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static void ToBytes(short value, byte[] destination, int startIndex)
        {
            destination[startIndex++] = (byte)value;
            destination[startIndex] = (byte)(value >> 8);
        }

        /// <summary>
        /// Returns a byte array containing the representation of the unsigned short input.
        /// </summary>
        /// <param name="value">Unsigned short input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(ushort value) => ToBytes((short)value);

        /// <summary>
        /// Stores the byte array representation of the unsigned short input into an existing array.
        /// </summary>
        /// <param name="value">Unsigned short input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static void ToBytes(ushort value, byte[] destination, int startIndex) => ToBytes((short)value, destination, startIndex);

        /// <summary>
        /// Returns a byte array containing the representation of the signed int32 input.
        /// </summary>
        /// <param name="value">Signed int32 input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(int value)
        {
            byte[] ret = new byte[sizeof(int)];
            ToBytes(value, ret, 0);
            return ret;
        }

        /// <summary>
        /// Stores the byte array representation of the signed int32 input into an existing array.
        /// </summary>
        /// <param name="value">Signed int32 input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static void ToBytes(int value, byte[] destination, int startIndex)
        {
            destination[startIndex++] = (byte)value;
            destination[startIndex++] = (byte)(value >> 8);
            destination[startIndex++] = (byte)(value >> 16);
            destination[startIndex] = (byte)(value >> 24);
        }

        /// <summary>
        /// Returns a byte array containing the representation of the unsigned int32 input.
        /// </summary>
        /// <param name="value">Unsigned int32 input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(uint value) => ToBytes((int)value);

        /// <summary>
        /// Stores the byte array representation of the unsigned int32 input into an existing array.
        /// </summary>
        /// <param name="value">Unsigned int32 input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static void ToBytes(uint value, byte[] destination, int startIndex) => ToBytes((int)value, destination, startIndex);

        /// <summary>
        /// Returns a byte array containing the representation of the signed int64 input.
        /// </summary>
        /// <param name="value">Signed int64 input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(long value)
        {
            byte[] ret = new byte[sizeof(long)];
            ToBytes(value, ret, 0);
            return ret;
        }

        /// <summary>
        /// Stores the byte array representation of the signed int64 input into an existing array.
        /// </summary>
        /// <param name="value">Signed int64 input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static void ToBytes(long value, byte[] destination, int startIndex)
        {
            destination[startIndex++] = (byte)value;
            destination[startIndex++] = (byte)(value >> 8);
            destination[startIndex++] = (byte)(value >> 16);
            destination[startIndex++] = (byte)(value >> 24);
            destination[startIndex++] = (byte)(value >> 32);
            destination[startIndex++] = (byte)(value >> 40);
            destination[startIndex++] = (byte)(value >> 48);
            destination[startIndex] = (byte)(value >> 56);
        }

        /// <summary>
        /// Returns a byte array containing the representation of the unsigned int64 input.
        /// </summary>
        /// <param name="value">Unsigned int64 input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(ulong value) => ToBytes((long)value);

        /// <summary>
        /// Stores the byte array representation of the unsigned int64 input into an existing array.
        /// </summary>
        /// <param name="value">Unsigned int64 input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static void ToBytes(ulong value, byte[] destination, int startIndex) => ToBytes((long)value, destination, startIndex);

        /// <summary>
        /// Returns a byte array containing the representation of the float32 input.
        /// </summary>
        /// <param name="value">Float32 input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(float value)
        {
            byte[] ret = new byte[sizeof(float)];
            ToBytes(value, ret, 0);
            return ret;
        }

        /// <summary>
        /// Stores the byte array representation of the float32 input into an existing array.
        /// </summary>
        /// <param name="value">Float32 input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static unsafe void ToBytes(float value, byte[] destination, int startIndex) => ToBytes((*(int*)&value), destination, startIndex);

        /// <summary>
        /// Returns a byte array containing the representation of the float64 input.
        /// </summary>
        /// <param name="value">Float64 input value.</param>
        /// <returns>Byte array.</returns>
        public static byte[] ToBytes(double value)
        {
            byte[] ret = new byte[sizeof(double)];
            ToBytes(value, ret, 0);
            return ret;
        }

        /// <summary>
        /// Stores the byte array representation of the float64 input into an existing array.
        /// </summary>
        /// <param name="value">Float64 input value.</param>
        /// <param name="destination">Destination array.</param>
        /// <param name="startIndex">Start index where the first byte is stored.</param>
        public static unsafe void ToBytes(double value, byte[] destination, int startIndex) => ToBytes((*(long*)&value), destination, startIndex);

        /// <summary>
        /// Returns the float32 value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Float32 value.</returns>
        public static float ToFloat32(byte[] byteArray) => ToFloat32(byteArray, 0);

        /// <summary>
        /// Returns the float32 value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Float32 value</returns>
        public static unsafe float ToFloat32(byte[] byteArray, int startIndex)
        {
            int d = ToInt32(byteArray, startIndex);
            return *(float*)&d;
        }

        /// <summary>
        /// Returns the float64 value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Float64 value.</returns>
        public static double ToFloat64(byte[] byteArray) => ToFloat64(byteArray, 0);

        /// <summary>
        /// Returns the float64 value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Float64 value</returns>
        public static unsafe double ToFloat64(byte[] byteArray, int startIndex)
        {
            long d = ToInt64(byteArray, startIndex);
            return *(double*)&d;
        }

        /// <summary>
        /// Returns the signed int16 value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Signed int16 value.</returns>
        public static short ToInt16(byte[] byteArray) => ToInt16(byteArray, 0);

        /// <summary>
        /// Returns the signed int16 value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Signed int16 value</returns>
        public static short ToInt16(byte[] byteArray, int startIndex)
        {
            short ret = 0;
            ret |= (short)byteArray[startIndex++];
            ret |= (short)(byteArray[startIndex] << 8);
            return ret;
        }

        /// <summary>
        /// Returns the signed int32 value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Signed int32 value.</returns>
        public static int ToInt32(byte[] byteArray) => ToInt32(byteArray, 0);

        /// <summary>
        /// Returns the signed int32 value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Signed int32 value</returns>
        public static int ToInt32(byte[] byteArray, int startIndex)
        {
            int ret = 0;
            ret |= byteArray[startIndex++];
            ret |= (byteArray[startIndex++] << 8);
            ret |= (byteArray[startIndex++] << 16);
            ret |= (byteArray[startIndex] << 24);
            return ret;
        }

        /// <summary>
        /// Returns the signed int64 value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Signed int64 value.</returns>
        public static long ToInt64(byte[] byteArray) => ToInt64(byteArray, 0);

        /// <summary>
        /// Returns the signed int64 value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Signed int64 value</returns>
        public static long ToInt64(byte[] byteArray, int startIndex)
        {
            long ret = 0;
            ret |= byteArray[startIndex++];
            ret |= ((long)byteArray[startIndex++] << 8);
            ret |= ((long)byteArray[startIndex++] << 16);
            ret |= ((long)byteArray[startIndex++] << 24);
            ret |= ((long)byteArray[startIndex++] << 32);
            ret |= ((long)byteArray[startIndex++] << 40);
            ret |= ((long)byteArray[startIndex++] << 48);
            ret |= ((long)byteArray[startIndex] << 56);
            return ret;
        }

        /// <summary>
        /// Returns the unsigned int16 value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Unsigned int16 value.</returns>
        public static ushort ToUInt16(byte[] byteArray) => (ushort)ToInt16(byteArray, 0);

        /// <summary>
        /// Returns the unsigned int16 value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Unsisgned int16 value</returns>
        public static ushort ToUInt16(byte[] byteArray, int startIndex) => (ushort)ToInt16(byteArray, startIndex);

        /// <summary>
        /// Returns the unsigned int32 value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Unsigned int32 value.</returns>
        public static uint ToUInt32(byte[] byteArray) => (uint)ToInt32(byteArray, 0);

        /// <summary>
        /// Returns the unsigned int32 value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Unsigned int32 value</returns>
        public static uint ToUInt32(byte[] byteArray, int startIndex) => (uint)ToInt32(byteArray, startIndex);

        /// <summary>
        /// Returns the unsigned int64 value from the provided byte representation.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <returns>Unsigned int64 value.</returns>
        public static ulong ToUInt64(byte[] byteArray) => (ulong)ToInt64(byteArray, 0);

        /// <summary>
        /// Returns the unsigned int64 value from the provided byte representation, starting at the specified index.
        /// </summary>
        /// <param name="byteArray">Byte representation.</param>
        /// <param name="startIndex">Specified index to start at.</param>
        /// <returns>Unsigned int64 value</returns>
        public static ulong ToUInt64(byte[] byteArray, int startIndex) => (ulong)ToInt64(byteArray, startIndex);
    }
}