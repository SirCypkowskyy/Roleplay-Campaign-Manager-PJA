import React, {ReactElement, useEffect, useState} from "react";
import {Button} from "@/components/ui/button";
import {Input} from "@/components/ui/input";
import {Label} from "@/components/ui/label";
import {useToast} from "@/components/ui/use-toast";
import {cn} from "@/lib/utils";
import {useNavigate, useSearchParams} from "react-router-dom";
import {Endpoints} from "@/lib/api/endpoints";
import {Tooltip, TooltipContent, TooltipProvider, TooltipTrigger} from "@/components/ui/tooltip";

export default function RegisterPage(): ReactElement {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [usernameError, setUsernameError] = useState("");
    const [emailError, setEmailError] = useState("");
    const [passwordError, setPasswordError] = useState("");
    const [confirmPasswordError, setConfirmPasswordError] = useState("");
    const [canRegister, setCanRegister] = useState(false);
    const [registerLoading, setRegisterLoading] = useState(false);
    const [accptedToS, setAcceptedToS] = useState(false);

    const { toast } = useToast();
    const navigate = useNavigate();
    const [searchParams, _] = useSearchParams();

    useEffect(() => {
        const usernameParam = searchParams.get("username");
        const emailParam = searchParams.get("email");
        if (usernameParam) setUsername(usernameParam);
        if (emailParam) setEmail(emailParam);
    }, [searchParams]);

    useEffect(() => {
        validateForm();
    }, [username, email, password, confirmPassword, accptedToS]);

    const validateUsername = (username: string) => {
        if (username.length > 32) setUsernameError("Username must be at most 32 characters long");
        else setUsernameError("");
    }

    const validateEmail = (email: string) => {
        if (!email.includes("@")) setEmailError("Invalid email address");
        else setEmailError("");
    }

    const validatePassword = (password: string) => {
        if (password.length < 6) setPasswordError("Password must be at least 6 characters long");
        else if (password.length > 32) setPasswordError("Password must be at most 32 characters long");
        else setPasswordError("");
    }

    const validateConfirmPassword = (confirmPassword: string) => {
        if (confirmPassword !== password) setConfirmPasswordError("Passwords do not match");
        else setConfirmPasswordError("");
    }

    const validateForm = () => {
        validateUsername(username);
        validateEmail(email);
        validatePassword(password);
        validateConfirmPassword(confirmPassword);
        if (!usernameError && !emailError && !passwordError && !confirmPasswordError && username && email && password && confirmPassword && accptedToS) {
            setCanRegister(true);
        } else {
            setCanRegister(false);
        }
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!canRegister) return;
        setRegisterLoading(true);

        try {
            const response = await Endpoints.User.REGISTER_API(username, email, password);
            if (response.isSuccess) {
                toast({
                    title: "Success",
                    description: "You have successfully registered",
                    duration: 2000,
                    variant: "default"
                });
                setTimeout(() => {
                    navigate("/dashboard");
                }, 3000);
            } else {
                throw new Error(response.errorMessage || "Failed to register");
            }
        } catch (error) {
            toast({
                title: "Error",
                description: "Failed to register",
                duration: 2500,
                variant: "destructive",
            });
        } finally {
            setRegisterLoading(false);
        }
    }

    return (
        <div className="flex flex-col h-[85vh] lg:grid lg:grid-cols-2">
            <div className="flex items-center justify-center py-12">
                <div className="mx-auto grid gap-6 w-full max-w-md">
                    <div className="grid gap-2 text-center">
                        <h1 className="text-3xl font-bold">Register</h1>
                        <p className="text-muted-foreground">
                            Create your account to get started
                        </p>
                    </div>
                    <div className="grid gap-4">
                        <div className="grid gap-2">
                            <Label htmlFor="username">Username</Label>
                            <Input
                                id="username"
                                type="text"
                                placeholder="Username"
                                value={username}
                                onChange={(e) => setUsername(e.target.value)}
                                error={usernameError}
                                required
                            />
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="email">Email</Label>
                            <Input
                                id="email"
                                type="email"
                                placeholder="m@example.com"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                error={emailError}
                                required
                            />
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="password">Password</Label>
                            <Input
                                id="password"
                                type="password"
                                placeholder="Password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                error={passwordError}
                                required
                            />
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="confirmPassword">Confirm Password</Label>
                            <Input
                                id="confirmPassword"
                                type="password"
                                placeholder="Confirm Password"
                                value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)}
                                error={confirmPasswordError}
                                required
                            />
                        </div>
                        <div className="grid gap-2">
                            <Label htmlFor="terms">
                                <TooltipProvider>
                                <Tooltip>
                                    By registering, you agree to our <TooltipTrigger>Terms and Conditions</TooltipTrigger>
                                    <TooltipContent>
                                        <p className="text-muted-foreground font-bold">
                                            MAS Roleplay App Terms and Conditions
                                        </p>
                                        <div className="text-muted-foreground text-sm">
                                            <ol className="list-decimal list-inside">
                                                <li>Don't be a jerk</li>
                                                <li>Don't break the law</li>
                                                <li>Don't cheat</li>
                                                <li>Don't exploit bugs</li>
                                                <li>Don't share your account</li>
                                            </ol>
                                            <p className="mt-2 text-xs">
                                                That's it! Enjoy your stay!
                                            </p>
                                        </div>
                                    </TooltipContent>
                                </Tooltip>
                                </TooltipProvider>
                            </Label>
                            <Input
                                id="terms"
                                type="checkbox"
                                className="h-5 w-5"
                                checked={accptedToS}
                                onChange={(e) => setAcceptedToS(e.target.checked)}
                                required
                            />
                        </div>
                        <Button type="submit" className={cn("w-full")} disabled={!canRegister || registerLoading} onClick={handleSubmit}>
                            {registerLoading && <svg className="animate-spin h-5 w-5 mr-3" viewBox="0 0 24 24">
                                <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"/>
                                <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0c-5.373 0-9.733 4.327-9.733 9.733H4z"/>
                            </svg>}
                            Register
                        </Button>
                    </div>
                </div>
            </div>
            <div className="hidden bg-muted-foreground lg:block relative">
                <img src="/img/register-art.jpg" alt="Image" className="object-cover max-h-[94vh] w-full" />
            </div>
        </div>
    );
}
