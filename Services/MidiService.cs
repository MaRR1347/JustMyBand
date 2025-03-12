using NAudio.Midi;
using System;

public class MidiService : IDisposable
{
    private MidiIn _midiIn;
    private readonly MidiCommunicationService _midiCommunicationService;

    public MidiService(MidiCommunicationService midiCommunicationService)
    {
        _midiCommunicationService = midiCommunicationService;

        // Initialize MIDI input
        if (MidiIn.NumberOfDevices > 0)
        {
            _midiIn = new MidiIn(0); // Use the first MIDI device
            _midiIn.MessageReceived += MidiIn_MessageReceived;
            _midiIn.Start();
        }
        else
        {
            throw new InvalidOperationException("No MIDI devices found.");
        }
    }

    private void MidiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
    {
        // Check if the event is a NoteOn or NoteOff event
        if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn || e.MidiEvent.CommandCode == MidiCommandCode.NoteOff)
        {
            // Cast the MidiEvent to a NoteEvent
            var noteEvent = (NoteEvent)e.MidiEvent;

            // Define the velocities to exclude
            int[] excludedVelocities = { 7, 8, 9, 10, 64 };

            // Check if the velocity is NOT in the excluded list
            if (!Array.Exists(excludedVelocities, v => v == noteEvent.Velocity))
            {
                // Notify the communication service with the note number
                _midiCommunicationService.NotifyMidiNoteReceived(noteEvent.NoteNumber);
            }
        }
    }

    public void Dispose()
    {
        _midiIn?.Stop();
        _midiIn?.Dispose();
    }
}