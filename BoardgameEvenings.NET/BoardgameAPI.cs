using RestSharp;
// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace BoardgameEvenings.NET;

public record Event(int ID, string Date, string Name);

public record Attendee(string Name, string Vote);

public class BoardgameAPI
{
    public BoardgameAPI()
    {
        Console.WriteLine("API");
    }

    private static readonly RestClient _client = new("http://localhost:3000");

    public async Task<List<Event>?> GetEvents() => (await _client.GetAsync<Event[]>(new("/events")))?.ToList();
    
    public async Task<Event?> GetEventById(int id)
    {
        RestRequest req = new("/event/{id}");
        req.AddUrlSegment("id", id);

        return await _client.GetAsync<Event>(req);
    }

    public async Task CreateEvent(Event gameEvent)
    {
        RestRequest req = new("/event", Method.Post);
        req.AddJsonBody(gameEvent);

        RestResponse res = await _client.PostAsync(req);
        if (!res.IsSuccessful) throw new (res.ErrorMessage);
    }

    public async Task UpdateEvent(int id, Event gameEvent)
    {
        RestRequest req = new("/event/{id}", Method.Put);
        req.AddUrlSegment("id", id);
        req.AddJsonBody(gameEvent);

        RestResponse res = await _client.PutAsync(req);
        if (!res.IsSuccessful) throw new(res.ErrorMessage);
    }

    public async Task DeleteEvent(int id)
    {
        RestRequest req = new("/event/{id}", Method.Delete);
        req.AddUrlSegment("id", id);

        RestResponse res = await _client.DeleteAsync(req);
        if (!res.IsSuccessful) throw new(res.ErrorMessage);
    }

    public async Task AttendEvent(int id, Attendee attendee)
    {
        RestRequest req = new("/event/:id/attend", Method.Post);
        req.AddUrlSegment("id", id);
        req.AddJsonBody(attendee);

        RestResponse res = await _client.PostAsync(req);
        if (!res.IsSuccessful) throw new(res.ErrorMessage);
    }

    public async Task RemoveAttendance(int EventId, int AttendeeId)
    {
        RestRequest req = new("/event/:eventId/attend/:attendeeId", Method.Delete);
        req.AddUrlSegment("eventId", EventId);
        req.AddUrlSegment("attendeeId", AttendeeId);

        RestResponse res = await _client.DeleteAsync(req);
        if (!res.IsSuccessful) throw new(res.ErrorMessage);
    }
}