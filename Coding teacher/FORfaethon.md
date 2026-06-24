# The Receiver CRUD Blueprint — what actually happened

Grab a coffee. We just spent a long session building one feature — Create/Read/Update/Delete for `Receiver` — and you wrote almost every line of it yourself while I poked holes in it. That's a lot of small moments to lose track of, so let's walk through the whole thing the way I'd explain it to you in person, not as a changelog.

## Step 1: The approach, and why we started where we started

You already had one working feature in this app: `Exporter`. Full CRUD wasn't built for it yet, but Create and Index were done, tested, working. So instead of inventing a new feature from a blank page, the whole session was built on a simple idea: **find the smallest already-working thing that looks like what you want, and adapt it.**

That's why every single step started with "look at how Exporter does it." The controller's constructor, the Index action, the Create GET/POST pair — none of it was designed fresh. It was read, understood, and re-typed with `Receiver` swapped in for `Exporter`. This sounds almost too simple to call a "strategy," but it's actually one of the most reliable moves in programming: when you don't know what the right shape of something is, find a working analog and copy its shape, not its exact text. You're not cargo-culting if you understand *why* each piece is there — and that's exactly what all those guiding questions were for, forcing you to explain the "why" before moving forward.

The starting point mattered too: the `Receiver` model and its `DbSet` already existed in `CustomsErpContext`. That meant the actual missing piece was narrower than "build a feature" — it was "build the Controller and Views that talk to data that's already wired up." Knowing exactly how much was already done versus how much was new is half of scoping any task correctly.

## Step 2: What we didn't do, and why

A few roads not taken, deliberately:

**Building `Shipment` first instead of `Receiver`.** Shipment is the aggregate root — it touches Exporter, Receiver, Port, Warehouse, ShippingCompany, and a list of ProductVarieties. It's the most *valuable* thing to eventually build, but it's also the most complex, with five foreign keys and at least one dropdown-heavy form. Starting there would have meant learning five new concepts simultaneously (relationships, dropdown population, nested collections) instead of one at a time. We picked the simplest unbuilt entity instead, so each new idea (Update's "fetch existing, write hidden id," Delete's "confirm before destroying") could be learned in isolation before the harder stuff.

**Me writing the code instead of you.** I could have produced working Controller/View code for Receiver in about thirty seconds. We didn't do that, on purpose, because you told me upfront you wanted to write it yourself with me guiding. The cost was real — this took many more rounds than if I'd just handed you finished files. The benefit is that you personally hit the `RedirectToAction` vs `View` bug, the missing-null-check bug, the duplicate-method-signature bug, and fixed every one of them with your own hands. That's the entire point of doing it this way: the bugs you debug yourself are the ones that actually teach you something.

**PUT/PATCH for Update, instead of POST.** You raised this yourself, and it's a great instinct — semantically, "replace this resource" *is* what PUT means in REST. We rejected it anyway, for a very concrete reason: this is a server-rendered MVC app where the browser submits plain HTML `<form>` elements, and forms can only ever send GET or POST. There's no JavaScript layer here making `fetch` calls with custom HTTP verbs. PUT/PATCH are the right call in a JSON API with a JS frontend; they're simply not available to you in a vanilla form-post architecture. Good instinct, wrong tool for *this* codebase.

**Trusting the hidden `Id` field alone for Update/Delete.** The simpler version — just read `Receiver.Id` off the posted form — works. We went one step further and made the POST take `int id` from the *route* as well, then check it against the posted object's `Id`, rejecting mismatches with `BadRequest()`. This is more code for a feature with no real auth system yet, so arguably it's premature. But it's a habit worth building now, while it's cheap, rather than retrofitting it later once there are real users and real stakes.

**Deleting immediately on a GET click.** The lazy version of Delete is a link that deletes the row the instant it's clicked. We explicitly didn't do this, because GET requests are supposed to be "safe" — no side effects — and a delete-on-GET can get triggered by things you never intended: a browser prefetching links, a search engine crawler following them, a user double-clicking and firing two requests. The GET shows a confirmation page; only the POST (which requires an explicit form submission and an anti-forgery token) actually deletes.

## Step 3: How the pieces fit together

Here's the dependency chain, bottom to top, and why it had to go in this order:

