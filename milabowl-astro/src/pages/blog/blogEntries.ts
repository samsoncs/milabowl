export interface BlogEntry {
  title: string;
  date: string;
  paragraphs: string[];
}

const blogEntries: BlogEntry[] = [
  {
    title: "Kunngjøring: Ny blog feature!💥",
    date: "04.01.2023",
    paragraphs: ["Milabowl har nå en egen blog! Rundesammendrag legges ut her."]
  },
  {
    title: "Game Week 18",
    date: "02.01.2023",
    paragraphs: [
      `Da har vi passert nyttår, og Milabowl starter 2023 med et realt fyrverkeri🎆. `,

      `Mye poenginflasjon pga. gule kort denne runden. Laveste score ble hele 4 poeng 
        (Boris ofc😅). Patterson sørget for 69 sub, og at Filip stikker av med rundeseier 
        med hele 9.19 poeng🏆👏! Ingen utslag på tabellen denne runden.`,

      `For undertegnede ble Bueno med 6 poeng byttet inn for en Pep-roulette Cancelo. 
        Med det kan jeg bekrefte at det er de spillerne som ender opp på benken som pr. 
        dags dato teller som bench fail, og ikke de som er satt på benken ved rundestart. 
        Dette sparte Haalandslaget for -2 benchfail, muchos Bueno! 🌮🌵`
    ]
  },
  {
    title: "Game Week 17",
    date: "29.12.2022",
    paragraphs: [
      `Endelig er en lang og velfortjent FPL ferie over! Vi kan legge bak oss bilder av 
      Messi som gråter, Ronaldo som gråter av andre grunner. Nå ser vi frem til bilder 
      av Haalands motspillere som gråter av tredje grunner. 😭`,

      `Vi har måtte lide gjennom et mesterskap styrt av korrupsjon, men får betalt i 
      form av ubegrensede bytter.🤑`,

      `Det ble en ganske mild start på FPL returen. Haalandslaget startet boxing day 
      med en real uppercut som førte til 3.5 poeng og en rundeseier i lommen 💪👊.`,

      `For noen (Boris... kremt!) startet romjulen nede for telling etter en kraftig 
      knockout🤕. Etter å ha etterlatt Mitrovic og hele 18 poeng på benken, endte 
      runden med -1.5 poeng og rundetap. For å gjøre vondt verre, ble han i tilleg 
      passert av Premier Lag, som nå ligger på 6 plass totalt.`,

      `I bunnen av tabellen plukket eMILA med seg 3 poeng, og tettet med det luken 
      til laget som ikke lenger ser ut som en million (fikk kun 0.5 poeng).💵`
    ]
  },
  {
    title: "Game Week 16",
    date: "11.20.2022",
    paragraphs: [
      `Da har vi omsider kommet til den mye omtalte VM-pausen. Endelig kan vi legge 
      fra oss uviktigheter som menneskerettigheter og arbeidsvilkår og heller fokusere 
      på årets viktigste og minst korrupte konkurranse... nemlig Milabowl 🏆⚽!`,

      `En liten oppdatering fra tech avdelningen i Mila Inc: `,

      `\t- 8 november lanserte Microsoft .NET 7, og i dag 20. november har vi latt oss 
      rive med og oppgradert Milabowl til nyeste versjon av .NET. `,

      `\t- Etter ønske fra flere, vil det på nyåret bli satt opp introduksjonskurs: 
      mila-101. Dette gjøres slik at flere kan være med å vedlikeholde, utvikle og 
      oppdatere Milabowl. 🤓`,

      `Når det kommer til GW 16 har Haalandaise Saus gjort det meget skarpt, med 7
       poeng. Det har denne sessongen gått så bra at undertegnede begynner å gå tom 
       for saus-relaterte ordspill! Håper Filip Coutinho gjør det bedre som dørvakt👮, 
       da han og eMILA kun tar med seg med 1 skarve poeng fra runden. Sistnevnte har 
       med det inntatt sisteplassen før landaslagspausen!📉`,

      `Refleksjoner rundt første "tredjedel" av sessongen kommer på et senere tidspunkt.
       For nå er det bare å nyte en kald øl, mens man ser på en gjeng supportere som 
       ikke får nyte en kald øl!🍻`
    ]
  }
];

export { blogEntries };
