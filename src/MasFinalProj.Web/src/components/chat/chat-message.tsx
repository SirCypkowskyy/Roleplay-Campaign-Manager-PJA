import {SignalRChatMessageReceivedArgumentsType, SignalRResponseMessage} from "@/lib/api/types.ts";
import {cn} from "@/lib/utils.ts";
import {useEffect, useState} from "react";
import {Avatar, AvatarFallback, AvatarImage} from "@/components/ui/avatar.tsx";
import Markdown from 'react-markdown'

interface ChatMessageProps {
    message: SignalRResponseMessage;
}

function ChatMessage({message}: ChatMessageProps) {
    const formattedTimeGeneric = new Date().toLocaleTimeString();
    const [formattedTime, setFormattedTime] = useState<string>(formattedTimeGeneric);
    const [formattedMessage, setFormattedMessage] = useState<SignalRChatMessageReceivedArgumentsType>({} as SignalRChatMessageReceivedArgumentsType);
    const [showAuthor, setShowAuthor] = useState<boolean>(false);
    const [imageUrl, setImageUrl] = useState<string | undefined>(undefined);

    const argumentsToMessageObject = (args: string[]): SignalRChatMessageReceivedArgumentsType => {
        if(args[6] !== undefined)
        {
            setFormattedTime(new Date(Date.parse(args[6]))
                .toLocaleTimeString('pl', {timeZone: 'Asia/Baku'}));
        }
        if(args[5] !== undefined && args[5] !== "")
        {
            const imageId = args[5];
            setImageUrl("http://localhost:5128/api/v1/image/" + imageId);
        }
        
        return {
            message: args[0],
            author: args[1],
            character: args[2],
            authorAppRole: args[3],
            authorCampaignRole: args[4],
            avatarResourceId: args[5],
            time: args[6]
        }
    }
    useEffect(() => {
        setFormattedMessage(argumentsToMessageObject(message.arguments));
    }, []);

    return {formattedMessage} && (
        <div className={cn(
            "flex space-x-2 py-2",
            message.arguments[1] === "System" ? "text-secondary-foreground" : "text-primary"
        )}>
            {formattedMessage.author !== 'System' && (
                <Avatar>
                    <AvatarImage src={imageUrl} alt={formattedMessage.character} />
                    <AvatarFallback>
                    <span className="text-sm text-secondary-foreground">
                        {formattedMessage.character === '' ? 'N' : formattedMessage.character?.slice(0, 2)}
                    </span>
                    </AvatarFallback>
                </Avatar>)}
            <div className="flex flex-col">
                <div className="flex space-x-2">
                    {formattedMessage.author !== 'System' && (
                        <>
                        <span className="font-semibold hover:underline" onMouseEnter={() => setShowAuthor(true)}
                              onMouseLeave={() => setShowAuthor(false)}>
                        {formattedMessage.character === '' ? 'Narrator' : formattedMessage.character}
                        </span>
                            <span className={cn("text-xs text-gray-500",
                                !showAuthor && 'hidden'
                            )}>
                        ({formattedMessage.author})
                        </span>
                        </>
                    )}
                    <span className="text-xs text-gray-500 text-center">
                        {formattedTime !== '' && formattedTime !== undefined ? formattedTime : formattedTimeGeneric}
                    </span>
                </div>
                {(formattedMessage.message !== '' && formattedMessage.message !== undefined) && (
                    <a className="text-sm text-gray-800 dark:text-gray-200">
                        <Markdown className={cn("prose dark:prose-invert animate-left-to-right-text-fade-in",
                        )}>
                            {formattedMessage.message}
                        </Markdown>
                    </a>
                )}
            </div>
        </div>
    );

}

export default ChatMessage;