`Core` (the `Receiver` model) → `Data` (the `DbSet<Receiver>` and its unique-index rule on `Eori`) → `Web/Controllers` (the actions that read/write through that `DbSet`) → `Web/Views` (the HTML that calls those actions). Each layer only makes sense once the layer below it exists — you can't write a controller action against a `DbSet` that isn't registered, and you can't write a view that posts to an action that doesn't exist yet. That's why we always wrote controller code *before* its corresponding view, even though visually the view feels like "the thing you actually see" — the view is just a thin skin over what the controller already promises to do.

Within the controller itself, the four operations also had to be learned in a specific order, because each one builds on a concept from the last:

- **Index** (read all) — the absolute baseline: fetch, hand to a view, loop in Razor.
- **Create** (write new) — introduces the GET/POST pair pattern, `asp-for` binding, and the anti-forgery token. The model object doesn't exist yet, so there's no `Id` to worry about.
- **Update** (read one, then write it back) — combines Index's "fetch by lookup" with Create's "POST and save," and adds the genuinely new problem: this object *already has* an identity (`Id`), and that identity has to survive the round-trip through an HTML form without becoming forgeable. This is where the route-id-plus-hidden-field cross-check was born.
- **Delete** (destroy) — reuses Update's "fetch by id" and "POST with anti-forgery," but strips away the editable fields entirely, which is exactly what exposed the duplicate-method-signature problem (more on that below) and forced the cleanest version of the id-only pattern.

Each piece really is a layer on the one before it. That's not an accident of how we happened to do it — it's the only sane order to learn this in.

## Step 4: Tools, and the road not taken for each

**ASP.NET Core MVC with Razor views** — already the project's chosen framework, and a good fit for what this is: a server-rendered, form-driven internal tool (an ERP), not a public-facing app needing rich client-side interactivity. The alternative would be a SPA frontend (React/Vue) talking to a JSON API — much more moving parts, much more setup, and total overkill for "fill out a form, see a table." If this app later needs live updates, drag-and-drop, or complex client interactivity, that calculus changes. Today, it doesn't need to.

**EF Core** as the ORM — also already chosen. The alternative would be raw ADO.NET or a micro-ORM like Dapper, hand-writing SQL. EF Core's `Add`/`Update`/`Remove`/`SaveChangesAsync` map almost one-to-one onto Create/Update/Delete, which is exactly why the controller code reads so cleanly. The tradeoff you're implicitly accepting is less control over the generated SQL and a bit of "magic" (e.g., `Update()` doesn't immediately hit the database — `SaveChangesAsync()` is what actually does). That magic bit you personally: you wrote `_dbContext.Update(receiver)` more than once before remembering `SaveChangesAsync()` has to follow it.

**SQLite** as the database — lightweight, file-based, zero setup. Great for development. Worth knowing this isn't usually the production choice for a multi-user ERP (file-based databases don't handle concurrent writers well) — that's a "later" problem, not a "now" problem, but worth flagging so it doesn't surprise you.

**`[ActionName]` attribute routing for Delete's POST** — instead of naming both GET and POST methods the literal same C# name (`Delete`), which would have caused the exact compiler conflict you hit. The alternative, more traditional approach (and the one classic ASP.NET MVC scaffolding uses) is `Delete(int id)` for GET and `DeleteConfirmed(int id)` with `[ActionName("Delete")]` for POST — keeping the *URL* the same for both while giving the compiler two distinct method names. You actually landed on a variant of this (different action name for each, `Delete` and `DeleteConfirmed`), which works just as well, just with two different URLs instead of one shared one. Both are legitimate; it's a style choice.

## Step 5: Tradeoffs — what we prioritized, what we paid for it

**Understanding over speed.** This was the biggest one. Every bug you hit and fixed yourself cost real time — probably five to ten times longer than if I'd written the files directly. What you got in exchange: you can now explain *why* `RedirectToAction` matters, not just that it's "the right line to write." That's the trade we made on purpose, per how you wanted to work.

**Defensive correctness over minimal code.** The route-id/hidden-field cross-check on Update, the `NotFound()` handling everywhere, the null-check before dereferencing `receiver.Id` — none of these are strictly required for the app to "work" in a happy-path demo. They cost extra lines and extra thinking. What they buy you: an app that fails predictably (a 404, a 400) instead of unpredictably (a crash, a silent no-op, a security hole) when something unexpected happens.

**Functional correctness over polish.** Right now, neither `Receiver` nor `Exporter` has a link in the site's navigation bar — you have to know the URL to get there. We chose to get the underlying logic exactly right first and explicitly deferred navigation/styling. The cost is the app looks unfinished if you clicked through it cold; the benefit is we didn't spend any of this session's energy on CSS while the actual data layer was still being debugged.

