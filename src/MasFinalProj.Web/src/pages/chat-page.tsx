import { useState } from "react";
import ChatBox from "@/components/chat/chat-box";
import ChatInput from "@/components/chat/chat-input";
import { myLog } from "@/lib/utils.ts";
import { LogLevel } from "@/shared/types.ts";
import signalR from "use-signalr-hub";
import { SignalRResponseMessage } from "@/lib/api/types.ts";

export default function ChatPage() {
    const [messages, setMessages] = useState<SignalRResponseMessage[]>([]);

    const signalRHub = signalR.useHub("http://localhost:5128/api/hubs/campaign/", {
        onConnected: (hub) => {
            hub.on("ReceiveMessage", (message, author, character) => {
                myLog(LogLevel.debug, "Message received from SignalR: ", message);
                const newMessage: SignalRResponseMessage = {
                    type: 0,
                    target: "ReceiveMessage",
                    arguments: [message, author, character]
                }
                setMessages((prev) => [...prev, newMessage]);
            });
        },
        onDisconnected: (hub) => {
            myLog(LogLevel.debug, "SignalR disconnected: ", hub?.message);
        },
        onError: (error) => {
            myLog(LogLevel.error, "SignalR error: ", error);
        }
    });

    const handleSubmit = (message: string, author: string) => {
        signalRHub?.invoke("SendMessage", message, author).catch((error) => {
            myLog(LogLevel.error, "Error sending message: ", error);
        });
    }
    return (
        <div className="max-h-[90vh]">
            <div className={"flex flex-col"}>
                <ChatBox messages={messages} className={"flex-grow max-h-[80vh]"}/>
                <ChatInput onSendMessage={handleSubmit} controlledCharacters={[]}
                           className={"p-4 border-t bottom-12"}/>
            </div>
        </div>
    );
}
