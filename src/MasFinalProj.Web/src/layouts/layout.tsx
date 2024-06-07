import {ReactElement} from "react";
import {Outlet} from "react-router-dom";
import Navbar from "@/components/partial/navbar.tsx";
import Footer from "@/components/partial/footer.tsx";

export default function Layout(): ReactElement {
    return (
        <div className={"h-screen flex flex-col"}>
            <Navbar/>
            <div className={"flex-grow"}>
                <Outlet />
            </div>
            <Footer/>
        </div>
    )
}