using BoardgameEvenings.NET;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new BoardgameAPI(builder.Configuration.GetValue<string>("NodeAPIBaseURI") ));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/events", async (BoardgameAPI api) =>
{
    try
    {
        List<EventObject>? list = await api.GetEvents();
        return list?.Count > 0 ? Results.Ok(list) : Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/event/{id:int}", async (BoardgameAPI api, int id) =>
{
    try
    {
        EventDetails? gameEvent = await api.GetEventById(id);
        return gameEvent != null ? Results.Ok(gameEvent) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/event", async (BoardgameAPI api, Event gameEvent) =>
{
    try
    {
        await api.CreateEvent(gameEvent);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/event/{id:int}", async (BoardgameAPI api, int id, Event gameEvent) =>
{
    try
    {
        await api.UpdateEvent(id, gameEvent);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/event/{id:int}", async (BoardgameAPI api, int id) =>
{
    try
    {
        await api.DeleteEvent(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/event{id:int}/attend", async (BoardgameAPI api, int id, Attendee attendee) =>
{
    try
    {
        await api.AttendEvent(id, attendee);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/event/{eventId:int}/attendee/{attendeeId:int}", async (BoardgameAPI api, int eventId, int attendeeId) =>
{
    try
    {
        await api.RemoveAttendance(eventId, attendeeId);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/event{eventId:int}/attendees", async (BoardgameAPI api, int eventId) =>
{
    try
    {
        List<AttendeeObject>? list = await api.GetAllAttendees(eventId);
        return list?.Count > 0 ? Results.Ok(list) : Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/event/{eventId:int}/attendee/{attendeeId:int}", async (BoardgameAPI api, int eventId, int attendeeId, Attendee attendee) =>
{
    try
    {
        await api.UpdateAttendee(eventId, attendeeId, attendee);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();
