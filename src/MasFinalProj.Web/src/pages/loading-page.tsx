import { ReactElement, useEffect, useState } from "react";
import {Progress} from "@/components/ui/progress.tsx";

type LoadingPageProps = {
    className?: string;
    loading: boolean;
}
export default function LoadingPage({className, loading}: LoadingPageProps): ReactElement {
    
    const [innerProgress, setInnerProgress] = useState<number>(0);

    useEffect(() => {
        if (loading) {
            const interval = setInterval(() => {
                setInnerProgress((prev) => prev + 1);
            }, 3000);
            return () => clearInterval(interval);
        }
    }, []);

    useEffect(() => {
        if(!loading && innerProgress < 100) {
            setInnerProgress(100);
        }
    }, [loading]);
    
    return (
        <div className={`${className} flex flex-col items-center justify-center`}>
            <Progress value={innerProgress} className="w-1/2"/>
        </div>
    );
}