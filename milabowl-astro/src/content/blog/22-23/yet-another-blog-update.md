---
title: Blogseksjonen bruker nå Markdown!
date: 2023-01-26
author: MilaCorp
tags: [Announcement]
---

Enda en oppdatering av bloggen, blogseksjonen benytter Markdown filer "under the hood".
For å legge inn et nytt inlegg trenger man kun legge en markdown fil i mappen
`src/pages/blog/[sesong]` hvor sessong er notert 22-23 (for 2022-2023 sessong).
Takket være Astro 2.0 vil markdown dokumentet være beskyttet av typescript,
som vil sikre at ting som **dato**, **tittel** etc. blir lagt til, og med riktig
type.

Markdown gjør det enkelt å lage nye blogposts og style som man ønsker. Man
kan enkelt lage overskrifter i ulike størrelser:

# Overskrift 1

## Overskrift 2

### Overskrift 3

Lage lister:

- Første punkt
- Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis sagittis
  laoreet turpis vel blandit. Cras quis arcu cursus, ultricies nisi in,
  rhoncus ligula. Sed tortor risus, sagittis nec ornare nec

Lage numererte lister:

1. En
2. To
3. Tre

Man kan fremheve tekst med _italics_ eller **fet skrift**

Man kan legge til [linker](https://milabowl.com)

> Legge til sitater

---

Legge til linjer

---

Man kan også legge til kode blokker i et hvilket som helst kodespråk:

```tsx
// Example of .tsx code block
const FunctionalClass: React.FC<{}> = () => (
  <div>Example of a React functional component</div>
);
```

Alt som er gyldig markdown er støttet!
