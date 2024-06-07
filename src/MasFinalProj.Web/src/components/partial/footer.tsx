import {ReactElement} from "react";
import {siteConfig} from "@/config/site.ts";

export default function Footer(): ReactElement {
    const currentYear = new Date().getFullYear();
    return (
        <footer className="sticky py-3 bottom-0 z-10 backdrop-filter backdrop-blur-lg bg-opacity-30 border-t border-primary-200">
            <div className="container items-center justify-between gap-4">
                <p className="text-center text-sm leading-loose text-muted-foreground">
                    Built by <a href={siteConfig.authorWebsite} className="text-primary-500 hover:underline">{siteConfig.author}</a> &copy; {currentYear} All rights reserved.
                </p>
            </div>
        </footer>
    )
}