import {Message} from "@/lib/api/types.ts";

interface ChatMessageProps {
    message: Message;
}

function ChatMessage({ message }: ChatMessageProps) {
    const formattedTime = message.time.split(':').slice(0, 2).join(':');
    return (
        <div className={`p-2 rounded ${message.sender === 'user' ? 'text-right' : 'text-left'}`}>
            <span className="text-sm text-gray-500">{formattedTime}
                &nbsp;
            </span>
            <span className="text-lg">{message.character}</span>
            {message.text}
        </div>
    );
}

export default ChatMessage;
