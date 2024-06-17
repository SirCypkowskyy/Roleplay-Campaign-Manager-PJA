import {ReactElement, useEffect} from "react";
import {Link, useNavigate} from "react-router-dom";
import {ModeToggle} from "@/components/mode-toggle.tsx";
import {CircleUser, Package2} from "lucide-react";
import {useAuthStore} from "@/store/auth-store.ts";

export default function Navbar(): ReactElement {
    // const auth = useAuth();
    const auth = useAuthStore();
    const navigate = useNavigate();

    useEffect(() => {
    }, []);
    
    return (
        <header className="sticky top-0 flex h-16 items-center gap-4 backdrop-filter backdrop-blur-lg bg-opacity-30 border-b px-4 md:px-6">
            <nav
                className="hidden flex-col gap-6 text-lg font-medium md:flex md:flex-row md:items-center md:gap-5 md:text-sm lg:gap-6">
                <Link
                    to="/"
                    className="flex items-center gap-2 text-lg font-semibold md:text-base"
                >
                    <Package2 className="h-6 w-6"/>
                    <span className="sr-only">
                        MAS Roleplay Master
                    </span>
                </Link>
            </nav>
            <div className="flex w-full items-center gap-4 md:ml-auto md:gap-2 lg:gap-4">
                <form className="ml-auto flex-1 sm:flex-initial">
                    <div className="relative">
                        {/*<Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground"/>*/}
                        {/*<Input*/}
                        {/*    type="search"*/}
                        {/*    placeholder="Search products..."*/}
                        {/*    className="pl-8 sm:w-[300px] md:w-[200px] lg:w-[300px]"*/}
                        {/*/>*/}
                    </div>
                </form>
                {!auth.isLoggedIn ? (
                    <>
                        <Link to={"/"} className="font-semibold link-underline">
                                Home
                        </Link>
                        <Link to={"/about"} className="font-semibold link-underline">
                                About
                        </Link>
                        <Link to={"/login"} className="font-semibold link-underline">
                                Login
                        </Link>
                        <Link to={"/register"} className="font-semibold link-underline">
                                Register
                        </Link>
                    </>
                ) : (
                    <>

                        <Link to={"/dashboard"} className="font-semibold link-underline">
                                Dashboard
                        </Link>
                        <a onClick={() => {
                            auth.logout()
                            navigate('/')
                        }} className="font-semibold link-underline cursor-pointer">
                            Logout
                        </a>
                    </>
                )
                }
                <ModeToggle className={"mt-2"}/>
                {
                    auth.isLoggedIn && (
                        <div className="flex items-center gap-2">
                            <CircleUser className="h-5 w-5"/>
                            <span className="text-sm font-medium">
                                {auth.userData?.username}
                            </span>
                        </div>
                    )
                }
                {/*<DropdownMenu>*/}
                {/*    <DropdownMenuTrigger asChild>*/}
                {/*        <Button variant="secondary" size="icon" className="rounded-full">*/}
                {/*            <CircleUser className="h-5 w-5"/>*/}
                {/*            <span className="sr-only">Toggle user menu</span>*/}
                {/*        </Button>*/}
                {/*    </DropdownMenuTrigger>*/}
                {/*    <DropdownMenuContent align="end">*/}
                {/*        <DropdownMenuLabel>My Account</DropdownMenuLabel>*/}
                {/*        <DropdownMenuSeparator/>*/}
                {/*        <DropdownMenuItem>Settings</DropdownMenuItem>*/}
                {/*        <DropdownMenuItem>Support</DropdownMenuItem>*/}
                {/*        <DropdownMenuSeparator/>*/}
                {/*        <DropdownMenuItem>Logout</DropdownMenuItem>*/}
                {/*    </DropdownMenuContent>*/}
                {/*</DropdownMenu>*/}
            </div>
        </header>
    )
}