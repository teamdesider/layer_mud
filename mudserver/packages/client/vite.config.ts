import { defineConfig } from "vite";

export default defineConfig({
  server: {
    port: 3099,
    fs: {
      strict: false,
    },
  },
  build: {
    target: "es2022",
  },
});