**Convention-based routing over attribute routing.** You actually tried attribute routing (`[HttpGet("{id}")]`) at one point, and we backed away from it — not because it's wrong in general, but because mixing it into a project that's otherwise 100% convention-routed, without a controller-level `[Route]` prefix, created a route that was accidentally far too broad (matching almost any single-segment URL on the site). Sticking with one routing style consistently is simpler to reason about than mixing two, especially before you fully understand the rules of each.

## Step 6: The mess — mistakes, dead ends, and how we got out

This is the part with the most signal, so let's not gloss over it. In rough chronological order:

- **Missing `using` statements.** Forgot them when first writing `ReceiverController`. Quick fix once you compared against `ExporterController`'s top lines.
- **`return View(nameof(Index))` instead of `RedirectToAction`.** This one's subtle and important: it would have compiled and even sort of "worked" in a demo, but it breaks the Post/Redirect/Get pattern, meaning a page refresh after saving would silently re-submit and create a duplicate row. The bug doesn't announce itself — it just causes weird behavior under specific conditions (hitting refresh).
- **Leftover `@model CustomsERP.Core.Exporter` in the Receiver Create view.** Pure copy-paste residue. It happened to still compile because Exporter and Receiver share the same property names — which is exactly what makes this kind of bug dangerous; it can hide for a long time before a property mismatch makes it loudly fail.
- **Table header/body column mismatch.** Added a column to the `<tbody>` rows without adding the matching `<th>` to `<thead>`. Classic "two places that have to agree, nothing enforces it" bug — we called this out explicitly because it's a pattern, not a one-off.
- **`[HttpPut]` on a page meant to be reached via a normal link.** Browsers can't issue PUT from a plain link or form — this method would have been literally unreachable through the UI as built.
- **Parameter named `ReceiverId` not matching the route's `id` token.** Wouldn't have crashed, just silently bound to `0` every time — a "fails quietly" bug, the worst kind.
- **Calling `.FirstOrDefault()` directly on `_dbContext` instead of `_dbContext.Receivers`.** Wouldn't compile — `DbContext` doesn't have query methods, only its `DbSet` properties do.
- **`async` without `await`, and `Task<IActionResult>` without `async`.** Two related slips around the same concept — marking something async when it isn't, or trying to return a plain value where a wrapped `Task` was expected.
- **Missing `@` before Razor expressions — twice.** `value="Model.Eori"` and `asp-route-id="Id"` both rendered as literal text instead of evaluating the C# behind them. This was the single most repeated mistake of the whole session, and it's worth burning into memory (see Step 7).
- **`<form>` nested inside `<tr>`.** Invalid HTML — browsers silently relocate the form outside the table, breaking the intended layout without throwing any visible error.
- **`<html>` tags inside a partial view.** The Delete confirmation view had its own `<html>` wrapper, even though it renders inside `_Layout.cshtml`'s existing one.
- **A bare `<button>` with `asp-action` attributes but no surrounding `<form>`.** Tag helpers like `asp-action` only function on `<form>` and `<a>` — on a lone button, they're inert and clicking does nothing.
- **Two methods both literally named `Delete(int id)`.** A genuine compiler error (`CS0111`), fixed by realizing `[HttpGet]`/`[HttpPost]` attributes mean nothing to the C# compiler's overload resolution — only distinct method signatures or distinct names do.
- **A leftover tamper-check that became tautological.** After simplifying Delete's POST to just take `int id` and look the receiver up itself, the line `if (Id != receiver.Id)` was comparing a value to itself — dead code masquerading as a security check, left over from when `receiver` used to come from client-posted data.
- **Stale dev server holding port 5146.** Not a code bug at all — a previous `dotnet run` (likely from Rider) was still running and blocking the new one from starting. Diagnosed with `lsof -i :5146`, fixed with `kill -9`.

None of these were exotic. Every single one is a normal part of writing real software, and you fixed all of them.

## Step 7: Pitfalls for next time — the "I wish someone told me" list

**The `@` symbol in Razor is not optional decoration.** Anywhere you want Razor to evaluate C# instead of printing literal text, it needs `@`. You hit this exact bug twice in one session (`value="Model.Eori"`, `asp-route-id="Id"`). Make it a reflex: any time you write an attribute value that "should" be dynamic, glance for the `@`.

