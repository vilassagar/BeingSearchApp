export interface SearchResult {
    id: string;
    name: string;
    url: string;
    snippet: string;
    dateLastCrawled: string;
}

export interface SearchResponse {
    webPages?: {
        value: SearchResult[];
        totalEstimatedMatches: number;
    };
    errors?: Array<{
        code: string;
        message: string;
        subCode?: string;
    }>;
}