import {useNavigate, useParams} from "react-router-dom";
import {ReactElement, useEffect} from "react";

export default function DiscordOAuthRedirectPage() : ReactElement {
    const {code} = useParams<{ code: string }>();
    const navigate = useNavigate();

    useEffect(() => {
        if (code) {
            navigate(`/auth/discord?code=${code}`);
        }
    }, []);
    
    return <div>Redirecting...</div>;
}