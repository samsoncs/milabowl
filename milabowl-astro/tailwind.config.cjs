const defaultTheme = require('tailwindcss/defaultTheme');
const colors = require('tailwindcss/colors');

/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ['./src/**/*.{astro,html,js,jsx,md,mdx,svelte,ts,tsx,vue}'],
    darkMode: 'class',
    theme: {
        extend: {
            boxShadow: {
                sm: 'rgb(149 157 165 / 15%) 0px 8px 24px',
                'sm-dark': 'rgb(0 0 0 / 5%) 0px 8px 24px',
            },
            fontFamily: {
                sans: ['Inter', ...defaultTheme.fontFamily.sans],
            },
            colors: {
                'dark-text': colors.violet[100],
            },
        },
    },
    plugins: [require('@tailwindcss/typography'), require('flowbite/plugin')],
};
