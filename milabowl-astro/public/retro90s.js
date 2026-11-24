(function () {
  'use strict';
  /*
   * retro90s.js — a "90s MODE" easter egg (Sixties Press style), fully self-contained.
   * Loaded by ONE line before </body>: <script src="/retro90s.js" defer></script>
   * It only: adds/removes the class `retro-90s` on <html>; adds two <style> tags to <head>; appends its own
   * elements at the END of <body> (never before or inside the framework's nodes). OFF leaves only the small pill.
   * Remove: delete this file and the one script line.
   */
  if (window.__retro90sLoaded) return;
  window.__retro90sLoaded = true;

  /* ===== CONFIG ===== */
  const CLS = 'retro-90s';
  const KEY = 'milabowl-90s';
  const HITS_KEY = 'milabowl-90s-hits';
  const HITS_SEED = 426;
  const TOGGLE_ID = 'retro90s-toggle';
  const HEADER_ID = 'retro90s-header';
  const FOOTER_ID = 'retro90s-footer';
  const BASE_STYLE_ID = 'retro90s-base-style';
  const STYLE_ID = 'retro90s-style';
  const TEXT_OFF = '📼 90s mode';
  const TEXT_ON = 'Back to 2026';
  const TITLE = '~*~ WeLcOmE tO MiLaBoWl ~*~';
  const MARQUEE =
    '⚽ Welcome to the OFFICIAL Milabowl Home Page!!! ⚽ Best viewed in Netscape Navigator 4.0 at 800x600 ⚽ Sign our guestbook!!! ⚽';
  const CLIP_ART = [
    ['📚', 'left:3%;top:8%;transform:rotate(-14deg)'],
    ['🎸', 'left:13%;top:60%;transform:rotate(12deg)'],
    ['☮️', 'left:29%;top:4%;transform:rotate(-6deg)'],
    ['🌼', 'left:44%;top:68%;transform:rotate(18deg)'],
    ['✨', 'left:61%;top:4%;transform:rotate(8deg)'],
    ['💾', 'left:73%;top:62%;transform:rotate(-12deg)'],
    ['🖥️', 'left:85%;top:8%;transform:rotate(10deg)'],
    ['⚽', 'left:92%;top:58%;transform:rotate(-8deg)'],
  ];
  const ALERTS = {
    guestbook: 'Guestbook is full!',
    prev: 'The previous site has moved to GeoCities!',
    next: 'Error 404: The next site is still loading over a 28.8k modem.',
  };
  const KONAMI = ['ArrowUp', 'ArrowUp', 'ArrowDown', 'ArrowDown', 'ArrowLeft', 'ArrowRight', 'ArrowLeft', 'ArrowRight', 'b', 'a'];
  const TRAIL_MAX = 15;
  const TRAIL_LIFE_MS = 1000;
  const TRAIL_THROTTLE_MS = 60;
  const TRAIL_CHARS = ['✨', '⭐'];
  const REDUCED_Q = '(prefers-reduced-motion: reduce)';
  const FINE_Q = '(hover: hover) and (pointer: fine)';
  const SVG_NS = 'http://www.w3.org/2000/svg';

  // Apply the saved mode first thing, so the page flips as early as a deferred script can.
  const startOn = read(KEY) === '1';
  if (startOn) document.documentElement.classList.add(CLS);

  const state = {
    on: false,
    hits: HITS_SEED,
    toggle: null,
    header: null,
    footer: null,
    counter: null,
    sparkles: [],
    lastSparkle: 0,
    trail: false,
    konami: 0,
    observer: null,
  };

  function read(k) {
    try {
      return localStorage.getItem(k);
    } catch (e) {
      return null;
    }
  }
  function write(k, v) {
    try {
      localStorage.setItem(k, v);
    } catch (e) {
      /* private mode: the mode still works for this page view */
    }
  }
  function mq(q) {
    try {
      return !!(window.matchMedia && window.matchMedia(q).matches);
    } catch (e) {
      return false;
    }
  }
  function byId(id) {
    return document.getElementById(id);
  }
  function el(tag, cls, text) {
    const n = document.createElement(tag);
    if (cls) n.setAttribute('class', cls);
    if (text) n.textContent = text;
    return n;
  }
  function detach(n) {
    if (n && n.parentNode) n.parentNode.removeChild(n);
  }

  /* ===== CSS ===== */
  // The pill: the only rules that exist while OFF. Every selector starts #retro90s-toggle.
  const BASE_CSS = `
#retro90s-toggle{position:fixed;top:8px;right:8px;z-index:2147483000;margin:0;padding:4px 10px;border:0;border-radius:999px;
  background:rgba(20,20,30,.72);color:#fff;font:12px/1.2 system-ui,-apple-system,"Segoe UI",Roboto,sans-serif;opacity:.75;cursor:pointer}
#retro90s-toggle:hover,#retro90s-toggle:focus-visible{opacity:1}
#retro90s-toggle:focus-visible{outline:2px solid #4f8cff;outline-offset:2px}
@media (min-width:768px){#retro90s-toggle{top:64px}}
`;

  const CURSOR_SVG =
    "<svg xmlns='" + SVG_NS + "' width='24' height='24' viewBox='0 0 24 24'><text x='2' y='19' font-size='18'>⭐</text></svg>";
  const CURSOR = 'url("data:image/svg+xml,' + encodeURIComponent(CURSOR_SVG) + '") 8 8, auto';

  const NEON = ['#ff00ff', '#00ff00', '#ffff00', '#00ffff', '#ff6600'];
  const BTN = 'html.retro-90s button:not(#retro90s-toggle):not(.retro90s-link)';
  const NAV_A = 'html.retro-90s body > nav a';
  const MOB_A = 'html.retro-90s #mobile-nav a';
  const neonRules = NEON.map(function (c, i) {
    const nth = ':nth-child(5n+' + (i + 1) + ')';
    return BTN + nth + ',' + NAV_A + nth + ',' + MOB_A + nth + '{background:' + c + '!important;border-color:' + c + '!important}';
  }).join('\n');
  const PAGE =
    'html.retro-90s body > div:not([id^="retro90s-"]):not(.md\\:hidden):not([data-kiln-pane]), html.retro-90s body > main';
  // This app draws its page and card titles as bold xl divs, not h1/h2: they get the heading look too.
  const TITLES = 'html.retro-90s div.font-bold.text-xl, html.retro-90s div.font-bold.md\\:text-xl';

  // Everything else. Every selector starts html.retro-90s; the tag is removed when OFF.
  const RETRO_CSS = `
html.retro-90s, html.retro-90s.dark{--color-app:#000080;--color-surface:#ffffcc;--color-surface-2:#c0c0c0;--color-content-primary:#000;
  --color-content-primary-muted:#222;--color-content-secondary:#000080;--color-primary:#ff00ff;--color-outline:#808080}
html.retro-90s body{display:flex!important;flex-direction:column!important;color:#000!important;
  font-family:"Times New Roman",Times,serif!important;cursor:${CURSOR};background-color:#000080!important;
  background-image:radial-gradient(circle at 50% 50%,#ffff00 0 7px,transparent 8px),
    repeating-radial-gradient(circle at 50% 50%,transparent 0 10px,#00ffff 10px 14px,transparent 14px 20px),
    repeating-conic-gradient(from 0deg at 50% 50%,#ff00ff 0 30deg,#00ff00 30deg 60deg,#ffff00 60deg 90deg,#00ffff 90deg 120deg)!important;
  background-size:80px 80px!important;background-position:0 0!important}
html.retro-90s a, html.retro-90s button{cursor:pointer}

/* the "page": a fixed-width table-layout box */
${PAGE}{max-width:800px!important;width:100%!important;margin:0 auto!important;box-sizing:border-box!important;
  border:8px ridge #ff00ff!important;background:#ffffcc!important;padding:8px!important;color:#000!important}
html.retro-90s body > nav{position:static!important;order:-1;background:#000080!important}

/* typography */
html.retro-90s h1, html.retro-90s h2, html.retro-90s h3, html.retro-90s h4{font-family:"Comic Sans MS","Comic Sans","Chalkboard SE",cursive!important}
html.retro-90s h1{color:#ff0000!important}
html.retro-90s h2{color:#ff00ff!important}
html.retro-90s h3{color:#0000ff!important}
html.retro-90s h4{color:#008000!important}
${TITLES}{font-family:"Comic Sans MS","Comic Sans","Chalkboard SE",cursive!important;color:#ff00ff!important}
html.retro-90s h1::after, html.retro-90s h2::after, ${TITLES.replace(/, /g, '::after, ')}::after{content:" NEW!";color:#ff0000;background:#ffff00;font-size:.5em;vertical-align:super;
  font-family:"Comic Sans MS",cursive;animation:retro90s-blink 1s steps(2) infinite}
html.retro-90s a:not(nav a):not(svg a):not(#mobile-nav a){color:#0000ee!important;text-decoration:underline!important}
html.retro-90s a:not(nav a):not(svg a):not(#mobile-nav a):visited{color:#551a8b!important}
html.retro-90s a:not(nav a):not(svg a):not(#mobile-nav a):hover{color:#ff0000!important}

/* buttons and nav links: neon bevels */
${BTN}, ${NAV_A}, ${MOB_A}{border:3px outset #ff00ff!important;border-radius:0!important;font-weight:bold!important;color:#000!important;
  text-decoration:none!important;background:#ff00ff!important}
${neonRules}
${BTN}:active, ${NAV_A}:active, ${MOB_A}:active{border-style:inset!important}
html.retro-90s #retro90s-toggle{top:8px;background:#c0c0c0;color:#000;border:2px outset #fff;border-radius:0;
  font:bold 13px "MS Sans Serif",Tahoma,sans-serif;padding:4px 10px;opacity:1}
html.retro-90s #retro90s-toggle:active{border-style:inset}

/* tables: border="1" */
html.retro-90s table{border:2px outset #c0c0c0!important;border-collapse:separate!important;border-spacing:1px}
html.retro-90s th{background:#c0c0c0!important;border:2px outset #fff!important;font-family:"Comic Sans MS",cursive!important;
  color:#000!important;text-align:center!important}
html.retro-90s td{border:1px inset #808080!important;color:#000!important;text-align:center!important}
html.retro-90s tbody tr:nth-child(odd) > td{background:#ffff66!important}
html.retro-90s tbody tr:nth-child(even) > td{background:#66ffff!important}

/* header banner */
html.retro-90s #retro90s-header{order:-2;width:100%;max-width:800px;margin:0 auto;box-sizing:border-box}
html.retro-90s .retro90s-banner{position:relative;height:160px;overflow:hidden;display:flex;align-items:center;justify-content:center}
html.retro-90s .retro90s-blob{position:absolute;top:0;left:0;width:100%;height:100%}
html.retro-90s .retro90s-blob svg{display:block;width:100%;height:100%}
html.retro-90s .retro90s-clip{position:absolute;z-index:1;font-size:28px;line-height:1}
html.retro-90s .retro90s-title{position:relative;z-index:2;padding:0 16px;text-align:center;
  font:bold 34px "Comic Sans MS","Comic Sans","Chalkboard SE",cursive;
  background:linear-gradient(90deg,red,orange,yellow,lime,cyan,blue,magenta);-webkit-background-clip:text;background-clip:text;color:transparent;
  filter:drop-shadow(2px 2px 0 #fff) drop-shadow(4px 4px 0 #808080)}
@supports not ((background-clip:text) or (-webkit-background-clip:text)){html.retro-90s .retro90s-title{color:#ff00ff;background:none}}
html.retro-90s .retro90s-marquee{overflow:hidden;white-space:nowrap;background:#000080;color:#ffff00;border:3px ridge #c0c0c0;
  padding:4px 0;font:bold 16px "Times New Roman",Times,serif}
html.retro-90s .retro90s-marquee-track{display:inline-block;animation:retro90s-marquee 18s linear infinite}
html.retro-90s .retro90s-marquee-track span{display:inline-block;padding-right:3em}

/* footer */
html.retro-90s #retro90s-footer{order:99;width:100%;max-width:800px;margin:12px auto 0;box-sizing:border-box;padding:10px;
  background:#c0c0c0;border:4px groove #fff;text-align:center;color:#000;font:15px "Times New Roman",Times,serif}
html.retro-90s #retro90s-footer > div{margin:6px 0}
html.retro-90s .retro90s-counter{font-family:"Courier New",monospace;background:#000;color:#00ff00;letter-spacing:3px;padding:2px 6px;
  border:2px inset #808080;font-weight:bold}
html.retro-90s .retro90s-construction{padding:8px 0;background:repeating-linear-gradient(45deg,#ffd500 0 20px,#000 20px 40px);
  background-size:56.57px 56.57px;animation:retro90s-stripes 1s linear infinite}
html.retro-90s .retro90s-construction span{background:#ffd500;color:#000;font-weight:bold;padding:2px 10px;border:2px solid #000}
html.retro-90s .retro90s-link{background:none!important;border:0!important;padding:0;margin:0 2px;color:#0000ee!important;
  text-decoration:underline!important;font:inherit;cursor:pointer}
html.retro-90s .retro90s-link:hover{color:#ff0000!important}
html.retro-90s .retro90s-badge{display:inline-flex;align-items:center;justify-content:center;width:88px;height:31px;box-sizing:border-box;
  margin:4px;vertical-align:middle;overflow:hidden;font:bold 10px/1.1 Arial,Helvetica,sans-serif}
html.retro-90s .retro90s-badge-notepad{background:#000080;color:#fff;outline:1px solid #fff;outline-offset:-3px}
html.retro-90s .retro90s-badge-netscape{background:linear-gradient(180deg,#40e0d0,#008080 55%,#000);color:#000;font-style:italic;
  text-shadow:0 0 2px #fff}
html.retro-90s .retro90s-badge-netscape b{font-size:18px;margin-right:4px;font-style:italic}

/* cursor trail */
html.retro-90s .retro90s-sparkle{position:fixed;z-index:2147482999;pointer-events:none;font-size:16px;line-height:1;
  animation:retro90s-sparkle 1s ease-out forwards}

@keyframes retro90s-blink{0%,49%{opacity:1}50%,100%{opacity:0}}
@keyframes retro90s-marquee{from{transform:translateX(0)}to{transform:translateX(-50%)}}
@keyframes retro90s-stripes{from{background-position:0 0}to{background-position:56.57px 0}}
@keyframes retro90s-sparkle{from{opacity:1;transform:translateY(0) scale(1)}to{opacity:0;transform:translateY(16px) scale(.4)}}

@media (prefers-reduced-motion: reduce){
  html.retro-90s h1::after, html.retro-90s h2::after, ${TITLES.replace(/, /g, '::after, ')}::after, html.retro-90s .retro90s-marquee-track,
  html.retro-90s .retro90s-construction, html.retro-90s .retro90s-sparkle{animation:none!important}
  html.retro-90s .retro90s-marquee{white-space:normal;text-align:center}
  html.retro-90s .retro90s-marquee-track span{padding:0 8px}
  html.retro-90s .retro90s-marquee-track span + span{display:none}
}
@media (max-width:820px){
  ${PAGE}{border-width:4px!important}
  html.retro-90s .retro90s-title{font-size:22px}
  html.retro-90s .retro90s-clip{font-size:20px}
  html.retro-90s #retro90s-footer{padding-bottom:5rem}
}
`;

  function mountStyle(id, css) {
    if (byId(id) || !document.head) return;
    const s = el('style');
    s.id = id;
    s.textContent = css;
    document.head.appendChild(s);
  }
  function mountBaseStyle() {
    mountStyle(BASE_STYLE_ID, BASE_CSS);
  }
  function mountRetroStyle() {
    mountStyle(STYLE_ID, RETRO_CSS);
  }
  function unmountRetroStyle() {
    detach(byId(STYLE_ID));
  }

  /* ===== TOGGLE ===== */
  function buildToggle() {
    const b = el('button');
    b.id = TOGGLE_ID;
    b.setAttribute('type', 'button');
    b.setAttribute('aria-label', 'Toggle 90s mode');
    b.setAttribute('aria-pressed', 'false');
    b.textContent = TEXT_OFF;
    b.addEventListener('click', toggle);
    return b;
  }

  function updateToggle() {
    if (!state.toggle) return;
    state.toggle.textContent = state.on ? TEXT_ON : TEXT_OFF;
    state.toggle.setAttribute('aria-pressed', state.on ? 'true' : 'false');
  }

  // The one mode switch: the click, the Konami code and the load all come through here.
  function setMode(on, opts) {
    const load = !!(opts && opts.load);
    state.on = !!on;
    const root = document.documentElement;
    if (state.on) root.classList.add(CLS);
    else root.classList.remove(CLS);
    if (!load) write(KEY, state.on ? '1' : '0');
    if (state.on) {
      mountRetroStyle();
      bumpCounter();
      ensureMounted();
      attachTrail();
    } else {
      unmountRetroStyle();
      detach(state.header);
      detach(state.footer);
      detachTrail();
      while (state.sparkles.length) dropSparkle(state.sparkles[0]);
    }
    updateToggle();
  }

  function toggle() {
    setMode(!state.on);
  }

  // Re-adds whatever of ours went missing (hydration, client-side navigation). Appends only, never inserts.
  function ensureMounted() {
    const body = document.body;
    if (!body) return;
    mountBaseStyle();
    if (state.toggle && !byId(TOGGLE_ID)) body.appendChild(state.toggle);
    if (!state.on) return;
    mountRetroStyle();
    if (!byId(HEADER_ID)) body.appendChild(state.header || (state.header = buildHeader()));
    if (!byId(FOOTER_ID)) body.appendChild(state.footer || (state.footer = buildFooter()));
  }

  /* ===== DECORATIONS ===== */
  function buildHeader() {
    const h = el('div');
    h.id = HEADER_ID;
    h.setAttribute('aria-hidden', 'true');
    const banner = el('div', 'retro90s-banner');
    const blob = el('div', 'retro90s-blob');
    blob.innerHTML =
      '<svg xmlns="' + SVG_NS + '" viewBox="0 0 800 160" preserveAspectRatio="none" focusable="false">' +
      '<path fill="#000" d="M40 80 C20 30 120 8 180 34 C240 2 320 30 380 14 C450 0 520 36 580 12 C650 -4 760 20 772 60 ' +
      'C800 92 782 140 720 140 C660 158 600 128 540 148 C470 164 410 130 350 150 C280 164 220 130 160 148 C90 164 10 140 40 80 Z"/></svg>';
    banner.appendChild(blob);
    CLIP_ART.forEach(function (c) {
      const s = el('span', 'retro90s-clip', c[0]);
      s.style.cssText = c[1];
      banner.appendChild(s);
    });
    banner.appendChild(el('div', 'retro90s-title', TITLE));
    h.appendChild(banner);
    const marquee = el('div', 'retro90s-marquee');
    const track = el('span', 'retro90s-marquee-track');
    track.appendChild(el('span', '', MARQUEE));
    track.appendChild(el('span', '', MARQUEE));
    marquee.appendChild(track);
    h.appendChild(marquee);
    return h;
  }

  function linkButton(text, message) {
    const b = el('button', 'retro90s-link', text);
    b.setAttribute('type', 'button');
    b.addEventListener('click', function () {
      sayAlert(message);
    });
    return b;
  }

  function buildFooter() {
    const f = el('div');
    f.id = FOOTER_ID;

    const visits = el('div');
    visits.appendChild(document.createTextNode('You are visitor no. '));
    state.counter = el('span', 'retro90s-counter', pad(state.hits));
    visits.appendChild(state.counter);
    f.appendChild(visits);

    const works = el('div', 'retro90s-construction');
    works.appendChild(el('span', '', '🚧 UNDER CONSTRUCTION 🚧'));
    f.appendChild(works);

    f.appendChild(el('div', '', 'Last updated: 13.03.1998 · © 1998 Milabowl Webmaster'));

    const guest = el('div');
    guest.appendChild(linkButton('Sign my Guestbook', ALERTS.guestbook));
    f.appendChild(guest);

    const ring = el('div');
    ring.appendChild(linkButton('<< Prev', ALERTS.prev));
    ring.appendChild(document.createTextNode(' | FPL Webring | '));
    ring.appendChild(linkButton('Next >>', ALERTS.next));
    f.appendChild(ring);

    const badges = el('div');
    badges.appendChild(el('span', 'retro90s-badge retro90s-badge-notepad', 'Made with Notepad'));
    const ns = el('span', 'retro90s-badge retro90s-badge-netscape');
    ns.appendChild(el('b', '', 'N'));
    ns.appendChild(document.createTextNode('Netscape NOW!'));
    badges.appendChild(ns);
    f.appendChild(badges);
    return f;
  }

  function pad(n) {
    return String(n).padStart(6, '0');
  }

  // +1 on every load while ON and on every switch to ON.
  function bumpCounter() {
    const n = parseInt(read(HITS_KEY), 10);
    state.hits = (isNaN(n) ? state.hits : n) + 1;
    write(HITS_KEY, String(state.hits));
    if (state.counter) state.counter.textContent = pad(state.hits);
  }

  function sayAlert(text) {
    try {
      window.alert(text);
    } catch (e) {
      /* a sandboxed frame may block alerts */
    }
  }

  /* ===== EASTER EGGS ===== */
  // Konami code: up up down down left right left right B A. Ignored while typing.
  function onKey(e) {
    const t = e.target;
    if (t && (t.isContentEditable || /^(INPUT|TEXTAREA|SELECT)$/.test(t.tagName || ''))) {
      state.konami = 0;
      return;
    }
    const k = e.key && e.key.length === 1 ? e.key.toLowerCase() : e.key;
    if (k === KONAMI[state.konami]) state.konami++;
    else if (k === KONAMI[0]) state.konami = state.konami === 2 ? 2 : 1;
    else state.konami = 0;
    if (state.konami === KONAMI.length) {
      state.konami = 0;
      toggle();
    }
  }

  // Sparkle trail: only with a fine pointer and no reduced motion; one sparkle per 60 ms, at most 15, each gone after 1 s.
  function attachTrail() {
    if (state.trail || !mq(FINE_Q) || mq(REDUCED_Q)) return;
    document.addEventListener('mousemove', onMove);
    state.trail = true;
  }
  function detachTrail() {
    if (!state.trail) return;
    document.removeEventListener('mousemove', onMove);
    state.trail = false;
  }
  function onMove(e) {
    const now = Date.now();
    if (now - state.lastSparkle < TRAIL_THROTTLE_MS || !document.body) return;
    state.lastSparkle = now;
    while (state.sparkles.length >= TRAIL_MAX) dropSparkle(state.sparkles[0]);
    const s = el('span', 'retro90s-sparkle', TRAIL_CHARS[now % 2]);
    s.setAttribute('aria-hidden', 'true');
    s.style.left = e.clientX + 6 + 'px';
    s.style.top = e.clientY + 6 + 'px';
    document.body.appendChild(s);
    state.sparkles.push(s);
    window.setTimeout(function () {
      dropSparkle(s);
    }, TRAIL_LIFE_MS);
  }
  function dropSparkle(s) {
    const i = state.sparkles.indexOf(s);
    if (i >= 0) state.sparkles.splice(i, 1);
    detach(s);
  }
  // The custom star cursor is CSS only (see CURSOR above).

  /* boot */
  function observeBody() {
    if (!window.MutationObserver || !document.body) return;
    if (state.observer) state.observer.disconnect();
    state.observer = new window.MutationObserver(ensureMounted);
    state.observer.observe(document.body, { childList: true });
  }
  // Astro view transitions swap <body>, reset <html> attributes and drop our <style> tags: put everything back
  // on the new page (not counted as a visit). The event never fires in other sites.
  function afterSwap() {
    if (state.on) document.documentElement.classList.add(CLS);
    ensureMounted();
    observeBody();
  }
  function init() {
    mountBaseStyle();
    state.toggle = buildToggle();
    setMode(startOn, { load: true });
    ensureMounted();
    document.addEventListener('keydown', onKey);
    document.addEventListener('astro:after-swap', afterSwap);
    observeBody();
  }

  if (document.body) init();
  else document.addEventListener('DOMContentLoaded', init);
})();
