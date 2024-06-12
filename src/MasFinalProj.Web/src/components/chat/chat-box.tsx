import {Message} from "@/lib/api/types.ts";
import ChatMessage from "@/components/chat/chat-message.tsx";


interface ChatBoxProps {
    messages: Message[];
}

function ChatBox({ messages }: ChatBoxProps) {
    return (
        <div className="p-4 h-96 overflow-y-auto">
            {messages.map((message, index) => (
                <ChatMessage key={index} message={message} />
            ))}
        </div>
    );
}

export default ChatBox;
