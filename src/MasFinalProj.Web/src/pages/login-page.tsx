import {Button} from "@/components/ui/button";
import {Input} from "@/components/ui/input";
import {Label} from "@/components/ui/label";
import React, {ReactElement, useEffect, useState} from "react";
import {Link} from "react-router-dom";
import {useDebounce} from "@uidotdev/usehooks";
import {SiDiscord} from "@icons-pack/react-simple-icons";

export default function LoginPage(): ReactElement {

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [passwordValidationErrorMessage, setPasswordValidationErrorMessage] = useState("");
    const [emailValidationErrorMessage, setEmailValidationErrorMessage] = useState("");
    const [canLogin, setCanLogin] = useState(false);
    const debouncedEmail = useDebounce(email, 500);
    const debouncedPassword = useDebounce(password, 500);

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
                        <Button type="submit" className="w-full" disabled={!canLogin}>
                            Login
                        </Button>
                        <Button variant="outline" className="w-full">
                            Login with Discord <SiDiscord className="pl-2"/>
                        </Button>
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
