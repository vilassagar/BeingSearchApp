
import axios from 'axios';
import type { AxiosError, AxiosResponse, InternalAxiosRequestConfig } from 'axios';

// Create a base axios instance for the Bing Search API
export const bingAxios = axios.create({
    baseURL: 'https://api.bing.microsoft.com/v7.0',
    headers: {
        'Ocp-Apim-Subscription-Key': 'YOUR_BING_API_KEY',
    }
});

// Create a base axios instance for our own Location API
export const locationsAxios = axios.create({
    baseURL: `${import.meta.env.VITE_API_BASE_URL}/api`, // Adjust to match your backend API URL
    headers: {
        'Content-Type': 'application/json'
    }
});

// Add request interceptor to the locations API
locationsAxios.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        // You can modify the request here before it's sent
        console.log(`Request: ${config.method?.toUpperCase()} ${config.url}`);
        return config;
    },
    (error: AxiosError) => {
        console.error('Request Error:', error);
        return Promise.reject(error);
    }
);

// Add response interceptor to the locations API
locationsAxios.interceptors.response.use(
    (response: AxiosResponse) => {
        // You can modify the response data here
        console.log(`Response: ${response.status} from ${response.config.url}`);
        return response;
    },
    (error: AxiosError) => {
        // Handle different error scenarios
        if (error.response) {
            console.error('Response Error:', error.response.status, error.response.data);

            // Handle specific status codes
            switch (error.response.status) {
                case 401:
                    console.error('Unauthorized. You need to login.');
                    break;
                case 403:
                    console.error('Forbidden. You don\'t have permission to access this resource.');
                    break;
                case 404:
                    console.error('Resource not found.');
                    break;
                case 500:
                    console.error('Server error. Please try again later.');
                    break;
                default:
                    console.error(`Error ${error.response.status}: ${error.message}`);
            }
        } else if (error.request) {
            console.error('No response received:', error.request);
        } else {
            console.error('Request setup error:', error.message);
        }

        return Promise.reject(error);
    }
);