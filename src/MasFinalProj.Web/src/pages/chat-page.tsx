import {useEffect, useState} from "react";
import ChatBox from "@/components/chat/chat-box";
import ChatInput from "@/components/chat/chat-input";
import {myLog} from "@/lib/utils.ts";
import {LogLevel} from "@/shared/types.ts";
import signalR from "use-signalr-hub";


export default function ChatPage() {
    const [messages, setMessages] = useState<string[]>([]);
    
    const signalRHub = signalR.useHub("http://localhost:5128/api/hubs/campaign/", {
        onConnected: (hub) => {
            hub.on("ReceiveMessage", (message) => {
                myLog(LogLevel.debug, "Message received from SignalR: ", message);
                setMessages((prev) => [...prev, message]);
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
        <div className="flex flex-col items-center justify-center h-screen p-4">
            <div className="w-full max-w-2xl bg-foreground text-background shadow-lg rounded-lg overflow-hidden">
                <>
                    <ChatBox messages={messages}/>
                    <ChatInput onSendMessage={handleSubmit} controlledCharacters={[]} />
                </>
            </div>
        </div>
    );
    
    
    
    // const [sampleCharacters, setSampleCharacters] = useState<string[]>([]);
    //
    // // const auth = useAuthStore();
    // const { newMessage, events } = Connector();
    // const [messages, setMessages] = useState<string[]>([]);
    //
    // const [message, setMessage] = useState("null");
    //
    // useEffect(() => {
    //     events((message) => {
    //         myLog(LogLevel.info, "Message received: ", message.arguments[0]);
    //         setMessage(message.arguments[0]);
    //     }, (message) => {
    //         myLog(LogLevel.info, "Message sent: ", message.arguments[0]);
    //         setMessage(message.arguments[0]);
    //     });
    // });
    //
    //
    // useEffect(() => {
    //     setSampleCharacters(["A", "B", "C"]);
    // }, []);
    //
    // useEffect(() => {
    //     setMessages((prev) => [...prev, message]);
    // }, [message]);
    //
    //
    // function handleSendMessage(message: string, character: string | null) {
    //     newMessage(message, character ?? "undefined user");
    // }
    //
    // return (
    //     <div className="flex flex-col items-center justify-center h-screen p-4">
    //         <div className="w-full max-w-2xl bg-foreground text-background shadow-lg rounded-lg overflow-hidden">
    //                 <>
    //                     <ChatBox messages={messages}/>
    //                     <ChatInput onSendMessage={handleSendMessage} controlledCharacters={sampleCharacters}/>
    //                 </>
    //         </div>
    //     </div>
    // );
}
