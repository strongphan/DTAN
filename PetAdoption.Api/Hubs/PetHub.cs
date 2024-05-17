using Microsoft.AspNetCore.SignalR;
using PetAdoption.Shared.IPetHub;

namespace PetAdoption.Api.Hubs;

public class PetHub : Hub<IPetClientHub>, IPetServerHub
{
    private static readonly IDictionary<int, int> _petsBeingViewed = new Dictionary<int, int>();
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
    public async Task PetAdopted(int petId)
    {
        await Clients.Others.PetAdopted(petId);
        if (_petsBeingViewed.ContainsKey(petId))
        {
            _petsBeingViewed.Remove(petId);
        }
    }

    public Task ReleaseViewingThisPet(int petId)
    {
        if (!_petsBeingViewed.TryGetValue(petId, out int currentlyViewing))
        {
            currentlyViewing--;
        }
        if (currentlyViewing <= 0)
        {
            _petsBeingViewed.Remove(petId);
        }
        else
        {
            _petsBeingViewed[petId] = currentlyViewing;

        }
        return Task.CompletedTask;
    }

    public async Task ViewingThisPet(int petId)
    {
        if (!_petsBeingViewed.TryGetValue(petId, out int currentlyViewing))
        {
            currentlyViewing = 0;
        }
        else
        {
            await Clients.Others.PetIsBeingViewed(petId);

        }
        _petsBeingViewed[petId] = currentlyViewing++;
        await Clients.Others.PetIsBeingViewed(petId);
    }
}
