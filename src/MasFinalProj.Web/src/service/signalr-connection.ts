import * as signalR from "@microsoft/signalr";
import { SignalRResponseMessage } from "@/lib/api/types";

const URL = "http://localhost:5128/api/hubs/campaign/";

class Connector {
    private connection: signalR.HubConnection;
    public events: (onMessageReceived: (message: SignalRResponseMessage) => void, onMessageSent: (message: SignalRResponseMessage) => void) => void;
    static instance: Connector;

    private constructor() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(URL)
            .withAutomaticReconnect()
            .build();

        this.connection.start().catch(err => console.error("SignalR Connection Error: ", err));

        this.events = (onMessageReceived: (message: SignalRResponseMessage) => void, onMessageSent: (message: SignalRResponseMessage) => void) => {
            this.connection.on("MessageReceived", (message: SignalRResponseMessage) => {
                onMessageReceived(message);
            });
            this.connection.on("MessageSent", (message: SignalRResponseMessage) => {
                onMessageSent(message);
            });
        }
    }

    public newMessage = (message: string, author: string) => {
        this.connection.send("MessageReceived", author, message).then(() => {
        }).catch(err => console.error("Error sending message: ", err));
    }

    public static getInstance(): Connector {
        if (!Connector.instance) {
            Connector.instance = new Connector();
        }
        return Connector.instance;
    }
}

export default Connector.getInstance;
