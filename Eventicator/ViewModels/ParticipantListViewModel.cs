using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eventicator.Services;
using Models;
using System.Collections.ObjectModel;

namespace Eventicator.ViewModels;

public partial class ParticipantListViewModel : ObservableObject
{
    private readonly ApiService _api;
    private readonly INavigation _navigation;

    [ObservableProperty] private int eventId;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string title = "Teilnehmer";

    public ObservableCollection<Participant> Participants { get; } = new();

    public ParticipantListViewModel(int eventId, INavigation navigation)
    {
        _api = new ApiService();
        _navigation = navigation;

        EventId = eventId;
    }

    // Public, damit die View es aufrufen kann (OnAppearing)
    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            Participants.Clear();

            var items = await _api.GetParticipantsForEventAsync(EventId);

            foreach (var p in items)
                Participants.Add(p);

            Title = $"Teilnehmer ({Participants.Count})";
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Fehler", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task RefreshAsync() => await LoadAsync();

    [RelayCommand]
    public async Task DeleteAsync(Participant participant)
    {
        if (participant is null) return;

        var ok = await Shell.Current.DisplayAlert(
            "Löschen?",
            $"{participant.FirstName} {participant.LastName} wirklich löschen?",
            "Ja", "Nein");

        if (!ok) return;

        var success = await _api.DeleteParticipantAsync(participant.Id);

        if (!success)
        {
            await Shell.Current.DisplayAlert("Fehler", "Teilnehmer konnte nicht gelöscht werden.", "OK");
            return;
        }

        Participants.Remove(participant);
        Title = $"Teilnehmer ({Participants.Count})";
    }
}