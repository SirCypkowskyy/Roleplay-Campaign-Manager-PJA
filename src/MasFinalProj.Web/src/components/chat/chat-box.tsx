import ChatMessage from "@/components/chat/chat-message.tsx";
import { SignalRResponseMessage } from "@/lib/api/types.ts";
import {cn} from "@/lib/utils.ts";

interface ChatBoxProps {
    messages: SignalRResponseMessage[];
    className?: string;
}

function ChatBox({ messages, className }: ChatBoxProps) {
    return (
        <main className={cn(
            "overflow-y-auto p-6",
            className
        )}>
            {messages.map((message, index) => (
                <ChatMessage key={index} message={message}/>
            ))}
        </main>
    );
}

export default ChatBox;
