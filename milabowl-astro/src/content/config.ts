import { defineCollection, z } from 'astro:content'

const blogCollection = defineCollection({
    schema: z.object({
        title: z.string().max(60),
        date: z.date(),
        author: z.enum([
            'MilaCorp',
            'Sam',
            'Anders',
            'Simen',
            'Henrik',
            'Mikkel',
            'Malte',
            'Eivind',
            'Markus',
            'Martin',
        ]),
        tags: z
            .enum(['GW Summary', 'Announcement', 'Tech', 'Awards'])
            .array()
            .optional()
            .default([]),
    }),
})

const rulesCollection = defineCollection({
    schema: z.object({
        title: z.string().max(60),
        date: z.date(),
    }),
})

const memesCollection = defineCollection({
    schema: z.object({
        title: z.string().max(60),
        date: z.date(),
    }),
})

export const collections = {
    blog: blogCollection,
    rules: rulesCollection,
    memes: memesCollection,
}
