import {Message} from "@/lib/api/types.ts";
import { useState } from "react";
import ChatBox from "@/components/chat/chat-box.tsx";
import ChatInput from "@/components/chat/chat-input.tsx";

export default function ChatPage() {
    const [messages, setMessages] = useState<Message[]>([]);
    const handleSendMessage = (message: string) => {
        const newMessage: Message = {
            text: message,
            sender: 'You',
            character: '😀',
            time: new Date().toLocaleTimeString()
        };
        setMessages([...messages, newMessage]);
    }
    
    return (
        <div className="flex flex-col items-center justify-center h-screen p-4">
            <div className="w-full max-w-2xl bg-foreground text-background shadow-lg rounded-lg overflow-hidden">
                <ChatBox messages={messages}/>
                <ChatInput onSendMessage={handleSendMessage}/>
            </div>
        </div>
    );
}