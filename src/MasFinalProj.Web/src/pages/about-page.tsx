import {ReactElement} from "react";
import {Building2Icon, ThumbsUpIcon, Users2Icon} from "lucide-react";

export default function AboutPage(): ReactElement {
    return (
        <>
            {/* Icon Blocks */}
            <div className="container py-24 lg:py-32 space-y-12 lg:space-y-20">
                <div className="max-w-2xl mx-auto">
                    <h1 className="text-4xl font-bold lg:text-5xl">About Roleplay Master</h1>
                    <p className="mt-3 text-muted-foreground mb-8">
                        Roleplay Master is a platform for roleplayers to connect, collaborate, and create amazing roleplay experiences together.
                        Our platform is designed to make it easier for roleplayers to find each other, share their stories, and collaborate on new projects.
                        Whether you are a seasoned roleplayer or just starting out, Roleplay Master has something for everyone.
                    </p>
                    {/* Out vision grid */}
                    <div className="grid gap-12">
                        <div>
                            <h2 className="text-3xl font-bold lg:text-4xl">Our vision</h2>
                            <p className="mt-3 text-muted-foreground">
                                The reasons why we created Roleplay Master are simple. We wanted to create a platform that would make it easier for roleplayers to connect, collaborate, and create amazing roleplay experiences together.
                            </p>
                        </div>
                        <div className="space-y-6 lg:space-y-10">
                            {/* Icon Block */}
                            <div className="flex">
                                <Building2Icon className="flex-shrink-0 mt-2 h-6 w-6" />
                                <div className="ms-5 sm:ms-8">
                                    <h3 className="text-base sm:text-lg font-semibold">
                                        Stability
                                    </h3>
                                    <p className="mt-1 text-muted-foreground">
                                        We provide a stable and reliable platform for all your roleplay needs.
                                        With our platform, you can focus on your roleplay, not on technical issues.
                                        We take care of the technical side of things so you can focus on what you do best.
                                    </p>
                                </div>
                            </div>
                            {/* End Icon Block */}
                            {/* Icon Block */}
                            <div className="flex">
                                <Users2Icon className="flex-shrink-0 mt-2 h-6 w-6" />
                                <div className="ms-5 sm:ms-8">
                                    <h3 className="text-base sm:text-lg font-semibold">
                                        Community
                                    </h3>
                                    <p className="mt-1 text-muted-foreground">
                                        We believe in the power of community. Our platform is built around the idea of bringing people together to create amazing roleplay experiences.
                                        We provide a space where you can connect with other roleplayers, share your stories, and collaborate on new projects.
                                    </p>
                                </div>
                            </div>
                            {/* End Icon Block */}
                            {/* Icon Block */}
                            <div className="flex">
                                <ThumbsUpIcon className="flex-shrink-0 mt-2 h-6 w-6" />
                                <div className="ms-5 sm:ms-8">
                                    <h3 className="text-base sm:text-lg font-semibold">
                                        Quality
                                    </h3>
                                    <p className="mt-1 text-muted-foreground">
                                        We are committed to providing the highest quality service to our users. We are constantly working to improve our platform and add new features to make your roleplay experience even better.
                                        We listen to your feedback and take your suggestions seriously. Your satisfaction is our top priority.
                                    </p>
                                </div>
                            </div>
                            {/* End Icon Block */}
                        </div>
                    </div>
                    {/* End our vision grid */}
                </div>
            </div>
            {/* End Icon Blocks */}
        </>
    );
}