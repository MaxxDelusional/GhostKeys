using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pillowsoft.GhostKeys
{
    public class MidiGpioHandler : GpioHandler
    {


        public static int ReadyPin { get; } = 40;
        public static int BusyPin { get; } = 7;

        private readonly MidiNoteMappings midiNoteMappings;

        public MidiGpioHandler(MidiNoteMappings midiNoteMappings)
        {
            this.midiNoteMappings = midiNoteMappings;
        }

        public  void Reset()
        {
            this.OpenOutputPin(ReadyPin, false);
            this.OpenOutputPin(BusyPin, false);

            foreach (var pin in midiNoteMappings.GetGpioPins())
            {
                this.OpenOutputPin(pin, true);
            }
        }

        public  void ToggleReadyLight(bool on)
        {
            this.WriteOutputPin(ReadyPin, on);
        }

        public  void ToggleBusyLight(bool on)
        {
            this.WriteOutputPin(BusyPin, on);
        }


        public void PlayNote(int midiNoteNumber)
        {
            PlayNote(midiNoteMappings[midiNoteNumber]);
        }
        public void PlayNote(MidiNoteMapping midiNoteMapping)
        {
            this.WriteOutputPin(midiNoteMapping.RelayChannel.GpioPin, !midiNoteMapping.RelayChannel.LowLevelTrigger);
        }

        public void StopNote(int midiNoteNumber)
        {
            StopNote(midiNoteMappings[midiNoteNumber]);
        }
        public void StopNote(MidiNoteMapping midiNoteMapping)
        {
            this.WriteOutputPin(midiNoteMapping.RelayChannel.GpioPin, midiNoteMapping.RelayChannel.LowLevelTrigger);
        }

    }
}
