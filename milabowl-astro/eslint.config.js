// @ts-check

import eslintPluginAstro from 'eslint-plugin-astro';
import eslint from '@eslint/js';
import tseslint from 'typescript-eslint';
import react from 'eslint-plugin-react';
import prettier from 'eslint-config-prettier';

export default [
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
  },
  eslint.configs.recommended,
  ...tseslint.configs.recommended,
  react.configs.flat.recommended,
  ...eslintPluginAstro.configs.recommended,
  prettier,
  {
    settings: {
      react: {
        version: 'detect',
      },
    },
  },
  {
    files: ['**/*.astro', '**/*.tsx'],
    rules: {
      'react/react-in-jsx-scope': 'off',
    },
  },
  {
    files: ['**/*.astro'],
    rules: {
      'react/no-unknown-property': 'off',
      'react/jsx-key': 'off',
    },
  },
  {
    files: ['**/*.tsx'],
    rules: {
      'react/prop-types': 'off',
    },
  },
];
