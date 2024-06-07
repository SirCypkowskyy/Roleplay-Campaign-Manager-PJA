import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import Layout from "@/layouts/layout.tsx";
import MainPage from "@/pages/main-page.tsx";
import LoginPage from "@/pages/login-page.tsx";
import {ThemeProvider} from "@/components/theme-provider.tsx";
import AboutPage from "@/pages/about-page.tsx";
import {QueryClient, QueryClientProvider} from "@tanstack/react-query";

const router = createBrowserRouter([
    {
        path: '/',
        element: <Layout/>,
        children: [
            {path: '/', element: <MainPage/>},
            {path: '/about', element: <AboutPage/>},
            {path: '/login', element: <LoginPage/>},
            {path: '/register', element: <div>Register</div>},
        ],
    }
]);

const queryClient = new QueryClient();

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <QueryClientProvider client={queryClient}>
            <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
                <RouterProvider router={router}/>
            </ThemeProvider>
        </QueryClientProvider>
    </React.StrictMode>,
)
