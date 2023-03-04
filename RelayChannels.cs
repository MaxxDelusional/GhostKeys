using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pillowsoft.GhostKeys
{

    public sealed class RelayChannels : ReadOnlyCollection<RelayChannel>
    {
        private static RelayChannels? instance = null;

        private RelayChannels() : base(new RelayChannel[]
        {
            new RelayChannel(4, 13) { GpioPinLabel = "GPIO 27", JumperColor = "Purple" },
            new RelayChannel(5, 15) { GpioPinLabel = "GPIO 22", JumperColor = "Blue" },
            new RelayChannel(6, 16) { GpioPinLabel = "GPIO 23", JumperColor = "Green" },
            new RelayChannel(7, 18) { GpioPinLabel = "GPIO 24", JumperColor = "Orange" },
            new RelayChannel(8, 22) { GpioPinLabel = "GPIO 25" },
            new RelayChannel(9, 38) { GpioPinLabel = "GPIO 20", JumperColor = "Brown" },
            new RelayChannel(10, 36) { GpioPinLabel = "GPIO 16", JumperColor = "Orange" },
            new RelayChannel(11, 32) { GpioPinLabel = "GPIO 12", JumperColor = "Yellow" },
            new RelayChannel(12, 37) { GpioPinLabel = "GPIO 26", JumperColor = "Green" },
            new RelayChannel(13, 35) { GpioPinLabel = "GPIO 19", JumperColor = "Blue" },
            new RelayChannel(14, 33) { GpioPinLabel = "GPIO 13", JumperColor = "Purple" },
            new RelayChannel(15, 31) { GpioPinLabel = "GPIO 6", JumperColor = "Gray" },
            new RelayChannel(16, 29) { GpioPinLabel = "GPIO 5", JumperColor = "White" }
        })
        {

        }

        public static RelayChannels Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new RelayChannels();
                }
                return instance;
            }
        }

        //private  List<Channel> channels = new()
        //{
        //    //new Channel(4, 13) { PinLabel = "GPIO 27", Color = "Purple" },
        //    //new Channel(5, 15) { PinLabel = "GPIO 22", Color = "Blue" },
        //    //new Channel(6, 16) { PinLabel = "GPIO 23", Color = "Green" },
        //    //new Channel(7, 18) { PinLabel = "GPIO 24", Color = "Orange" },
        //    //new Channel(8, 22) { PinLabel = "GPIO 25" },
        //    new Channel(9, 38) { PinLabel = "GPIO 20", JumperColor = "Brown" },
        //    new Channel(10, 36) { PinLabel = "GPIO 16", JumperColor = "Orange" },
        //    new Channel(11, 32) { PinLabel = "GPIO 12", JumperColor = "Yellow" },
        //    new Channel(12, 37) { PinLabel = "GPIO 26", JumperColor = "Green" },
        //    new Channel(13, 35) { PinLabel = "GPIO 19", JumperColor = "Blue" },
        //    //new Channel(14, 33) { PinLabel = "GPIO 13", Color = "Purple" },
        //    //new Channel(15, 31) { PinLabel = "GPIO 6", Color = "Gray" },
        //    //new Channel(16, 29) { PinLabel = "GPIO 5", Color = "White" },

        //};


        public RelayChannel? GetByChannelNumber(int channelNumber)
        {
            return this.FirstOrDefault(x => x.RelayChannelNumber == channelNumber);
        }

       

    }

    public record RelayChannel
    {

        public int RelayChannelNumber { get; }

        public int GpioPin { get; }

        public string? JumperColor { get; init; }

        public string? GpioPinLabel { get; init; }

        public bool LowLevelTrigger { get; init; } = true;


        public RelayChannel(int channelNumber, int gpioPin)
        {
            RelayChannelNumber = channelNumber;
            GpioPin = gpioPin;
        }

    }
}
