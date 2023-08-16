import {PinochleApi} from "./GeneratedClients"
import GameHub from "./RealTime/GameHub";

export default class GameRoom {
    public readonly Play: PinochleApi.IPlayClient

    public readonly GameHub: GameHub

    constructor(baseUrl: string, tokenFactory: () => Promise<string> ) {
        let fetchFactory = {fetch(uri: string, options: RequestInit = {}) {
                return fetch(uri, {
                    ...options,
                    headers: {
                        ...options.headers,
                        Authorization: `bearer ${tokenFactory()}`
                    }
                })
            }}

        this.Play = new PinochleApi.PlayClient(baseUrl, fetchFactory)
        this.GameHub = new GameHub(baseUrl, tokenFactory)
    }
}