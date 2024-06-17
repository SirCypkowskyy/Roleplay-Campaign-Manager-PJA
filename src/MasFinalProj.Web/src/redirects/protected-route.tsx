import {useAuthStore} from "@/store/auth-store.ts";
import {Outlet, useNavigate} from "react-router-dom";
import {myLog} from "@/lib/utils.ts";
import {LogLevel} from "@/shared/types.ts";
import {useEffect, useState} from "react";

type ProtectedRouteProps = {
    redirectTo?: string;
};

/**
 * Komponent zabezpieczający przed nieautoryzowanym dostępem
 * @param children dzieci komponentu
 * @param redirectTo ścieżka przekierowania
 */
export default function ProtectedRoute({redirectTo = '/login'}: ProtectedRouteProps) {
    const auth = useAuthStore();
    const navigate = useNavigate();
    
    const queryParams = new URLSearchParams(window.location.search);
    const [authCalledOnce, setAuthCalledOnce] = useState(false);
    
    const queryObject = ({
        code: queryParams.get("code") ?? null,
    });


    useEffect(() => {
        if (queryObject.code && !authCalledOnce) {
            auth.loginWithOAuthRetrieveCodeAsync(queryObject.code).then(() => {
                    setAuthCalledOnce(true);
                    auth.challengeAsync().then(() => {
                            if (!auth.isLoggedIn) {
                                navigate("/");
                            }
                        }
                    );
                }
            );
        }
                
        if (!auth.isLoggedIn) {
            myLog(LogLevel.warn, '[ProtectedRoute]: redirecting to login page, user is not logged in');
            navigate(redirectTo);
        }
        
        if (auth.isLoggedIn) {
            myLog(LogLevel.info, '[ProtectedRoute]: user is claming to be logged in, checking if token is valid');
            auth.refreshTokenAsync().then(() => {
                auth.challengeAsync().then(() => {
                    if (!auth.isLoggedIn) {
                        myLog(LogLevel.warn, '[ProtectedRoute]: token is invalid, redirecting to login page');
                        navigate(redirectTo);
                    }
                });
            });
        }
    }, [auth.isLoggedIn]);

    return <Outlet/>;
}