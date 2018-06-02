namespace BTBulldozer
{
    public static class Command
    {
        public static byte[] FORWARD { get; } = new byte[] { 0x00 };
        public static byte[] BACK { get; } = new byte[] { 0x01 };
        public static byte[] RIGHT { get; } = new byte[] { 0x02 };
        public static byte[] LEFT { get; } = new byte[] { 0x03 };
        public static byte[] STOP { get; } = new byte[] { 0x0F };
        public static byte[] UP { get; } = new byte[] { 0x10 };
        public static byte[] DOWN { get; } = new byte[] { 0x11 };
        public static byte[] KEEP { get; } = new byte[] { 0x1F };
        public static byte[] VOLTAGE { get; } = new byte[] { 0xF0 };
    }
}
