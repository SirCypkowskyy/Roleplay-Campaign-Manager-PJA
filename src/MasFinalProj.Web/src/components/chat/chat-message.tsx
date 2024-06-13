
interface ChatMessageProps {
    message: string;
}

function ChatMessage({ message }: ChatMessageProps) {
    const formattedTime = new Date().toLocaleTimeString();
    return (
        <div className={`p-2 rounded ${message === 'user' ? 'text-right' : 'text-left'}`}>
            <span className="text-sm text-gray-500">{formattedTime}
                &nbsp;
            </span>
            <span className="text-lg">{message}</span>
        </div>
    );
}

export default ChatMessage;
