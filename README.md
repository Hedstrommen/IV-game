# PVK-spelet

Ett utbildningsspel i WinUI 3 som lär sjuksköterskestudenter att lägga in en perifer venkateter (PVK) på rätt sätt enligt [Vårdhandboken – Perifer venkateter](https://www.vardhandboken.se/katetrar-sonder-och-dran/perifer-venkateter/).

Spelet är en stegvis simulering: du väljer ett patientfall och arbetar igenom hela proceduren – förberedelser, val av ven, inläggning, fixering och dokumentation, avlägsnande samt komplikationer (tromboflebit). Varje val ger omedelbar återkoppling med hänvisning till Vårdhandboken, och kritiska fel (t.ex. att palpera med skyddshandskar eller återföra en mandräng) markeras särskilt.

Spelet innehåller interaktiva moment: du plockar material direkt i skåpet, väljer stickställe genom att klicka på platsen, utför desinfektionen och avlägsnandet i rätt ordning och väljer rätt fixeringsmaterial. Efter varje moment får du omedelbar återkoppling per valt föremål, och på resultatsidan får du en genomgång av exakt vad du behöver förbättra. Ikoner i spelet kommer från OpenMoji (CC BY-SA 4.0, se `IV-game.Presentation/Assets/Icons/OPENMOJI_LICENSE.txt`).

## Köra spelet

### Visual Studio

1. Klona repot.
2. Öppna `IV-game.sln` (eller `IV-game.Presentation.slnx`) i Visual Studio 2022 med arbetsbelastningen **.NET Desktop Development** (inkluderar WinUI/Windows App SDK-stöd).
3. Vänta på att NuGet återställer paket (sker automatiskt vid första bygget).
4. Tryck F5 (Start) med `x64` som plattform.

### Visual Studio Code

1. Klona repot och öppna mappen i VS Code.
2. Installera de rekommenderade tilläggen (får du förslag på automatiskt): **C#** och **C# Dev Kit**.
3. Tryck F5 – bygguppgiften `build` kompilerar lösningen och startar spelet.

Krav i båda fallen: .NET 8 SDK och Windows App SDK (hämtas automatiskt av NuGet vid det första bygget). Inga andra nedladdningar behövs.

## Arkitektur (DDD)

Lösningen följer Domain-Driven Design med fyra projekt:

| Projekt | Ansvar |
|---|---|
| `IV-game.Domain` | Domänmodell och affärsregler: procedurens faser och steg, patientfall, vener, venvalsprioritering, tromboflebit-gradering samt poängsättning. Inga beroenden utåt. |
| `IV-game.Application` | Användningsfall som orkestrerar spelet (`StartGameUseCase`, `SubmitAnswerUseCase`, `FinishGameUseCase`) samt repo-abstraktioner. Beroende: Domain. |
| `IV-game.Infrastructure` | Implementeringar: JSON-baserade repos för patientfall och procedursteg, samt poänghantering i lokal fil. Beroende: Application + Domain. |
| `IV-game.Presentation` | WinUI 3-app (MVVM-aktigt med vyer/kod bakom): StartView, GameView, ResultView, HighScoresView. Beroende: alla lager. |

Spelinnehållet (patientfall och alla 20 procedursteg) ligger som JSON under `IV-game.Infrastructure/Content/` och kan utökas utan kodändringar.

## Innehåll enligt Vårdhandboken

Spelet täcker hela kedjan:

- **Förberedelser** – miljö, identitetskontroll och anamnes, basala hygienrutiner, material och smärtlindring
- **Val av ven** – prioriteringsordning (underarm → handrygg → armveck → överarm → nedre extremitet), undvik dialysfistel/trombosarm, stas med blodtrycksmanschett
- **Inläggning** – skyddshandskar, desinfektion med klorhexidinsprit, mandrängshantering, stas, spolning med NaCl
- **Fixering och dokumentation** – högpermeabelt polyuretanförband, märkning med datum/klockslag/signatur, yttre förband, journaldokumentation
- **Avlägsnande** – när PVK ska avlägsnas och korrekt arbetsgång
- **Komplikationer** – tromboflebitens symtom, åtgärd och byte av PVK var 72:a timme

## Källor

Allt innehåll bygger på [Vårdhandboken – Perifer venkateter](https://www.vardhandboken.se/katetrar-sonder-och-dran/perifer-venkateter/), framför allt avsnittet [Inläggning och avlägsnande](https://www.vardhandboken.se/katetrar-sonder-och-dran/perifer-venkateter/inlaggning-och-avlagsnande/).
