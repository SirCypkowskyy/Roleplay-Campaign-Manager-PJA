import {HeroSection} from "@/components/partial/sections/hero-section.tsx";
import {ReactElement, useEffect} from "react";
import {useAuthStore} from "@/store/auth-store.ts";
import { useNavigate } from "react-router-dom";

export default function MainPage(): ReactElement {
    const auth = useAuthStore();
    const navigate = useNavigate();
    
    useEffect(() => {
        if (auth.isLoggedIn) 
            navigate("/dashboard");
    }, []);
    
    return (
        <div className={"container mx-auto"}>
            <HeroSection/>
        </div>
    );
}