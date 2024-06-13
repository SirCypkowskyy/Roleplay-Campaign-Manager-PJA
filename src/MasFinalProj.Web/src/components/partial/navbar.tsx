import {ReactElement, useEffect} from "react";
import {Link, useNavigate} from "react-router-dom";
import {ModeToggle} from "@/components/mode-toggle.tsx";
import {Button} from "@/components/ui/button.tsx";
import {CircleUser, Menu, Package2} from "lucide-react";
import {Sheet, SheetContent, SheetTrigger} from "@/components/ui/sheet.tsx";
import {useAuthStore} from "@/store/auth-store.ts";

export default function Navbar(): ReactElement {
    // const auth = useAuth();
    const auth = useAuthStore();
    const navigate = useNavigate();

    useEffect(() => {
    }, []);
    
    return (
        // <nav className={"mx-4 sticky top-0 z-10 backdrop-filter backdrop-blur-lg bg-opacity-30 border-b border-primary-200"}>
        //     <div className="flex items-center justify-between h-16">
        //         <span className="text-2xl font-semibold">Roleplay Master</span>
        //         <div className="flex space-x-4 mt-4">
        //             <Link to={"/"}>Home</Link>
        //             <Link to={"/about"}>About</Link>
        //             <Link to={"/login"}>Login</Link>
        //             <Link to={"/register"}>Register</Link>
        //             <ModeToggle/>
        //         </div>
        //     </div>
        // </nav>
        <header className="sticky top-0 flex h-16 items-center gap-4 backdrop-filter backdrop-blur-lg bg-opacity-30 border-b px-4 md:px-6">
            <nav
                className="hidden flex-col gap-6 text-lg font-medium md:flex md:flex-row md:items-center md:gap-5 md:text-sm lg:gap-6">
                <Link
                    to="#"
                    className="flex items-center gap-2 text-lg font-semibold md:text-base"
                >
                    <Package2 className="h-6 w-6"/>
                    <span className="sr-only">
                        MAS Roleplay Master
                    </span>
                </Link>
            </nav>
            <Sheet>
                <SheetTrigger asChild>
                    <Button
                        variant="outline"
                        size="icon"
                        className="shrink-0 md:hidden"
                    >
                        <Menu className="h-5 w-5"/>
                        <span className="sr-only">Toggle navigation menu</span>
                    </Button>
                </SheetTrigger>
                <SheetContent side="left">
                    <nav className="grid gap-6 text-lg font-medium">
                        <Link
                            to="#"
                            className="flex items-center gap-2 text-lg font-semibold"
                        >
                            <Package2 className="h-6 w-6"/>
                            <span className="sr-only">MAS Roleplay App</span>
                        </Link>
                        <Link to="#" className="hover:text-foreground">
                            Dashboard
                        </Link>
                        <Link
                            to="#"
                            className="text-muted-foreground hover:text-foreground"
                        >
                            Orders
                        </Link>
                        <Link
                            to="#"
                            className="text-muted-foreground hover:text-foreground"
                        >
                            Products
                        </Link>
                        <Link
                            to="#"
                            className="text-muted-foreground hover:text-foreground"
                        >
                            Customers
                        </Link>
                        <Link
                            to="#"
                            className="text-muted-foreground hover:text-foreground"
                        >
                            Analytics
                        </Link>
                    </nav>
                </SheetContent>
            </Sheet>
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
                        <Link to={"/"}>
                                Home
                        </Link>
                        <Link to={"/about"}>
                                About
                        </Link>
                        <Link to={"/login"}>
                                Login
                        </Link>
                        <Link to={"/register"}>
                                Register
                        </Link>
                    </>
                ) : (
                    <>

                        <Link to={"/dashboard"}>
                                Dashboard
                        </Link>
                        <a onClick={() => {
                            auth.logout()
                            navigate('/')
                        }}>
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