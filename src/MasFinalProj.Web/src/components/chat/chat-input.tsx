import React, { useState } from 'react';

interface ChatInputProps {
    onSendMessage: (message: string, author: string) => void;
    controlledCharacters: string[];
}

function ChatInput({ onSendMessage, controlledCharacters }: ChatInputProps) {
    const [input, setInput] = useState<string>('');
    const [character, setCharacter] = useState<string>('');

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (input.trim() !== '') {
            onSendMessage(input, character);
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
            <select
                value={character || ''}
                onChange={(e) => setCharacter(e.target.value)}
                className="w-full p-2 border rounded mt-2"
            >
                <option value="">Select Character</option>
                {controlledCharacters.map((char, idx) => (
                    <option key={idx} value={char}>{char}</option>
                ))}
            </select>
            <button type="submit" className="w-full p-2 border rounded mt-2 bg-blue-500 text-white">Send</button>
        </form>
    );
}

export default ChatInput;
