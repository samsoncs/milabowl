---
title: Tech update - omskriving av rules engine 🤖
date: 2024-03-15
author: "Sam"
description: Omskrivning av Milabowl sin rules engine
tags: [Tech]
---

Backend prosesseringen til Milabowl bærer mye preg av teknsik gjeld og dårlig struktur som følge av mange snarveier når man gikk fra live
applikasjon i Azure til Github Pages. En omfattende omskrivning nærmer seg nå slutten. Hovedfokuset er å senke terskelen for å legge til 
nye regler. Dette blir gjort ved å ta i bruk en helt ny rules engine, "MilaEngine"🧙🏼‍♂️. Det eneste man trenger for å legge til en ny regel 
er å implementere IMilaRule interfacet, deretter skjer alt automagisk:

``` C#
public record MilaRuleResult(string RuleName, string RuleShortName, decimal Points);

public interface IMilaRule
{
    MilaRuleResult Calculate(MilaGameWeekState userGameWeek);
}
```

Alle implementasjoner vil automatisk bli plukket opp av .NET sin DI container, og automatisk kalkulert og lagt inn i game_state.json. 
Frontendend vil også bli oppdatert til å automatisk plukke opp alle nye regler, og rendre nye regler. For å forenkle enda mer kan man
implementere den abstrakte klassen MilaRule. Man må da sette et ShortName (som vil dukke opp i tabellene i front end), og en metode 
for å beregne poeng. Regelen vil dukke opp med samme navn som klassen i JSON resultatet.

``` C#
public abstract class MilaRule : IMilaRule
{
    protected abstract string ShortName { get; }
    protected abstract decimal CalculatePoints(MilaGameWeekState userGameWeek);

    public MilaRuleResult Calculate(MilaGameWeekState userGameWeek)
    {
        return new MilaRuleResult(GetType().Name, ShortName, CalculatePoints(userGameWeek));
    }
}
```

For å vise hvor enkelt det blir å legge til en ny regel kan vi se på implementasjonen av regelen for gult kort:

``` C#
public class YellowCards : MilaRule
{
    protected override string ShortName => "YC";

    protected override decimal CalculatePoints(MilaGameWeekState userGameWeek)
    {
        return userGameWeek.User.Lineup
            .Where(pe => pe.YellowCards == 1)
            .Sum(pe => pe.Multiplier);
    }
}
```

For å sikre at nye regler fungerer som tenkt vil byggen feile ❌ om man ikke har lagt til minst en test pr. regel.

I tillegg til å gjøre det enklere å legge til nye regler, vil prosesseringene gå mye raskere og favhengigheten til SQL databasen i bunn 
blir fjernet. Planen er å ta nåværende løsning for import av data til SQL, lage et eget prosjekt og rename det til Milalytics, så man
fortsatt har mulighet å analysere FPL / Mila data med SQL (spesielt handy rundt MilaAwards).

NB: Navn på klasser og interfaces er fortsatt i endring