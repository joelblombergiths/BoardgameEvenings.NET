using BoardgameEvenings.NET;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new BoardgameAPI(builder.Configuration.GetValue<string>("NodeAPIBaseURI") ));

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "BoardgameEvenings API .NET");
    config.RoutePrefix = "";
});

app.UseHttpsRedirection();

app.MapGet("/events", async (BoardgameAPI api) =>
{
    try
    {
        List<EventDTO>? list = await api.GetEvents();
        return list?.Count > 0 ? Results.Ok(list) : Results.NoContent();
    }
    catch (NotFound)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/event/{id}", async (BoardgameAPI api, string id) =>
{
    try
    {
        EventDetails? gameEvent = await api.GetEventById(id);
        return gameEvent != null ? Results.Ok(gameEvent) : Results.NotFound();
    }
    catch (NotFound)
    {
        return Results.NotFound();
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
    catch (NotFound)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/event/{id}", async (BoardgameAPI api, string id, Event gameEvent) =>
{
    try
    {
        await api.UpdateEvent(id, gameEvent);
        return Results.Ok();
    }
    catch (NotFound)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/event/{id}", async (BoardgameAPI api, string id) =>
{
    try
    {
        await api.DeleteEvent(id);
        return Results.Ok();
    }
    catch (NotFound)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/event{id}/attend", async (BoardgameAPI api, string id, Attendee attendee) =>
{
    try
    {
        await api.AttendEvent(id, attendee);
        return Results.Ok();
    }
    catch (NotFound)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/event/{eventId}/attendee/{attendeeId}", async (BoardgameAPI api, string eventId, string attendeeId) =>
{
    try
    {
        await api.RemoveAttendance(eventId, attendeeId);
        return Results.Ok();
    }
    catch (NotFound)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/event{eventId}/attendees", async (BoardgameAPI api, string eventId) =>
{
    try
    {
        List<AttendeeDTO>? list = await api.GetAllAttendees(eventId);
        return list?.Count > 0 ? Results.Ok(list) : Results.NoContent();
    }
    catch (NotFound)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/event/{eventId}/attendee/{attendeeId}", async (BoardgameAPI api, string eventId, string attendeeId, Attendee attendee) =>
{
    try
    {
        await api.UpdateAttendee(eventId, attendeeId, attendee);
        return Results.Ok();
    }
    catch (NotFound)
    {
        return Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();
