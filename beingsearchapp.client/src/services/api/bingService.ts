import { bingAxios } from '../../utils/httpClient';
import type { AxiosError } from 'axios';
import type { SearchResponse } from '../../types/types';

export const searchBing = async (query: string): Promise<SearchResponse> => {
    try {
        const response = await bingAxios.get('/search', {
            params: {
                q: query,
                count: 10,
                responseFilter: 'Webpages',
                textDecorations: true,
                textFormat: 'HTML',
            },
        });
        return response.data;
    } catch (error) {
        console.error('Bing API Error:', error);
        const axiosError = error as AxiosError;
        return {
            errors: [{
                code: axiosError.response?.status?.toString() || '500',
                message: axiosError.message
            }]
        };
    }
};