import {HeroSection} from "@/components/partial/sections/hero-section.tsx";
import {ReactElement} from "react";

export default function MainPage(): ReactElement {
    return (
        <div className={"container mx-auto"}>
            <HeroSection/>
        </div>
    );
}