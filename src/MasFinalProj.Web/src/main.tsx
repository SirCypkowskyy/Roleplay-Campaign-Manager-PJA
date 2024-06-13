import ReactDOM from 'react-dom/client'
import './index.css'
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import Layout from "@/layouts/layout.tsx";
import MainPage from "@/pages/main-page.tsx";
import LoginPage from "@/pages/login-page.tsx";
import AboutPage from "@/pages/about-page.tsx";
import {ThemeProvider} from "@/providers/theme-provider.tsx";
import DashboardPage from "@/pages/dashboard-page.tsx";
import RegisterPage from "@/pages/register-page.tsx";
import ChatPage from "@/pages/chat-page.tsx";
import ProtectedRoute from "@/redirects/protected-route.tsx";
import CampaignPage from "@/pages/campaign-page.tsx";

const router = createBrowserRouter([
    {
        path: '/',
        element: <Layout/>,
        children: [
            {path: '/', element: <MainPage/>},
            {path: '/about', element: <AboutPage/>},
            {path: '/login', element: <LoginPage/>},
            {path: '/register', element: <RegisterPage/>},
            {
                element: <ProtectedRoute redirectTo="/login"/>,
                children: [
                    {path: '/dashboard', element: <DashboardPage/>},
                    {path: '/profile', element: <div>Profile</div>},
                    {path: '/campaign/chat', element: <ChatPage />},
                ]
            },

        ],
    }
]);

ReactDOM.createRoot(document.getElementById('root')!).render(
    // <React.StrictMode>
            <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
                <RouterProvider router={router}/>
            </ThemeProvider>
    // </React.StrictMode>
)
