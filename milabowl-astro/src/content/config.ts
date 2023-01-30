import { z, defineCollection }  from "astro:content";

const blogCollection = defineCollection({ 
  schema: z.object({
    title: z.string().max(60),
    date: z.date(),
    author: z.enum(["MilaCorp", "Sam"]),
    tags: z.enum(["GW Summary", "Announcement", "Tech"]).array().optional().default([])
  })
});

const rulesCollection = defineCollection({ 
  schema: z.object({
    title: z.string().max(60),
    date: z.date(),
  })
});

export const collections = {
  'blog': blogCollection,
  'rules': rulesCollection
};