using System;
using Microsoft.AspNetCore.Components;

public class MidiCommunicationService
{
    // Event to notify subscribers when a MIDI note is received
    public event EventHandler<int> OnMidiNoteReceived;

    // Method to trigger the event
    public void NotifyMidiNoteReceived(int noteNumber)
    {
        OnMidiNoteReceived?.Invoke(this, noteNumber);
    }
}