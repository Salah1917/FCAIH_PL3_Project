namespace Dictionary.Controllers

open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Dictionary

[<ApiController>]
[<Route("api/[controller]")>]
type WordsController(db: DictionaryContext) =
    inherit ControllerBase()
    
    // Get all words
    // GET: api/words
    [<HttpGet>]
    member _.GetAll() = task {
        let! entries = db.Entries.ToListAsync()
        return OkObjectResult(entries)
    }

    
    // Get word by Id
    // GET: api/words/5
    [<HttpGet("{id}")>]
    member _.Get(id: int) = task {
        let! entry = db.Entries.FindAsync(id)

        match entry with
        | null -> return NotFoundResult() :> IActionResult
        | _    -> return OkObjectResult(entry) :> IActionResult
    }


    // Create word
    // POST: api/words
    [<HttpPost>]
    member this.Post([<FromBody>] entry: WordEntity) : IActionResult =
        db.Entries.Add(entry) |> ignore
        db.SaveChanges() |> ignore

        this.CreatedAtAction("Get", {| id = entry.Id |}, entry) :> IActionResult


    // Update Word
    // PUT: api/words/5
    [<HttpPut("{id}")>]
    member this.Update(id: int, [<FromBody>] updated: WordEntity) : IActionResult =
        let entry = db.Entries.Find(id)

        if isNull entry then
            this.NotFound() :> IActionResult
        else
            entry.Word <- updated.Word
            entry.Definition <- updated.Definition

            db.SaveChanges() |> ignore
            this.Ok(entry) :> IActionResult


    // Delete Word
    // DELETE: api/words/5
    [<HttpDelete("{id}")>]
    member this.Delete(id:int) : IActionResult =
        let entry = db.Entries.Find(id)
        if isNull entry then
            this.NotFound() :> IActionResult
        else
            db.Entries.Remove(entry) |> ignore
            db.SaveChanges() |> ignore
            this.NoContent() :> IActionResult


    // Search for words
    // GET: api/words/search?query=someText
    [<HttpGet("search")>]
    member this.Search([<FromQuery>] query: string) = task {
        if System.String.IsNullOrWhiteSpace(query) then
            return this.BadRequest("Query cannot be empty.") :> IActionResult
        else
            // Case-insensitive, partial match search
            let! results =
                db.Entries.Where(fun e -> e.Word.ToLower().Contains(query.ToLower())).ToListAsync()
            return this.Ok(results) :> IActionResult
    }
