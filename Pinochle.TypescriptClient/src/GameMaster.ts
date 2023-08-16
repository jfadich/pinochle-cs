import {PinochleApi} from "./GeneratedClients"
import MatchmakingHub from "./RealTime/MatchmakingHub"

export default class GameMaster {
    public readonly Games: PinochleApi.IGamesClient

    public readonly Matchmaking: PinochleApi.IMatchmakingClient

    public readonly MatchmakingHub: MatchmakingHub

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

        this.Games = new PinochleApi.GamesClient(baseUrl, fetchFactory)
        this.Matchmaking = new PinochleApi.MatchmakingClient(baseUrl, fetchFactory)
        this.MatchmakingHub = new MatchmakingHub(baseUrl, tokenFactory)
    }
}