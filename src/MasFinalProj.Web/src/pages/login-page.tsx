import {Button} from "@/components/ui/button";
import {Input} from "@/components/ui/input";
import {Label} from "@/components/ui/label";
import React, {ReactElement, useEffect, useState} from "react";
import {Link, useNavigate} from "react-router-dom";
import {useDebounce} from "@uidotdev/usehooks";
import {SiDiscord} from "@icons-pack/react-simple-icons";
import {useToast} from "@/components/ui/use-toast.ts";
import {cn} from "@/lib/utils.ts";
import {useAuth} from "@/providers/auth-provider.tsx";
import {useAuthStore} from "@/store/auth-store.ts";

export default function LoginPage(): ReactElement {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [passwordValidationErrorMessage, setPasswordValidationErrorMessage] = useState("");
    const [emailValidationErrorMessage, setEmailValidationErrorMessage] = useState("");
    const [canLogin, setCanLogin] = useState(false);

    const debouncedEmail = useDebounce(email, 500);
    const debouncedPassword = useDebounce(password, 500);

    const [loginLoading, setLoginLoading] = useState(false);
    const [successLogin, setSuccessLogin] = useState(false);
    const [errorLogin, setErrorLogin] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const navigate = useNavigate();
    const debouncedSuccessLogin = useDebounce(successLogin, 500);
    const {toast} = useToast();
    // const auth = useAuth()
    
    const [viteBackendUrl, setViteBackendUrl] = useState('');
    
    const auth = useAuthStore();
    
    useEffect(() => {
        setViteBackendUrl(__BACKEND_URL__);
    }, []);

    useEffect(() => {
        if (debouncedEmail)
            validateEmail(debouncedEmail);

        emailValidationErrorMessage && setEmailValidationErrorMessage("");
    }, [debouncedEmail]);

    useEffect(() => {
        if (debouncedPassword)
            validatePassword(debouncedPassword);

        passwordValidationErrorMessage && setPasswordValidationErrorMessage("");

    }, [debouncedPassword]);

    useEffect(() => {
        if (email && password && !emailValidationErrorMessage && !passwordValidationErrorMessage)
            setCanLogin(true);
        else
            setCanLogin(false);
    }, [email, password, emailValidationErrorMessage, passwordValidationErrorMessage]);

    useEffect(() => {
        if (debouncedSuccessLogin && successLogin)
            navigate("/dashboard");
    }, [successLogin]);

    const validateEmail = (email: string) => {
        if (!email.includes("@"))
            setEmailValidationErrorMessage("Invalid email");
    }

    const validatePassword = (password: string) => {
        if (password.length < 6)
            setPasswordValidationErrorMessage("Password must be at least 6 characters long");
    }

    const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setEmail(e.target.value);
    }
    const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPassword(e.target.value);
    }

    /**
     * Rozpatruje próbę zalogowania użytkownika
     */
    const handleSubmit = async () => {
        if (!canLogin) return;
        setSuccessLogin(false);
        setErrorLogin(false);
        setErrorMessage("");
        setLoginLoading(true);

        try {
            // const response = await fetch("api/v1/user/auth", {
            //     method: "POST",
            //     headers: {
            //         "Content-Type": "application/json",
            //     },
            //     body: JSON.stringify({email, password})
            // });
            //
            // if (!response.ok) {
            //     if (response.status >= 400 && response.status < 500)
            //         throw new Error("Invalid email or password");
            //     else
            //         throw new Error("Server error");
            // }
            //
            // const convertedResponse = await response.json() as JwtResponse;
            if(await auth.loginAsync(email, password))
            {
                setSuccessLogin(true);
                toast({
                    title: "Success",
                    description: "You have successfully logged in",
                    duration: 2000
                });
                setTimeout(() => {
                    navigate("/dashboard");
                }, 3000);
                
                return;
            }
            
            throw new Error("Invalid email or password");

        } catch (e) {
            setErrorMessage(e instanceof Error ? e.message : "An error occurred");
            setErrorLogin(true);
            toast({
                title: "Error",
                description: errorMessage,
                duration: 2500,
                variant: "destructive"
            });
        } finally {
            setLoginLoading(false);
        }
    }

    return (
        <div className="flex flex-col h-[85vh] lg:grid lg:grid-cols-2">
            <div className="flex items-center justify-center py-12">
                <div className="mx-auto grid gap-6 w-full max-w-md">
                    <div className="grid gap-2 text-center">
                        <h1 className="text-3xl font-bold">Login</h1>
                        <p className="text-balance text-muted-foreground">
                            Enter your account details to continue
                        </p>
                    </div>
                    <div className="grid gap-4">
                        <div className="grid gap-2">
                            <Label htmlFor="email">Email</Label>
                            <Input
                                id="email"
                                type="email"
                                placeholder="m@example.com"
                                value={email}
                                onChange={handleEmailChange}
                                error={emailValidationErrorMessage}
                                required
                            />
                        </div>
                        <div className="grid gap-2">
                            <div className="flex items-center">
                                <Label htmlFor="password">Password</Label>
                                <Link
                                    to="/forgot-password"
                                    className="ml-auto inline-block text-sm underline"
                                >
                                    Forgot your password?
                                </Link>
                            </div>
                            <Input id="password" type="password" required
                                   placeholder="Password"
                                   value={password}
                                   onChange={handlePasswordChange}
                                   error={passwordValidationErrorMessage}
                            />
                        </div>
                        {errorLogin && <div className="text-red-500 text-sm">{errorMessage}</div>}
                        <Button type="submit"
                                className={cn("w-full")} disabled={!canLogin}
                                onClick={async (e) => {
                                    e.preventDefault();
                                    await handleSubmit();
                                }}
                        >
                            {loginLoading && <svg className="animate-spin h-5 w-5 mr-3" viewBox="0 0 24 24">
                                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor"
                                        strokeWidth="4"/>
                                <path className="opacity-75" fill="currentColor"
                                      d="M4 12a8 8 0 018-8V0c-5.373 0-9.733 4.327-9.733 9.733H4z"/>
                            </svg>}
                            Login
                        </Button>
                        <Link to={`${viteBackendUrl}/api/v1/user/auth/discord`}>
                            <Button variant="outline" className="w-full">
                                Login with Discord <SiDiscord className="pl-2"/>
                            </Button>
                        </Link>
                    </div>
                    <div className="mt-4 text-center text-sm">
                        Don&apos;t have an account?{" "}
                        <Link to="#" className="underline">
                            Sign up
                        </Link>
                    </div>
                </div>
            </div>
            <div className="hidden bg-muted-foreground lg:block relative">
                <img
                    src="/img/login-art.jpg"
                    alt="Image"
                    className={`object-cover max-h-[94vh] w-full`}
                />
            </div>
        </div>
    );
}
