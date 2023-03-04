using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pillowsoft.GhostKeys
{
    public class MidiNoteMappings : Dictionary<int, MidiNoteMapping>
    {
        public int? RequiredChannels { get; set; }
        public int? MaxVelocity { get;  set; }
        public int? MinVelocity { get;  set; }
        public int MaxMidiDeltaTime { get; set; } = 0;

        internal IEnumerable<int> GetGpioPins()
        {
            return this.Values.Select(x => x.RelayChannel.GpioPin);


        }


        public void WriteToConsole()
        {
            Console.WriteLine();


            Console.WriteLine($"Required Number of Channels: {this.RequiredChannels}");
            Console.WriteLine($"Max Velocity: {this.MaxVelocity?.ToString() ?? "N/A"}");
            Console.WriteLine($"Min Velocity: {this.MinVelocity?.ToString() ?? "N/A"}");
            Console.WriteLine($"Max Midi Delta Time: {this.MaxMidiDeltaTime }");

            Console.WriteLine();

            foreach (var (midiNote, mapping) in this)
            {
                Console.WriteLine($"CH {mapping.RelayChannel.RelayChannelNumber} = {mapping.NoteName} ({midiNote})");
            }
            Console.WriteLine();
        }


    }

    public record MidiNoteMapping
    {
        public string? NoteName { get; init; }
        public RelayChannel RelayChannel { get; }

        public MidiNoteMapping(RelayChannel relayChannel)
        {
            this.RelayChannel = relayChannel;
        }


    }
}
