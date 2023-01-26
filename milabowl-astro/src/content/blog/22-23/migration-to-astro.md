---
title: Migration of frontend to Astro JS
date: 2023-01-25
author: MilaCorp
description: "Frontend has been migrated to Astro JS 2.0, which allows for
low javascript footprint, framework agnostic code which is lightning fast!"
tags: [Tech]
---

The frontend of Milabowl has officially been migrated from create-react-app 
to using Astro JS 2.0 (released yesterday)💻. Astro JS is a framework for 
building static websites, with minimal JS bundles. Its default static
HTML rendering allows for blazing fast initial page loads. Astro also comes
with built in support for Markdown, which is going to be used for the blogging 
section in the future. For styling and responsiveness TailwindCSS has been used.

So, what does this have to say for Milabowl frontend? Pretty much nothing, other 
than getting to learn a hot new JS framework. Astro is framework agnostic,
and has first-class support for writing UI componentss in all the popular frameworks 
such as React, Vue, Svelt and Solid. All libraries can be mixed and matched in 
the same app. Is it smart to mix and match UI technologies inside one app? Probably 
not, but if you want to test out a different UI library feel free to do so!

Lastly Astro JS comes with Vite as the dev server, which builds super fast
compared to webpack, and hot module reloads (HMR) are near instant. In the process
of migrating a dark-mode has also been added to the UI☀️🌙