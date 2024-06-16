import {useEffect, useRef, useState} from "react";
import ChatBox from "@/components/chat/chat-box";
import ChatInput from "@/components/chat/chat-input";
import {myLog} from "@/lib/utils.ts";
import {LogLevel} from "@/shared/types.ts";
import signalR from "use-signalr-hub";
import {CharacterResponseDto, SignalRResponseMessage} from "@/lib/api/types.ts";
import {useNavigate, useParams} from 'react-router-dom';
import {Endpoints} from "@/lib/api/endpoints.ts";
import {useAuthStore} from "@/store/auth-store.ts";
import LoadingPage from "@/pages/loading-page.tsx";

export default function ChatPage() {
    const [messages, setMessages] = useState<SignalRResponseMessage[]>([]);
    const {campaignId} = useParams();
    const [characters, setCharacters] = useState<CharacterResponseDto[]>([]);
    const [loading, setLoading] = useState(true);


    const auth = useAuthStore();
    const navigate = useNavigate();


    const signalRHub = signalR.useHub(`http://localhost:5128/api/hubs/campaign?campaignId=${campaignId}`, {
        onConnected: (hub) => {
            loadMessages();
            setTimeout(() => {
                hub.on("ReceiveMessage", (message, author, character) => {
                    myLog(LogLevel.debug, "Message received from SignalR: ", message);
                    const newMessage: SignalRResponseMessage = {
                        type: 0,
                        target: "ReceiveMessage",
                        arguments: [message, author, character]
                    }
                    setMessages((prev) => [...prev, newMessage]);
                });
            }, 1000);
        },
        onDisconnected: (hub) => {
            myLog(LogLevel.debug, "SignalR disconnected: ", hub?.message);
            navigate("/dashboard");
        },
        onError: (error) => {
            myLog(LogLevel.error, "SignalR error: ", error);
        }
    });

    const skip = useRef(0);
    const numToQuery = 50;
    const loadingRef = useRef(false);

    const loadMessages = () => {
        if (loadingRef.current) return;
        loadingRef.current = true;
        Endpoints.User.GET_CAMPAIGN_MESSAGES_API(auth.getBearerToken(), campaignId as string, numToQuery, skip.current)
            .then((response) => {
                if (!response.isSuccess) {
                    myLog(LogLevel.error, "Failed to retrieve campaign messages: ", response.errorDetails);
                    return;
                }
                myLog(LogLevel.debug, "Messages retrieved: ", response.data)
                const convertedMessages = response.data?.map((message) => {
                    return {
                        type: 0,
                        target: "ReceiveMessage",
                        arguments: [message.text, message.sender, message.character, "", "", message.imageId ?? "", message.time]
                    }
                }) || [];

                myLog(LogLevel.debug, "Messages retrieved: ", convertedMessages)
                // 2024-06-16T16:53:55.762603
                setMessages((prev) => [...prev, ...convertedMessages].sort((a, b) => {
                    const aTime = new Date(Date.parse(a.arguments[6]));
                    const bTime = new Date(Date.parse(b.arguments[6]));
                    return aTime.getTime() - bTime.getTime();
                }));
                skip.current += numToQuery;
            })
            .finally(() => {
                loadingRef.current = false;
            });
    }


    useEffect(() => {
        // const handleScroll = (e: any) => {
        //     if (e.target.scrollTop === 0 && !loadingRef.current) {
        //         loadMessages();
        //     }
        // }
        // const chatBox = document.querySelector(".chat-box");
        // chatBox?.addEventListener("scroll", handleScroll);
        // return () => chatBox?.removeEventListener("scroll", handleScroll);

        Endpoints.User.GET_CONTROLLABLE_CHARACTERS_API(auth.getBearerToken(), campaignId as string)
            .then((response) => {
                if (!response.isSuccess) {
                    myLog(LogLevel.error, "Failed to retrieve controllable characters: ", response.errorDetails);
                    return;
                }
                myLog(LogLevel.debug, "Controllable characters retrieved: ", response.data);
                const characters = response.data as unknown as CharacterResponseDto[];
                setCharacters(characters);
                setLoading(false);
            })
            .catch((error) => {
                myLog(LogLevel.error, "Error retrieving controllable characters: ", error);
            });

    }, []);

    const handleSubmit = (message: string, author: string) => {
        signalRHub?.invoke("SendMessage", message, author).catch((error) => {
            myLog(LogLevel.error, "Error sending message: ", error);
        });
    }

    return loading ? <LoadingPage loading={loading} /> : (
        <div className="max-h-[90vh]">
            <div className={"flex flex-col"}>
                <ChatBox messages={messages} className={"chat-box flex-grow max-h-[80vh]"}/>
                <ChatInput onSendMessage={handleSubmit} controlledCharacters={characters}
                           className={"p-4 border-t bottom-12"}/>
            </div>
        </div> 
    );
}
