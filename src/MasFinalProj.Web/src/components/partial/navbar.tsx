import {ReactElement} from "react";
import {Link} from "react-router-dom";
import {ModeToggle} from "@/components/mode-toggle.tsx";

export default function Navbar(): ReactElement {
    return (
        <nav className={"mx-4 sticky top-0 z-10 backdrop-filter backdrop-blur-lg bg-opacity-30 border-b border-primary-200"}>
            <div className="flex items-center justify-between h-16">
                <span className="text-2xl font-semibold">Roleplay Master</span>
                <div className="flex space-x-4 mt-4">
                    <Link to={"/"}>Home</Link>
                    <Link to={"/about"}>About</Link>
                    <Link to={"/login"}>Login</Link>
                    <Link to={"/register"}>Register</Link>
                    <ModeToggle/>
                </div>
            </div>
        </nav>
    )
}