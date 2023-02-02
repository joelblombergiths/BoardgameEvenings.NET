using System.Net;
using RestSharp;

// ReSharper disable ClassNeverInstantiated.Global

namespace BoardgameEvenings.NET;

public record Event(string Date, string Name);
public record EventDTO(string ID, string Date, string Name);
public record EventDetails(string Date, string Name, string TopVote);

public record Attendee(string Name, string Vote);
public record AttendeeDTO(string ID, string Name, string Vote);


public class BoardgameAPI
{
    private readonly RestClient _client;

    public BoardgameAPI(string? BaseURI)
    {
        if (string.IsNullOrEmpty(BaseURI)) throw new("Node API Base URI is not Defined!");
        _client = new(BaseURI);
    }

    public async Task<List<EventDTO>?> GetEvents()
    {
        RestRequest req = new("/events");

        RestResponse<EventDTO[]> res = await _client.ExecuteAsync<EventDTO[]>(req);
        if (!res.IsSuccessful)
        {
            throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }

        return res.Data?.ToList();
    }

    public async Task<EventDetails?> GetEventById(string id)
    {
        RestRequest req = new("/event/{id}");
        req.AddUrlSegment("id", id);

        RestResponse<EventDetails> res = await _client.ExecuteAsync<EventDetails>(req);
        if (!res.IsSuccessful)
        {
           throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }

        return res.Data;
    }

    public async Task CreateEvent(Event gameEvent)
    {
        RestRequest req = new("/event", Method.Post);
        req.AddJsonBody(gameEvent);

        RestResponse res = await _client.ExecuteAsync(req);
        if (!res.IsSuccessful)
        {
            throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }
    }

    public async Task UpdateEvent(string id, Event gameEvent)
    {
        RestRequest req = new("/event/{id}", Method.Put);
        req.AddUrlSegment("id", id);
        req.AddJsonBody(gameEvent);

        RestResponse res = await _client.ExecuteAsync(req);
        if (!res.IsSuccessful)
        {
            throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }
    }

    public async Task DeleteEvent(string id)
    {
        RestRequest req = new("/event/{id}", Method.Delete);
        req.AddUrlSegment("id", id);

        RestResponse res = await _client.DeleteAsync(req);
        if (!res.IsSuccessful)
        {
            throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }
    }

    public async Task AttendEvent(string id, Attendee attendee)
    {
        RestRequest req = new("/event/{id}/attend", Method.Post);
        req.AddUrlSegment("id", id);
        req.AddJsonBody(attendee);

        RestResponse res = await _client.ExecuteAsync(req);
        if (!res.IsSuccessful)
        {
            throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }
    }

    public async Task RemoveAttendance(string eventId, string attendeeId)
    {
        RestRequest req = new("/event/{eventId}/attendee/{attendeeId}", Method.Delete);
        req.AddUrlSegment("eventId", eventId);
        req.AddUrlSegment("attendeeId", attendeeId);

        RestResponse res = await _client.ExecuteAsync(req);
        if (!res.IsSuccessful)
        {
            throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }
    }

    public async Task<List<AttendeeDTO>?> GetAllAttendees(string eventId)
    {
        RestRequest req = new("/event/{eventId}/attendees");
        req.AddUrlSegment("eventId", eventId);

        RestResponse<AttendeeDTO[]> res = await _client.ExecuteAsync<AttendeeDTO[]>(req);
        if (!res.IsSuccessful)
        {
            throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }

        return res.Data?.ToList();
    }

    public async Task UpdateAttendee(string eventId, string attendeeId, Attendee attendee)
    {
        RestRequest req = new("/event/{eventId}/attendee/{attendeeId}", Method.Put);
        req.AddUrlSegment("eventId", eventId);
        req.AddUrlSegment("attendeeId", attendeeId);
        req.AddJsonBody(attendee);

        RestResponse res = await _client.ExecuteAsync(req);
        if (!res.IsSuccessful)
        {
            throw res.StatusCode.Equals(HttpStatusCode.NotFound) ? new NotFound() : new Exception(res.Content);
        }
    }
}
