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

This Markdown file creates a page at `your-domain.com/page-1/`

Lage lister:
 - Første punkt
 - Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis sagittis 
 laoreet turpis vel blandit. Cras quis arcu cursus, ultricies nisi in, 
 rhoncus ligula. Sed tortor risus, sagittis nec ornare nec
 - Man kan fremheve tekst med _italics_ eller **fet skrift**
 - Legge ved [linker](https://milabowl.com) til sider
 
 Lage numererte lister:
1. En
2. To
2. Tre

---
Legge til linjer

---

Man kan også legge til kode blokker i et hvilket som helst kodespråk:
``` tsx
// Example of .tsx code block
const FunctionalClass: React.FC<{}> = () => (
    <div>
        Example of a React functional component
    </div>
);
```

Alt som er gyldig markdown er støttet (men det er ikke sikkert at man har 
CSS for det enda). Dersom man finner noe som ikke er stylet riktig kan man
fikse dette i `src/pages/blog/blog.css`