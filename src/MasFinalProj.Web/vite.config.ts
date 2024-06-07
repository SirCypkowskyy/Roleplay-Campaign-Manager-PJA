import path from "path"
import react from "@vitejs/plugin-react"
import {defineConfig, ProxyOptions} from "vite"

const possibleBackendUrls: string[] = process.env.ASPNETCORE_URLS
    ? process.env.ASPNETCORE_URLS.split(";")
    : ["http://localhost:5128"];

const aspNetCore_environment: string =
    process.env.ASPNETCORE_ENVIRONMENT || "Development";

const backendUrl: string =
    possibleBackendUrls.find((url) => url.startsWith("https")) ||
    possibleBackendUrls[0];

console.log(`Backend URL: ${backendUrl}`);
const runAsHttps: boolean = backendUrl.startsWith("https");
const backendPort: number = parseInt(backendUrl.split(":")[2] || "5128");

let serverProxies: Record<string, ProxyOptions> = {
    "/api": {
        target: backendUrl,
    },
    "/swagger": {
        target: backendUrl,
    },
};
  

export default defineConfig({
    plugins: [react(),],
    resolve: {
        alias: {
            "@": path.resolve(__dirname, "./src"),
        },
    },
    server: {
        proxy: serverProxies,
      port: backendPort,
    }
})
