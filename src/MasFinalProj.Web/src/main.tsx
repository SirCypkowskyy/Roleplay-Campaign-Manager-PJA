import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import Layout from "@/layouts/layout.tsx";
import MainPage from "@/pages/main-page.tsx";
import LoginPage from "@/pages/login-page.tsx";
import AboutPage from "@/pages/about-page.tsx";
import {ThemeProvider} from "@/providers/theme-provider.tsx";
import DashboardPage from "@/pages/dashboard-page.tsx";
import {AuthProvider} from "@/providers/auth-provider.tsx";
import RegisterPage from "@/pages/register-page.tsx";

const router = createBrowserRouter([
    {
        path: '/',
        element: <Layout/>,
        children: [
            {path: '/', element: <MainPage/>},
            {path: '/about', element: <AboutPage/>},
            {path: '/login', element: <LoginPage/>},
            {path: '/register', element: <RegisterPage/>},
            {path: '/dashboard', element: <DashboardPage/>},
        ],
    }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <AuthProvider>
            <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
                <RouterProvider router={router}/>
            </ThemeProvider>
        </AuthProvider>
    </React.StrictMode>,
)
