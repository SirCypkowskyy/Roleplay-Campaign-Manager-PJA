import {SignalRChatMessageReceivedArgumentsType, SignalRResponseMessage} from "@/lib/api/types.ts";
import {cn, myLog} from "@/lib/utils.ts";
import {LogLevel} from "@/shared/types.ts";
import {useEffect, useState} from "react";
import {Avatar, AvatarFallback, AvatarImage} from "@/components/ui/avatar.tsx";
import Markdown from 'react-markdown'

interface ChatMessageProps {
    message: SignalRResponseMessage;
}

function ChatMessage({message}: ChatMessageProps) {
    const formattedTime = new Date().toLocaleTimeString();
    const [formattedMessage, setFormattedMessage] = useState<SignalRChatMessageReceivedArgumentsType>({} as SignalRChatMessageReceivedArgumentsType);
    const [showAuthor, setShowAuthor] = useState<boolean>(false);

    const argumentsToMessageObject = (args: string[]): SignalRChatMessageReceivedArgumentsType => {
        return {
            message: args[0],
            author: args[1],
            character: args[2],
            authorAppRole: args[3],
            authorCampaignRole: args[4]
        }
    }

    useEffect(() => {
        setFormattedMessage(argumentsToMessageObject(message.arguments));

        myLog(LogLevel.debug, "Message formatted: ", formattedMessage);
    }, []);

    return {formattedMessage} && (
        <div className={cn(
            "flex space-x-2 py-2",
            message.arguments[1] === "System" ? "text-secondary-foreground" : "text-primary"
        )}>
            {formattedMessage.author !== 'System' && (
                <Avatar>
                    <AvatarImage src="/placeholder.svg" alt="Avatar"/>
                    <AvatarFallback>
                    <span className="text-sm text-white">
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
                        {formattedTime}
                    </span>
                </div>
                {formattedMessage.message !== '' && (
                    <p className="text-sm text-gray-800 dark:text-gray-200">
                        <Markdown className="prose dark:prose-invert">
                            {formattedMessage.message}
                        </Markdown>
                    </p>
                )}
            </div>
        </div>
    );

}

export default ChatMessage;