**HTML tables have a strict content model.** `<tr>` accepts only `<td>`/`<th>` as direct children. Putting anything else (like a `<form>`) inside one doesn't error — the browser just silently moves it elsewhere in the rendered DOM. If something with a table ever looks visually wrong despite the source "looking right," check for invalid nesting first.

**Route parameter names matter, and they're invisible until they fail.** If your action parameter doesn't match the route token name (`id` by convention here), model binding fails silently, leaving the parameter at its default value. No exception, no warning — just wrong data flowing through with nobody complaining.

**GET must never have side effects.** Before adding any logic to a GET action, ask: would it be a problem if a browser prefetcher, search crawler, or accidental double-click triggered this twice, or triggered it by just *looking* at a link? If yes, it belongs behind a POST.

**Don't trust IDs (or anything else) coming from the client without a server-side anchor.** A hidden form field is not a security boundary — it's just another editable value sitting in HTML. If an action does something destructive or sensitive based on an ID, prefer getting that ID from a source the user can't casually edit (the URL, fetched fresh from a trusted query) over trusting whatever the form happens to post.

**Watch for dead checks after a refactor.** When you simplify a method's parameters or logic, go back and ask whether every line still does what it originally did. The tautological `Id != receiver.Id` check is a perfect example of code that *looks* protective but had quietly stopped doing anything.

**`[HttpGet]`/`[HttpPost]` are invisible to the C# compiler's overload resolution.** If you give a GET and POST method the same name and same parameter list, expecting the attributes to disambiguate them, you'll get a compile error. The compiler only sees method name + parameter types.

## Step 8: What an expert notices that a beginner doesn't

A beginner gets a feature working and stops there. An expert, looking at the exact same working code, asks a different question: *what happens at the edges?* What if the row doesn't exist? What if the user double-submits? What if someone edits the hidden field in dev tools? What if two people edit the same row at once? None of these show up in a normal click-through demo — they only show up when you deliberately go looking for them, which is exactly what the `BadRequest()`/`NotFound()` checks and the route-id cross-check were doing throughout this session.

An expert also notices when code has become *vestigial* — technically still there, but no longer serving its original purpose, like that leftover tamper-check after Delete's POST got simplified. Beginners tend to leave code alone if it "still compiles and still works." Experts re-read code after every refactor and ask "does this line still mean what it used to mean?"

And an expert recognizes structural smells even when nothing is currently broken — like two places (table header, table body) that have to be kept in sync by hand, with nothing enforcing that sync. It's not a bug today. It's a *future* bug, waiting for the day someone edits one side and forgets the other. Spotting "this works now but is fragile" is a different skill from spotting "this is currently broken," and it's the harder, more valuable one.

## Step 9: What carries over to completely different projects

A few of these ideas aren't specific to ASP.NET, EF Core, or even web development at all:

**Find a working analog, then adapt it — don't start from a blank page if you don't have to.** This applies to writing a new function in an unfamiliar language, setting up a new CI pipeline, or even structuring an essay. Find the closest thing that already works, understand why it works, then change only what genuinely needs to change.

**"Read" operations should never have side effects.** This isn't an HTTP-specific rule — it's a general design principle. A command-line tool's `--list` flag shouldn't modify anything. A database "SELECT" shouldn't trigger a trigger that writes data (without making that very obvious). Anytime an operation is framed as "just looking," guard it from accidentally also "doing."

**Never trust data from outside your boundary without checking it against something you control.** Whether that's a hidden form field, a query parameter, a JSON payload from an API caller, or a file someone uploaded — the same instinct applies: cross-check it against a source of truth you actually control before acting on it.

**Pass the minimum data an operation actually needs, not the most convenient amount.** Delete's POST went from taking a full `Receiver` object to just an `int id` — fetching the rest itself, server-side. This isn't just cleaner code; it's a smaller attack surface and fewer ways for stale or tampered data to sneak in. This generalizes directly to API design: design endpoints around what an operation *needs*, not what's lying around in the caller's context.

**When you simplify something, re-verify every line still makes sense afterward.** Refactoring is never "just deleting the parts you don't need anymore" — it's an invitation to recheck whether the remaining parts still mean what you think they mean.

That's the whole arc — from "copy a working Exporter pattern" to a fully working, reasonably hardened Receiver CRUD blueprint, with every wrong turn along the way being the actual mechanism by which you learned it. Next up: doing the same thing again on `Exporter`, except this time you're the one who already knows the pattern.
