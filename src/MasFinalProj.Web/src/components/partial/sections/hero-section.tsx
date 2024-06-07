import {ReactElement} from "react";
import {cn} from "@/lib/utils.ts";
import {Button} from "@/components/ui/button.tsx";
import {Link} from "react-router-dom";

type HeroSectionProps = {
    className?: string;
}

export function HeroSection(props: HeroSectionProps): ReactElement {
    return (
        <div className={cn(props.className)}>
            {/* Hero center vert and horizontally */}
            <div className="flex items-center justify-center min-h-[85vh] bg-hero-pattern bg-cover bg-center">
                <div className="relative z-10">
                    <div className="container py-10 lg:py-16">
                        <div className="max-w-2xl text-center mx-auto">
                            <p className="">Explore the world of roleplaying games</p>
                            {/* Title */}
                            <div className="mt-5 max-w-2xl">
                                <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl">
                                    MAS Project Final
                                </h1>
                            </div>
                            {/* End Title */}
                            <div className="mt-5 max-w-3xl">
                                <p className="text-xl text-muted-foreground">
                                    Create and manage your roleplaying adventures with ease. Add your players, write your
                                    stories, and let the adventure begin.
                                </p>
                            </div>
                            {/* Buttons */}
                            <div className="mt-8 gap-3 flex justify-center">
                                <Link to="/register">
                                    <Button size={"lg"}>
                                        Get started
                                    </Button>
                                </Link>
                                <Link to="/login">
                                    <Button size={"lg"} variant={"outline"}>
                                        Login
                                    </Button>
                                </Link>
                                <Link to="/about">
                                    <Button size={"lg"} variant={"outline"}>
                                        Learn more
                                    </Button>
                                </Link>
                            </div>
                            {/* End Buttons */}
                        </div>
                    </div>
                </div>
            </div>
            {/* End Hero */}
        </div>
    );

}