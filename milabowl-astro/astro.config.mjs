import { defineConfig } from 'astro/config';

import react from '@astrojs/react';
import mdx from '@astrojs/mdx';

import tailwindcss from '@tailwindcss/vite';

// https://astro.build/config
export default defineConfig({
  integrations: [react(), mdx()],
  devToolbar: { enabled: false },
  vite: {
    ssr: {
      // Needed because of ESM wonky support in Nivo
      noExternal: ['@nivo/*'],
    },

    plugins: [tailwindcss()],
  },
});
