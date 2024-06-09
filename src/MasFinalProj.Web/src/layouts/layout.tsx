import {ReactElement} from "react";
import {Outlet} from "react-router-dom";
import Navbar from "@/components/partial/navbar.tsx";
import Footer from "@/components/partial/footer.tsx";
import {Toaster} from "@/components/ui/toaster.tsx";

export default function Layout(): ReactElement {
    return (
        <div className={"h-screen flex flex-col"}>
            <Navbar/>
            <main className={"flex-grow"}>
                <Outlet />
            </main>
            <Footer/>
            <Toaster />
        </div>
    )
}