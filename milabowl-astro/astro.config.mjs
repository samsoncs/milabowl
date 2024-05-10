import { defineConfig } from 'astro/config';

import react from '@astrojs/react';

// https://astro.build/config
import tailwind from '@astrojs/tailwind';

// https://astro.build/config
export default defineConfig({
    integrations: [react(), tailwind()],
    devToolbar: { enabled: false },
    vite: {
        ssr: {
            // Needed because of ESM wonky support in Nivo
            noExternal: ["@nivo/*"],
        },
    }
});
