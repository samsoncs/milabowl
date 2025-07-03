// @ts-check

import eslintPluginAstro from 'eslint-plugin-astro';
import eslint from '@eslint/js';
import tseslint from 'typescript-eslint';

export default tseslint.config(
    eslint.configs.recommended,
    tseslint.configs.recommended,
    ...eslintPluginAstro.configs.recommended,
    {
        ignores: [
            '**/node_modules/**',
            '**/dist/**',
            '**/build/**',
            '**/coverage/**',
            'public/**',
            '.astro/**',
            'astro.config.mjs',
            'tailwind.config.cjs',
            '.prettierrc.mjs',
            '.eslintrc.js',
            'src/env.d.ts',
        ],
    }
);
