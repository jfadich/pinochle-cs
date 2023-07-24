import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr"

export default abstract class RealtimeClient {
    private baseUrl: string;

    private subscriptions: Array<string> = []

    private tokenFactory: (() => Promise<string>)

    private connection: null | HubConnection = null

    private connectionPromise : null | Promise<HubConnection> = null

    private hub: string

    protected constructor(url: string, hub:string, tokenFactory: () => Promise<string>) {
        this.baseUrl = url
        this.hub = hub
        this.tokenFactory = tokenFactory
    }

    abstract Start() : Promise<RealtimeClient>

    protected Connect(): Promise<HubConnection> {
        if(this.connection != null) {
            return Promise.resolve(this.connection)
        }
        if (this.connectionPromise != null) {
            return this.connectionPromise
        }

        let connection = new HubConnectionBuilder()
            .withUrl(`${this.baseUrl}/realtime/${this.hub}`, {accessTokenFactory: this.tokenFactory})
            .build()

        this.connectionPromise = connection.start().then(() => {
            this.connection = connection
            this.subscriptions.forEach((subscription) => connection.invoke(subscription))

            return this.connection
        }).finally(() => {
            this.connectionPromise = null
        })

        return this.connectionPromise
    }

    protected Subscribe(subscription: string)
    {
        this.subscriptions.push(subscription)

        if (this.connection) {
            this.connection.invoke(subscription)
        }
    }

    protected On(action: string, callback: (...args: any[]) => any)
    {
        if (this.connection) {
            this.connection.on(action, callback)
        } else {
            console.error("Attempted to register event before connecting. Call 'Start()'")
        }
    }
}