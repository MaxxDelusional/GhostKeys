using NAudio.Midi;
using System.Diagnostics;

namespace Pillowsoft.GhostKeys
{
    public class MidiReader
    {
        private readonly string fileName;
        private readonly int trackNumber;

        private bool stopRequested = false;

        public MidiReader(string fileName, int trackNumber = 1)
        {
            this.fileName = fileName;
            this.trackNumber = trackNumber;
        }

       // public Action<NoteEvent>? NoteEventCallback { get; set; }

        public MidiNoteMappings CreateMappings()
        {


            var mf = new MidiFile(fileName, false);
            var allNoteEvents = mf.Events[trackNumber].OfType<NoteEvent>();
            Dictionary<int, NoteEvent> usedNotes = new Dictionary<int, NoteEvent>();


            foreach (var midiEvent in allNoteEvents)
            {
                if (MidiEvent.IsNoteOn(midiEvent))
                {
                    usedNotes[midiEvent.NoteNumber] = midiEvent;
                }
                else
                {
                    // Console.Write("    ");
                }

            }

            MidiNoteMappings noteMappings = new MidiNoteMappings();

            noteMappings.RequiredChannels = usedNotes.Count;
            noteMappings.MaxVelocity = usedNotes.Values.Max(x => x.Velocity);
            noteMappings.MinVelocity = usedNotes.Values.Min(x => x.Velocity);


         


            var channelQueue = new Queue<RelayChannel>(RelayChannels.Current);
          



            foreach (var (number, midiEvent) in usedNotes.OrderBy(x => x.Key))
            {

                if (channelQueue.TryDequeue(out var channel))
                {

                    if (midiEvent is NoteOnEvent noteOnEvent)
                    {

                        if (noteOnEvent.OffEvent != null)
                        {
                            if (noteOnEvent.OffEvent.DeltaTime > noteMappings.MaxMidiDeltaTime)
                            {
                                noteMappings.MaxMidiDeltaTime = noteOnEvent.OffEvent.DeltaTime;
                            }
                        }

                    }

                    noteMappings[midiEvent.NoteNumber] = new MidiNoteMapping(channel)
                    {
                        NoteName = midiEvent.NoteName
                    };

                }
                else
                {
                    Console.WriteLine("WARNING: Not enough channels!!");
                    break;
                }

            }

            return noteMappings;
        }


        public void Play(Action<NoteEvent> noteEventCallback)
        {
            stopRequested = false;

            var mf = new MidiFile(fileName, false);

            //var timeSignature = mf.Events[0].OfType<TimeSignatureEvent>().FirstOrDefault();
            var tempoEvent = mf.Events[0].OfType<TempoEvent>().FirstOrDefault();

            TimeSpan tickDuraration = TimeSpan.FromMicroseconds(tempoEvent.MicrosecondsPerQuarterNote / mf.DeltaTicksPerQuarterNote);

            var noteQueue = new Queue<NoteEvent>(mf.Events[trackNumber].OfType<NoteEvent>());

            int tick = 0;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (noteQueue.Count > 0 && !stopRequested)
            {
                while (noteQueue.TryPeek(out var peekedEvent) && peekedEvent.AbsoluteTime <= tick)
                {
                    var noteEvent = noteQueue.Dequeue();

                    noteEventCallback?.Invoke(noteEvent);
                }
                if (stopWatch.Elapsed.TotalMicroseconds > tickDuraration.TotalMicroseconds)
                {
                    tick++;
                    stopWatch.Restart();
                }
               
                if (Console.KeyAvailable) 
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        stopRequested = true;
                    }
                }
            }

        }

        public void Stop()
        {
            stopRequested = true;
        }
    }
}
