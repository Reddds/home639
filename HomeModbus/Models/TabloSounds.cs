namespace HomeModbus.Models
{
    public class TabloSounds
    {
        public class SoundAtom
        {
            public byte Time256Ms { get; set; }
            public byte Period { get; set; }
            public byte Duration { get; set; }

            public byte[] ToBytes()
            {
                return new[] {Time256Ms, Period, Duration};
            }
        }

        public SoundAtom[] Sequense { get; set; }
    }
}
