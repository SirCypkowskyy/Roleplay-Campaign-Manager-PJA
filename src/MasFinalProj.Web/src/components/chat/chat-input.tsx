import React, { useState } from 'react';

interface ChatInputProps {
    onSendMessage: (message: string) => void;
}

function ChatInput({ onSendMessage }: ChatInputProps) {
    const [input, setInput] = useState<string>('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (input.trim() !== '') {
            onSendMessage(input);
            setInput('');
        }
    };

    return (
        <form onSubmit={handleSubmit} className="p-4 border-t border-gray-200">
            <input
                type="text"
                value={input}
                onChange={(e) => setInput(e.target.value)}
                className="w-full p-2 border rounded"
                placeholder="Type your message..."
            />
        </form>
    );
}

export default ChatInput;
