using System;
using System.Device.Gpio;
using System.Threading;
using NAudio.Midi;
using System.Diagnostics;
using Pillowsoft.GhostKeys;
using System.Linq;

//https://onlinesequencer.net/1248237
//string fileName = "MidiFiles/Heart_and_Soul_bass_v2.mid";


string? fileName = null;




Dictionary<int, string> midiFiles = new Dictionary<int, string>();


int midiFileCount = 0;

foreach(var file in Directory.GetFiles("MidiFiles").OrderBy(x => x))
{
    midiFileCount++;
    midiFiles[midiFileCount] = file;
}


while (fileName == null)
{

    Console.Clear();

    Console.WriteLine("Select a file");
    Console.WriteLine();
    foreach (var (i, file) in midiFiles)
    {
        Console.WriteLine($"{i.ToString().PadLeft(3)} - {Path.GetFileNameWithoutExtension(file)}");
    }
    Console.WriteLine();
    string input = Console.ReadLine();


    if (int.TryParse(input, out var selection))
    {
        fileName = midiFiles.GetValueOrDefault(selection);
    }
}

Console.Clear();

Console.WriteLine("Playing " + fileName);
Console.WriteLine();
Console.WriteLine("Arrange solenoids, and turn on relay board.");
Console.WriteLine();

MidiReader midiReader = new MidiReader(fileName, 1);


var mappings = midiReader.CreateMappings();

mappings.WriteToConsole();


MidiGpioHandler gpio = new MidiGpioHandler(mappings);
gpio.Reset();
gpio.ToggleReadyLight(true);

Console.WriteLine();
Console.WriteLine("When ready, press any key to continue.");
Console.ReadKey(true);



while (true)
{
    Console.Clear();

    gpio.ToggleReadyLight(false);
    gpio.ToggleBusyLight(true);

    Thread.Sleep(1000);

    midiReader.Play((midiEvent) =>
    {

        var mapping = mappings.GetValueOrDefault(midiEvent.NoteNumber);

        if (mapping != null)
        {
            Console.SetCursorPosition(0, mapping.RelayChannel.RelayChannelNumber);

            if (MidiEvent.IsNoteOn(midiEvent))
            {

                gpio.PlayNote(mapping);
                Console.Write(midiEvent.NoteName);

            }
            else
            {
                gpio.StopNote(mapping);
                Console.Write("    ");
            }
        }
    });

     Console.Clear();
    Console.WriteLine("Song finished.");



    gpio.Reset();
    gpio.ToggleReadyLight(true);
    gpio.ToggleBusyLight(false);
   
    

    mappings.WriteToConsole();

    Console.WriteLine("Press escape to exit, or any other key to play again.");
    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        break;
    }

}
Console.WriteLine("Bye!");

