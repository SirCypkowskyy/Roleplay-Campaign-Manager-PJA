import React, { useState } from 'react';

interface ChatInputProps {
    onSendMessage: (message: string, author: string) => void;
    controlledCharacters: string[];
    className?: string;
}

function ChatInput({ onSendMessage, controlledCharacters, className }: ChatInputProps) {
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
        <form onSubmit={handleSubmit} className={`${className} fixed w-full px-4`}>
            <div className="flex space-x-2">
                <textarea
                    value={input}
                    onChange={(e) => setInput(e.target.value)}
                    className="flex-1 p-2 border rounded focus:outline-none focus:ring-2 focus:ring-primary dark:bg-muted dark:border-muted-foreground dark:text-muted-foreground"
                    placeholder="Type your message..."
                    rows={3}
                />
                <select
                    value={character || ''}
                    onChange={(e) => setCharacter(e.target.value)}
                    className="p-2 border rounded dark:bg-muted dark:border-muted-foreground dark:text-muted-foreground"
                >
                    <option value="">Select Character</option>
                    {controlledCharacters.map((char, idx) => (
                        <option key={idx} value={char}>{char}</option>
                    ))}
                </select>
                <button type="submit" className="p-2 border rounded bg-primary text-primary-foreground">Send</button>
            </div>
        </form>
    );
}

export default ChatInput;
